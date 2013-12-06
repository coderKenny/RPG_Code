using UnityEngine;
using System.Collections;

public class HitScript : MonoBehaviour 
{
    tk2dTextMesh textMesh;	

	public Singleton sinkku;
	
	
    void Start()
    {
        sinkku = Singleton.Instance;
        textMesh = GetComponent<tk2dTextMesh>();
        //CenterMe();
    }

    //5 (miekan os) - armor (1)+dodge )0)
    void UpdateInfo(string text)
    {
        textMesh.text = text;
    }


    void CenterMe()
    {
        Vector3 where = transform.position;
        where.x = 3.4f;
        where.y = 8.6f;
        where.z = -5.34f;
        transform.localPosition = where;
    }
}
