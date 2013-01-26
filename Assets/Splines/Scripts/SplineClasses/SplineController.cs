using UnityEngine;
using System.Collections;

/*
 * http://www.opensource.org/licenses/lgpl-2.1.php
 * Copyright Defective Studios 2009-2011
 */
///<author>Matt Schoen</author>
///<date>5/21/2011</date>
///<email>schoen@defectivestudios.com</email>
///<version>10</version>
/// <summary>
/// Spline-based motion controller
/// </summary>

[RequireComponent(typeof(Rigidbody))]
public class SplineController : MonoBehaviour {
	const int MAX_OVER_ITER = 10;
	public bool advancedMode;

	public enum Mode { KEYBOARD, MOUSE, AUTO }
	public Mode mode;

	public bool looseFollow;
	public float snapDistance = 0.1f;			//Mess with this if he "sticks" when he jumps

	public static int SplineTopLayer = 31;

	public float airForce = 10;
	public float maxAirSpeed = 8;
	public float runForce = 10;
	public float maxRunSpeed = 10;
	public float stopForce = 60;
	public float coastForce = 8;
	public float stopThresh = 2; 	// Cuts velocity at this value, to cancel back & forth jitters in stopping.
	public float gravityForce = -25f;
	public float jumpVelocity = 11;

	public float horizAcceleration;
	public float horizSpeed;
	public int predictionLength = 80;	//150?
	public int predictionStep = 5;		//10?

	public Spline gravSpline;
	public Spline currentSpline;
	public int startVert;
	public float rotationDamping = 10;
	public SplineNode gravNode;
	public SplineNode groundNode;
	protected Vector3 gravPosition;
	protected bool gravAhead;

	public Vector3 hoverOffset;

	public Vector3 dragSplinePosition;

	public bool goInReverse;
	public bool reverseOrientation;
	public bool go;
	public bool ignoreNodes;
	public bool ignoreNextNode;
	public bool startOnVert;

	public bool KeyControl = true;

	public float pauseTime;
	float pauseStart = Mathf.Infinity;
	float resumeSpeed;

	void OnGUI() {
		GUI.Box(new Rect(Screen.width - 75, 25, 75, 20), rigidbody.velocity.ToString());
	}
	public virtual void Start() {
		if(gravSpline) {			//If the user has specified a gravity spline, set it up
			gravNode = gravSpline.begin;
			gravPosition = gravNode.transform.position;
		}
		if(currentSpline) {
			currentSpline.followers.Add(this);
			if(startOnVert) {
				groundNode = currentSpline[startVert];
				if(groundNode)
					transform.position = groundNode.transform.position;
			}
		}
	}
	protected virtual void DoNode(SplineNode node) {
		switch(node.type) {
		case SplineNode.Type.CONTINUE:
			horizSpeed = node.speed;
			break;
		case SplineNode.Type.PAUSE:
			horizSpeed = 0;
			pauseTime = node.pauseTime;
			pauseStart = Time.time;
			resumeSpeed = node.speed;
			break;
		case SplineNode.Type.STOP:
			horizSpeed = 0;
			break;
		case SplineNode.Type.REVERSE:
			horizSpeed = 0;
			goInReverse = !goInReverse;
			pauseStart = Time.time;
			pauseTime = node.pauseTime;
			resumeSpeed = node.speed;
			break;
		}
	}
	void Drive(Vector3 velocity, float input) {
		if(groundNode)
			horizSpeed = Vector3.Dot(velocity, groundNode.forward.normalized);
		else if(gravNode)
			horizSpeed = Vector3.Dot(velocity, gravNode.forward.normalized);
		else
			horizSpeed = Vector3.Dot(velocity, Vector3.right.normalized);
		if(input == 0)
			Stop(coastForce, horizSpeed);
		else if((horizSpeed > 1 && input < 0) || (horizSpeed < -1 && input > 0))
			Stop(stopForce, horizSpeed);
		else {
			if(input > 0)
				goInReverse = true;
			else
				goInReverse = false;
			if(groundNode) {
				if(Mathf.Abs(horizSpeed) < maxRunSpeed)
					horizAcceleration = input * runForce;
			} else {
				if(Mathf.Abs(horizSpeed) < maxAirSpeed) {
					horizAcceleration = input * airForce;
				} else {
					//Debug.Break();
					horizAcceleration = 0;
				}
			}
		}
	}
	public virtual void Update() {
		if(go) {
			/*
			 * Horizontal control input handling
			 */
			switch(mode) {
			case Mode.KEYBOARD:
				float input = Input.GetAxis("Horizontal");
				if(KeyControl)
					Drive(rigidbody.velocity, input);
				break;
			}
			///*
			// * For debugging: Press the b key to pause
			// */
			//if(Input.GetKeyDown(KeyCode.B)) {
			//    //Debug.Break();
			//}
		}
	}
	public virtual void FixedUpdate() {
		if(go) {
			/*
			 * Update velocity
			 * THIS IS THE ONLY PLACE WE SHOULD DO THIS!!!
			 */
			if(Time.time > pauseStart + pauseTime) {
				horizSpeed = resumeSpeed;
				pauseStart = Mathf.Infinity;
				ignoreNextNode = true;
			} else ignoreNextNode = false;
			if(groundNode) rigidbody.velocity = SplineMovement();
			else rigidbody.velocity = AirMovement();
			if(gravSpline)
				GravMovement(transform.position, out gravPosition);
			/*
			 * Set the rotation sonic will Slerp to (hehe)
			 * if we have a ground vertex, use it's forward vector
			 * if we have a grav vertex, use it's forward vector
			 * if we don't have either, look forward
			 */
			Quaternion targetRotation = Quaternion.identity;
			if(groundNode)
				targetRotation = Quaternion.LookRotation((goInReverse ^ reverseOrientation ? 1 : -1) * groundNode.transform.forward, groundNode.transform.up);
			else if(gravNode)
				targetRotation = Quaternion.LookRotation((goInReverse ^ reverseOrientation ? 1 : -1) * gravNode.transform.forward, gravNode.transform.up);
			else
				targetRotation = Quaternion.LookRotation((goInReverse ^ reverseOrientation ? 1 : -1) * Vector3.right, Vector3.up);
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationDamping);
		}
	}
	void OnDrawGizmos() {
		//Ground = red
		//Grav = green
		//Other = blue
		Gizmos.color = Color.cyan;
		if(groundNode) {
			Gizmos.DrawCube(groundNode.transform.position, Vector3.one * 0.1f);
			Gizmos.DrawLine(groundNode.transform.position, groundNode.transform.position + groundNode.forward);
		}
		Gizmos.color = Color.green;
		if(currentSpline)
			Gizmos.DrawCube(currentSpline.transform.position, Vector3.one * 0.1f);
	}
	//	public void UpdateSpline(){
	//		if(currentSpline != currentSplineSource.spline){
	//			Svertex oldBegin = currentSpline.begin;
	//			Svertex newBegin = currentSplineSource.spline.begin;
	//			while(oldBegin.next != null && newBegin.next != null){		//roll forward through both splines until we find the old groundCV
	//				if(oldBegin == groundCV)
	//					break;
	//				oldBegin = oldBegin.next;
	//				newBegin = newBegin.next;
	//			}
	//			groundCV = newBegin;
	//			currentSpline = currentSplineSource.spline;
	//		}
	//	}
	public void Detach() {
		//Debug.Log("detach" + Time.frameCount);
		currentSpline = null;
		groundNode = null;
		transform.parent = null;
	}
	public bool FindSplineVertex(Spline s, Vector3 a, Vector3 b, out SplineNode vert, out Vector3 position) {
		return FindSplineVertex(s, a, b, null, out vert, out position);
	}
	bool FindSplineVertex(Spline s, Vector3 a, Vector3 b, SplineNode guess, out SplineNode vert, out Vector3 position) {
		SplineNode countup, countdown;
		if(s != null) {
			if(guess == null) {
				if(!(s.begin && s.end)) {
					Debug.LogError("Could not find vertex, begin and end not set");
					vert = null;
					position = Vector3.zero;
					return false;
				}
				countup = s.begin;
				countdown = s.end;
			} else {
				countdown = guess;
				countup = guess;
			}
			Utility.Vector3Pair points;
			while(countdown != null || countup != null) {
				if(countup.next != null) {
					//#if(DEBUG_DRAW||DEBUG_DRAW_LND)
					Debug.DrawLine(countup.transform.position, countup.next.transform.position, Color.magenta);
					//#endif
					points = Utility.Dist3DSegToSeg(a, b, countup.transform.position, countup.next.transform.position);
					if((points.a - points.b).magnitude < snapDistance) {
						//Debug.Log(points.a + ", " + points.b);
						Debug.DrawLine(points.b, transform.position, Color.magenta);
						vert = countup;
						if(looseFollow)
							position = transform.position;
						else
							position = points.b;
						return true;
					}
				}
				countup = countup.next;
				if(countdown.previous != null) {
					//#if(DEBUG_DRAW||DEBUG_DRAW_LND)
					Debug.DrawLine(countdown.transform.position, countdown.previous.transform.position, Color.cyan);
					//#endif
					points = Utility.Dist3DSegToSeg(a, b, countdown.transform.position, countdown.previous.transform.position);
					if((points.a - points.b).magnitude < snapDistance) {
						Debug.DrawLine(points.b, transform.position, Color.green);
						vert = countdown.previous;
						if(looseFollow)
							position = transform.position;
						else
							position = points.b;
						return true;
					}
				}
				countdown = countdown.previous;
			}
		} else
			Debug.LogError("Error in land, spline is null");
		Debug.LogWarning("Warning in land - Couldn't find a vert");
		vert = null;
		position = Vector3.zero;
		return false;
	}
	bool FindNextSpline(Vector3 position, Vector3 velocity, Vector3 acceleration, out Vector3 landing, out bool landed, out int traveltime) {
#if (DEBUG_LOG)
		Debug.Log("findNextSpline");
#endif
		traveltime = 0;
		if(predictionStep > 0) {
			Vector3 tmpvel = velocity;
			RaycastHit hit;
			Vector3 curr;
			bool found = false;
			int x;
			for(x = 0; x < predictionLength; x += predictionStep) {
				tmpvel += acceleration * Time.fixedDeltaTime * predictionStep;
				curr = position + tmpvel * Time.fixedDeltaTime * predictionStep;
				traveltime += predictionStep;
				//Debug.DrawLine(position, curr);
				if(Physics.Linecast(position, curr, out hit, 1 << SplineTopLayer)) {
					found = true;
					break;
				}
				if(found)
					break;
				position = curr;
			}
			if(x == 0) {		//This means that the first cast hit the collider
				for(int i = 0; i < predictionStep; i++) {
					tmpvel += acceleration * Time.fixedDeltaTime;
					//Do we really want to do this?
					snapDistance = tmpvel.magnitude * Time.fixedDeltaTime;
					curr = position + tmpvel * Time.fixedDeltaTime;
					traveltime++;
					if(Physics.Linecast(position, curr, out hit, 1 << SplineTopLayer)) {
						// Debug.Log(zPlane.print());
						if(i == 0) {	//This means that we'll land there this frame.
							//projectedPosition = last;
							//						if(currentSplineSource)
							//							currentSplineSource.followers.Remove(this);
							//Search despearately for the spline
							if(hit.transform.parent)
								currentSpline = hit.transform.parent.GetComponent<Spline>();
							else
								currentSpline = hit.transform.GetComponent<Spline>();
							if(!currentSpline)
								currentSpline = hit.transform.GetComponentInChildren<Spline>();
							if(currentSpline) {
								currentSpline.followers.Add(this);
								transform.parent = currentSpline.transform;
								//projectedPosition.z = currentSpline.transform.position.z;
								if(FindSplineVertex(currentSpline, position, curr, out groundNode, out landing)) {
									landed = true;
								} else {
									landed = false;
									Debug.LogWarning("Error in findNextSpline - Couldn't Land");
								}
								return true;
							}
						}
					}
				}
			}
		} else Debug.LogError("cannot find next spline, prediction step < 1!");
		landing = Vector3.zero;
		landed = false;
		return false;
	}

	public bool FindNextSplineMouse(out Vector3 position, Vector3 point) {
		RaycastHit hit;
		if(Physics.Raycast(point, Vector3.down, out hit, snapDistance, 1 << SplineTopLayer)) {
			if(hit.transform.parent.GetComponent<Spline>()) {
				currentSpline = hit.transform.parent.GetComponent<Spline>();
				if(FindSplineVertex(currentSpline, point, point + Vector3.down * snapDistance, out groundNode, out position)) {
					return true;
				}
			}
		}
		position = Vector3.zero;
		return false;
	}

	protected void Stop(float acceleration, float speed) {
		if(speed >= stopThresh)
			horizAcceleration = -acceleration;
		else if(speed <= -stopThresh)
			horizAcceleration = acceleration;
		else {
			if(groundNode)
				rigidbody.velocity = Vector3.zero;
			else if(gravNode)
				rigidbody.velocity = Vector3.Project(rigidbody.velocity, gravNode.transform.up);
			else
				rigidbody.velocity = Vector3.Project(rigidbody.velocity, Vector3.up);
			horizAcceleration = 0;
		}
	}
	//-------------------//
	//	SPLINE PHYSICS!!!//
	//-------------------//
	bool NextNode() {
		if(groundNode.next) {
			groundNode = groundNode.next;
			return true;
		} else if(currentSpline.next) {
			if(currentSpline.next.begin) {
				currentSpline = currentSpline.next;
				groundNode = currentSpline.begin;
				return true;
			} else Debug.LogWarning("Next spline missing begin");
		}
		return false;
	}
	bool PreviousNode() {
		if(groundNode.previous) {
			groundNode = groundNode.previous;
			return true;
		} else {
			if(currentSpline.previous) {
				if(currentSpline.previous.end) {
					currentSpline = currentSpline.previous;
					groundNode = currentSpline.end;
					return true;
				} else Debug.LogWarning("Previous spline missing end");
			}
		}
		return false;
	}
	protected Vector3 SplineMovement() {
		if(!groundNode)
			return rigidbody.velocity;
		float horizFrameVelocity = 0;
		switch(mode) {
		case Mode.KEYBOARD:
			horizFrameVelocity = (Vector3.Dot(rigidbody.velocity, groundNode.forward.normalized)
				+ horizAcceleration * Time.fixedDeltaTime)
				* Time.fixedDeltaTime;
			break;
		case Mode.AUTO:
			horizFrameVelocity = (horizSpeed + horizAcceleration * Time.fixedDeltaTime) * Time.fixedDeltaTime * (goInReverse ? -1 : 1);
			if(!ignoreNodes && !ignoreNextNode) {
				if(horizFrameVelocity > 0 && groundNode.next) {	//We're moving "forward" and there's a next vert
					if(horizFrameVelocity > (rigidbody.position - groundNode.next.transform.position).magnitude)
						if(groundNode.next)
							if(groundNode.next.fromPrevious)	//this node wants to be activated going forward
								DoNode(groundNode.next);
				} else if(groundNode) {		//We're moving "backward" and there's a previous vert
					if(-horizFrameVelocity > (rigidbody.position - groundNode.transform.position).magnitude)
						if(groundNode)
							if(groundNode.fromNext)	//this node wants to be activated going forward
								DoNode(groundNode);
				}
				horizFrameVelocity = (horizSpeed + horizAcceleration * Time.fixedDeltaTime) * Time.fixedDeltaTime * (goInReverse ? -1 : 1);
			}
			break;
		}
		Vector3 position = rigidbody.position;
		Vector3 newVelocity = rigidbody.velocity;
		if(horizFrameVelocity != 0) {
			float distance = Mathf.Abs(horizFrameVelocity);
			int itercount = MAX_OVER_ITER;
			int iter = MAX_OVER_ITER;
			/*
			 * This while loop "adjusts" for any incoming error on groundNode.  If the first condition is
			 * met, the node reference is too far forward, and must be decremented.  If the second is met,
			 * the opposite is true and the node must be incremented.
			 * niiiiice 8)
			 */
			while(iter-- > 0) {
				if(Vector3.Dot(groundNode.forward, transform.position - groundNode.transform.position) < 0) {
					if(!PreviousNode()) {
						Detach();
						return rigidbody.velocity;
					}
					continue;
				} else if(groundNode.next) {
					if(Vector3.Dot(groundNode.forward, transform.position - groundNode.next.transform.position) > 0) {
						if(!NextNode()) {
							Detach();
							return rigidbody.velocity;
						}
						continue;
					}
				}
				break;
			}
			if(horizFrameVelocity < 0)		//If we're going backward, come from the other direction
				groundNode = groundNode.next;
			if(groundNode) {
				Debug.DrawLine(transform.position, transform.position + groundNode.forward * horizFrameVelocity, Color.red);
				//We will loop until we have gone one node past the next desired position
				while(distance > 0) {
					if(horizFrameVelocity > 0) {		//Reaching the next vert going forward
						if(!NextNode()) {
							Detach();
							return rigidbody.velocity;
						}
					}
					if(horizFrameVelocity < 0) {	//Reaching the previous vert going backward
						if(!PreviousNode()) {
							Detach();
							return rigidbody.velocity;
						}
					}
					//calculate the distance to the next node
					distance -= (position - groundNode.transform.position).magnitude;
					//update position to the next node
					position = groundNode.transform.position;
					if(itercount-- < 0)
						break;
				}
				//This statement is complicated
				newVelocity = ((position + (position - transform.position).normalized * distance) - transform.position) * (1 / Time.fixedDeltaTime);
				//if(groundNode.num == 2)
				//    Debug.Log(horizFrameVelocity + ", " + distance + " | " + (transform.position + newVelocity * Time.fixedDeltaTime) + " - " + Time.frameCount);
				if(horizFrameVelocity > 0)
					groundNode = groundNode.previous;
			} else return rigidbody.velocity;
		} else return Vector3.zero;
		return newVelocity;
	}
	protected Vector3 AirMovement() { return AirMovement(null); }
	protected Vector3 AirMovement(Set levels) {
		Vector3 absoluteAcceleration;
		if(gravNode)
			absoluteAcceleration = gravityForce * gravNode.transform.up + horizAcceleration * gravNode.transform.forward;
		else
			absoluteAcceleration = gravityForce * Vector3.up + horizAcceleration * Vector3.right;
		//Debug.Log(horizAcceleration + ", " + absoluteAcceleration);
		Vector3 newVelocity = rigidbody.velocity + absoluteAcceleration * Time.fixedDeltaTime;
		Vector3 landing;
		int traveltime;
		bool landed;
		if(Vector3.Dot(newVelocity, Vector3.up) < 0) {
			if(FindNextSpline(rigidbody.position, rigidbody.velocity, absoluteAcceleration, out landing, out landed, out traveltime)) {
				if(landed) {
					Land();
					transform.position = landing;
					//rigidbody.velocity = Vector3.Project(rigidbody.velocity, groundNode.forward);
					//if(horizAcceleration == 0)
					//    rigidbody.velocity = Vector3.zero;
					return SplineMovement();
				}
			}
		}
		return newVelocity;
	}
	protected void GravMovement(Vector3 position, out Vector3 gravPosition) {	//change the grav vert dependent on ground getPosition()
		gravPosition = Vector3.zero;
	}
	public virtual void Land() { }
}