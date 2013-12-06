using UnityEngine;
using System.Collections;

public class RotationElimination : MonoBehaviour 
{

    private Quaternion iniRot;

	
	void Start () 
    {
        iniRot = transform.rotation;
	
	}
	
	
    void LateUpdate()
    {
        transform.rotation = iniRot;
    }
}
