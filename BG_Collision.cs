using UnityEngine;
using System.Collections;

public class BG_Collision : MonoBehaviour 
{
    public AudioClip myTown; 
    public AudioClip myBuilding; 
    public AudioClip myNormal;

	void Start () 
    {
        audio.clip = myNormal;
        audio.Play();
	
	}
	

    void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Town":
                audio.clip = myTown;
                audio.Play();
                break;

            case "Building":
                audio.clip = myBuilding;
                audio.Play();
                break;
        }
    }

    void OnTriggerExit(Collider other)
    {
        audio.clip = myNormal;
        audio.Play();
    }
}
