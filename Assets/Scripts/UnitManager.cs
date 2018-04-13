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

	// Use this for initialization
	void Start ()
    {
		
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
    }
}
