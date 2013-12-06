using UnityEngine;
using System.Collections;

public class BlinkColliderLights : MonoBehaviour 
{
    //private MeshRenderer rendaaja;
    //private Component komponentti;

    private Light valo;
    private bool toggle = false;
	
	void Start () 
    {
        //rendaaja = gameObject.GetComponent("MeshRenderer") as MeshRenderer; // Must be casted from Component to MeshRenderer
        
        //// OR -->
        
        //rendaaja = gameObject.GetComponent<MeshRenderer>();

        //// OR -->

        //komponentti = gameObject.GetComponent("MeshRenderer");
        //rendaaja = komponentti as MeshRenderer;

        valo = gameObject.GetComponent("Light") as Light;

        valo.intensity = 0;

        InvokeRepeating("toggleVisible", 0f, 1.0f);
	}

    public void toggleVisible()
    {
        if (!toggle)
        {
            valo.intensity = 0;
            toggle = !toggle;
        }

        else
        {
            valo.intensity = 3;
            toggle = !toggle;
        }
    }
}
