using UnityEngine;
using System.Collections;

/*
 * http://www.opensource.org/licenses/lgpl-2.1.php
 * Copyright Defective Studios 2011
 */
///<author>Matt Schoen</author>
///<date>5/21/2011</date>
///<email>schoen@defectivestudios.com</email>
/// <summary>
/// Spline Tester for Defective Spline
/// </summary>

[RequireComponent(typeof(Rigidbody))]
public class SplineTester : SplineController {

	public float initSpeed;
	public float accelScale;
	public Transform accelDirection;

	void Awake() {
		rigidbody.velocity = transform.forward * initSpeed;
	}
	void OnDrawGizmos() {
		if(accelDirection && predictionStep > 0) {		//This is noooo good if predictionStep < 1
			Vector3 position = transform.position;
			Vector3 acceleration = gravityForce * Vector3.up + accelScale * accelDirection.forward;
			Vector3 tmpvel = transform.forward * initSpeed;
			Vector3 curr;
			int x;
			for(x = 0; x < predictionLength; x += predictionStep) {
				tmpvel += acceleration * Time.fixedDeltaTime * predictionStep;
				curr = position + tmpvel * Time.fixedDeltaTime * predictionStep;
				Gizmos.DrawLine(position, curr);
				position = curr;
			}
		}
	}
}
