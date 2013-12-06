using UnityEngine;
using System.Collections;

public class SetArmor : MonoBehaviour 
{
    tk2dTextMesh armorMesh;
    public Singleton sinkku;

	// Use this for initialization
	void Start () 
    {
        sinkku = Singleton.Instance;
        armorMesh = GetComponent<tk2dTextMesh>();	
	}
	
	// Update is called once per frame
	void Update () 
    {
        //armorMesh.text = "Armor : "+ResolveArmorIntegerValue(sinkku.armor);
        armorMesh.text = "Armor : " + sinkku.armor;
	}

    public static int ResolveArmorIntegerValue(Singleton.Armors armor)
    {
        if (armor == Singleton.Armors.light)
            return 1;

        else if (armor == Singleton.Armors.medium)
            return 3;

        else if (armor == Singleton.Armors.heavy)
            return 5;

        else // Unknown !!!
            return 0;
    }
}
