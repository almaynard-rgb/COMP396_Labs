using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using static GuardFSM;

public class GuardController : MonoBehaviour
{

    //***ALL FROM GUARDFSM***
    public GameObject enemy;
    public float GuardFOV = 89; // degrees
    private float cosGuardFOVOver2InRAD;

    public float closeEnoughAttackCutoff = 2; // if distance of guard to enemy <= 2m  close enough to Attack
    public float closeEnoughSenseCutoff = 15; // if distance of guard to enemy <= 15m  close enough to start chasing



    public float strength = 90; //[0, 100]

    public float speed = 2; //2 meters per second


    public Transform[] waypoints;
    public int nextWaypointindex = 0;








    public StateMachine stateMachine;

    public StateMachine.State patrol, chase, attack, runAway, death;
    // Start is called before the first frame update
    void Start()
    {
        stateMachine = new StateMachine();
        cosGuardFOVOver2InRAD = Mathf.Cos(GuardFOV / 2f * Mathf.Deg2Rad); // in radians


        //Use factory pattern
        //StateMachine.State patrol = new StateMachine.State();
        patrol = stateMachine.CreateState("Patrol");
        patrol.onEnter = delegate { Debug.Log("Patrol.onEnter"); };
        patrol.onExit = delegate { Debug.Log("Patrol.onExit"); };
        patrol.onFrame = PatrolOnFrame;

        chase = stateMachine.CreateState("Chase");
        chase.onEnter = delegate { Debug.Log("Chase.onEnter"); };
        chase.onExit = delegate { Debug.Log("Chase.onExit"); };
        chase.onFrame = ChaseOnFrame;


        attack = stateMachine.CreateState("Attack");
        attack.onEnter = delegate { Debug.Log("Attack.onEnter"); };
        attack.onExit = delegate { Debug.Log("Attack.onExit"); };
        attack.onFrame = AttackOnFrame;

        runAway = stateMachine.CreateState("RunAway");
        runAway.onEnter = delegate { Debug.Log("RunAway.onEnter"); };
        runAway.onExit = delegate { Debug.Log("RunAway.onExit"); };
        runAway.onFrame = RunAwayOnFrame;


        //State transition content added by me Alexander Maynard (ID: 301170707)
        death = stateMachine.CreateState("Death");
        death.onEnter = delegate { Debug.Log("Death.onEnter"); };
        death.onExit = delegate { Debug.Log("Death.onExit"); };
        death.onFrame = DeathOnFrame;
    }



    //DeathOnFrame added by me Alexander Maynard (ID: 301170707)
    void DeathOnFrame()
    {
        Debug.Log("Death.onFrame");

        //Default death action calls the Death() method
        Death();


        //NOTE: death ends all other states. Therefore there needs to be no other states after the death state
    }




    void PatrolOnFrame()
    {
        Debug.Log("Patrol.onFrame");

        //DEFAULT actions during patrol state
        Patrol();

        //CHECK TRANSITION CONDITIONS
        //T1 - SensePlayer/Enemy (from draw.io file for GuardFSM)
        if (Utilities.SenseEnemy(this.transform.position, enemy.transform.position, 
            this.transform.forward, cosGuardFOVOver2InRAD, closeEnoughSenseCutoff))
        {
            stateMachine.ChangeState(chase);
        }

        //Check T3 ThreatenedAndWeakerThanEnemy  (from draw.io file for GuardFSM)
        if (ThreatenedAndWeakerThanEnemy())
        {
            stateMachine.ChangeState(runAway);
        }


        //if condition added by Alexander Maynard(301170707)
        //death can occur in any state...
        if (strength <= 0)
        {
            stateMachine.ChangeState(death);
        }
    }


    void ChaseOnFrame()
    {
        Debug.Log("Chase.onFrame");

        //default ACTIONS during chase state
        Chase();

        //CHECK TRANSITION CONDITIONS

        //T2 Within range

        if (WithinRangeAndStrongerThanEnemy())
        {
            stateMachine.ChangeState(attack);
        }


        //if condition added by Alexander Maynard(301170707)
        //death can occur in any state...
        if (strength <= 0)
        {
            stateMachine.ChangeState(death);
        }
    }


    void AttackOnFrame()
    {
        Debug.Log("Attack.onFrame");


        //default ACTIONS during Attack state

        //CHECK TRANSITION CONDITIONS
        if(ThreatenedAndWeakerThanEnemy())
        {
            stateMachine.ChangeState(runAway);
        }

        if (!Utilities.SenseEnemy(this.transform.position, enemy.transform.position,
            this.transform.forward, cosGuardFOVOver2InRAD, closeEnoughSenseCutoff))
        {
            stateMachine.ChangeState(patrol);
        }

        if(WeakerThanEnemy())
        {
            stateMachine.ChangeState(runAway);
        }


        //if condition added by Alexander Maynard(301170707)
        //death can occur in any state...
        if (strength <= 0)
        {
            stateMachine.ChangeState(death);
        }
    }


    void RunAwayOnFrame()
    {
        Debug.Log("RunAway.onFrame");

        RunAway();


        //CHECK TRANSITION CONDITIONS

        //T4 if safe
        if (Safe())
        {
            stateMachine.ChangeState(patrol);
        }


        //if condition added by Alexander Maynard(301170707)
        //death can occur in any state...
        if (strength <= 0)
        {
            stateMachine.ChangeState(death);
        }
    }



    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
    }

    private void Patrol()
    {
        if (Vector3.Distance(this.transform.position, waypoints[nextWaypointindex].transform.position) < float.Epsilon)
        {
            nextWaypointindex = (nextWaypointindex + 1) % waypoints.Length;
        }
        Vector3 target = waypoints[nextWaypointindex].transform.position;
        Vector3 movement = Vector3.MoveTowards(this.transform.position, target, speed * Time.deltaTime);
        //movement.y = 0.5f;
        this.transform.position = movement;
    }

    private void Chase()
    {
        //E.heading = {E - G}.

        Vector3 enemyHeading = (enemy.transform.position - this.transform.position);
        float enemyDistance = enemyHeading.magnitude;
        enemyHeading.Normalize();

        //rb.velocity = enemyHeading*speed;
        //
        Vector3 movement = enemyHeading * speed * Time.deltaTime; //m/s *s/frame=meters/frame
        Vector2.ClampMagnitude(movement, enemyDistance);
        this.transform.position += movement;
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
        //////////////////
        return EnemyCloseEnough(closeEnoughSenseCutoff);
    }


    private bool WeakerThanEnemy()
    {
        PlayerController enemyController = enemy.GetComponent<PlayerController>();

        if (strength < enemyController.strength)
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

        if (Vector3.Distance(this.transform.position, enemy.transform.position) <= distance)
        {
            return true;
        }
        else
        {
            return false;
        }
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


    private bool WithinRange()
    {
        return EnemyCloseEnough(closeEnoughAttackCutoff);
    }


    private bool Safe()
    {
        return !Threatened();
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


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        for (int i = 0; i < waypoints.Length; i++)
        {
            int i1 = (i + 1) % waypoints.Length;
            Gizmos.DrawLine(waypoints[i].transform.position, waypoints[i1].transform.position);
        }

        Gizmos.color = Color.blue;
        //want to see cone from fwd-FOV/2 to fwd+FOV/2
        //Gizmos.DrawFrustum(this.transform.forward, GuardFOV/10f, closeEnoughSenseCutoff, 0.5f, 10f);
        Vector3[] pointsArray = new Vector3[20];
        float dAlpha = GuardFOV / pointsArray.Length;
        Vector3 fwdWorldSpace = this.transform.TransformDirection(this.transform.forward);


        for (int i = 0; i < pointsArray.Length / 4; i++)
        {
            float dAlphaPlus = dAlpha * i * Mathf.Deg2Rad;
            float dAlphaMinus = -dAlphaPlus;
            Vector3 target = new Vector3(Mathf.Cos(dAlphaPlus), 0, Mathf.Sin(dAlphaPlus));
            Vector3 v = Vector3.RotateTowards(fwdWorldSpace, target, dAlphaPlus, 10);

            pointsArray[2 * i] += this.transform.position; //P0
            pointsArray[2 * i + 1] = this.transform.position + v * 10;
        }

        for (int i = pointsArray.Length / 4; i < pointsArray.Length / 2; i++)
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
