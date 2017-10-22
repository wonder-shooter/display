using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointHandler : MonoBehaviour {

	public GameObject BlockPrefab;

	private Vector3[] points;

	private Vector3 next;
	private int current = 0;
	private bool isMove = false;

	private IEnumerator StartTimer()
	{
		yield return new WaitForSeconds(5.0f);
		isMove = true;
	}
	
	void Start ()
	{
		points = Commons.CheckPoints[Random.Range(0, Commons.CheckPoints.Length)];

		transform.position = points[0] - transform.forward;
		
		foreach (var point in points)
		{
			Instantiate(BlockPrefab, point, Quaternion.identity);
		}
		next = points[current];

		StartCoroutine(StartTimer());
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(isMove){
			Quaternion targetRotation = Quaternion.LookRotation(next - transform.position);
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10);
	
			transform.position += transform.forward * Time.deltaTime * Commons.ChaserSpeed;
		}
	}
	
	void OnTriggerEnter(Collider other)
	{
		Debug.Log(other.tag);
		
		if (other.tag == "CheckPoint")
		{
			if (current < points.Length - 1)
			{
				next = points[++current];
			}
			else{
				isMove = false;
			}
		}
	}
}
