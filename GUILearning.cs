using UnityEngine;
using System.Collections;

public class GUILearning : MonoBehaviour 
{
    public float top, left;



    private GUIStyle style;
    private GUIStyle style2;

    private bool allowDrag = true;

    //private Vector3 uusPositio = Vector3.zero;
    //private Vector3 p = Vector3.zero;

    //private Vector3 uusPositio2 = Vector3.zero;
    //private Vector3 p2 = Vector3.zero;

    //private Vector3 uusPositio3 = Vector3.zero;
    //private Vector3 p3 = Vector3.zero;

    //private Vector3 uusPositio4 = Vector3.zero;
    //private Vector3 p4 = Vector3.zero;

    //private int kosketuksia = 0;

    private Rect startRect;
    private Rect targetRect;

    private float currentWidth;
    private float currentHeight;


    private float topButtonOffset;
    private float leftButtonOffset;

    private float buttonSpacing;

    public long i=1;


    private bool toggler = false;

    void Start()
    {
        currentWidth = Screen.width / 3.5F;
        currentHeight = Screen.height / 4F;

        startRect = new Rect(10, 10, currentWidth + 300, currentHeight + 200); 
    }



	void OnGUI () 
    {

        top = Screen.height / 2;
        left = Screen.width / 2;

        topButtonOffset = top + 30; // Dynamic Y !

        leftButtonOffset = left + 10; // Static X ! 
        
        buttonSpacing= 30;
        
		// Make a background box
		GUI.Box(new Rect(left,top,100,90), "Loader Menu");

		// Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
		if(GUI.Button(new Rect(leftButtonOffset,topButtonOffset,80,20), "Level 1")) 
        {
			Application.LoadLevel(1);
		}

		// Make the second button.
		if(GUI.Button(new Rect(leftButtonOffset,topButtonOffset+=buttonSpacing,80,20), "Level 2")) 
        {
			Application.LoadLevel(2);
		}
        

        //Debug.Log("Käyty on " +(++i)+" kertaa ! Edellisen framen kesto : "+Time.deltaTime*1000+" ms");


        // Make GUI boxes
        if (style == null)
        {
            style = new GUIStyle(GUI.skin.label);
            style.normal.textColor = Color.red;
            style.alignment = TextAnchor.MiddleCenter;
            style.fontSize = 20;
        }

        // Make GUI box
        if (style2 == null)
        {
            style2 = new GUIStyle(GUI.skin.button);
            style2.normal.textColor = Color.blue;
            style2.alignment = TextAnchor.MiddleCenter;
            style2.fontSize = 30;
            style2.hover.textColor = Color.green;
            style2.onHover.textColor = Color.magenta;
        }


        // Register the window (window ID must be unique)
        startRect = GUI.Window(1, startRect, DoMyWindow, "");
    }



    void DoMyWindow(int windowID)
    {
      
        GUI.Label(new Rect(20, -50, startRect.width, startRect.height), "Top arvo : " + top+ "\nLeft arvo " + left+ "\nbuttonSpacing arvo : " + buttonSpacing + "\ntopButtonOffset arvo : " + topButtonOffset +"\nleftButtonOffset arvo : "+ leftButtonOffset + "\nAlkuRectangeli : \n" + startRect + "\nAudioListener bool : " + AudioListener.pause + "\nRayCastPlaneen muutettu pystyskaala (Z) : ",style);
        //GUI.Label(new Rect(0, 0, startRect.width, startRect.height), "isMuted arvo (negated) : " + !sinkku.giveMuteState() + "\nisPaused arvo : " + sinkku.isPaused() + "\nGravitaatio : " + Physics.gravity.y, style);
        
        if (allowDrag)
        {
            //GUI.DragWindow(new Rect(0, 0, Screen.width, Screen.height));
            // Drag from upper "bar"
            //GUI.DragWindow(new Rect(0, 0, 10000, 20));


            if (GUI.Button(new Rect(10, 8, 300, 20), "Arvoja"))
            {
                toggler = !toggler;
                Debug.Log("Painettu !!!");

            }

            switch (toggler)
            {
                case true :
                    style2.alignment = TextAnchor.MiddleRight;
                    style2.normal.textColor = Color.red;
                    GUI.Button(new Rect(100, 300, 300, 50), "ChangingButton",style2);
                    break;

                case false :
                    style2.alignment = TextAnchor.MiddleCenter;
                    style2.normal.textColor = Color.blue;
                    GUI.Button(new Rect(100, 300, 300, 50), "Button",style2);
                    break;

            }

            
            // Insert a huge dragging area at the end.
            // This gets clipped to the window (like all other controls) so you can never
            //  drag the window from outside it.
            GUI.DragWindow();
        }
	}
}