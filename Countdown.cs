using UnityEngine;
using System.Collections;

public class Countdown : MonoBehaviour 
{
    //private float siirtyma;

	void Start () 
    {
        StartCoroutine(WaitAndPrint(0.2f));
    }

    void Update()
    {
        //siirtyma = Mathf.PingPong(Time.time, 3);

        //transform.position = new Vector3(Mathf.PingPong(Time.time, 2), transform.position.y, transform.position.z);

        
    }

    public IEnumerator WaitAndPrint(float waitTime)
    {            
        for (int i = 0; i < 1; i++)
        {
            yield return new WaitForSeconds(waitTime);               
            renderer.enabled = false;        
            
            yield return new WaitForSeconds(waitTime);
            renderer.enabled = true;
            
            yield return new WaitForSeconds(waitTime);
            renderer.enabled = false;    

            yield return new WaitForSeconds(waitTime);
            renderer.enabled = true;
            yield return new WaitForSeconds(waitTime);
            renderer.enabled = false;
            yield return new WaitForSeconds(waitTime);
            renderer.enabled = true;
            yield return new WaitForSeconds(waitTime);
            renderer.enabled = false;
            yield return new WaitForSeconds(waitTime);
            renderer.enabled = true;
            yield return new WaitForSeconds(waitTime);
            renderer.enabled = false;
      }
   }
}