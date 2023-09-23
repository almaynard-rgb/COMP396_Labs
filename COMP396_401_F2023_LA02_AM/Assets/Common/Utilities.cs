using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities
{
    public static bool EnemyCloseEnough(Vector3 start, Vector3 enemyPosition, float distance)
    {

        if (Vector3.Distance(start, enemyPosition) <= distance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

        public static bool EnemyInFront(Vector3 start, Vector3 enemyPos, Vector3 thisForward, float cutOff)
        {
            //Angle(Guard.Fwd, EasingFunction.heading) < GuardFOV/2 => true, else false
            // <=> cos(Angle)>cos(Guardfov/2)
            //E.heading = {E - G}.
            Vector3 enemyHeading = (enemyPos - start).normalized;

            //if(Vector3.Angle(enemyHeading, this.transform.forward))
            //{
            //return true;
            //}
            float cosAngle = Vector3.Dot(enemyHeading, thisForward);
            if (cosAngle > cutOff)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


    public static bool SenseEnemy(Vector3 start, Vector3 enemyPos, Vector3 thisForward, float cutOff, float distance)
    {

        //Case 1: Enemy in front and close enough
        if (EnemyInFront(start, enemyPos, thisForward, cutOff)
            && EnemyCloseEnough(start, enemyPos, distance))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
