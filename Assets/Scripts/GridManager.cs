using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Represents one space on the grid/influence map
struct GridSpace
{
    //Influence of red and green teams on this space
    float redInfluence;
    float greeninfluence;

    //This space's position in the world
    Vector3 position;
    //Color that this space should be drawn with on influence map
    public Color color;

    //Constructor takes world pos
    public GridSpace(Vector3 pos)
    {
        redInfluence = 0;
        greeninfluence = 0;
        position = pos;

        color = Color.grey;//Color defaults to grey
    }

    //Add influence from a new unit
    public void AddInfluence(Vector3 unitPos, int strength, UnitManager.Team team)
    {
        //Calculate distance in grid spaces
        float dist = (unitPos - position).magnitude / 20;

        //Divide influence by dist
        float influence = strength;
        if(dist > 0)
            influence /= dist;

        //Increase influence for appropriate team
        if (team == UnitManager.Team.Green)
        {
            greeninfluence += influence;
        }
        else
        {
            redInfluence += influence;
        }

        //Update color for this space
        CalculateColor();
    }

    //Calculate what color should be used to represent the current influence state
    private void CalculateColor()
    {
        //If no influence or it's a tie, it's just grey
        if (greeninfluence == redInfluence)
            color = Color.grey;
        //If green is winning, determine how green to make the space based on how strong influence is combared to red
        else if (greeninfluence > redInfluence)
            color = Color.Lerp(Color.grey, Color.green, greeninfluence - redInfluence);//Using lerp to blend intensity of green based on influence strength
        //Do vice versa if red is winning
        else
            color = Color.Lerp(Color.grey, Color.red, redInfluence - greeninfluence);
    }
}

public class GridManager : MonoBehaviour {

    public GameObject mapSpacePrefab;//Prefab to construct influence map UI

    //10x10 grid for 100 spaces
    const int ROWS = 10;
    const int COLS = 10;

    //Array of spaces on grid
    GridSpace[] gridSpaces;

    //Array of UI elements for each space in influence map
    Image[] mapSpaces;

	// Use this for initialization
	void Start () {
        gridSpaces = new GridSpace[ROWS * COLS];
        mapSpaces = new Image[ROWS * COLS];

        Transform canvas = GameObject.Find("Canvas").transform;

        int i = 0;

		for(int y = 0; y < ROWS; ++y)
        {
            for (int x = 0; x < COLS; ++x)
            {
                //Make new grid space at appropriate world pos
                //Breaking terrain down into ten 20x20 chunks
                gridSpaces[i] = new GridSpace(new Vector3((20*x)-90, 0, (20 * y) - 90));
                
                //Construct influence map grid UI on canvas
                GameObject newMapSpace = Instantiate(mapSpacePrefab, canvas);
                newMapSpace.transform.localPosition = new Vector3(450 + (20 * x), 150 + (20 * y), 0);
                mapSpaces[i] = newMapSpace.GetComponent<Image>();

                ++i;
            }
        }

        //Update colors of influence map to grey
        UpdateInfluenceMap();
	}

    //Add a new unit to update the influence map calculations
    public void AddUnit(Unit u)
    {
        //Get closest grid position for unit
        Vector3 unitGridPos = new Vector3(Mathf.Round(u.transform.position.x / 20) * 20, 0, Mathf.Round(u.transform.position.z / 20) * 20);

        //Update influence for all grid spaces to include info from this unit
        for(int i = 0; i < gridSpaces.Length; ++i)
        {
            gridSpaces[i].AddInfluence(unitGridPos, u.strength, u.team);
        }

        //Redraw influence map automatically
        UpdateInfluenceMap();
    }

    //Updates colors of influence map to match the current state of grid spaces
    void UpdateInfluenceMap()
    {
        for(int i = 0; i < gridSpaces.Length; ++i)
        {
            mapSpaces[i].color = gridSpaces[i].color;
        }
    }
}
