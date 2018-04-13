using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    //Basic attributes
    public Material color;
    public int strength;

    public UnitManager.Team team;

    //Constructor
    public Unit(Material mat, int str, UnitManager.Team tm)
    {
        color = mat;
        strength = str;
        team = tm;
    }

	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
