using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour {

    //list of all units in the flock
    public List<GameObject> flockers = new List<GameObject>();

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Calculates the flock's average direction
    /// </summary>
    /// <returns>The direction.</returns>
    public Vector3 AverageDirection()
    {
        Vector3 avgForward = Vector3.zero;

        foreach (GameObject flocker in flockers)
        {
            //add up the forward vectors
            avgForward += flocker.transform.forward;
        }

        //return the sum
        return avgForward;
    }

    /// <summary>
    /// Calculates the flock's average position
    /// </summary>
    /// <returns>The position.</returns>
    public Vector3 AveragePosition()
    {
        Vector3 avgPos = Vector3.zero;

        foreach (GameObject flocker in flockers)
        {
            avgPos += flocker.transform.position;
        }

        //divide by the number of flockers
        avgPos /= flockers.Count;

        return avgPos;
    }
}
