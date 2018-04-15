using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCam : MonoBehaviour {

    private float speed;
    private Camera cam;

    //Vars to keep track when user clicks and drags on screen in free cam
    private Vector3 clickOrigin;//Origin pos of mouse on click
    private Vector3 dragCamPosOrigin;//Origin pos of camera on click
    private bool clickHeld;//Whether click is being held

    // Use this for initialization
    void Start()
    {
        gameObject.transform.position = new Vector3(0, 200, 0);
        gameObject.transform.eulerAngles = new Vector3(90, 0, 0);
        speed = 2;
        cam = gameObject.GetComponent<Camera>();
        
        //cam.orthographicSize = 100;
    }
	
	// Update is called once per frame
	void Update () {
        CamMove();
	}

    void CamMove()
    {
        //move up
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            Vector3 move = gameObject.transform.position;
            move.z += speed;
            gameObject.transform.position = move;
        }
        //move down
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            Vector3 move = gameObject.transform.position;
            move.z -= speed;
            gameObject.transform.position = move;
        }
        //move right
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            Vector3 move = gameObject.transform.position;
            move.x += speed;
            gameObject.transform.position = move;
        }
        //move left
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            Vector3 move = gameObject.transform.position;
            move.x -= speed;
            gameObject.transform.position = move;
        }

        //Pan camera on click and drag
        if (Input.GetMouseButton(0))
        {
            //If button just pressed, store initial pos of click and camera
            if (!clickHeld)
            {
                clickHeld = true;
                clickOrigin = cam.ScreenToViewportPoint(Input.mousePosition);//Convert mouse pos to viewport pos for easier use
                dragCamPosOrigin = transform.position;
            }
            //Move camera based on mouse movement
            else
            {
                //Difference calculated based on double the othographic size of cam so mouse movement maps to camera movement better
                Vector3 diff = 2 * cam.orthographicSize * (cam.ScreenToViewportPoint(Input.mousePosition) - clickOrigin);
                transform.position = dragCamPosOrigin - new Vector3(diff.x, 0, diff.y);
            }
        }
        //Reset when click released
        else if (Input.GetMouseButtonUp(0))
        {
            clickHeld = false;
        }


        //zoom
        //Use scroll wheel to zoom camera out/increase orthographic size
        if (Input.mouseScrollDelta.y != 0)
        {
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize + -Input.mouseScrollDelta.y * speed, 10f, 150f);
        }

        else if (Input.GetKey(KeyCode.Q) && cam.orthographicSize > 10)
        {
            cam.orthographicSize -= speed;
        }
        else if (Input.GetKey(KeyCode.E) && cam.orthographicSize < 150)
        {
            cam.orthographicSize += speed;
        }
    }
}
