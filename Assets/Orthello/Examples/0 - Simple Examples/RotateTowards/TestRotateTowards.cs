using UnityEngine;
using System.Collections;

public class TestRotateTowards : MonoBehaviour {
	
	public OTSprite target;
	public OTSprite pointer;
	
	// Use this for initialization
	void Start () {
		pointer = GetComponent<OTSprite>();
	}
	
	// Update is called once per frame
	void Update () {
		if (target==null || pointer == null) return;
		pointer.RotateTowards(target);		
	}
}
