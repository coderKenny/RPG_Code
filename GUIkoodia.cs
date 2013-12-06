using UnityEngine;
using System.Collections;
using System;

using Vectrosity; // Drawing


public class GUIkoodia : MonoBehaviour 
{
    public Texture pushDownIndicator;

    private GUIStyle style;

    private Rect startRect;
    private Rect targetRect;

    public Camera playerCamera;

    private float currentWidth;
    private float currentHeight;

    public Singleton sinkku;

    private bool allowDrag = true;


	
	void Start () 
    {
        currentWidth = Screen.width / 3.5F;
        currentHeight = Screen.height / 4F;

        float kokoX = currentWidth + 80;
        float kokoY = currentHeight + 200;

        startRect = new Rect(Screen.width / 2 - kokoX / 2, Screen.height / 2 - kokoY / 2, kokoX, kokoY);


        sinkku = Singleton.Instance;


        Debug.Log("Funktion tulos : " + AspectRatios.GetAspectRatio());

	}
	
	
	void Update () 
    {
	
	}





    public void OnGUI()
    {
        
        //Vector2 pointA = new Vector2(Screen.width / 2, Screen.height / 2);
        //Vector3 vektori = pointA;

        //vektori.z = 50f;

    if (Input.touchCount > 0)
    {
        Vector3 withDepth = Input.GetTouch(0).position;
        Vector3 withDepth2 = Input.GetTouch(0).position;

        withDepth.z = 0f;


        withDepth2 = withDepth;

        withDepth2.z = -20;




        Rect cursorPos = new Rect(Input.GetTouch(0).position.x - 150, Screen.height - Input.GetTouch(0).position.y - 150, 300, 300);

        GUI.DrawTexture(cursorPos, pushDownIndicator);
    }


        // Make GUI box
        if(style == null)
        {
            style = new GUIStyle(GUI.skin.label);
            style.normal.textColor = Color.blue;
            style.alignment = TextAnchor.MiddleCenter;
            style.fontSize = 30;
        }

        // Register the window (window ID must be unique)
        startRect = GUI.Window(3, startRect, DoMyWindow, "");
    }



    void DoMyWindow(int windowID)
    {
        // Debug box content ->
        
        GUI.Label(new Rect(0, 0, startRect.width, startRect.height), "Ruudun leveys : "+Screen.width+"\nRuudun korkeus : "+Screen.height+"\nFunktion tulos : " + AspectRatios.GetAspectRatio(),style);
        
        if (allowDrag)
        {
            //GUI.DragWindow(new Rect(0, 0, Screen.width, Screen.height));
            // Drag from upper "bar"
            //GUI.DragWindow(new Rect(0, 0, 10000, 20));


            GUI.Button(new Rect(10, 8, 120, 20), "Arvoja");
            // Insert a huge dragging area at the end.
            // This gets clipped to the window (like all other controls) so you can never
            //  drag the window from outside it.
            GUI.DragWindow();
        }
    }
}
