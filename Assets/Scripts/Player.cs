using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public int attackPerClick;
    public int currentGold = 0;

    void Start () {
        // Initialize from player stats
        attackPerClick = 1;
        currentGold = 0;
    }
	
	void Update () {	
	}
}
