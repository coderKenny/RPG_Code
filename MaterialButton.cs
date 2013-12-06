using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using Vectrosity; // Drawing

public class MaterialButton : MonoBehaviour 
{
    public Singleton sinkku;
    public Material apperiance1;
    public Material apperiance2;
    public GameObject player;
    private Camera characterCamera;
    private Camera mainCamera;
    private Camera scriptCamera;
    public List<Camera> Cameras;
    public AudioClip CharAmbient;
    public List<bool> CollidersKilled;
    private string originalText;

    

    void Start()
    {
        characterCamera = GameObject.FindGameObjectWithTag("CharCam").GetComponent<Camera>();
        mainCamera = Camera.main;
        scriptCamera = GameObject.FindGameObjectWithTag("ScriptiCamera").GetComponent<Camera>();

        Cameras.Add(characterCamera);
        Cameras.Add(mainCamera);      
        Cameras.Add(scriptCamera);

        sinkku = Singleton.Instance;
        player = GameObject.FindGameObjectWithTag("Pelaaja");

    }

    void OnClick()
    {
        if (!sinkku.isMoving && sinkku.playerTurn)
        {

            enableButtons();

            // Store beginning
            Singleton.alkuRotaatio = player.transform.rotation;

            Cameras[0].enabled = true;

            for (int i = 1; i < 3; i++)
                Cameras[i].enabled = false;

            Singleton.checkingPlayer = !Singleton.checkingPlayer;

            //(player.GetComponent("Halo") as MonoBehaviour).enabled = false;
            (player.GetComponent("Halo") as Behaviour).enabled = false;

            Cameras[1].audio.enabled = false;

            //AudioSource.PlayClipAtPoint(CharAmbient, gameObject.transform.position);
            audio.clip = CharAmbient;
            audio.loop = true;
            audio.Play();

            UILabel label = GetComponentInChildren<UILabel>();

            GetComponent<UIButton>().isEnabled = false;

            originalText = label.text;
            Singleton.labelText = label.text;
            label.text = "";

            //GetComponent<UIButton>().


            #region Kill alive colliders beneath

            if (GameObject.FindGameObjectWithTag("HitCollider1").GetComponent<BoxCollider>().enabled)
            {
                GameObject.FindGameObjectWithTag("HitCollider1").GetComponent<BoxCollider>().enabled = false;
                CollidersKilled.Add(true);
            }
            else
                CollidersKilled.Add(false);



            if (GameObject.FindGameObjectWithTag("HitCollider2").GetComponent<BoxCollider>().enabled)
            {
                GameObject.FindGameObjectWithTag("HitCollider2").GetComponent<BoxCollider>().enabled = false;
                CollidersKilled.Add(true);
            }
            else
                CollidersKilled.Add(false);



            if (GameObject.FindGameObjectWithTag("HitCollider3").GetComponent<BoxCollider>().enabled)
            {
                GameObject.FindGameObjectWithTag("HitCollider3").GetComponent<BoxCollider>().enabled = false;
                CollidersKilled.Add(true);
            }
            else
                CollidersKilled.Add(false);


            #endregion

        }
    }

    public void enableButtons()
    {
        Singleton.arrowCollidersAlive = true;

        GameObject.Find("ButtonLeft").GetComponent<BoxCollider>().enabled = true;
        GameObject.Find("ButtonRight").GetComponent<BoxCollider>().enabled = true;

        GameObject.Find("ChangeMaterial").GetComponent<BoxCollider>().enabled = true;
        GameObject.Find("ReturnButton").GetComponent<BoxCollider>().enabled = true;
        GameObject.Find("ChangeFaceMaterial").GetComponent<BoxCollider>().enabled = true;
        GameObject.Find("ChangeHairMaterial").GetComponent<BoxCollider>().enabled = true;
        GameObject.Find("ChangeWeapon").GetComponent<BoxCollider>().enabled = true;
        GameObject.Find("ChangeBodyMesh").GetComponent<BoxCollider>().enabled = true;
    }

    public void StopAudio()
    {
        audio.Stop();
    }

    public void RestoreButton()
    {
        Debug.Log("Heard !!!");

        GetComponent<UIButton>().isEnabled = true;
        UILabel label = GetComponentInChildren<UILabel>();


        label.text = Singleton.labelText;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            GetComponent<UIButton>().isEnabled = !GetComponent<UIButton>().isEnabled;
        }
    }

}
