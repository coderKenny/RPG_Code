using UnityEngine;
using System.Collections;

public class RotateLeft : MonoBehaviour 
{
    public AudioClip sound;
    private float timer = 100.0f;
    private float clipLength;
    private float rotationAmount = 90.0f;
    public GameObject player;
    public Singleton sinkku;

    public bool isPressed;

	// Use this for initialization
	void Start () 
    {
        sinkku = Singleton.Instance;
        player = GameObject.FindGameObjectWithTag("Pelaaja");

        audio.clip = sound;
        //audio.PlayOneShot(sound);
        audio.playOnAwake = false;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if(sinkku.getEtuNapinState())
            player.transform.Rotate(0, rotationAmount * Time.deltaTime, 0);

        if (sinkku.getTakaNapinState())
            player.transform.Rotate(0, rotationAmount * -Time.deltaTime, 0);
	
	}

    void OnClick()
    { 
        audio.Play();
        Debug.Log("click");
    }

    //void OnPress()
    //{
    //    audio.Play();
    //    Debug.Log("click");
    //    player.transform.Rotate(0, rotationAmount * Time.deltaTime, 0);
    //}



    void OnPress(bool isDown)
    {
        isPressed = isDown;

        

        if (this.tag.Equals("Left"))
        {
            sinkku.setEtuNapinState(isPressed);
            //Debug.Log("EtuTouchi is : "+isPressed);
        }
        else
        {
            sinkku.setTakaNapinState(isPressed);
            //Debug.Log("takaTouchi is : "+isPressed);
        }
    }
}
