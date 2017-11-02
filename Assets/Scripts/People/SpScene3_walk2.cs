using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpScene3_walk2 : MonoBehaviour {

	private Animator animator;
	// Use this for initialization

	void Start () {
		this.animator = this.GetComponent<Animator>();
		this.animator.SetFloat("Speed_f",0.3f);
	}

}
