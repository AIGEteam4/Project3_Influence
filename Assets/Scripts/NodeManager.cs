using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    public Vector3 defaultNodePosition;
    public float defaultNodeRadius;
    public Node[] nodes;

    //used for fixed path
    int currentNode;
    int nodeCount;

    //used for random path
    Node randomNode;

    //Terrain
    Terrain terrain;
    TerrainData tData;

    //What type of nodes to use
    enum PathType
    {
        Random,
        Fixed
    }
    PathType pType;

    //Debug variables
    bool useDebug;
    float debugTime;
    //Debug object
    GameObject debugSphere;

    //Property for the current node
    public int CurrentNode
    {
        get { return currentNode; } //Getter
        set                         //Setter:  Keeps the current node within the boundaries of the nodes array length
                                    //In other words, wraps the current node around the array if it goes past an end
        {
            currentNode = value;

            if(currentNode >= nodeCount)
            {
                int newNode = -1;

                while(currentNode >= nodeCount)
                {
                    currentNode--;
                    newNode++;
                }

                currentNode = newNode;
            }
            else if(currentNode < 0)
            {
                int newNode = nodeCount;

                while (currentNode < 0)
                {
                    currentNode++;
                    newNode--;
                }

                currentNode = newNode;
            }
        }
    }

    // Use this for initialization
    void Start ()
    {
        currentNode = 0;
        pType = PathType.Random;

        useDebug = true;
        debugTime = 0.0f;

        nodeCount = nodes.Length;

        //Add a basic node if no nodes exist
        if(nodeCount == 0)
        {
            nodeCount = 1;
            nodes = new Node[1];

            nodes[0].position = defaultNodePosition;
        }

        terrain = Terrain.activeTerrain;
        tData = terrain.terrainData;

        randomNode = GenerateRandomNode();

        //Set up the debug sphere
        debugSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        GameObject.Destroy(debugSphere.GetComponent<Collider>());
        debugSphere.transform.position = nodes[currentNode].position;
	}
	
	// Update is called once per frame
	void Update ()
    {	
        if(pType == PathType.Fixed)
        {


            //Debug mode
            if (useDebug)
            {
                debugTime += Time.deltaTime;

                if (debugTime >= 4.0f)
                {
                    currentNode++;
                    debugTime = 0.0f;

                    if (currentNode >= nodes.Length)
                    {
                        currentNode = 0;
                    }

                    debugSphere.transform.position = nodes[currentNode].position;
                }
            }
        }
        else if(pType == PathType.Random)
        {

            //Debug mode
            if (useDebug)
            {
                debugTime += Time.deltaTime;

                if (debugTime >= 4.0f)
                {
                    debugTime = 0.0f;

                    randomNode = GenerateRandomNode();

                    debugSphere.transform.position = randomNode.position;
                }
            }
        }

        
	}

    private void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            ShowDebug(!useDebug);
        }
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            pType = PathType.Fixed;
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            pType = PathType.Random;
        }
    }

    //Returns a randomly generated node
    Node GenerateRandomNode()
    {
        Vector3 tSize = tData.size;
        Vector3 tPos = terrain.GetPosition();

        float xPos = Random.Range(-tSize.x / 2.0f, tSize.x / 2.0f);
        float zPos = Random.Range(-tSize.z / 2.0f, tSize.z / 2.0f);
        float yPos = terrain.SampleHeight(new Vector3(xPos, 0.0f, zPos)) + 2.0f;

        return new Node(new Vector3(xPos, yPos, zPos), defaultNodeRadius);
    }

    void ShowDebug(bool option)
    {
        useDebug = option;
        debugSphere.SetActive(option);
    }
}
