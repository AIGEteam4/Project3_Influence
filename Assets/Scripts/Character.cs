using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    public float moveSpeed;//Move speed for character

    // Use this for initialization
    void Start () {
        //Set up initial position for terrain
        Vector3 pos = transform.position;
        pos.y = Terrain.activeTerrain.SampleHeight(pos);
        transform.position = pos;
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 moveVec = Vector3.zero;//Vector representing any movements character needs to make

        //If user presses WASD, set x and z values of move vec for appropriate direction
        if (Input.GetKey(KeyCode.W))
        {
            moveVec.z = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            moveVec.z = -1;
        }

        if (Input.GetKey(KeyCode.D))
        {
            moveVec.x = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            moveVec.x = -1;
        }

        //If we're moving this frame, time to do some math to make steep terrain impassible
        if(moveVec.x != 0 || moveVec.z != 0)
        {
            moveVec = moveVec.normalized;

            //Send raycast down to terrain to get its normal where we're standing
            RaycastHit hit;
            Vector3 terrainNormal = Vector3.zero;

            if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit))
            {
                terrainNormal = 1.1f * hit.normal;
                terrainNormal.y = 0;
            }

            //If moving against the slope, apply the terrain normals to movement to slow it
            if(Mathf.Sign(moveVec.x) != Mathf.Sign(terrainNormal.x))
            {
                moveVec.x += terrainNormal.x;
            }
            if(Mathf.Sign(moveVec.z) != Mathf.Sign(terrainNormal.z))
            {
                moveVec.z += terrainNormal.z;
            }

            //Apply move speed to move vec
            moveVec *= moveSpeed * Time.deltaTime;

            Vector3 newPos = transform.position + moveVec;//Get final new pos
            newPos.y = Terrain.activeTerrain.SampleHeight(newPos);//Get y pos for terrain height

            transform.position = newPos;//Update character position
        }
    }
}
