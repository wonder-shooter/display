using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hit : MonoBehaviour{

	private Animator animator;
	private bool isMoving = false;
	// Use this for initialization
	void Start () {
		this.animator = this.GetComponent<Animator>();
	}
	void Update () {
		ClikMovement();
	}
	private void ClikMovement()
	{
		if (this.animator != null)
		{
			// Get the movement input (if any) from the horizontal and vertical axes.
			float weAxis = Input.GetAxis("Horizontal");
			float nsAxis = Input.GetAxis("Vertical");

			// Process the movement input.
			if ((Mathf.Abs(weAxis) > 0.0f) || (Mathf.Abs(nsAxis) > 0.0f))
			{
				// If the character is currently idle...
				if (!this.isMoving)
				{
					// Transition to the walking state.
					this.isMoving = true;
					this.animator.SetFloat("Speed_f", 0.4f);
				}

				if (weAxis < 0.0f)
				{
					// If the character should be walking west, rotate them to face west.
					//this.transform.rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
					Debug.Log("test");
				}
				// else if (weAxis > 0.0f)
				// {
				// 	// If the character should be walking east, rotate them to face east.
				// 	this.transform.rotation = Quaternion.Euler(0.0f, -90.0f, 0.0f);
				// }
				// else if (nsAxis < 0.0f)
				// {
				// 	// If the character should be walking south, rotate them to face south.
				// 	this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
				// }
				// else if (nsAxis > 0.0f)
				// {
				// 	// If the character should be walking north, rotate them to face north.
				// 	this.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
				// }
			}
			// else if (this.isMoving)
			// {
			// 	// If there is no movement input and the character is currently moving, transition to the idle state.
			// 	this.isMoving = false;
			// 	this.animator.SetFloat("Speed_f", 0.0f);
			// }
		}
	}
}
