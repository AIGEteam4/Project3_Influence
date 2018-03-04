using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node : System.Object
{
    //Node attributes
    public Vector3 position;    //position of the ndoe
    public float radius;        //Radius of the node

    /// <summary>
    /// Default constructor
    /// </summary>
    public Node()
    {
        position = Vector3.zero;
        radius = 10.0f;
    }

    public Node(Vector3 pos, float rad)
    {
        position = pos;
        radius = rad;
    }
}
