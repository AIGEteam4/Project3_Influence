using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    //Basic attributes
    //public Material color;
    public int strength;
    public Color color;
    public Color teamColor;

    public UnitManager.Team team;

    public void init(int str, UnitManager.Team tm)
    {
        strength = str;
        team = tm;

        teamColor = (team == UnitManager.Team.Green ? Color.green : Color.red);

        switch (str)
        {
            case 1:
                color = Color.white;
                break;
            case 2:
                color = Color.blue;
                break;
            case 3:
                color = Color.yellow;
                break;
            case 4:
                color = Color.black;
                break;
        }
    }
}
