using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

struct GridSpace
{
    float redInfluence;
    float greeninfluence;

    float totalInfluence;
    Vector3 position;
    public Color color;

    public GridSpace(Vector3 pos)
    {
        redInfluence = 0;
        greeninfluence = 0;
        totalInfluence = 0;
        position = pos;

        color = Color.grey;
    }

    public void AddInfluence(Vector3 unitPos, int strength, UnitManager.Team team)
    {

        float dist = (unitPos - position).magnitude / 20;

        float influence = strength;
        if(dist > 0)
            influence /= dist;

        /*
        if (dist > 0)
            influence = influence / dist;*/

        if(team == UnitManager.Team.Green)
        {
            greeninfluence += influence;
        }
        else
        {
            redInfluence += influence;
        }

        totalInfluence += influence;

        CalculateColor();
    }

    private void CalculateColor()
    {
        if (totalInfluence == 0 || greeninfluence == redInfluence)
            color = Color.grey;
        else if (greeninfluence > redInfluence)
            color = Color.Lerp(Color.grey, Color.green, greeninfluence / totalInfluence);
        else
            color = Color.Lerp(Color.grey, Color.red, redInfluence / totalInfluence);
            //color = Color.green * greeninfluence / totalInfluence + Color.red * redInfluence / totalInfluence;
    }
}

public class GridManager : MonoBehaviour {

    public GameObject mapSpacePrefab;

    const int ROWS = 10;
    const int COLS = 10;

    //Each grid space is 20 units x 20 units
    GridSpace[] gridSpaces;

    Image[] mapSpaces;

	// Use this for initialization
	void Start () {
        gridSpaces = new GridSpace[ROWS * COLS];//new GameObject[ROWS * COLS];
        mapSpaces = new Image[ROWS * COLS];

        Transform canvas = GameObject.Find("Canvas").transform;

        int i = 0;

		for(int y = 0; y < ROWS; ++y)
        {
            for (int x = 0; x < COLS; ++x)
            {
                gridSpaces[i] = new GridSpace(new Vector3((20*x)-90, 0, (20 * y) - 90));
                
                GameObject newMapSpace = Instantiate(mapSpacePrefab, canvas);
                newMapSpace.transform.localPosition = new Vector3(450 + (20 * x), 150 + (20 * y), 0);
                mapSpaces[i] = newMapSpace.GetComponent<Image>();

                ++i;
            }
        }

        UpdateInfluenceMap();
	}

    public void AddUnit(Unit u)
    {
        Vector3 unitGridPos = new Vector3(Mathf.Round(u.transform.position.x / 20) * 20, 0, Mathf.Round(u.transform.position.z / 20) * 20);

        for(int i = 0; i < gridSpaces.Length; ++i)
        {
            gridSpaces[i].AddInfluence(unitGridPos, u.strength, u.team);
        }

        UpdateInfluenceMap();
    }

    void UpdateInfluenceMap()
    {
        for(int i = 0; i < gridSpaces.Length; ++i)
        {
            mapSpaces[i].color = gridSpaces[i].color;
        }
    }
	
    //Get the index for a grid space given a position - allows to determine which grid a unit is in
	int GetGridIndexFromPos(Vector3 pos)
    {
        int x = ((int)pos.x + 90) / 20;
        int y = ((int)pos.y + 90) / 20;

        return y * 10 + x;
    }
}
