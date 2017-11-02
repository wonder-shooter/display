using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpScene3People : MonoBehaviour {

	private Animator animator;
	// Use this for initialization

	void Start () {
		this.animator = this.GetComponent<Animator>();
	}

	public void DieOn(){
		this.animator.SetBool("Death_b",true);
	}
}
