using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene4Director : MonoBehaviour {

	private GameDirector gemeDirector;

	// Use this for initialization
	void Start () {
		gemeDirector = GameDirector.GetSheredInstance();		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0))
		{
			gemeDirector.GameEnd();
			gemeDirector.Action(GameActionEvent.EventType.GameEnd);
		}	
	}
}
