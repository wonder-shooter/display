using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpScene0 : MonoBehaviour {
	private Animator animator;
	// Use this for initialization

	void Start () {

		this.animator = this.GetComponent<Animator>();
		
		//7秒後 時計を見る
		StartCoroutine(DelayMethod(7f, () =>
		{
			this.animator.SetInteger("Animation_int",3);
		}));

		//8秒後 走る
		StartCoroutine(DelayMethod(8f, () =>
		{
			this.animator.SetInteger("Animation_int",0);
			this.animator.SetFloat("Speed_f",0.6f);
		}));

		//10秒後 ジャンプ
		StartCoroutine(DelayMethod(10f, () =>
		{
			this.animator.SetBool("Jump_b",true);
		}));

		//11秒後 歩く
		StartCoroutine(DelayMethod(11f, () =>
		{
			this.animator.SetBool("Jump_b",false);
			this.animator.SetFloat("Speed_f",0.5f);
		}));

		//13秒後 時計を見る
		StartCoroutine(DelayMethod(13f, () =>
		{
			this.animator.SetInteger("Animation_int",3);
		}));
		
		//14秒後 走る
		StartCoroutine(DelayMethod(14f, () =>
		{
			this.animator.SetInteger("Animation_int",0);
			this.animator.SetFloat("Speed_f",0.6f);
		}));
	}

	private IEnumerator DelayMethod(float waitTime, Action action)
	{
		yield return new WaitForSeconds(waitTime);
		action();
	}

}