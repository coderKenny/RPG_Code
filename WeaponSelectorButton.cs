using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponSelectorButton : MonoBehaviour 
{
    //public List<GameObject> allMaterials;
    public GameObject[] GameObjects;
    public tk2dTextMesh weaponMesh;
    public List<Material> allMaterials;

    private int changedNumberOfTimes = 0;
	
	void Start () 
    {
        weaponMesh = GameObject.Find("EquippedWeapon").GetComponent<tk2dTextMesh>();
        GameObjects=GameObject.FindGameObjectsWithTag("Ase");



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

        if (changedNumberOfTimes == 1)
        {
            Debug.Log("GUUGGUU__1");
            GameObjects[changedNumberOfTimes - 1].GetComponent<MeshRenderer>().material = allMaterials[changedNumberOfTimes];
            changedNumberOfTimes++;
        }


        if (changedNumberOfTimes == 0)
        {
            Debug.Log("GUUGGUU__2");
            GameObjects[0].renderer.enabled = false;
            GameObjects[1].renderer.enabled = true;
            GameObject temp;
            temp = GameObjects[0];
            GameObjects[0] = GameObjects[1];
            GameObjects[0].GetComponent<MeshRenderer>().material = allMaterials[0];
            changedNumberOfTimes++;
        }
    }
}
