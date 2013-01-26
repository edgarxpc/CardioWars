using UnityEngine;
using System.Collections;

public class MultiResolution : MonoBehaviour {

	public enum ArtResolution { small, normal, hd }
	public ArtResolution artResolution = ArtResolution.normal;
		
	
	// Use this for initialization
	void Start () {
		HandleResolution();
		TweenUp(null);
	}
	
	void TweenUp(OTTween tween)
	{
		new OTTween(OT.Sprite("sprite-background"), 15, OTEasing.SineInOut).
			TweenAdd("position",new Vector2(0,4000)).
				onTweenFinish = TweenDown;
			
	}
	void TweenDown(OTTween tween)
	{
		new OTTween(OT.Sprite("sprite-background"), 15, OTEasing.SineInOut).
			TweenAdd("position",new Vector2(0,-4000)).
				onTweenFinish = TweenUp;			
	}
	
	void HandleResolution()
	{
		switch(artResolution)
		{
		case ArtResolution.hd :
			OT.sizeFactor = 2;
			break;
		case ArtResolution.normal :
			OT.sizeFactor = 1;
			break;
		case ArtResolution.small :
			OT.sizeFactor = .5f;
			break;
		}		
	}
	
	
	float time = 0;
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		if (time >= 0.6f)
		{
			time -= 0.6f;
			if (OT.Sprite("sprite-atlas").frameIndex<OT.Sprite("sprite-atlas").spriteContainer.frameCount-1)
				OT.Sprite("sprite-atlas").frameIndex++;
			else
				OT.Sprite("sprite-atlas").frameIndex=0;
		}
	}
	
	void OnGUI()
	{
		if (GUI.Button(new Rect(2,2,150,30),"small"))
		{
			artResolution = ArtResolution.small;
			HandleResolution();			
			OT.Reset();
		}
		if (GUI.Button(new Rect(200,2,150,30),"normal"))
		{
			artResolution = ArtResolution.normal;
			HandleResolution();			
			OT.Reset();
		}
		if (GUI.Button(new Rect(400,2,150,30),"HD"))
		{
			artResolution = ArtResolution.hd;
			HandleResolution();			
			OT.Reset();
		}
	}
}
