using UnityEngine;
using System.Collections;

public class ProgressCode : MonoBehaviour 
{
    UISprite progressSprite;

    float progress = 1;
    float time = 0;
    bool up = true;

    bool down = true;

    int frames = 0;

	// Use this for initialization
	void Start () 
    {
        progressSprite = GetComponent<UISprite>();
        progressSprite.fillAmount = 1;
        
	}
	
	// Update is called once per frame
	void Update () 
    {
        //progressSprite.fillAmount=Mathf.PingPong(Time.time/5, 1);

        Debug.Log("fillAmount " + progressSprite.fillAmount);



        if (!up)
        {
            frames++;
            progress += Time.deltaTime / 5;
            progressSprite.fillAmount = progress;
            Debug.Log("Time flies : " + progress);
            time = Time.time;
        }

        else
        {
            frames++;
            progress -= Time.deltaTime / 5;
            progressSprite.fillAmount = progress;
            Debug.Log("Time flies : " + progress);
            time = Time.time;
        }

        //if(progressSprite.fillAmount>=1)
        //    up = false;

        //if(progressSprite.fillAmount<=0)
        //    up = true;

        Debug.Log("Redi : " + progressSprite.fillAmount + "\nframes : " + frames + "UP ? +" + up);
    }
}
