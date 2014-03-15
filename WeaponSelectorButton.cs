using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponSelectorButton : MonoBehaviour 
{
    //public List<GameObject> allMaterials;
    public GameObject[] GameObjects;
    public tk2dTextMesh weaponMesh;
    public List<Material> allMaterials;
    private string subString;

    public int changedNumberOfTimes = 0;

    private GameObject player;
	
	void Start () 
    {
        weaponMesh = GameObject.Find("EquippedWeapon").GetComponent<tk2dTextMesh>();
        GameObjects=GameObject.FindGameObjectsWithTag("Ase");

        player = GameObject.FindGameObjectWithTag("Pelaaja");

        for (int i = 1; i < GameObjects.Length; i++)
            GameObjects[i].renderer.enabled = false;

        allMaterials.Add(Resources.Load("Dwarf_weapons_01") as Material);           
        allMaterials.Add(Resources.Load("Dwarf_weapons_02") as Material);
	}

    void Update() 
    {
        weaponMesh.text = "Weapon at hand : \n^6" + GameObjects[0].name;
	}

    void OnClick()
    {

        if (changedNumberOfTimes == 3)
        {
            GameObjects[0].GetComponent<MeshRenderer>().material = allMaterials[1];

            changedNumberOfTimes = 0;
        }


        else if (changedNumberOfTimes == 2)
        {
            
            GameObjects[0].renderer.enabled = false;
            GameObjects[2].renderer.enabled = true;
            GameObject temp;
            temp = GameObjects[0];
            GameObjects[0] = GameObjects[2];

            subString = GameObjects[0].name.Substring(6, 3);

            Debug.Log("Nimi on : " + subString);

            if (subString == "cro")
                player.GetComponent<Player>().ase = Player.Weapons.crossBow;

            GameObjects[0].GetComponent<MeshRenderer>().material = allMaterials[0];

            changedNumberOfTimes++;
        }


        else if (changedNumberOfTimes == 1)
        {
            Debug.Log("GUUGGUU__1");
            GameObjects[changedNumberOfTimes - 1].GetComponent<MeshRenderer>().material = allMaterials[changedNumberOfTimes];

            changedNumberOfTimes++;
        }



        else if (changedNumberOfTimes == 0)
        {
            Debug.Log("GUUGGUU__2");
            GameObjects[0].renderer.enabled = false;
            GameObjects[1].renderer.enabled = true;
            GameObject temp;
            temp = GameObjects[0];
            GameObjects[0] = GameObjects[1];

            subString = GameObjects[0].name.Substring(6, 3);

            Debug.Log("Nimi on : " + subString);

            if (subString == "axe")
                player.GetComponent<Player>().ase = Player.Weapons.axe;

            GameObjects[0].GetComponent<MeshRenderer>().material = allMaterials[0];
            changedNumberOfTimes++;
        }
    }
}
