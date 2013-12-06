using UnityEngine;
using System.Collections;

public class CustomScaler : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {
        // LG-E400
        if (Screen.height == 240 && Screen.width == 320)
            Camera.main.fieldOfView = 70;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
