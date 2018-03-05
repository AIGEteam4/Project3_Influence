using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarUnit : MonoBehaviour {

    public GameObject testSphere;

    AStarManager asMgr;

    Vector3 target;
    List<Vector3> path;

    int currentPathPoint;

	// Use this for initialization
	void Start () {
        asMgr = FindObjectOfType<AStarManager>();

        currentPathPoint = -1;

        RaycastHit hit;

        if (Physics.Raycast(new Vector3(transform.position.x, 500, transform.position.z), Vector3.down, out hit))
        {
            transform.position = new Vector3(transform.position.x, hit.point.y + 1, transform.position.z);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if(Input.GetMouseButtonDown(1))
        { 
            RaycastHit hit;

            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, (1<<9)))
            {
                GoTo(hit.point);
            }
        }

		if(currentPathPoint > 0)
        {
            Vector3 vecToPoint = path[currentPathPoint] - transform.position;

            if(vecToPoint.sqrMagnitude >= 2)
            {
                Move(vecToPoint);
            }
            else
            {
                --currentPathPoint;
            }
            
        }
        
        else if(currentPathPoint == 0)
        {
            Vector3 vecToTarget = target - transform.position;
            vecToTarget.y = 0;

            if(vecToTarget.sqrMagnitude >= 1)
            {
                Move(vecToTarget);
            }
            else
            {
                --currentPathPoint;
            }
        }
	}

    void Move(Vector3 moveVec)
    {
        moveVec.y = 0;
        moveVec = moveVec.normalized;

        Vector3 newPos = transform.position;

        RaycastHit hit;

        if(Physics.Raycast(new Vector3(transform.position.x + moveVec.x, 500, transform.position.z + moveVec.z), Vector3.down, out hit))
        {
            newPos.y = hit.point.y + 1;
        }

        Collider[] colls = Physics.OverlapSphere(transform.position + moveVec, 1.0f, 1<<8);

        for(int i = 0; i < colls.Length; ++i)
        {
            Vector3 vecFromColl = transform.position - colls[i].transform.position;
            moveVec += 3.0f * vecFromColl / vecFromColl.sqrMagnitude;
        }

        newPos += moveVec * 10.0f * Time.deltaTime;

        transform.position = newPos;
    }

    public void GoTo(Vector3 point)
    { 
        target = point;
        path = asMgr.GetPath(transform.position, target);

        currentPathPoint = path.Count - 1;
    }
}
