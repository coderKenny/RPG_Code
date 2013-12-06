using UnityEngine;
using System.Collections;

public class SetHighScore : MonoBehaviour 
{
    public tk2dTextMesh mesh;

    void Start()
    {
        mesh = GetComponent<tk2dTextMesh>();
        setPoints();
    }
	

    public void setPoints()
    {
        mesh.text = "Highest Score : " + PlayerPrefs.GetInt("RopeHighScore", 0);
    }
}
