using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FlockUnitMode
{
    flock//Insert more modes here?
}

public class FlockUnit : MonoBehaviour {

    public FlockUnitMode mode;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        switch(mode)
        {
            case FlockUnitMode.flock:
                UpdateFlock();
                break;
        }
	}

    void UpdateFlock()
    {
        //Flocking stuff goes here
    }
}
