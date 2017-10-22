using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDirector : MonoBehaviour {
	
	private Vector3 deffPositon;
	private Quaternion deffRotation;
	
	private bool isFoword = true;

	private IEnumerator Switch()
	{
		yield return new WaitForSeconds(35f);
		isFoword = false;
		yield break;
	}

	private IEnumerator ResetLoop()
	{
		while (true)
		{
			transform.position = deffPositon;
			transform.rotation = deffRotation;
			yield return new WaitForSeconds(100f);
			StartCoroutine(Switch());
			StartCoroutine(ResetLoop());
		}
	}
	
	void Start ()
	{
		// 初期位置
		deffPositon = transform.position;
		deffRotation = transform.rotation;
		
		StartCoroutine(Switch());
		StartCoroutine(ResetLoop());
	}
	
	void Update ()
	{
		transform.position += new Vector3((isFoword ? -1f : 0.75f) * Time.deltaTime, 0f, 0f);
	}
}
