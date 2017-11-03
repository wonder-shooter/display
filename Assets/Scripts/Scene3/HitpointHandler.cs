using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitpointHandler : MonoBehaviour {

    private int hitCount = 0;
    private int hitPoint = 5;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Hit()
    {
        hitCount++;
    }

    public bool isDie() {
        return hitPoint <= hitCount;
    }
}
