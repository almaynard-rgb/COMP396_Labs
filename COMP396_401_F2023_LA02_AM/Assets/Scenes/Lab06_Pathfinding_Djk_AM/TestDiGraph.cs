using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDiGraph : MonoBehaviour
{
    DiGraph myGraph;
    // Start is called before the first frame update


    DiGraph CreateGraph1()
    {
        //'A' to 'D' is 4 in class diagram, we changed it to be more interesting
        myGraph.add_vertex_Dijkstra('A', new Dictionary<char, int>() { { 'B', 10 }, { 'C', 12 }, { 'D', 6 }, { 'E', 2 } });
        myGraph.add_vertex_Dijkstra('B', new Dictionary<char, int>() { { 'C', 2 }, { 'D', 4 }, { 'F', 5 } });
        myGraph.add_vertex_Dijkstra('C', new Dictionary<char, int>() { { 'B', 6 }, { 'F', 2 } });
        myGraph.add_vertex_Dijkstra('D', new Dictionary<char, int>() { { 'B', 3 }, { 'E', 3 } });
        //'E' to 'F' is 9 in class diagram, we changed it to be more interesting
        myGraph.add_vertex_Dijkstra('E', new Dictionary<char, int>() { { 'D', 3 }, { 'F', 11 } });
        myGraph.add_vertex_Dijkstra('F', new Dictionary<char, int>() { });

        return myGraph;
    }


    DiGraph CreateGraph2()
    {
        myGraph = new DiGraph();
        //TODO: create the graph in: https://en.wiklipedia.org/wiki
        //Note; this is not a digraph (directed graph)
        //To make it diagraph, you have to add both edges
        return myGraph;
    }


    void Start()
    {
        myGraph = CreateGraph1();


        char[] neighbors = myGraph.GetNeighbors('A');
        print("Neighbors of A: " + neighbors);
        myGraph.printArray<char>(neighbors);

        List<char> path = myGraph.Find_Shortest_Path_via_Dijkstra_Algo('A', 'F');
        myGraph.printArray(path.ToArray());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
