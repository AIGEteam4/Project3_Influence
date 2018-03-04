using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    /********************
      Public attributes
    *********************/
    public Vector3 defaultNodePosition; //Default position used for creating a node
    public float defaultNodeRadius;     //Default radius used for creating a node
    public Node[] nodes;                //Array of nodes

    /********************
      Private attributes
    *********************/
    //Used for fixed path
    int currentFixedNode;
    int nodeCount;

    //Used for random path
    //Seperate to allow for nodes to be loosly saved
    Node randomNode;

    //The current Node
    Node currentNode;

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

    //Debug variables, currently enabled
    bool useDebug;
    float debugTime;
    //Debug object
    GameObject debugSphere;

    /********************
          Properties
    *********************/
    /// <summary>
    /// Allows getting or setting the current fixed node number.  NOTE: Does not change the current node.  Use NextNode instead.
    /// </summary>
    public int CurrentFixedNode
    {
        get { return currentFixedNode; } //Getter
        set                         //Setter:  Keeps the current node within the boundaries of the nodes array length
                                    //In other words, wraps the current node around the array if it goes past an end
        {
            //Temporarily assigns the the value provided
            //If the value is within the node range it will be set to that node
            //Otherwise checks will be done to wrap it
            currentFixedNode = value;

            //Checks if the value is higher than the amount of nodes
            if(currentFixedNode >= nodeCount)
            {
                //Temporary value to find the new current node
                int newNode = -1;

                //Loops until the currentFixedNode is equal to the nodeCount
                //This is to find the new node number
                while(currentFixedNode >= nodeCount)
                {
                    currentFixedNode--;
                    newNode++;
                }

                //Once the new node number is found assign it to the currentFixedNode
                currentFixedNode = newNode;
            }
            //Checks if the value is less below zero
            else if(currentFixedNode < 0)
            {
                //Temporary value to find the new current node
                int newNode = nodeCount;

                //Loops until the currentFixedNode is equal to 0
                //This is to find the new node number
                while (currentFixedNode < 0)
                {
                    currentFixedNode++;
                    newNode--;
                }

                //Once the new node number is found assign it to the currentFixedNode
                currentFixedNode = newNode;
            }
        }
    }

    /// <summary>
    /// Returns the current Node
    /// </summary>
    public Node CurrentNode
    {
        get { return currentNode; }
    }

    /********************
           Methods
    *********************/
    // Use this for initialization
    void Start ()
    {
        //Initialize attributes
        currentFixedNode = 0;
        pType = PathType.Fixed;

        useDebug = true;
        debugTime = 0.0f;

        nodeCount = nodes.Length;

        //Add a default node if no nodes exist
        if(nodeCount == 0)
        {
            nodeCount = 1;
            nodes = new Node[1];

            nodes[0] = new Node(defaultNodePosition, defaultNodeRadius);
        }

        //Get the terrain information
        terrain = Terrain.activeTerrain;
        tData = terrain.terrainData;

        //Get an initial random node
        randomNode = GenerateRandomNode();

        //Set up the debug sphere
        //Create the sphere
        debugSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //Remove the collider to prevent collisions
        GameObject.Destroy(debugSphere.GetComponent<Collider>());
        //Set the position to the initial node
        debugSphere.transform.position = nodes[currentFixedNode].position;
	}
	
	// Update is called once per frame
	void Update ()
    {	
        //Added Debug stuff used for testing (Everything seems to work)
        /*
        if(useDebug)
        {
            //Increments the time
            debugTime += Time.deltaTime;

            //If the time is over 4 seconds move to a new node
            if (debugTime >= 4.0f)
            {
                debugTime = 0.0f;
                NextNode();
            }
        }
        */
	}

    //Used for input handling
    private void FixedUpdate()
    {
        //0 key for toggling debug mode
        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            ShowDebug(!useDebug);
        }
        //1 key makes nodes progress on a fixed path
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            pType = PathType.Fixed;
            currentNode = nodes[currentFixedNode];
            debugSphere.transform.position = currentNode.position;
        }
        //2 key has nodes become randomly placed
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            pType = PathType.Random;
            currentNode = randomNode;
            debugSphere.transform.position = currentNode.position;
        }
    }

    /// <summary>
    /// Cycles to the next node and returns it
    /// </summary>
    /// <returns></returns>
    public Node NextNode()
    {
        //If fixed path type, cycle to the next node
        if(pType == PathType.Fixed)
        {
            CurrentFixedNode++;
            currentNode = nodes[currentFixedNode];
        }
        //If random path type, create a new node
        else if(pType == PathType.Random)
        {
            randomNode = GenerateRandomNode();
            currentNode = randomNode;
        }

        //Update the debugSphere
        debugSphere.transform.position = currentNode.position;

        //Return the current node
        return currentNode;
    }

    /// <summary>
    /// Returns a randomly generated node with default radius
    /// </summary>
    /// <returns></returns>
    Node GenerateRandomNode()
    {
        //Get the size of the terrain
        Vector3 tSize = tData.size;
        //Vector3 tPos = terrain.GetPosition();

        //Choose random x and z positions on the terrain
        float xPos = Random.Range(-tSize.x / 2.0f, tSize.x / 2.0f);
        float zPos = Random.Range(-tSize.z / 2.0f, tSize.z / 2.0f);

        //Find the y position at that (x,z) point
        float yPos = terrain.SampleHeight(new Vector3(xPos, 0.0f, zPos)) + 2.0f;

        //Create a new Node at that position with default radius
        return new Node(new Vector3(xPos, yPos, zPos), defaultNodeRadius);
    }

    /// <summary>
    /// Toggles debug mode on the nodes
    /// </summary>
    /// <param name="option"></param>
    void ShowDebug(bool option)
    {
        useDebug = option;
        debugSphere.SetActive(option);
    }
}
