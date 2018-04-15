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

    public enum UnitColor
    {
        White,
        Blue,
        Yellow,
        Black
    }

    public Team selectedTeam;
    public UnitColor selectedColor;

    public GameObject unitPrefab;

    public Material whiteMat;
    public Material blueMat;
    public Material yellowMat;
    public Material blackMat;

    public List<GameObject> unitList;

    public Canvas canvas;
    Button teamChanger;
    Dropdown unitDropDown;

    GridManager gridMgr;

	// Use this for initialization
	void Start ()
    {
        unitList = new List<GameObject>();

        teamChanger = canvas.GetComponentInChildren<Button>();

        unitDropDown = canvas.GetComponentInChildren<Dropdown>();

        gridMgr = GetComponent<GridManager>();
    }
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetMouseButtonDown(1))
        {
            RaycastHit pos;

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
        GameObject newUnit = Instantiate(unitPrefab, position, Quaternion.identity);

        Unit newUnitComp = newUnit.GetComponent<Unit>();
        newUnitComp.init(unitDropDown.value + 1, selectedTeam);

        gridMgr.AddUnit(newUnitComp);

        //Change the material colors
        newUnit.GetComponent<MeshRenderer>().material.color = newUnitComp.color;
        newUnit.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = newUnitComp.teamColor;

        unitList.Add(newUnit);
    }
}
