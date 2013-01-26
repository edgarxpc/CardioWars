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
/// Spline Collider Drawer for Defective Spline
/// </summary>

public class SplineColliderDraw : MonoBehaviour {
	public bool draw;
	void OnDrawGizmos() {
		if(draw) {
			Gizmos.matrix = transform.localToWorldMatrix;
			if(GetComponent<CapsuleCollider>()) {
				CapsuleCollider cap = GetComponent<CapsuleCollider>();
				Vector3 size = Vector3.one;
				switch(cap.direction) {
				case 0:
					size.x = cap.height;
					size.y = cap.radius * 2;
					size.z = cap.radius * 2;
					break;
				case 1:
					size.x = cap.radius * 2;
					size.y = cap.height;
					size.z = cap.radius * 2;
					break;
				case 2:
					size.x = cap.radius * 2;
					size.y = cap.radius * 2;
					size.z = cap.height;
					break;
				}
				Gizmos.DrawWireCube(Vector3.zero, size);
			} else if(GetComponent<BoxCollider>()) {
				Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
			}
		}
	}
}
