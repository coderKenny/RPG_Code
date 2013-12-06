using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BG_Playlist : MonoBehaviour 
{
    public List<AudioClip> myList = new List<AudioClip>();
    public int myIterator = 0;

	// Use this for initialization
	void Start () 
    {
        audio.clip = myList[myIterator];
        audio.Play();
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (!audio.isPlaying)
        {
            if (myIterator >= myList.Capacity)
                myIterator = 0;

            else
                myIterator++;

            audio.clip = myList[myIterator];
            audio.Play();
        }
   
	}
}
