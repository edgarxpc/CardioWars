using UnityEngine;
using System.Collections;

/*
 * http://www.opensource.org/licenses/lgpl-2.1.php
 * Spline Tester for Defective Spline
 * Copyright Defective Studios 2009-2011
 */
[ExecuteInEditMode]
public class SplineNode : MonoBehaviour {
	public int num;						//THIS IS NOT AN INDEX
	public float colliderRadius = 0.125f;
	public enum Type { CONTINUE, PAUSE, STOP, REVERSE }
	public Type type;
	public PrimitiveType colliderType = PrimitiveType.Capsule;
	public bool colliderFreedom;
	public bool rotationFreedom;
	public bool hideHandles;
	public float pauseTime = 1;
	public float speed = 2;
	public float addOffset = .5f;

	public bool fromPrevious = true;
	public bool fromNext = true;

	public SplineNode next, previous;
	public Collider spanCollider;
	public Spline spline;
	public bool destroyed;
	bool locked;
	public bool Locked {
		get { return locked; }
		set {
			locked = value;
			if(value)
				LockObjects();
			else
				UnlockObjects();
		}
	}

	public Vector3 forward {
		get {
			if(next)
				return next.transform.position - transform.position;
			else
				return Vector3.right;
		}
	}
	public Vector3 getForward(bool ahead) {
		if(ahead)
			return forward;
		else if(previous != null)
			return previous.forward;
		else {
			Debug.Log("Error in SplineNode - getForward() - No previous vert");
			return Vector3.right;
		}
	}
	public GameObject
		node,
		pause,
		stop,
		reverse,
		nextArrow,
		prevArrow,
		handles;

	public static SplineNode Create() {
		GameObject obj = new GameObject();
		SplineNode node = obj.AddComponent<SplineNode>();
		obj.transform.Rotate(Vector3.up, 90);
		return node;
	}
	void Start() {
		if(!spline)
			if(transform.parent)
				spline = transform.parent.GetComponent<Spline>();
				
		foreach(Transform child in transform) {
			child.gameObject.active = false;
			switch(child.name) {
			case "NextArrow": if(!nextArrow) nextArrow = child.gameObject;	break;
			case "PrevArrow": if(!prevArrow) prevArrow = child.gameObject; break;
			case "node": if(!node) node = child.gameObject; break;
			case "pause": if(!pause) pause = child.gameObject; break;
			case "reverse": if(!reverse) reverse = child.gameObject; break;
			case "stop": if(!stop) stop = child.gameObject; break;
			case "RotationHandle":
				handles = child.gameObject;
				break;
			}
		}
		RefreshModel();
		ReOrient();
	}
	public void Update() {
		if(!Application.isPlaying) {
			RefreshModel();
			ReOrient();
		}
	}
	public void ReOrient() {
		if(!rotationFreedom) {
			if(next){ transform.LookAt(next.transform, transform.up);}
			//else if(previous) {
			//    transform.LookAt(previous.transform, transform.up);
			//    //transform.Rotate(transform.up, 180);
			//}
		}
		if(!colliderFreedom && next && spanCollider) {
			spanCollider.transform.position = transform.position + forward * 0.5f;
			spanCollider.transform.LookAt(next.transform, transform.up);
			switch(colliderType) {
			case PrimitiveType.Capsule:
				CapsuleCollider cap = (CapsuleCollider)spanCollider;
				if(cap) {
					cap.radius = colliderRadius;
					cap.height = forward.magnitude + colliderRadius * 2;							//+ radius * 2 for half a sphere on each side
				}
				break;
			case PrimitiveType.Plane:
				spanCollider.transform.localScale = new Vector3(colliderRadius * 2, 0.1f, forward.magnitude); // *0.1f;
				break;
			default: colliderType = PrimitiveType.Capsule; break;
			}
		}
		if(nextArrow) {
			if(next && !hideHandles) {
				nextArrow.active = true;
				nextArrow.transform.LookAt(next.transform, Vector3.forward);
			} else nextArrow.active = false;
		}
		if(prevArrow) {
			if(previous && !hideHandles) {
				prevArrow.active = true;
				prevArrow.transform.LookAt(previous.transform, Vector3.forward);
			} else prevArrow.active = false;
		}
	}
	public void RefreshModel() {
		if(handles) {
			if(hideHandles) handles.SetActiveRecursively(false);
			else			handles.SetActiveRecursively(true);
		}
		if(GetComponent<MeshFilter>() && renderer) {
			MeshFilter mesh = GetComponent<MeshFilter>();
			switch(type) {
			case Type.CONTINUE:
				if(node) {
					MeshFilter nodeMesh = node.GetComponent<MeshFilter>();
					mesh.sharedMesh = nodeMesh.sharedMesh;
					renderer.sharedMaterial = node.renderer.sharedMaterial;
				}
				break;
			case Type.PAUSE:
				if(pause) {
					MeshFilter pauseMesh = pause.GetComponent<MeshFilter>();
					mesh.sharedMesh = pauseMesh.sharedMesh;
					renderer.sharedMaterial = pause.renderer.sharedMaterial;
				}
				break;
			case Type.STOP:
				if(stop) {
					MeshFilter stopMesh = stop.GetComponent<MeshFilter>();
					mesh.sharedMesh = stopMesh.sharedMesh;
					renderer.sharedMaterial = stop.renderer.sharedMaterial;
				}
				break;
			case Type.REVERSE:
				if(reverse) {
					MeshFilter reverseMesh = reverse.GetComponent<MeshFilter>();
					mesh.sharedMesh = reverseMesh.sharedMesh;
					renderer.sharedMaterial = reverse.renderer.sharedMaterial;
				}
				break;
			}
		}
	}
	public void AddCollider() {
		if(spanCollider)
			DestroyImmediate(spanCollider.gameObject);
		if(next) {
			switch(colliderType) {
			case PrimitiveType.Capsule:
				GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Capsule);
				spanCollider = obj.collider;
				CapsuleCollider cap = obj.GetComponent<CapsuleCollider>();
				cap.isTrigger = true;
				cap.direction = 2;
				cap.height = forward.magnitude + colliderRadius * 2;
				cap.radius = colliderRadius;
				break;
			case PrimitiveType.Plane:
				GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Cube);	//Plane has waay more triangles than we need
				spanCollider = plane.collider;
				plane.transform.localScale = new Vector3(colliderRadius * 2, 0.1f, forward.magnitude);// * 0.1f;
				break;
			case PrimitiveType.Sphere:
				Debug.LogWarning("you can't use spheres! Spheres are right out.");
				break;
			case PrimitiveType.Cylinder:
				Debug.LogWarning("Oh, just use a capsule");
				break;
			case PrimitiveType.Cube:
				Debug.LogWarning("Oh, just use a capsule");
				break;
			}
			spanCollider.gameObject.AddComponent<SplineColliderDraw>();
//			DestroyImmediate(spanCollider.GetComponent<MeshFilter>());
//			DestroyImmediate(spanCollider.renderer);
			spanCollider.renderer.enabled = false;
			spanCollider.transform.parent = transform.parent;
			
			if(transform.parent)
				if(transform.parent.GetComponent<Spline>())
					if(transform.parent.GetComponent<Spline>().playerWalkable)
						spanCollider.gameObject.layer = SplineController.SplineTopLayer;
			spanCollider.isTrigger = true;
		}
	}
	public GameObject AddNext() {
		//New Node
		GameObject dup = (GameObject)GameObject.Instantiate(gameObject);
		dup.SetActiveRecursively(false);
		dup.active = true;
		if(transform.parent)
			dup.transform.parent = transform.parent;
		dup.transform.position = transform.position + Vector3.right * addOffset;
		dup.transform.rotation = Quaternion.FromToRotation(Vector3.forward, Vector3.right);
		dup.transform.localScale = transform.localScale;
		dup.name = "Node";
		SplineNode dupNode = dup.GetComponent<SplineNode>();
		dupNode.spanCollider = null;
		if(dupNode) {
			if(next) {
				next.previous = dupNode;
				dupNode.next = next;
				dupNode.AddCollider();
			}
			next = dupNode;													//The dup is now my previous node
			dupNode.previous = this;										//I'm the dup's next node
		}
		AddCollider();
		if(spline)
			spline.AddVert(dupNode);
		return dup;
	}
	public GameObject AddPrev() {
		GameObject dup = (GameObject)GameObject.Instantiate(gameObject);			//Start by copying me
		//For some reason, all children are active on duplication.  We just need to deactivate all of the children, and reactivate the parent
		dup.SetActiveRecursively(false);
		dup.active = true;
		if(transform.parent)
			dup.transform.parent = transform.parent;						//If I have a parent, add it to the new node
		dup.transform.position = transform.position + Vector3.left * addOffset;
		dup.transform.localScale = transform.localScale;
		dup.name = "Node";
		SplineNode dupNode = dup.GetComponent<SplineNode>();
		dupNode.spanCollider = null;
		if(dupNode) {
			if(previous) {
				previous.next = dupNode;									//My previous node now points to the new node
				dupNode.previous = previous;								//The new node points back to my old previous
			}
			previous = dupNode;												//The dup is now my previous node
			dupNode.next = this;											//I'm the dup's next node
		}
		dupNode.AddCollider();
		if(spline)
			spline.AddVert(dupNode);
		return dup;
	}
	void LockObjects() {
		if(handles) {
			handles.hideFlags = HideFlags.HideInHierarchy;
			foreach(Transform hchild in handles.transform)
				hchild.gameObject.hideFlags = HideFlags.HideInHierarchy;
		}
		if(nextArrow)
			nextArrow.hideFlags = HideFlags.HideInHierarchy;
		if(prevArrow)
			prevArrow.hideFlags = HideFlags.HideInHierarchy;
	}
	void UnlockObjects() {
		if(handles) {
			handles.hideFlags = 0;
			foreach(Transform hchild in handles.transform)
				hchild.gameObject.hideFlags = 0;
		}
		if(spanCollider)
			spanCollider.gameObject.hideFlags = 0;
		if(nextArrow)
			nextArrow.hideFlags = 0;
		if(prevArrow)
			prevArrow.hideFlags = 0;
	}
	public void Disconnect() {
		if(next) {
			if(spanCollider) {
				DestroyImmediate(spanCollider.gameObject);
				spanCollider = null;
			}
			if(previous) {
				next.previous = previous;
				previous.AddCollider();
			} else next.previous = null;
		}
		if(previous) {
			if(next) {
				previous.next = next;
				previous.AddCollider();
			} else {
				previous.next = null;
				if(previous.spanCollider) {
					DestroyImmediate(previous.spanCollider.gameObject);
				}
				previous.spanCollider = null;
			}
		}
		if(spline) {
			if(this == spline.end)
				spline.end = previous;
			if(this == spline.begin)
				spline.begin = next;
		}
	}
	//This introduced a very interesting bug... It seems the entire scene is destoyed and re-instantiated on running the game.
	void OnDestroy() {
		if(!destroyed)
			Disconnect();
	}
}
