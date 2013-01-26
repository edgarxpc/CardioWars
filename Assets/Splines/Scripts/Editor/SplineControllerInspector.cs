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
/// SplineController Inspector for Defective Spline
/// </summary>

[CustomEditor(typeof(SplineController))]
public class SplineControllerInspector : Editor {
	public override void OnInspectorGUI() {
		SplineController controller = (SplineController)target;										//Cast the target (from Unity) as a SplineController
		controller.go = EditorGUILayout.Toggle("Go", controller.go);
		controller.reverseOrientation = EditorGUILayout.Toggle("Rev Orientation", controller.reverseOrientation);
		controller.currentSpline = (Spline)EditorGUILayout.ObjectField("Current Spline", controller.currentSpline, typeof(Spline));
		controller.gravSpline = (Spline)EditorGUILayout.ObjectField("Gravity Spline", controller.gravSpline, typeof(Spline));
		controller.gravityForce = EditorGUILayout.FloatField("Gravity Force", controller.gravityForce);
		controller.looseFollow = EditorGUILayout.Toggle("Loose Follow", controller.looseFollow);
		controller.snapDistance = EditorGUILayout.FloatField("Snap Distance", controller.snapDistance);
			if(controller.currentSpline){
			controller.startOnVert = EditorGUILayout.Toggle("Start On Vertex", controller.startOnVert);
			if(controller.startOnVert)
				controller.startVert = EditorGUILayout.IntSlider("Initial Vertex", controller.startVert, 0, controller.currentSpline.Length - 1);
		}
		controller.mode = (SplineController.Mode)EditorGUILayout.EnumPopup("Mode:", controller.mode);
		switch(controller.mode) {
		case SplineController.Mode.KEYBOARD:
			controller.airForce = EditorGUILayout.FloatField("Air Force", controller.airForce);
			controller.maxAirSpeed = EditorGUILayout.FloatField("Max Air Speed", controller.maxAirSpeed);
			controller.runForce = EditorGUILayout.FloatField("Run Force", controller.runForce);
			controller.maxRunSpeed = EditorGUILayout.FloatField("Max Run Speed", controller.maxRunSpeed);
			controller.stopForce = EditorGUILayout.FloatField("Stop Force", controller.stopForce);
			controller.jumpVelocity = EditorGUILayout.FloatField("Jump Velocity", controller.jumpVelocity);
			controller.horizSpeed = EditorGUILayout.FloatField("Initial Speed", controller.horizSpeed);
			break;
		case SplineController.Mode.MOUSE:
			controller.hoverOffset = EditorGUILayout.Vector3Field("Hover Offset", controller.hoverOffset);
			break;
		default:
			controller.goInReverse = EditorGUILayout.Toggle("Go In Reverse", controller.goInReverse);
			controller.horizSpeed = EditorGUILayout.FloatField("Initial Speed", controller.horizSpeed);
			break;
		}
		//GUILayout.BeginHorizontal();
		//if(GUILayout.Button("Add Prev")) {											//Create a new node before this one
		//    Selection.activeObject = node.AddPrev();
			
		//}
		//if(GUILayout.Button("Add Next")) {
		//    Selection.activeObject = node.AddNext();
		//}
		//GUILayout.EndHorizontal();
		//if(GUILayout.Button("Disconnect")) {
		//    try {
		//        if(node.previous) Selection.activeObject = node.previous;
		//        else if(node.next) Selection.activeObject = node.next;
		//        else Selection.activeObject = null;
		//        node.Disconnect();
		//    } catch(System.Exception e) { }
		//}
		controller.advancedMode = EditorGUILayout.Foldout(controller.advancedMode, "Advanced");
		if(controller.advancedMode) DrawDefaultInspector();
	}
}
