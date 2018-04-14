using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public Camera cam;
    public GameObject unitPrefab;

    public Material whiteMat;
    public Material blueMat;
    public Material yellowMat;
    public Material blackMat;

    public List<GameObject> unitList;

	// Use this for initialization
	void Start ()
    {
        unitList = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetMouseButtonDown(1))
        {
            //Vector3 pos = Input.mousePosition;
            //pos = Camera.main.ScreenToWorldPoint(pos);
            RaycastHit pos;

            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out pos))
            {
                SpawnUnit(selectedTeam, pos.point);
            }
        }
	}

    //Handles keyboard input
    private void FixedUpdate()
    {
        //Set of code to change unit color/strength
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedColor = UnitColor.White;
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedColor = UnitColor.Blue;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectedColor = UnitColor.Yellow;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            selectedColor = UnitColor.Black;
        }

        //Detects team changing
        if(Input.GetKeyDown(KeyCode.T))
        {
            if (selectedTeam == Team.Green)
                selectedTeam = Team.Red;
            else
                selectedTeam = Team.Green;
        }
    }

    public void SpawnUnit(Team team, Vector3 position)
    {
        GameObject newUnit = Instantiate(unitPrefab, position, Quaternion.identity);

        Unit newUnitComp = newUnit.GetComponent<Unit>();

        int strength = 1;
        Material mat = whiteMat;

        //Get the strength and material based off the unit color
        switch(selectedColor)
        {
            case UnitColor.White:
                strength = 1;
                mat = whiteMat;
                break;
            case UnitColor.Blue:
                strength = 2;
                mat = blueMat;
                break;
            case UnitColor.Yellow:
                strength = 3;
                mat = yellowMat;
                break;
            case UnitColor.Black:
                strength = 4;
                mat = blackMat;
                break;
        }

        //Change the type of Unit
        newUnitComp.color = mat;
        newUnitComp.strength = strength;
        newUnitComp.team = selectedTeam;

        //Change the material
        newUnit.GetComponent<MeshRenderer>().material = mat;
        unitList.Add(newUnit);
    }
}
