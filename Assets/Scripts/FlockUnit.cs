using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FlockUnitMode
{
    flock//Insert more modes here?
}

public class FlockUnit : MonoBehaviour {

    public FlockUnitMode mode;

    public List<GameObject> neighbors = new List<GameObject>(); //list of all other units in flock
    public FlockManager fM; //reference to FlockManager script
    public GameObject target; //object to seek for movement test

    //attributes (public for debugging purposes)
    public Vector3 position;
    public Vector3 direction;
    public Vector3 velocity;
    public Vector3 acceleration;
    public float mass;
    public float maxSpeed;

    public float alignWeight;
    public float cohesionWeight;
    public float separationWeight;
    public float seperationRange;
    public float seekWeight;

    public float height; //height off ground

    // Use this for initialization
    void Start () {
        //initialize
        fM = GameObject.Find("GameManager").GetComponent<FlockManager>(); //gain access to FlockManager
        GetNeighbors(); //build the neighbors list for each unit
        position = transform.position; //starting position is equal to placement in scene
        target = GameObject.Find("Target"); //test target
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
        //Move the flock
        CalcSteeringForces();
        UpdatePosition();
    }



    /// <summary>
    /// Builds the neighbors list
    /// </summary>
    void GetNeighbors()
    {
        foreach (GameObject flock in fM.flockers)
        {
            if (flock != gameObject) //every GameObject except this one
            {
                neighbors.Add(flock);
            }
        }
    }
    
    //steering force calculations
    /// <summary>
    /// Seek the specified targetPosition.
    /// </summary>
    /// <param name="targetPosition">Target position.</param>
    public Vector3 Seek(Vector3 targetPosition)
    {
        //calculate a desired velocity from this to target's position
        Vector3 desiredVelocity = targetPosition - position;

        //Scale to the maximum speed
       desiredVelocity = Vector3.ClampMagnitude(desiredVelocity, maxSpeed);

        //calculate the steering force
        //steeringForce = desired - current
        Vector3 steeringForce = desiredVelocity - velocity;

        return steeringForce;
    }

    /// <summary>
    /// Flee the specified targetPosition.
    /// </summary>
    /// <param name="targetPosition">Target position.</param>
    public Vector3 Flee(Vector3 targetPosition)
    {
        //calculate a desired velocity from this to target's position
        Vector3 desiredVelocity = position - targetPosition;

        //Scale to the maximum speed
        desiredVelocity = Vector3.ClampMagnitude(desiredVelocity, maxSpeed);

        //calculate the steering force
        //steeringForce = desired - current
        Vector3 steeringForce = desiredVelocity - velocity;

        return steeringForce;
    }

    /// <summary>
	/// Aligns the flock to the same direction
	/// </summary>
	/// <param name="average">Average.</param>
	public Vector3 Alignment(Vector3 average)
    {
        //normalize the average vector
        average.Normalize();

        //multiply by max speed
        average *= maxSpeed;

        //desired - current
        Vector3 steeringForce = average - velocity;

        return steeringForce;
    }

    /// <summary>
    /// Keeps the flock together
    /// </summary>
    /// <param name="average">Average.</param>
    public Vector3 Cohesion(Vector3 average)
    {
        Vector3 steeringForce = Seek(average);

        return steeringForce;
    }

    /// <summary>
    /// Keep the flocking units from clustering too tightly
    /// </summary>
    /// <param name="neighbor">Other Unit in Flock</param>
    public Vector3 Separation(GameObject neighbor)
    {
        Vector3 steeringForce = Vector3.zero;

        //check distance from current
        float distance = Vector3.Magnitude(position - neighbor.transform.position);

        Debug.DrawLine(gameObject.transform.position, neighbor.transform.position, Color.cyan);

        //if within a certain range flee
        if (distance < seperationRange)
        {
            steeringForce = Flee(neighbor.transform.position / distance);
        }
        return steeringForce;

    }

    //applying movement to unit
    /// <summary>
    /// Calculates the forces that steer unit's direction
    /// </summary>
    public void CalcSteeringForces()
    {
        //Create a new ultimate force that's zeroed
        Vector3 ultForce = Vector3.zero;

        //Move towards the given target
        ultForce += Seek(target.transform.position) * seekWeight;

        //flocking
        //align
        ultForce += Alignment(fM.AverageDirection()) * alignWeight;

        //cohere
        ultForce += Cohesion(fM.AveragePosition()) * cohesionWeight;

        //separate
        foreach (GameObject flock in neighbors)
        {
            ultForce += Separation(flock) * separationWeight;
        }

        //apply the force to the units acceleration
        acceleration += ultForce / mass;
    }

    /// <summary>
	/// Updates the position of the unit (does the actual moving).
	/// </summary>
	void UpdatePosition()
    {
        //add accel to velocity
        velocity += acceleration * Time.deltaTime;

        //clamp velocity
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        //add velocity to position
        position += velocity * Time.deltaTime;

        //keep units at level with floor
        position.y = Terrain.activeTerrain.SampleHeight(position) + height;

        //zero out accel
        acceleration = Vector3.zero;

        gameObject.transform.position = position;

        //calculate direction from velocity
        //direction = velocity.normalized;

        //turn object toward direction
        //gameObject.transform.forward = direction;
    }

}
