using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    //Basic attributes
    public Material color;
    public int strength;
    Vector3 position;

    Transform trans;

    //Constructor
    public Unit(Material mat, int str, Vector3 pos)
    {
        color = mat;
        strength = str;
        position = pos;
    }

	// Use this for initialization
	void Start ()
    {
        trans = GetComponent<Transform>();

        trans.position = position;
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
