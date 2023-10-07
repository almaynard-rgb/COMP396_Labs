using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public static class FuzzyFunctions
{

    // NOTE: Formulas for L-SH, R-SH, TRI, TRA are all from the WEEK04 in class excel sheet
    public static float LeftShoulder(float x, float a, float b)
    {
        float result = 0;
        //Guards
        //  0<=x<=1
        if (x < 0 || x > 1)
        {
            throw new System.Exception($"x={x}: It should be in [0,1]");
        }
        //params 0 <= a < b <= 1
        if (!(0 <= a && a < b && b <= 1))
        {
            throw new System.Exception($"a={a}, b={b}: They should obey 0 <= a < b <= 1");
        }

        if (x <= a)
        {
            result = 1;
        }
        else if (x <= b)
        {
            result = 1 + ((0 - 1) / (b - a)) * (x - a);
        }
        return result;
    }



    public static float RightShoulder(float x, float a, float b)
    {
        float result = 1;
        //Guards
        //  0<=x<=1
        if (x < 0 || x > 1)
        {
            throw new System.Exception($"x={x}: It should be in [0,1]");
        }
        //params 0 <= a < b <= 1
        if (!(0 <= a && a < b && b <= 1))
        {
            throw new System.Exception($"a={a}, b={b}: They should obey 0 <= a < b <= 1");
        }

        //right-shoudler evaluations
        if (x <= a)
        {
            result = 0;
        }

        else if (x <= b)
        {
            result = ((1) / (b - a)) * (x - a);
        }
        return result;
    }



    //Triangular and test

    public static float Triangular(float x, float a, float b, float c)
    {
        float result = 0;
        //Guards
        //  0<=x<=1
        if (x < 0 || x > 1)
        {
            throw new System.Exception($"x={x}: It should be in [0,1]");
        }
        //params 0 <= a < b <= 1
        if (!(0 <= a && a < b && b <= 1))
        {
            throw new System.Exception($"a={a}, b={b}: They should obey 0 <= a < b <= 1");
        }
        //triangluar evaluations
        if (x <= a)
        {
            result = 0;
        }
        else if (x <= b)
        {
            result = ((1) / (b - a)) * (x - a);
        }
        else if (x <= c)
        {
            result = 1 + ((0 - 1) / (c - b) * (x - b));
        }

        return result;
    }






    //Trapesoidal and test
    public static float Trapesoidal(float x, float a, float b, float c, float d)
    {
        float result = 0;
        //Guards
        //  0<=x<=1
        if (x < 0 || x > 1)
        {
            throw new System.Exception($"x={x}: It should be in [0,1]");
        }
        //params 0 <= a < b <= 1
        if (!(0 <= a && a < b && b <= 1))
        {
            throw new System.Exception($"a={a}, b={b}: They should obey 0 <= a < b <= 1");
        }

        Debug.Log("");

        //trapesoidal evaluations
        if (x <= a)
        {
            result = 0;
        }
        else if (x <= b)
        {
            result = ((1) / (b - a)) * (x - a);
        }
        else if (x <= c)
        {
            result = 1;
        }
        else if (x <= d)
        {
            result = 1 + ((0 - 1) / (d - c) * (x - c));
        }

        return result;
    }





    //optional crisp and s_curve

    //...


    //CaseN: Smells the enemy

    //implement minimum in utilities --> test in bigo
}
