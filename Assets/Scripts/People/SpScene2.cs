using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpScene2 : MonoBehaviour {

	private Animator animator;
	// Use this for initialization

	void Start () {
		this.animator = this.GetComponent<Animator>();
	}
	void Update()
	{
		// デバッグ用
		if (Input.GetKeyDown(KeyCode.Space))
		{
			HeadUp();
		}
	}

	public void HeadUp(){
		this.animator.SetInteger("Head_int",1);
		this.transform.localScale = new Vector3(3, 3, 3);
	}
}
