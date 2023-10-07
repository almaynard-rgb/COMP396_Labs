using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFuzzyFunctions : MonoBehaviour
{
    //Parameters
    [Header("Parameters")]

    [Tooltip("a and b parameters for LeftShoulder")]
    public float a_lsh = 0.1f, b_lsh = 0.4f;

    [Tooltip("a and b parameters for RightShoulder")]
    public float a_rsh = 0.3f, b_rsh = 0.7f;

    [Tooltip("a and b and c parameters for Triangular")]
    public float a_tri = 0.15f, b_tri = 0.45f, c_tri = 0.85f;

    [Tooltip("a, b, c and d parameters for Trapesodial")]
    public float a_tra = 0.2f, b_tra = 0.4f, c_tra = 0.55f, d_tra = 0.8f;

    [Tooltip("a parameters for Crisp")]
    public float a_crips = 0.35f;

    [Tooltip("dx is interval between points x-s of points")]
    public float dx = 0.05f;

    [Header("Calculated Quantities")]
    [Tooltip("Number of Points")]
    public int numberOfPoints = System.Convert.ToInt32(1f/0.05f) + 1;

    //
    public float[] xs;
    public float[] ys_lsh;
    public float[] ys_rsh;
    public float[] ys_tri;
    public float[] ys_tra;
    public float[] ys_crisp;
    public float[] ys_s_curve;

    // Start is called before the first frame update
    void Start()
    {
        xs = new float[numberOfPoints];
        ys_lsh = new float[numberOfPoints];
        ys_rsh = new float[numberOfPoints];
        ys_tri = new float[numberOfPoints];
        ys_tra = new float[numberOfPoints];
        ys_crisp = new float[numberOfPoints];
        ys_s_curve = new float[numberOfPoints];



        //for left-shoulder
        LineRenderer lr_lsh = GameObject.Find("LSH_Visual").GetComponent<LineRenderer>();
        lr_lsh.positionCount = numberOfPoints;
        for (int i = 0; i < numberOfPoints; i++)
        {
            xs[i] = i*dx;
            ys_lsh[i] = FuzzyFunctions.LeftShoulder(xs[i], a_lsh, b_lsh);
            print($"x={xs[i]}, y={ys_lsh[i]}");
            //For visualizing later you could use LineTrailer comp.
        
            lr_lsh.SetPosition(i, new Vector3(xs[i], ys_lsh[i], 0));
        }


        //test for for right-shoulder (same as left-shoulder but calls right hands values instead)
        LineRenderer lr_rsh = GameObject.Find("RSH_Visual").GetComponent<LineRenderer>();
        lr_rsh.positionCount = numberOfPoints;
        for (int i = 0; i < numberOfPoints; i++)
        {
            xs[i] = i * dx;
            //call to RightShoulder method in FuzzyFunctions
            ys_rsh[i] = FuzzyFunctions.RightShoulder(xs[i], a_rsh, b_rsh);
            print($"x={xs[i]}, y={ys_rsh[i]}");
            //For visualizing later you could use LineTrailer comp.

            lr_rsh.SetPosition(i, new Vector3(xs[i], ys_rsh[i], 0));
        }




        //test for for triangular
        LineRenderer lr_tri = GameObject.Find("TRI_Visual").GetComponent<LineRenderer>();
        lr_tri.positionCount = numberOfPoints;
        for (int i = 0; i < numberOfPoints; i++)
        {
            xs[i] = i * dx;
            //call to triangular method in FuzzyFunctions
            ys_tri[i] = FuzzyFunctions.Triangular(xs[i], a_tri, b_tri, c_tri);
            print($"x={xs[i]}, y={ys_tri[i]}");
            //For visualizing later you could use LineTrailer comp.

            lr_tri.SetPosition(i, new Vector3(xs[i], ys_tri[i], 0));
        }



        //test for for trapesoidal
        LineRenderer lr_tra = GameObject.Find("TRA_Visual").GetComponent<LineRenderer>();
        lr_tra.positionCount = numberOfPoints;
        for (int i = 0; i < numberOfPoints; i++)
        {
            xs[i] = i * dx;
            //call to trapesoidal method in FuzzyFunctions
            ys_tra[i] = FuzzyFunctions.Trapesoidal(xs[i], a_tra, b_tra, c_tra, d_tra);
            print($"x={xs[i]}, y={ys_tra[i]}");
            //For visualizing later you could use LineTrailer comp.

            lr_tra.SetPosition(i, new Vector3(xs[i], ys_tra[i], 0));
        }


    }

    // Update is called once per frame
    void Update()
    {
    }
}
