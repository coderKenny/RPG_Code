using UnityEngine;
using System.Collections;

public class UpdatePoints : MonoBehaviour 
{
    tk2dTextMesh stats;
    public Singleton sinkku;


    // Use this for initialization
    void Start()
    {
        stats = GetComponent<tk2dTextMesh>();
        sinkku = Singleton.Instance;
	}
	
	// Update is called once per frame
	void Update () 
    {
        stats.text = "Score :\n" + Singleton.points;	
	}
}
