//#define DEBUG_ORDERVERTS
//#define DEBUG_MAKESPLINE

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
/// Spline Class for Defective Spline
/// </summary>

[ExecuteInEditMode]
public class Spline : MonoBehaviour {
	public static int MAX_SPLINE_LENGTH = 200;
	public bool playerWalkable = true;

	public Set followers = new Set();
	public static int bumpHeight = 4;

	public SplineNode begin, end;
	public Spline next, previous;
	[HideInInspector]
	public float colliderRadius = .125f;
	[HideInInspector]
	public float maxColliderRadius = 2;
	public PrimitiveType globalColliderType = PrimitiveType.Capsule;

	public int length;
	public int Length {
		get { return length; }
		set {
			if(value > MAX_SPLINE_LENGTH)
				Debug.LogWarning("Warning - Spline length greater than max spline length");
			length = value;
		}
	}
	public float setPauseTime;
	public float setSpeed;

	[HideInInspector]
	public bool handlesOn = true;
	[HideInInspector]
	public bool nodesOn = true;
	[HideInInspector]
	public bool collidersOn = true;
	[HideInInspector]
	public bool locked = true;

	void Start() {
		FindEnds();
	}
	public bool FindEnds() {
		SplineNode[] nodes = GetComponentsInChildren<SplineNode>();
		if(!begin)
			foreach(SplineNode node in nodes)
				if(!node.previous)
					begin = node;
		if(!end)
			foreach(SplineNode node in nodes)
				if(!node.next)
					end = node;
		return begin || end;
	}

	public void Oust() {
		foreach(SplineController follower in followers) {
			follower.Detach();
		}
	}
	public SplineNode this[int index] {
		get {
			SplineNode temp;
			if(begin) temp = begin;
			else temp = null;
			if(temp)
				while(temp != null && index-- > 0)
					temp = temp.next;
			return temp;
		}
	}
	public void AddVert(SplineNode vert) {
		Length++;
		if(!vert.next)
			end = vert;
		if(!vert.previous)
			begin = vert;
	}
	void OnDestroy() {
		if(begin) {
			SplineNode node = begin;
			do {
				node.spanCollider = null;
				node.destroyed = true;
				node = node.next;
			} while(node && node != begin);
		}
	}
}