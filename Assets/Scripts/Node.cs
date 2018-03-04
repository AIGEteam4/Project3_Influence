using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node : System.Object
{
    public Vector3 position;
    public float radius;

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
