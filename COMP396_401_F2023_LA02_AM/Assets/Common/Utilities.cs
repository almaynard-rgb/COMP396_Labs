using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities
{
    //NOTE: MyMin function is based off of the WEEK05 in class MyMax function
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


    public static float[] MySortAscending(float[] xs)
    {
        //in ascending order
        float[] result = new float[xs.Length];
        //Naive sorting
        //x0 x1 ... xn-1
        //start with x0,
        //compare each other value z1, ... xn-1 with x0; if it is less, swap
        //goto x1

        for (int i = 0; i < xs.Length; i++)
        {
            for (int j = i; j < xs.Length; j++)
            {
                if (xs[i] > xs[j])
                {
                    //swap xi with xj
                    float temp = xs[i];
                    xs[i] = xs[j];
                    xs[j] = temp;
                }
            }
        }


        //analysis:
        //let n= xs.Length
        //Then is can be proved that is an O(n^2) algorithm, in the worse case
        //Proof:
        //Outer cycle runs n - 1 times
        //inner cycle n -1 then n-2, n-3.., n-(n-1) = 1 
        //(n-1) + (n-2) + ... + 1 = (n-1) *(n)/2 = n^2/2-n/2
        // f(n) = n^2/2-n/2
        // What is O(f(n)) = O(n^2)

        //n/2, n/2 elements are not sorted O(n^/4) = O(n^2)

        return result;
    }


    //Different alternative function for the Max called MyMax done in class
    public static float MyMax(float[] xs)
    {
        if(xs.Length == 0)
        {
            throw new System.Exception("array has no elements");
        }
        else if (xs.Length == 1) 
        {
            return xs[0];
        }

        //xs.Length>1
        float res = xs[0];
        for(int i = 0; i < xs.Length; i++)
        {
            if (xs[i] > res)
            {
                res = xs[i];
            }
        }
        return res;
    }


    //Different alternative function for the Min called MyMin
    public static float MyMin(float[] xs)
    {
        //if there are no array items throw exception
        if (xs.Length == 0)
        {
            throw new System.Exception("array has no elements");
        }
        //if there is only one item in array, return that item as min
        else if (xs.Length == 1)
        {
            return xs[0];
        }

       //initialize the smallest value as the first item in the array to start
        float res = xs[0];
         //if the array xs has a xs.Length > 1
         //loop through the array
        for (int i = 0; i < xs.Length; i++)
        {
            //if (res) is bigger that the current array item in loop, res is assigned to that new smallest value 
            if (xs[i] < res)
            {
                res = xs[i];
            }
        }
        return res;
    }

}
