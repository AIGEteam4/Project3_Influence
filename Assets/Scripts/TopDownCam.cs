using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCam : MonoBehaviour {

    private float speed;
    private Camera cam;

    // Use this for initialization
    void Start()
    {
        gameObject.transform.position = new Vector3(0, 200, 0);
        gameObject.transform.eulerAngles = new Vector3(90, 0, 0);
        speed = 2;
        cam = gameObject.GetComponent<Camera>();
        cam.orthographicSize = 100;
    }
	
	// Update is called once per frame
	void Update () {
        CamMove();
	}

    void CamMove()
    {
        //move up
        if(Input.GetKey(KeyCode.W))
        {
            Vector3 move = gameObject.transform.position;
            move.z += speed;
            gameObject.transform.position = move;
        }
        //move down
        if (Input.GetKey(KeyCode.S))
        {
            Vector3 move = gameObject.transform.position;
            move.z -= speed;
            gameObject.transform.position = move;
        }
        //move right
        if (Input.GetKey(KeyCode.D))
        {
            Vector3 move = gameObject.transform.position;
            move.x += speed;
            gameObject.transform.position = move;
        }
        //move left
        if (Input.GetKey(KeyCode.A))
        {
            Vector3 move = gameObject.transform.position;
            move.x -= speed;
            gameObject.transform.position = move;
        }

        //zoom
        if (Input.GetKey(KeyCode.Q) && cam.orthographicSize > 10)
        {
            cam.orthographicSize -= speed * 2;
        }
        if (Input.GetKey(KeyCode.E) && cam.orthographicSize < 150)
        {
            cam.orthographicSize += speed * 2;
        }
    }
}
