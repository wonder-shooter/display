using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSimplePeople : MonoBehaviour{

	private Animator animator;
	// Use this for initialization

	void Start () {
		this.animator = this.GetComponent<Animator>();
	}

	void Update () {

	}

	public void DieOn(){
		Debug.Log("hit");
		this.animator.SetBool("Death_b",true);
	}
}
