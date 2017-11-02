using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpScene3_run1 : MonoBehaviour {

	private Animator animator;
	// Use this for initialization

	void Start () {
		this.animator = this.GetComponent<Animator>();
		this.animator.SetInteger("Movement_int",3);
		this.animator.SetFloat("Speed_f",0.35f);
	}

}
