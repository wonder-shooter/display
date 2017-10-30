using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpScene3_dance : MonoBehaviour {

	private Animator animator;
	// Use this for initialization

	void Start () {
		this.animator = this.GetComponent<Animator>();
		this.animator.SetFloat("Speed_f",0.3f);
		this.animator.SetInteger("Movement_int",1);
	}
}
