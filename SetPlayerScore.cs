using UnityEngine;
using System.Collections;

public class SetPlayerScore : MonoBehaviour 
{
    public tk2dTextMesh mesh;

	// Use this for initialization
	void Start () 
    {
        mesh = GetComponent<tk2dTextMesh>();
        setPoints(Singleton.points);
	
	}

    public void setPoints(int points)
    {
        mesh.text = "Your Score : " + Singleton.points;
 
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
