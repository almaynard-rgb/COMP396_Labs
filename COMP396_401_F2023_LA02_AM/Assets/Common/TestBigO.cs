using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System.Linq;

public class TestBigO : MonoBehaviour
{
    public int NumberOfElements = 1000;
    public int MaxandMinNumberOfElements = 10000000;
    public float[] xs;
    public float[] xs_sorted;

    
    //variables for max
    public float maxVal;
    public float[] maxArray;



    //variables for min
    public float minVal;
    public float[] minArray;


    // Start is called before the first frame update
    void Start()
    {
        xs = new float[NumberOfElements];
        xs_sorted = new float[NumberOfElements];
        ArrayList xs_al = new ArrayList();
        //TimeSpan timeSpan = new TimeSpan(0);
        //Populate first xs
        //Populate xs_al
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        print($"Start populating: n={NumberOfElements}: " + Time.time);
        for (int i = 0; i < xs.Length; i++)
        {
            xs[i] = UnityEngine.Random.value;
            xs_al.Add(xs[i]);
        }
        long populatingMS = stopwatch.ElapsedMilliseconds;
        print($"populatingMS= {populatingMS}");


        //Then sort with ArrayList.Sort

        xs_al.Sort();
        print($"Start ArrayList.Sort");
        //Then measure how long it took for sorting
        long al_sortingMS = stopwatch.ElapsedMilliseconds - populatingMS;
        print($"al_sortingMS = {al_sortingMS}");


        //Sort with MySortAscending
        //should be from utilitites
        print($"Start Utilities.MySortAcsending");
        xs_sorted = Utilities.MySortAscending(xs);
        print($"End of sorting");
        long MySortAscendingMS = stopwatch.ElapsedMilliseconds - al_sortingMS;
        print($"MySortAscendingMS = {MySortAscendingMS} ");








        //Comparing Max and MyMax
        //initialize maxArray with size of 1000
        maxArray = new float[MaxandMinNumberOfElements];
        List<float> maxList = new List<float>();

        //creating new stopwatch to time MyMax and Max 
        Stopwatch stopwatch1 = new Stopwatch();
        stopwatch1.Start();
        print($"Start populating: maxArrayList={MaxandMinNumberOfElements}: ");

        //populating maxArrayList
        for (int i = 0; i < maxArray.Length; i++)
        {
            //assigning random value for each item in maxArray
            maxArray[i] = UnityEngine.Random.value;
            //adding those random values into maxArray
            maxList.Add(maxArray[i]);
        }
        long populateMax = stopwatch1.ElapsedMilliseconds;
        print($"Done populating maxList= {populateMax}");


        //Find max with MyMax
        print($"Start Max()");
        maxVal = maxList.Max();


        //find time it took.
        long timeMax = stopwatch1.ElapsedMilliseconds - populateMax;
        print($"Time for Max() = {timeMax}");

        maxVal = 0;

        //find max with myMax
        //From utilitites
        print($"Start Utilities.MyMax");
        maxVal = Utilities.MyMax(maxArray);

        long myMaxTime = stopwatch1.ElapsedMilliseconds - timeMax;
        print($"MyMax() time = {myMaxTime} ");





        //Comparing Min and MyMin
        //initialize minArray with size of 1000
        minArray = new float[MaxandMinNumberOfElements];
        List<float> minList = new List<float>();

        //creating new stopwatch to time MyMax and Max 
        Stopwatch stopwatch2 = new Stopwatch();
        stopwatch2.Start();
        print($"Start populating: minList={MaxandMinNumberOfElements}: ");

        //populating maxArrayList
        for (int i = 0; i < minArray.Length; i++)
        {
            //assigning random value for each item in maxArray
            minArray[i] = UnityEngine.Random.value;
            //adding those random values into maxArray
            minList.Add(maxArray[i]);
        }
        long populateMin = stopwatch2.ElapsedMilliseconds;
        print($"Done populating minList= {populateMin}");


        //Find max with MyMax
        print($"Start Min()");
        minVal = minList.Min();


        //find time it took.
        long timeMin = stopwatch2.ElapsedMilliseconds - populateMin;
        print($"Min() time = {timeMin}");

        maxVal = 0;

        //find max with myMax
        //From utilitites
        print($"Start Utilities.MyMin");
        minVal = Utilities.MyMin(minArray);

        long myMinTime = stopwatch2.ElapsedMilliseconds - timeMin;
        print($"MyMin() time = {myMinTime} ");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
