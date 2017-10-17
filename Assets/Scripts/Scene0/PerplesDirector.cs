using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PerplesDirector : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += new Vector3(1f * Time.deltaTime, 0f, 0f);
	}
}
