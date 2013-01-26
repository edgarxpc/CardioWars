using UnityEngine;
using System.Collections;

/*
 * http://www.opensource.org/licenses/lgpl-2.1.php
 * Copyright Defective Studios 2010-2011
 */
///<author>Matt Schoen</author>
///<date>5/21/2011</date>
///<email>schoen@defectivestudios.com</email>
/// <summary>
/// Defective Studios Utility class
/// </summary>

public static class Utility {
	public static bool Intersection2D(Vector3 a1, Vector3 a2, Vector3 b1, Vector3 b2, out Vector3 result) {
		return Intersection2D(a1, a2, b1, b2, out result, true, true, 0, "");
	}
	public static bool Intersection2D(Vector3 a1, Vector3 a2, Vector3 b1, Vector3 b2, out Vector3 result, float z) {
		return Intersection2D(a1, a2, b1, b2, out result, true, true, z, "");
	}
	public static bool Intersection2D(Vector3 a1, Vector3 a2, Vector3 b1, Vector3 b2, out Vector3 result, bool sega, bool segb, float z) {
		return Intersection2D(a1, a2, b1, b2, out result, sega, segb, z, "");
	}
	public static bool Intersection2D(Vector3 a1, Vector3 a2, Vector3 b1, Vector3 b2, out Vector3 result, 
bool sega, bool segb, float z, string ident) {
		//this code from Neil Carter (a.k.a. the man), http://nether.homeip.net:8080/unity/
		//Code butchered from http://flassari.is/2008/11/line-line-intersection-in-cplusplus/
		//Debug.Log(a1 + ", " + a2 + ", " + b1 + ", " + b2);
		result = Vector3.zero;
		float x1 = a1.x, x2 = a2.x, x3 = b1.x, x4 = b2.x;
		float y1 = a1.y, y2 = a2.y, y3 = b1.y, y4 = b2.y;

		float denominator = (y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1);

		// If denominator is zero, the lines are parallel:

		if(denominator == 0.0f)
			return false;

		float ua = ((x4 - x3) * (y1 - y3) - (y4 - y3) * (x1 - x3)) / denominator;
		float ub = ((x2 - x1) * (y1 - y3) - (y2 - y1) * (x1 - x3)) / denominator;

		//Debug.DrawLine(a1, a2, Color.yellow);
		//Debug.DrawLine(b1, b2, Color.magenta);
		//Debug.Log(ua + ", " + ub);
		if(sega)	//If "a" is a line segment
			if(ua <= 0 || ua >= 1)
				return false;
		if(segb)	//If "b" is a line segment
			if(ub <= 0 || ub >= 1)
				return false;
		float x = x1 + ua * (x2 - x1);
		float y = y1 + ua * (y2 - y1);
		if(x == 0 || y == 0) {
			Debug.Log(a1 + ", " + a2 + ", " + b1 + ", " + b2 + " - " + ident);
		}
		result = new Vector3(x, y, z);	//This is a function for 2D intersection, the z is just for fun (actually it just puts the intersection at the right spot)
		return true;
	}
	public struct Vector3Pair { public Vector3 a, b; }
	//Distance formula borrowed from:
	//http://softsurfer.com/Archive/algorithm_0106/algorithm_0106.htm
	public static Vector3Pair Dist3DSegToSeg(Vector3 a1, Vector3 a2, Vector3 b1, Vector3 b2) {
		//Debug.Log("dist between (" + a1 + " - " + a2 + ") and (" + b1 + ", " + b2 + ")");
		Vector3Pair result = new Vector3Pair();
		Vector3 u = a2 - a1;
		Vector3 v = b2 - b1;
		Vector3 w = a1 - b1;
		float a = Vector3.Dot(u, u);
		float b = Vector3.Dot(u, v);
		float c = Vector3.Dot(v, v);
		float d = Vector3.Dot(u, w);
		float e = Vector3.Dot(v, w);
		float D = a * c - b * b;
		float sc, sN, sD = D;
		float tc, tN, tD = D;

		if(D < Mathf.Epsilon) {	//Lines almost parallel
			sN = 0;
			sD = 1;
			tN = e;
			tD = c;
		} else {
			sN = b * e - c * d;
			tN = a * e - b * d;
			if(sN < 0) {
				sN = 0;
				tN = e;
				tD = e;
			} else if(sN > sD) {
				sN = sD;
				tN = e + b;
				tD = c;
			}
		}

		if(tN < 0) {
			tN = 0;
			if(-d < 0)
				sN = 0;
			else if(-d > a)
				sN = sD;
			else {
				sN = -d + b;
				sD = a;
			}
		} else if(tN > tD) {      // tc > 1 => the t=1 edge is visible
			tN = tD;
			// recompute sc for this edge
			if((-d + b) < 0.0)
				sN = 0;
			else if((-d + b) > a)
				sN = sD;
			else {
				sN = (-d + b);
				sD = a;
			}
		}
		sc = Mathf.Abs(sN) < Mathf.Epsilon ? 0 : sN / sD;
		tc = Mathf.Abs(tN) < Mathf.Epsilon ? 0 : tN / tD;
		result.a = a1 + sc * u;
		result.b = b1 + tc * v;
		return result;
	}
	public static bool DistThresh(Vector3 a1, Vector3 a2, Vector3 b1, Vector3 b2, float thresh) {
		Vector3Pair points = Dist3DSegToSeg(a1, a2, b1, b2);
		if((points.a - points.b).magnitude < thresh)
			return true;
		return false;
	}

	// Returns the full hierarchy path name of a GO. For example, 'BeeSystem/Bee Rig/gibs/bum/Collider'.
	public static string GOHierarchyName(GameObject go) {
		string name = go.name;
		Transform t = go.transform;
		while(t = t.parent)
			name = t.gameObject.name + "/" + name;

		return "'" + name + "'";
	}
}
