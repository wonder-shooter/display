using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;


public class PerplesDirector : MonoBehaviour
{

	private Vector3 deffPositon;
	private Quaternion deffRotation;

	private IEnumerator Reset()
	{
		while (true)
		{
			transform.position = deffPositon;
			transform.rotation = deffRotation;
			yield return new WaitForSeconds(100f);
			StartCoroutine(Reset());
		}
	}

	// Use this for initialization
	void Start ()
	{
		deffPositon = transform.position;
		deffRotation = transform.rotation;
		StartCoroutine(Reset());
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.position += new Vector3(1f * Time.deltaTime, 0f, 0f);
	}
}
