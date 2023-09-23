using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;

public class GuardFSM : MonoBehaviour
{
    public enum GuardState
    {
        //**Death GuardState added by Alexander Maynard(301170707)**
        Patrol, Chase, Attack, RunAway, Death
    }

    public GuardState currentState;
    public GameObject enemy;
    public float GuardFOV = 89; // degrees
    private float cosGuardFOVOver2InRAD;

    public float closeEnoughAttackCutoff = 2; // if distance of guard to enemy <= 2m  close enough to Attack
    public float closeEnoughSenseCutoff = 15; // if distance of guard to enemy <= 15m  close enough to start chasing



    public float strength = 90; //[0, 100]

    public float speed = 2; //2 meters per second


    public Transform[] waypoints;
    public int nextWaypointindex = 0; 


    // Start is called before the first frame update
    void Start()
    {
        cosGuardFOVOver2InRAD = Mathf.Cos(GuardFOV / 2f * Mathf.Deg2Rad); // in radians 
        
    }

    // Update is called once per frame
    void Update()
    {
        FSM();
    }

    private void FSM()
    {
        switch (currentState)
        {
            case GuardState.Patrol:
                HandlePatrol();
                break;
            case GuardState.Chase:
                HandleChase();
                break;
            case GuardState.Attack:
                HandleAttack();
                break;
            case GuardState.RunAway:
                HandleRunAway();
                break;
            //GuardState.Death added by Alexander Maynard(301170707)**
            case GuardState.Death:
                HandleDeath();
                break;
            default:
                break;
        }
    }

    private void HandleRunAway()
    {
        //default ACTIONS during RunaAway state
        print("Running Away...");

        RunAway();


        //CHECK TRANSITION CONDITIONS

        //T4 if safe
        if(Safe())
        {
            ChangeState(GuardState.Patrol);
        }


        //if condition added by Alexander Maynard(301170707)
        //death can occur in any state...
        if (strength <= 0)
        {
            ChangeState(GuardState.Death);
        }
    }


    //added by Alexander Maynard(301170707)
    //HandleDeath() to handle defualt conditions for when the enemy dies
    private void HandleDeath()
    {
        //default actions during death
        print("Enemy dying...");
        //Call to the death method
        Death();


        //NOTE: death ends all other states.
    }

    

    private void HandleAttack()
    {
        //default ACTIONS during Attack state
        print("Attacking...");



        //CHECK TRANSITION CONDITIONS
        if(ThreatenedAndWeakerThanEnemy())
        {
            ChangeState(GuardState.RunAway);
        }


        //if condition added by Alexander Maynard(301170707)
        //death can occur in any state...
        if (strength <= 0)
        {
            ChangeState(GuardState.Death);
        }
    }

    private void HandleChase()
    {
        //default ACTIONS during chase state
        print("Chasing...");
        Chase();

        //CHECK TRANSITION CONDITIONS

        //T2 Within range

        if(WithinRangeAndStrongerThanEnemy())
        {
            ChangeState(GuardState.Attack);
        }


        //if condition added by Alexander Maynard(301170707)
        //death can occur in any state...
        if (strength <= 0)
        {
            ChangeState(GuardState.Death);
        }
    }


    private void HandlePatrol()
    {
        //DEFAULT actions during patrol state
        print("Patrolling....");
        Patrol();

        //CHECK TRANSITION CONDITIONS
        //T1 - SensePlayer/Enemy (from draw.io file for GuardFSM)
        if (SenseEnemy())
        {
            ChangeState(GuardState.Chase);
        }

        //Check T3 ThreatenedAndWeakerThanEnemy  (from draw.io file for GuardFSM)
        if(ThreatenedAndWeakerThanEnemy())
        {
            ChangeState(GuardState.RunAway);
        }


        //if condition added by Alexander Maynard(301170707)
        //death can occur in any state...
        if (strength <= 0)
        {
            ChangeState(GuardState.Death);
        }

    }


    //**Death method added by Alexander Maynard(301170707) 
    private void Death()
    {
        //stop movement of player upon death. Player also sinks to the ground
        //to simulate falling to the ground upon death 
        Vector3 noMovement = new Vector3(this.transform.position.x, (float)-0.25, this.transform.position.z);
        this.transform.position = noMovement;


        //enemy annouces death to console to confirm death
        print("The enemy has been vanquished!");

        //enemy changes color to signify it has died
        this.gameObject.GetComponent<Renderer>().material.color = Color.green;
    }
    
    
    
    
    private void Patrol()
    {
        if(Vector3.Distance(this.transform.position, waypoints[nextWaypointindex].transform.position) < float.Epsilon)
        {
            nextWaypointindex = (nextWaypointindex + 1) % waypoints.Length;
        }
        Vector3 target = waypoints[nextWaypointindex].transform.position;
        Vector3 movement = Vector3.MoveTowards(this.transform.position, target, speed*Time.deltaTime);
        //movement.y = 0.5f;
        this.transform.position = movement;
    }



    private void RunAway()
    {
        //E.heading = {E - G}.

        Vector3 enemyHeading = (enemy.transform.position - this.transform.position);
        float enemyDistance = enemyHeading.magnitude;
        enemyHeading.Normalize();

        //rb.velocity = enemyHeading*speed;
        //
        Vector3 movement = enemyHeading * speed * Time.deltaTime; //m/s *s/frame=meters/frame
        Vector2.ClampMagnitude(movement, enemyDistance);
        this.transform.position -= movement;
    }



    private void Chase()
    {
        //E.heading = {E - G}.
        
        Vector3 enemyHeading = (enemy.transform.position - this.transform.position);
        float enemyDistance = enemyHeading.magnitude;
        enemyHeading.Normalize();

        //rb.velocity = enemyHeading*speed;
        //
        Vector3 movement = enemyHeading * speed*Time.deltaTime; //m/s *s/frame=meters/frame
        Vector2.ClampMagnitude(movement, enemyDistance);
        this.transform.position += movement;
    }

    private bool Safe()
    {
        return !Threatened();
    }


    private bool WithinRangeAndStrongerThanEnemy()
    {
        if (WithinRange() && !WeakerThanEnemy())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool WeakerThanEnemy()
    {
        PlayerController enemyController =  enemy.GetComponent<PlayerController>();

        if (strength < enemyController.strength)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool WithinRange()
    {
        return EnemyCloseEnough(closeEnoughAttackCutoff);
    }

    private bool ThreatenedAndWeakerThanEnemy()
    {
        if (Threatened() && WeakerThanEnemy())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool Threatened()
    {
        return EnemyCloseEnough(closeEnoughSenseCutoff);
    }

    private void ChangeState(GuardState newGuardState)
    {
        currentState = newGuardState;
    }

    private bool SenseEnemy()
    {

        //Case 1: Enemy in front and close enough
        if(EnemyInFront() && EnemyCloseEnough(closeEnoughSenseCutoff))
        {
            return true;
        } 
        else
        {
            return false;
        }
    }

    private bool EnemyCloseEnough(float distance)
    {

        if(Vector3.Distance(this.transform.position, enemy.transform.position) <= distance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool EnemyInFront()
    {
        //Angle(Guard.Fwd, EasingFunction.heading) < GuardFOV/2 => true, else false
        // <=> cos(Angle)>cos(Guardfov/2)
        //E.heading = {E - G}.
        Vector3 enemyHeading = (enemy.transform.position - this.transform.position).normalized;

        //if(Vector3.Angle(enemyHeading, this.transform.forward))
        //{
        //return true;
        //}
        float cosAngle = Vector3.Dot(enemyHeading, this.transform.forward);
        if (cosAngle > cosGuardFOVOver2InRAD)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnDrawGizmos()
    {
         Gizmos.color = Color.yellow;
         for(int i = 0; i < waypoints.Length; i++)
         {
             int i1 = (i + 1) % waypoints.Length;
             Gizmos.DrawLine(waypoints[i].transform.position, waypoints[i1].transform.position);
         }

        Gizmos.color = Color.blue;
        //want to see cone from fwd-FOV/2 to fwd+FOV/2
        //Gizmos.DrawFrustum(this.transform.forward, GuardFOV/10f, closeEnoughSenseCutoff, 0.5f, 10f);
        Vector3[] pointsArray = new Vector3[20];
        float dAlpha=GuardFOV/pointsArray.Length;
        Vector3 fwdWorldSpace = this.transform.TransformDirection(this.transform.forward);


        for(int i = 0; i < pointsArray.Length/4; i++)
        {
            float dAlphaPlus = dAlpha * i * Mathf.Deg2Rad;
            float dAlphaMinus = -dAlphaPlus;
            Vector3 target = new Vector3(Mathf.Cos(dAlphaPlus), 0, Mathf.Sin(dAlphaPlus));
            Vector3 v = Vector3.RotateTowards(fwdWorldSpace, target, dAlphaPlus, 10);
            
            pointsArray[2 * i] += this.transform.position; //P0
            pointsArray[2 * i + 1] = this.transform.position + v * 10;        
        }

        for (int i = pointsArray.Length /4; i < pointsArray.Length / 2; i++)
        {
            float dAlphaPlus = dAlpha * (i - pointsArray.Length / 4) * Mathf.Deg2Rad;
            float dAlphaMinus = -dAlphaPlus;
            Vector3 target = new Vector3(Mathf.Cos(dAlphaMinus), 0, Mathf.Sin(dAlphaMinus));
            Vector3 v = Vector3.RotateTowards(fwdWorldSpace, target, dAlphaMinus, 10);

            pointsArray[2 * i] += this.transform.position; //P0
            pointsArray[2 * i + 1] = this.transform.position + v * 10;
        }



        pointsArray[0] = this.transform.position;
        pointsArray[1] = this.transform.position + fwdWorldSpace * 10;
        ReadOnlySpan<Vector3> points = new ReadOnlySpan<Vector3>(pointsArray);

        Gizmos.DrawLineList(points);
    }
}

