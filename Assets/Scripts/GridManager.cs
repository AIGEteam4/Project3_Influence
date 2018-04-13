using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour {

    public GameObject spacePrefab;

    //Each grid space is 20 units x 20 units
    GameObject[] gridSpaces = new GameObject[100];


	// Use this for initialization
	void Start () {
        int i = 0;

		for(int y = 0; y < 10; ++y)
        {
            for (int x = 0; x < 10; ++x)
            {
                gridSpaces[i] = Instantiate(spacePrefab, new Vector3((20 * x) - 90, 40, (20 * y) - 90), Quaternion.identity);
                //gridSpaces[i].GetComponent<MeshRenderer>().material.color = new Color(Random.value,Random.value,Random.value,.2f);
                
                ++i;
            }
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
