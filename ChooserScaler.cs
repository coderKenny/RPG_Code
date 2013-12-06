using UnityEngine;
using System.Collections;

public class ChooserScaler : MonoBehaviour 
{
	public float screenHeight;
	public float screenWidth;
	
	public Vector3 scale;
	public Vector3 scaledPosition; 
	
	public Vector3 worldScale; 
	
	public Camera kamera;

	
	void Start() 
	{
		screenWidth=Screen.width;
		screenHeight=Screen.height;
		
		scale.x = screenWidth/1154.054f;
		scale.y = screenHeight/1297.297f;
		scale.z = 0;
		
		transform.localScale=scale;
		
		scaledPosition.x=-screenHeight/1.8f;
		scaledPosition.y=-screenWidth/5.8f;
		scaledPosition.z=0;
		
		transform.localPosition=scaledPosition;
	
	}
}
