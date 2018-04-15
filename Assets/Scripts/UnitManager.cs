using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitManager : MonoBehaviour {

    public enum Team
    {
        Red,
        Green
    }

    public Team selectedTeam;

    public GameObject unitPrefab;

    public Canvas canvas;
    Button teamChanger;
    Dropdown unitDropDown;

    GridManager gridMgr;

	// Use this for initialization
	void Start ()
    {
        teamChanger = canvas.GetComponentInChildren<Button>();

        unitDropDown = canvas.GetComponentInChildren<Dropdown>();

        gridMgr = GetComponent<GridManager>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        //On right click, spawn unit on terrain at mouse pos
		if(Input.GetMouseButtonDown(1))
        {
            RaycastHit pos;

            //Use raycast to get where user clicked on terrain
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out pos))
            {
                SpawnUnit(selectedTeam, pos.point);
            }
        }
	}

    //Changes the current team used in unit spawning
    public void ChangeCurrentTeam()
    {
        if (selectedTeam == Team.Green)
        {
            selectedTeam = Team.Red;
            teamChanger.GetComponentInChildren<Text>().text = "Team Red";
        }
        else
        {
            selectedTeam = Team.Green;
            teamChanger.GetComponentInChildren<Text>().text = "Team Green";
        }
    }

    //Spawns a unit
    public void SpawnUnit(Team team, Vector3 position)
    {
        //Instantiate new unit
        GameObject newUnit = Instantiate(unitPrefab, position, Quaternion.identity);

        //Initialize unit component with selected strength and team
        Unit newUnitComp = newUnit.GetComponent<Unit>();
        newUnitComp.init(unitDropDown.value + 1, selectedTeam);

        //Add unit to grid manager to update the influence map
        gridMgr.AddUnit(newUnitComp);

        //Change the material colors to reflect unit type and team
        newUnit.GetComponent<MeshRenderer>().material.color = newUnitComp.color;
        newUnit.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = newUnitComp.teamColor;
    }
}
