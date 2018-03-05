using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarNode
{
    private float costSoFar;
    private float estTotalCost;
    private int index;

    public float CostSoFar { get { return costSoFar; } }
    public float EstTotalCost { get { return estTotalCost; } }
    public int Index { get { return index; } }

    private int previousConnection;
    public int PreviousConnection {
        get { return previousConnection; }
        set { previousConnection = value; }
    }

    public void SetCostSoFar(float soFar)
    {
        costSoFar = soFar;
    }

    public void SetCosts(float soFar, float est)
    {
        costSoFar = soFar;
        estTotalCost = est;
    }

    public void Reset()
    {
        costSoFar = 0;
        estTotalCost = 0;
        previousConnection = -1;
    }

    private Vector3 center;

    public Vector3 Center
    {
        get { return center; }
    }

    private bool passable;

    public bool Passable
    {
        get { return passable; }
    }

    private List<int> neighborIndices;

    public int[] Neighbors
    {
        get { return neighborIndices.ToArray(); }
    }

    public AStarNode(Vector3 center, bool passable, int index)
    {
        this.center = center;
        this.passable = passable;
        this.index = index;

        costSoFar = 0;
        estTotalCost = 0;
        previousConnection = -1;

        neighborIndices = new List<int>();
    }
    
    public void AddNeighbor(int nodeInd)
    {
        neighborIndices.Add(nodeInd);
    }
}

public class AStarManager : MonoBehaviour {

    const int TERRAIN_SIZE = 180;
    const int HALF_TERRAIN_SIZE = 90;

    const int NODE_SIZE = 5;

    int numNodesPerRow = TERRAIN_SIZE / NODE_SIZE;

    private AStarNode[] nodes;

    public AStarNode[] Nodes { get { return nodes; } }

    // Use this for initialization
    void Awake ()
    { 
        CreateNodes();
        LinkNeighbors();
    }

    void CreateNodes()
    {
        nodes = new AStarNode[numNodesPerRow * numNodesPerRow];

        float xPos, zPos;

        RaycastHit hit;

        //int layerMask = ~(1 << 8);
        float halfNodeSize = NODE_SIZE / 2.0f;

        TerrainData terrainData = GameObject.Find("Terrain").GetComponent<Terrain>().terrainData;

        for (int z = 0; z < numNodesPerRow; ++z)
        {
            for(int x = 0; x < numNodesPerRow; ++x)
            {
                xPos = ((NODE_SIZE * x) - HALF_TERRAIN_SIZE) + halfNodeSize;
                zPos = ((NODE_SIZE * z) - HALF_TERRAIN_SIZE) + halfNodeSize;

                if (Physics.Raycast(new Ray(new Vector3(xPos, 100, zPos), Vector3.down), out hit, 150))
                {
                    int index = x + z * numNodesPerRow;

                    //Set up position of node and determine if it's passable based on if it's in water
                    AStarNode newNode = new AStarNode(new Vector3(xPos, hit.point.y + 1, zPos), hit.transform.tag == "Terrain" || hit.transform.tag == "Bridge", index);

                    nodes[index] = newNode;
                }
            }
        }
    }

    void LinkNeighbors()
    {
        for (int z = 0; z < numNodesPerRow; ++z)
        {
            for (int x = 0; x < numNodesPerRow; ++x)
            {
                int currentIndex = x + z * numNodesPerRow;

                if (!nodes[currentIndex].Passable) continue;

                List<int> indsToCheck = new List<int>();

                if (x > 0)
                {
                    indsToCheck.Add(currentIndex - 1);

                    if (z > 0)
                        indsToCheck.Add(currentIndex - numNodesPerRow - 1);
                    if (z < numNodesPerRow - 1)
                        indsToCheck.Add(currentIndex + numNodesPerRow - 1);
                }
                if (x < numNodesPerRow - 1)
                {
                    indsToCheck.Add(currentIndex + 1);

                    if (z > 0)
                        indsToCheck.Add(currentIndex - numNodesPerRow + 1);
                    if (z < numNodesPerRow - 1)
                        indsToCheck.Add(currentIndex + numNodesPerRow + 1);
                }
                if (z > 0)
                {
                    indsToCheck.Add(currentIndex - numNodesPerRow);
                }
                if (z < numNodesPerRow - 1)
                {
                    indsToCheck.Add(currentIndex + numNodesPerRow);
                }

                for (int j = 0; j < indsToCheck.Count; ++j)
                {
                    AStarNode otherNode = nodes[indsToCheck[j]];

                    if (!otherNode.Passable) continue;

                    Vector3 vecToNeighbor = otherNode.Center - nodes[currentIndex].Center;
                    
                    if (!Physics.Raycast(nodes[currentIndex].Center, vecToNeighbor, vecToNeighbor.magnitude, 1<<9))
                    {
                        nodes[currentIndex].AddNeighbor(otherNode.Index);
                    }
                }
            }
        }
    }

    public List<Vector3> GetPath(Vector3 start, Vector3 end)
    {
        for(int i = 0; i < nodes.Length; ++i)
        {
            nodes[i].Reset();
        }

        int startInd = GetNodeIndexForPos(start);
        int endInd = GetNodeIndexForPos(end);

        int frontierEndPtInd = 0;

        List<Vector3> pathPositions = new List<Vector3>();

        if (!nodes[endInd].Passable) return pathPositions;

        //List of indices for open nodes, sorted by cost
        List<int> open = new List<int>();
        List<int> closed = new List<int>();

        open.Add(startInd);

        nodes[startInd].SetCosts(0, (end - start).sqrMagnitude);

        //Continue looping until out of nodes to check,
        //break early if reach end node
        while(open.Count > 0)
        {
            //Pop next node ind to check off last element in open
            int currentInd = open[open.Count - 1];

            //If this is the goal node, stop early and compile/return a list of positions for
            //nodes in closed
            if (currentInd == endInd)
            {
                //Reconstruct shortest path from end to start
                int indIter = currentInd;

                while(indIter != -1)
                {
                    pathPositions.Add(nodes[indIter].Center);

                    indIter = nodes[indIter].PreviousConnection;
                }

                break;
            }

            //Get array of indices for neighbor nodes to current node
            int[] neighbors = nodes[currentInd].Neighbors;
            //Get current cost so far
            float currentCost = nodes[currentInd].CostSoFar;
            //Get position of current node
            Vector3 currentCenter = nodes[currentInd].Center;

            //Check all neighbors
            for(int i = 0; i < neighbors.Length; ++i)
            {
                frontierEndPtInd = neighbors[i];

                //Cost from start to this point
                float costToFrontier = currentCost + (nodes[frontierEndPtInd].Center - currentCenter).sqrMagnitude;
                    
                //Get index of node in closed if it's already been visited - will be -1 if not
                int closedIndex = closed.IndexOf(frontierEndPtInd);
                int openIndex = open.IndexOf(frontierEndPtInd);

                //If this neighbor is already in closed, check if the route from current to this neighbor is shorter
                if (closedIndex >= 0)
                {
                    //If this path is longer than an already existing path to the node, discard it
                    if (costToFrontier < nodes[frontierEndPtInd].CostSoFar)
                    {
                        closed.RemoveAt(closedIndex);
                    }
                }
                //If this neighbor is already in open, update its costs if there's a quicker route from current
                else if (openIndex >= 0)
                {
                    if(costToFrontier < nodes[frontierEndPtInd].CostSoFar)
                    {
                        nodes[frontierEndPtInd].PreviousConnection = currentInd;

                        //Calc new estimated cost
                        float estCost = costToFrontier + (end - nodes[frontierEndPtInd].Center).sqrMagnitude;
   
                        nodes[frontierEndPtInd].SetCosts(costToFrontier, estCost);

                        open.RemoveAt(openIndex);

                        for (int j = open.Count - 1; j >= 0; --j)
                        {
                            //Insert in order so last element always has lowest cost
                            if (estCost < nodes[open[j]].EstTotalCost)
                            {
                                open.Insert(j+1, frontierEndPtInd);
                                break;
                            }
                            else if(j == 0)
                            {
                                open.Insert(0, frontierEndPtInd);
                            }
                        }
                    }
                }
                //Node hasn't been visited yet
                else
                {
                    //Loc of other node
                    Vector3 neighborCenter = nodes[frontierEndPtInd].Center;

                    //Est vec from frontier to end
                    Vector3 frontierToEnd = end - neighborCenter;

                    //Set costs of current neighbor
                    nodes[frontierEndPtInd].SetCosts(costToFrontier,
                        costToFrontier + frontierToEnd.sqrMagnitude);

                    nodes[frontierEndPtInd].PreviousConnection = currentInd;

                    float neighborEstCost = nodes[frontierEndPtInd].EstTotalCost;

                    if (open.Count == 0)
                        open.Add(frontierEndPtInd);
                    else
                    {
                        for (int j = open.Count - 1; j >= 0; --j)
                        {
                            //Insert in order so last element always has lowest cost
                            if (neighborEstCost < nodes[open[j]].EstTotalCost)
                            {
                                open.Insert(j+1, frontierEndPtInd);
                                break;
                            }
                            else if(j == 0)
                            {
                                open.Insert(0, frontierEndPtInd);
                            }
                        }
                    }
                }
            }

            open.Remove(currentInd);

            if(!closed.Contains(currentInd))
                closed.Add(currentInd);
        }

        return pathPositions;
    }


    //Will need to verify this works...
    int GetNodeIndexForPos(Vector3 pos)
    {
        //Reversing these calcs
        /*xPos = ((NODE_SIZE * x) - HALF_TERRAIN_SIZE) + halfNodeSize;
        zPos = ((NODE_SIZE * z) - HALF_TERRAIN_SIZE) + halfNodeSize;*/

        //Reverse the position calculations and round to nearest int to get x/z of the best node for this position
        int x = Mathf.RoundToInt(((pos.x + HALF_TERRAIN_SIZE) - (NODE_SIZE/2)) / NODE_SIZE);
        int z = Mathf.RoundToInt(((pos.z + HALF_TERRAIN_SIZE) - (NODE_SIZE/2)) / NODE_SIZE);

        //Clamp within bounds of terrain's nodes
        x = x < 0 ? 0 : x;
        x = x > numNodesPerRow - 1 ? numNodesPerRow - 1 : x;
        z = z < 0 ? 0 : z;
        z = z > numNodesPerRow - 1 ? numNodesPerRow - 1 : z;

        //Calculate the index
        return (z * numNodesPerRow) + x;
    }
}
