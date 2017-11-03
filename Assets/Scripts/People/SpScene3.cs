using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpScene3 : MonoBehaviour {

	private Animator animator;
	// Use this for initialization

	void Start () {
		this.animator = this.GetComponent<Animator>();
		int x = 0;
		x = Random.Range(1,4);
		this.animator.SetInteger("Movement_int",x);
	}
}
