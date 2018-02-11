using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    public float moveSpeed;//Move speed for character

    private Rigidbody rb;

    // Use this for initialization
    void Start () {
        //Set up initial position so it's flush w/ terrain
        Vector3 pos = transform.position;
        pos.y = Terrain.activeTerrain.SampleHeight(pos) + 1f;
        transform.position = pos;

        rb = GetComponent<Rigidbody>();//Get rigidbody for movement
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 moveVec = Vector3.zero;//Vector representing any movements character needs to make

        Vector3 newVel = rb.velocity;
        float yVel = newVel.y;
        newVel.y = 0;

        //If user presses WASD, modify velocity to accel in appropriate direction
        if (Input.GetKey(KeyCode.W))
        {
            newVel += Vector3.forward;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            newVel += Vector3.back;
        }

        if (Input.GetKey(KeyCode.D))
        {
            newVel += Vector3.right;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            newVel += Vector3.left;
        }

        newVel = Vector3.ClampMagnitude(newVel, moveSpeed);//Clamp velocity on xz
        newVel.y = yVel;//Maintain previous y velocity
        rb.velocity = newVel;//Update rigidbody velocity
    }
}
