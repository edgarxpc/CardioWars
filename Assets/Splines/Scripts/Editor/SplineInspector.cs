using UnityEngine;
using UnityEditor;

/*
 * http://www.opensource.org/licenses/lgpl-2.1.php
 * Copyright Defective Studios 2011
 */
///<author>Matt Schoen</author>
///<date>5/21/2011</date>
///<email>schoen@defectivestudios.com</email>
/// <summary>
/// Spline Inspector for Defective Spline
/// </summary>

[CustomEditor(typeof(Spline))]
public class SplineInspector : Editor {
	public override void OnInspectorGUI() {
		DrawDefaultInspector();
		Spline spline = (Spline)target;
		if(spline) {
			if(spline.begin) {
				EditorGUILayout.BeginVertical();
				EditorGUILayout.BeginHorizontal();
				if(GUILayout.Button("Set Speed")) {
					SplineNode begin = spline.begin;
					do {
						begin.speed = spline.setSpeed;
						begin = begin.next;
					} while(begin && begin != spline.begin);
				}
				if(GUILayout.Button("Set Pause Time")) {
					SplineNode begin = spline.begin;
					do {
						begin.pauseTime = spline.setPauseTime;
						begin = begin.next;
					} while(begin && begin != spline.begin);
				}
				if(GUILayout.Button("Toggle handles")) {
					SplineNode begin = spline.begin;
					spline.handlesOn = !spline.handlesOn;
					do {
						begin.hideHandles = spline.handlesOn;
						begin.RefreshModel();
						begin = begin.next;
					} while(begin && begin != spline.begin);
				}
				if(GUILayout.Button("Toggle nodes")) {
					SplineNode begin = spline.begin;
					spline.nodesOn = !spline.nodesOn;
					do {
						begin.renderer.enabled = spline.nodesOn;
						begin = begin.next;
					} while(begin && begin != spline.begin);
				}
				EditorGUILayout.EndHorizontal();
			}
			spline.colliderRadius = EditorGUILayout.Slider("Global Collider Radius", spline.colliderRadius, Mathf.Epsilon, spline.maxColliderRadius);
			SplineNode node = spline.begin;
			do {
				if(!node.colliderFreedom) {
					node.colliderRadius = spline.colliderRadius;
					node.ReOrient();
				}
				node = node.next;
			} while(node && node != spline.begin);
			EditorGUILayout.BeginHorizontal();
			if(GUILayout.Button("Toggle colliders")) {
				spline.collidersOn = !spline.collidersOn;
				SplineColliderDraw[] colliders = spline.GetComponentsInChildren<SplineColliderDraw>();
				foreach(SplineColliderDraw draw in colliders)
					draw.draw = spline.collidersOn;
			}
			if(GUILayout.Button("Reset Colliders")) {
				node = spline.begin;
				do {
					node.colliderType = spline.globalColliderType;
					node.AddCollider();
					node = node.next;
				} while(node && node != spline.begin);
				SplineColliderDraw[] colliders = spline.GetComponentsInChildren<SplineColliderDraw>();
				foreach(SplineColliderDraw draw in colliders)
					draw.draw = spline.collidersOn;
			}
			if(GUILayout.Button("Lock Ornaments")) {
				node = spline.begin;
				spline.locked = !spline.locked;
				do {
					node.Locked = spline.locked;
					node = node.next;
				} while(node && node != spline.begin) ;
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndVertical();
		}
	}
}
