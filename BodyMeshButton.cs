using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BodyMeshButton : MonoBehaviour
{
    public Singleton sinkku;
    public GameObject player;
    public AudioClip changeSound;
    private int iterator = 1;
    private bool suunta = true;
    public List<GameObject> allBodies;
    public List<Singleton.BodyShapes> bodyShapes;
    public tk2dTextMesh bodyMesh;

    void Update()
    {
        bodyMesh.text = "Body shape :\n^6" + Singleton.bodyshape.ToString(); 
    }

    void Start()
    {
        bodyMesh = GameObject.Find("BodyShape").GetComponent<tk2dTextMesh>();

        allBodies = new List<GameObject>();
        sinkku = Singleton.Instance;
        player = GameObject.FindGameObjectWithTag("Pelaaja");

        changeSound = Resources.Load("159450__twisterman__weaponswap-custom-sound-effect") as AudioClip;


        allBodies.Add(GameObject.Find("dwarf_skn_body"));
        allBodies.Add(GameObject.Find("dwarf_str_body"));
        allBodies.Add(GameObject.Find("dwarf_fat_body"));

        Singleton.bodyshape = Singleton.BodyShapes.dwarf_skn_body;

        allBodies[0].GetComponent<SkinnedMeshRenderer>().enabled = true; 
        allBodies[1].GetComponent<SkinnedMeshRenderer>().enabled = false; 
        allBodies[2].GetComponent<SkinnedMeshRenderer>().enabled = false;


        bodyShapes.Add(Singleton.BodyShapes.dwarf_skn_body);
        bodyShapes.Add(Singleton.BodyShapes.dwarf_str_body);
        bodyShapes.Add(Singleton.BodyShapes.dwarf_fat_body);
    }

    void OnClick()
    {
        Debug.Log("Iteraattorri : " + iterator);

            
        Singleton.bodyshape = bodyShapes[iterator];
            
        handleDisableEnable(bodyShapes[iterator]);
            
            
        iterator++;

        if(iterator==3)
            iterator=0;
            

    }

    public void handleDisableEnable(Singleton.BodyShapes shape)
    {
        if (shape == Singleton.BodyShapes.dwarf_skn_body)
        {
            allBodies[0].GetComponent<SkinnedMeshRenderer>().enabled = true;
            allBodies[1].GetComponent<SkinnedMeshRenderer>().enabled = false;
            allBodies[2].GetComponent<SkinnedMeshRenderer>().enabled = false;
        }

        if (shape == Singleton.BodyShapes.dwarf_fat_body)
        {
            allBodies[0].GetComponent<SkinnedMeshRenderer>().enabled = false;
            allBodies[1].GetComponent<SkinnedMeshRenderer>().enabled = false;
            allBodies[2].GetComponent<SkinnedMeshRenderer>().enabled = true;
        }

        if (shape == Singleton.BodyShapes.dwarf_str_body)
        {
            allBodies[0].GetComponent<SkinnedMeshRenderer>().enabled = false;
            allBodies[1].GetComponent<SkinnedMeshRenderer>().enabled = true;
            allBodies[2].GetComponent<SkinnedMeshRenderer>().enabled = false;
        }
    }
}
