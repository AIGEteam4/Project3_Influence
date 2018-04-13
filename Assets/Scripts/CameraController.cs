using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //Enum for camera modes - can follow character or be moved independently
    enum CameraMode
    {
        followCharacter,
        freeCam
    }

    private CameraMode mode;//Current mode

    public GameObject character;//Reference to character
    private Camera cam;//Reference to camera component for easier use

    //Vars to keep track when user clicks and drags on screen in free cam
    private Vector3 clickOrigin;//Origin pos of mouse on click
    private Vector3 dragCamPosOrigin;//Origin pos of camera on click
    private bool clickHeld;//Whether click is being held

    //Move speed for free cam
    public float moveSpeed;

    private float zoom;

    // Use this for initialization
    void Start()
    {
        mode = CameraMode.followCharacter;
        clickHeld = false;

        cam = GetComponent<Camera>();

        zoom = 100f;
        cam.orthographicSize = zoom;
    }

    // Update is called once per frame
    void Update()
    {
        //Move camera around with arrow keys so that character can still be independently controlled
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += Vector3.forward * moveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += Vector3.back * moveSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;
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
                Vector3 diff = 2 * zoom * (cam.ScreenToViewportPoint(Input.mousePosition) - clickOrigin);
                transform.position = dragCamPosOrigin - new Vector3(diff.x, 0, diff.y);
            }
        }
        //Reset when click released
        else if (Input.GetMouseButtonUp(0))
        {
            clickHeld = false;
        }

        //Switch mode if user presses 1
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            mode = CameraMode.followCharacter;
        }

        //Use scroll wheel to zoom camera out/increase orthographic size
        if (Input.mouseScrollDelta.y != 0)
        {
            zoom = Mathf.Clamp(zoom - Input.mouseScrollDelta.y, 10f, 200f);
            cam.orthographicSize = zoom;

            //transform.position = new Vector3(transform.position.x,zoom,transform.position.z);
        }
    }
}
