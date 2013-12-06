using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]

public class Invocation : MonoBehaviour 
{
    private MeshRenderer rendaaja;
    private Component komponentti;

    public AnimationCurve XCurve;

    public float TotalTravelTime = 5.0f;

    public float TravelSpeed = 50.0f;

    public float XRange = 10.0f;

	// Use this for initialization
	void Start () 
    {
        rendaaja = gameObject.GetComponent("MeshRenderer") as MeshRenderer; // Must be casted from Component to MeshRenderer
        rendaaja = gameObject.GetComponent<MeshRenderer>();

        komponentti = gameObject.GetComponent("MeshRenderer");

        rendaaja = komponentti as MeshRenderer;

        InvokeRepeating("toggleVisible", 0f, 1.0f);

        //StartCoroutine("Travel");

        //Invoke("toggleVisible", 5);
	}

    public void toggleVisible()
    {
        rendaaja.enabled =! rendaaja.enabled;
    }


    IEnumerator Travel()
    {
        float ElapsedTime = 0.0f;

        while (ElapsedTime < TotalTravelTime)
        {
            float xPos = XCurve.Evaluate(ElapsedTime / TotalTravelTime) * XRange; // Generates the position for the object in x-axis
            transform.position = new Vector3(xPos, transform.position.y, transform.position.z + TravelSpeed * -Time.deltaTime);    // Update the position

            yield return null;

            ElapsedTime += Time.deltaTime;
        }
    }

}
