using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ReturnButton : MonoBehaviour 
{
    public Singleton sinkku;
    public GameObject player;
    public List<Camera> Cameras;
    public List<bool> CollidersKilled;
	
	void Start () 
    {
        sinkku = Singleton.Instance;
        player = GameObject.FindGameObjectWithTag("Pelaaja");

        Cameras = findCameraArray();

        CollidersKilled = findBoolArray();      	
	}

    void OnClick()
    {
        if (!sinkku.isMoving)
        {
            GameObject.FindGameObjectWithTag("EnterCharScreen").SendMessage("StopAudio");

            Cameras[1].audio.enabled = true;

            restoreColliders(CollidersKilled);

            player.transform.rotation = Singleton.alkuRotaatio;

            Cameras[0].enabled = false;

            for (int i = 1; i < 3; i++)
                Cameras[i].enabled = true;

            killArrowColliders();

            restoreCharButton();

            //(player.GetComponent("Halo") as Behaviour).enabled = true;
            Singleton.checkingPlayer = !Singleton.checkingPlayer;

            disableButtons();
        }
    }

    public void disableButtons()
    {
        GameObject.Find("ChangeMaterial").GetComponent<BoxCollider>().enabled = false;
        GameObject.Find("ReturnButton").GetComponent<BoxCollider>().enabled = false;
        GameObject.Find("ChangeFaceMaterial").GetComponent<BoxCollider>().enabled = false;
        GameObject.Find("ChangeHairMaterial").GetComponent<BoxCollider>().enabled = false;
        GameObject.Find("ChangeWeapon").GetComponent<BoxCollider>().enabled = false;
        GameObject.Find("ChangeBodyMesh").GetComponent<BoxCollider>().enabled = false;
    }



    public void restoreCharButton()
    {
        GameObject.FindGameObjectWithTag("EnterCharScreen").SendMessage("RestoreButton");
    }


    public List<Camera> findCameraArray()
    {
        return GameObject.FindGameObjectWithTag("EnterCharScreen").GetComponent<MaterialButton>().Cameras;
       
    }

    public List<bool> findBoolArray()
    {
        return GameObject.FindGameObjectWithTag("EnterCharScreen").GetComponent<MaterialButton>().CollidersKilled;

    }

    public void restoreColliders(List<bool> Cameras)
    {
        if (Cameras[0])
            GameObject.FindGameObjectWithTag("HitCollider1").GetComponent<BoxCollider>().enabled = true;

        if (Cameras[1])
            GameObject.FindGameObjectWithTag("HitCollider2").GetComponent<BoxCollider>().enabled = true;

        if (Cameras[2])
            GameObject.FindGameObjectWithTag("HitCollider3").GetComponent<BoxCollider>().enabled = true;

    }

    public void killArrowColliders()
    {
        if (Singleton.arrowCollidersAlive)
        {
            Singleton.arrowCollidersAlive = false;

            GameObject.Find("ButtonLeft").GetComponent<BoxCollider>().enabled = false;

            GameObject.Find("ButtonRight").GetComponent<BoxCollider>().enabled = false;
        }
    }
}
