using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class ChangeArmorButton : MonoBehaviour 
{
    public Singleton sinkku;
    public Material original;
    public Material apperiance2;
    public GameObject player;
    public AudioClip changeSound;
    public Transform playerTorso;
    public Component renderer;
    public List<Material> allMaterials;

    

    private int iterator=0;

    private bool suunta = true;

	void Start () 
    {
        sinkku = Singleton.Instance;
        player = GameObject.FindGameObjectWithTag("Pelaaja");

        changeSound = Resources.Load("159450__twisterman__weaponswap-custom-sound-effect") as AudioClip;

        allMaterials = new List<Material>();

        playerTorso = player.transform.FindChild("dwarf_skn_body") as Transform;

        renderer = playerTorso.GetComponent<SkinnedMeshRenderer>();

        for (int i = 1; i < 10; i++)
        {
            allMaterials.Add(Resources.Load("Dwarf_0" + i.ToString()) as Material);
        }

        for (int i = 10; i < 13; i++)
        {
            allMaterials.Add(Resources.Load("Dwarf_" + i.ToString()) as Material);
        }

        Singleton.originalMaterial = (renderer as SkinnedMeshRenderer).material;
	
	}
	
	
	void Update () 
    {
        if (Singleton.bodyshape == Singleton.BodyShapes.dwarf_skn_body)
        {
            playerTorso = player.transform.FindChild("dwarf_skn_body") as Transform;
            renderer = playerTorso.GetComponent<SkinnedMeshRenderer>();
        }

        if (Singleton.bodyshape == Singleton.BodyShapes.dwarf_fat_body)
        {
            playerTorso = player.transform.FindChild("dwarf_fat_body") as Transform;
            renderer = playerTorso.GetComponent<SkinnedMeshRenderer>();
        }

        if (Singleton.bodyshape == Singleton.BodyShapes.dwarf_str_body)
        {
            playerTorso = player.transform.FindChild("dwarf_str_body") as Transform;
            renderer = playerTorso.GetComponent<SkinnedMeshRenderer>();
        }



        //GameObject.FindGameObjectsWithTag("SkinnyBody");
            
            
	
	}

    void OnClick()
    {    
        //int randomInteger = UnityEngine.Random.Range(0, 12);

        Debug.Log("Luku on : " + iterator);
        //(renderer as SkinnedMeshRenderer).material = allMaterials[randomInteger];

        if (iterator < 11 & suunta)
        {
            iterator++;
            (renderer as SkinnedMeshRenderer).material = allMaterials[iterator];
            

            if (iterator == 11)
                suunta = !suunta;
        }

        else
        {
            iterator--;
            (renderer as SkinnedMeshRenderer).material = allMaterials[iterator];

            if (iterator == 0)
                suunta = !suunta;
        }
 
    }
}
