using UnityEngine;
using System.Collections;

public class UpdateOtherStats : MonoBehaviour 
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
        stats.text = "Str : " + sinkku.str + "\nDex : " + sinkku.dex + "\nWsd : " + sinkku.wsd;	
	}
}
