using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HairMaterialButton : MonoBehaviour
{
    public Singleton sinkku;
    public Material original;
    public Material apperiance2;
    public GameObject player;
    public AudioClip changeSound;
    public GameObject playerHair;
    public Component renderer;
    public List<Material> allMaterials;

    private int iterator = 0;

    private bool suunta = true;

    void Start()
    {
        sinkku = Singleton.Instance;
        player = GameObject.FindGameObjectWithTag("Pelaaja");

        changeSound = Resources.Load("159450__twisterman__weaponswap-custom-sound-effect") as AudioClip;

        allMaterials = new List<Material>();

        playerHair = GameObject.FindGameObjectWithTag("Hair1");

        renderer = playerHair.GetComponent<MeshRenderer>();

        for (int i = 1; i < 10; i++)
        {
            allMaterials.Add(Resources.Load("Dwarf_0" + i.ToString()) as Material);
        }

        for (int i = 10; i < 13; i++)
        {
            allMaterials.Add(Resources.Load("Dwarf_" + i.ToString()) as Material);
        }

        Singleton.originalHairMaterial = (renderer as MeshRenderer).material;

    }

    void OnClick()
    {
        //int randomInteger = UnityEngine.Random.Range(0, 12);

        Debug.Log("Luku3 on : " + iterator);
        //(renderer as SkinnedMeshRenderer).material = allMaterials[randomInteger];

        if (iterator < 11 & suunta)
        {
            iterator++;
            (renderer as MeshRenderer).material = allMaterials[iterator];


            if (iterator == 11)
                suunta = !suunta;
        }

        else
        {
            iterator--;
            (renderer as MeshRenderer).material = allMaterials[iterator];

            if (iterator == 0)
                suunta = !suunta;
        }

    }
}
