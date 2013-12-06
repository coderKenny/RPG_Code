using UnityEngine;
using System.Collections;

public class LevelShifter : MonoBehaviour 
{
    Vector3 startingPoint;
    Vector3 playerPlace;

    public Singleton sinkku;
    public GameObject player;

    void Start()
    {
        sinkku = Singleton.Instance;
        player = GameObject.FindGameObjectWithTag("Pelaaja");
        startingPoint = gameObject.transform.position;
    }
	
	// Update is called once per frame
	void Update () 
    {
        playerPlace = player.transform.position;
        transform.position = new Vector3(startingPoint.x, startingPoint.y, playerPlace.z);
	}
}
