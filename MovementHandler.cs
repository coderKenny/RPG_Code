using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using Vectrosity; // Drawing

public class MovementHandler : MonoBehaviour 
{
    private GameObject aloitusBlokki;
    public List<GameObject> allMovementBlocks;
    public Singleton sinkku;
    public GameObject player;
    private RaycastHit hit;
    public Animation playerAnim;
    //private bool isMoving = false;
    private Vector3 TranslateSpeed = Vector3.zero;
    private float MaxZ = -0.5f;
    private Camera mainCam;

	
	void Start () 
    {
        GameObject.Find("Spotlight2").GetComponent<Light>().enabled = false;
        mainCam = Camera.main;
        TranslateSpeed.z = 5f;
        sinkku = Singleton.Instance;
        playerAnim = GameObject.FindGameObjectWithTag("Pelaaja").GetComponent<Animation>();

        //playerHPmesh = transform.FindChild("PlayerHP").gameObject;

        allMovementBlocks = new List<GameObject>();
        allMovementBlocks.Add(GameObject.FindGameObjectWithTag("MoveCollider1"));
        allMovementBlocks.Add(GameObject.FindGameObjectWithTag("MoveCollider2"));
        allMovementBlocks.Add(GameObject.FindGameObjectWithTag("MoveCollider3"));
        
        Ray ray = new Ray();
        ray.origin = player.transform.position;
        ray.direction = -transform.up;
        Physics.Raycast(ray, out hit, 100f);
        //ActiveColliders[0] = hit.collider.gameObject;

        //hit.collider.gameObject.GetComponent<BoxCollider>().enabled = false;
        
        //generalMask = 1 << hit.collider.gameObject.layer;


	}
	
    void Update()
    {
        //Debug.Log("APt : " + sinkku.ap);
        Ray checkupRay = new Ray();
        RaycastHit checkupHit = new RaycastHit();
        checkupRay.origin = player.transform.position;
        checkupRay.direction = -transform.up;

        Physics.Raycast(checkupRay, out checkupHit, 100f);

        Debug.DrawRay(player.transform.position, -transform.up * 100f, Color.red, 1f);

        #region Checks for lights

        // Shut down lights if unable to move there
        if (checkupHit.collider.gameObject.tag == "MoveCollider3" && sinkku.ap == 5)
        {
            GameObject.Find("Spotlight2").GetComponent<Light>().enabled = true;
            GameObject.Find("Spotlight1").GetComponent<Light>().enabled = false;
            GameObject.Find("Spotlight3").GetComponent<Light>().enabled = false;
        }

        if (checkupHit.collider.gameObject.tag == "MoveCollider1" && sinkku.ap == 5)
        {
            GameObject.Find("Spotlight2").GetComponent<Light>().enabled = true;
            GameObject.Find("Spotlight1").GetComponent<Light>().enabled = false;
            GameObject.Find("Spotlight3").GetComponent<Light>().enabled = false;
        }


        if (checkupHit.collider.gameObject.tag == "MoveCollider3" && sinkku.ap >= 10)
        {
            GameObject.Find("Spotlight2").GetComponent<Light>().enabled = true;
            GameObject.Find("Spotlight1").GetComponent<Light>().enabled = true;
            GameObject.Find("Spotlight3").GetComponent<Light>().enabled = false;
        }

        if (checkupHit.collider.gameObject.tag == "MoveCollider1" && sinkku.ap >= 10)
        {
            GameObject.Find("Spotlight2").GetComponent<Light>().enabled = true;
            GameObject.Find("Spotlight1").GetComponent<Light>().enabled = false;
            GameObject.Find("Spotlight3").GetComponent<Light>().enabled = true;
        }


        if (sinkku.ap == 0)
        {
            GameObject.Find("Spotlight2").GetComponent<Light>().enabled = false;
            GameObject.Find("Spotlight1").GetComponent<Light>().enabled = false;
            GameObject.Find("Spotlight3").GetComponent<Light>().enabled = false;
        
        }

        if (checkupHit.collider.gameObject.tag == "MoveCollider2" && sinkku.ap != 0)
        {
            GameObject.Find("Spotlight2").GetComponent<Light>().enabled = false;
            GameObject.Find("Spotlight1").GetComponent<Light>().enabled = true;
            GameObject.Find("Spotlight3").GetComponent<Light>().enabled = true;

        }

        #endregion

        Debug.DrawRay(player.transform.position, -transform.up * 5f, Color.magenta, 1f);

        if (sinkku.playerTurn && !sinkku.isMoving && sinkku.ap != 0 && mainCam.enabled == true)
        {
              
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Input.GetMouseButtonDown(0))
            {
                Physics.Raycast(ray, out hit, Mathf.Infinity);
                Debug.DrawRay(ray.origin, ray.direction * 20, Color.blue, 1f);
                //Debug.Log("Ray info :  " + hit.collider.tag);
                Vector3 temp = ray.origin;
                temp.x -= 0.2f;
                //temp.y -= 0.5f;

               // VectorLine.SetRay(Color.yellow, 5f, temp, ray.direction * 20);

                ////Debug.Log("MAski on, boss mask : " + finalMask + " , " + bossMask);
                int downMask = 1 << 20;
                int upMask = 1 << 22;

                int centerMask =  1 << 21;

                

                Ray playerRay = new Ray();
                RaycastHit playerHit = new RaycastHit();
                playerRay.origin = player.transform.position;
                playerRay.direction = -transform.up;


                Physics.Raycast(playerRay, out playerHit, 100f);

               

                if (playerHit.collider.gameObject.tag=="MoveCollider2")         // Center
                {

                    if (sinkku.ap != 0 && Physics.Raycast(ray, out hit, 50f, upMask))
                    {
                        //Debug.Log("Out info boss haarassa :  " + hit.collider.tag);
                        moveUp();
                    }

                    else if (sinkku.ap != 0 && Physics.Raycast(ray, out hit, 50f, downMask))
                    {
                        //Debug.Log("Out info boss haarassa :  " + hit.collider.tag);
                        moveDown();
                    }

                }

                else if (playerHit.collider.gameObject.tag == "MoveCollider3")  // Upper block
                {
                    if (sinkku.ap != 0 && Physics.Raycast(ray, out hit, 50f, downMask))
                    {
                        //Debug.Log("Out info boss haarassa :  " + hit.collider.tag);
                        move(hit);
                    }

                    else if (sinkku.ap != 0 && Physics.Raycast(ray, out hit, 50f, centerMask))
                    {
                        //Debug.Log("Out info boss haarassa :  " + hit.collider.tag);
                        move(hit);
                    }
                }


                else if (playerHit.collider.gameObject.tag == "MoveCollider1")  // Lower block
                {
                    if (sinkku.ap != 0 && Physics.Raycast(ray, out hit, 50f, upMask))
                    {
                        //Debug.Log("Out info boss haarassa :  " + hit.collider.tag);
                        move2(hit);
                    }

                    else if (sinkku.ap != 0 && Physics.Raycast(ray, out hit, 50f, centerMask))
                    {
                        //Debug.Log("Out info boss haarassa :  " + hit.collider.tag);
                        Debug.Log("Info1 : check ");
                        move2(hit);
                    }
                }
            }
        }
    }


    void move(RaycastHit hit)
    {
        sinkku.isMoving = true;
        //player.transform.Translate(0, 0, 50f * Time.deltaTime);
        if(hit.collider.gameObject.tag=="MoveCollider2")
            StartCoroutine("MovePlayerUpToCenter");

        else if (sinkku.ap >= 10)
            StartCoroutine("MovePlayerUpToDown");

    }

    void move2(RaycastHit hit)
    {
        sinkku.isMoving = true;
        Debug.Log("Info2 : check ");
        //player.transform.Translate(0, 0, 50f * Time.deltaTime);
        if (hit.collider.gameObject.tag == "MoveCollider2")
        {
            Debug.Log("Info3 : check ");
            StartCoroutine("MovePlayerDownToCenter");
        }

        else if (sinkku.ap >= 10)
            StartCoroutine("MovePlayerDownToUp");

    }


    void moveUp()
    {
        sinkku.isMoving = true;
        //player.transform.Translate(0, 0, 50f * Time.deltaTime);
        StartCoroutine("MovePlayerUp");
        //transform.Translate(0,0, -speed, Space.World);
 
    }

    void moveDown()
    {
        sinkku.isMoving = true;
        //player.transform.Translate(0, 0, 50f * Time.deltaTime);
       // transform.Translate(0, 0, speed, Space.World);
        StartCoroutine("MovePlayerDown");

    }

    #region Coroutines

    IEnumerator MovePlayerUp()
    {
        //yield return new WaitForSeconds(2.0f);

        transform.Rotate(0, -90, 0);
        //playerHPmesh.transform.Rotate(0, 90, 0);
        //playerHPmesh.transform.Translate(0f, 0.7f, 0f);

        playerAnim["walk"].wrapMode = WrapMode.Loop;

        playerAnim.Play("walk");

            
        while (player.transform.position.z < MaxZ)
        {
            transform.Translate(TranslateSpeed.x * Time.deltaTime, TranslateSpeed.y * Time.deltaTime, TranslateSpeed.z * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        transform.Rotate(0, 90, 0);
        //playerHPmesh.transform.Rotate(0, -90, 0);
        //playerHPmesh.transform.Translate(0f, -0.7f, 0f);

        playerAnim["idle"].wrapMode = WrapMode.Loop;

        playerAnim.Play("idle");

        sinkku.ap -= 5;


        GameObject.Find("Spotlight2").GetComponent<Light>().enabled = true;
        GameObject.Find("Spotlight3").GetComponent<Light>().enabled = false;

        Debug.Log("Sequence Completed");
        sinkku.isMoving = false;
    }

    IEnumerator MovePlayerDown()
    {
        //TranslateSpeed.z *= -1;
        //yield return new WaitForSeconds(2.0f);

        transform.Rotate(0, 90, 0);
        //playerHPmesh.transform.Rotate(0, -90, 0);
        //playerHPmesh.transform.Translate(0f, -0.7f, 0f);

        playerAnim["walk"].wrapMode = WrapMode.Loop;

        playerAnim.Play("walk");


        while (player.transform.position.z > -10)
        {
            transform.Translate(TranslateSpeed.x * Time.deltaTime, TranslateSpeed.y * Time.deltaTime, TranslateSpeed.z * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        transform.Rotate(0, -90, 0);
        //playerHPmesh.transform.Rotate(0, 90, 0);
        //playerHPmesh.transform.Translate(0f, 0.7f, 0f);

        playerAnim["idle"].wrapMode = WrapMode.Loop;

        playerAnim.Play("idle");

        sinkku.ap -= 5;

        GameObject.Find("Spotlight2").GetComponent<Light>().enabled = true;
        GameObject.Find("Spotlight1").GetComponent<Light>().enabled = false;

        Debug.Log("Sequence Completed");
        sinkku.isMoving = false;
        //TranslateSpeed.z *= -1;
    }

    IEnumerator MovePlayerUpToCenter()
    {
        //TranslateSpeed.z *= -1;
        //yield return new WaitForSeconds(2.0f);

        transform.Rotate(0, 90, 0);
        //playerHPmesh.transform.Rotate(0, -90, 0);
        //playerHPmesh.transform.Translate(0f, -0.7f, 0f);

        playerAnim["walk"].wrapMode = WrapMode.Loop;

        playerAnim.Play("walk");


        while (player.transform.position.z > -5.5f)
        {
            transform.Translate(TranslateSpeed.x * Time.deltaTime, TranslateSpeed.y * Time.deltaTime, TranslateSpeed.z * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        transform.Rotate(0, -90, 0);
        //playerHPmesh.transform.Rotate(0, 90, 0);
        //playerHPmesh.transform.Translate(0f, 0.7f, 0f);

        playerAnim["idle"].wrapMode = WrapMode.Loop;

        playerAnim.Play("idle");

        sinkku.ap -= 5;

        GameObject.Find("Spotlight2").GetComponent<Light>().enabled = false;
        GameObject.Find("Spotlight1").GetComponent<Light>().enabled = true;
        GameObject.Find("Spotlight3").GetComponent<Light>().enabled = true;


        Debug.Log("Sequence Completed");
        sinkku.isMoving = false;
        //TranslateSpeed.z *= -1;
    }

    IEnumerator MovePlayerDownToCenter()
    {
        Debug.Log("Info4 : check ");
        //TranslateSpeed.z *= -1;
        //yield return new WaitForSeconds(2.0f);

        transform.Rotate(0, -90, 0);
        //playerHPmesh.transform.Rotate(0, -90, 0);
        //playerHPmesh.transform.Translate(0f, -0.7f, 0f);

        playerAnim["walk"].wrapMode = WrapMode.Loop;

        playerAnim.Play("walk");


        while (player.transform.position.z < -5.5f)
        {
            transform.Translate(TranslateSpeed.x * Time.deltaTime, TranslateSpeed.y * Time.deltaTime, TranslateSpeed.z * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        transform.Rotate(0, 90, 0);
        //playerHPmesh.transform.Rotate(0, 90, 0);
        //playerHPmesh.transform.Translate(0f, 0.7f, 0f);

        playerAnim["idle"].wrapMode = WrapMode.Loop;

        playerAnim.Play("idle");

        sinkku.ap -= 5;

        GameObject.Find("Spotlight2").GetComponent<Light>().enabled = false;
        GameObject.Find("Spotlight1").GetComponent<Light>().enabled = true;
        GameObject.Find("Spotlight3").GetComponent<Light>().enabled = true;


        Debug.Log("Sequence Completed");
        sinkku.isMoving = false;
        //TranslateSpeed.z *= -1;
    }

    IEnumerator MovePlayerUpToDown()
    {
        //TranslateSpeed.z *= -1;
        //yield return new WaitForSeconds(2.0f);

        transform.Rotate(0, 90, 0);
        //playerHPmesh.transform.Rotate(0, -90, 0);
        //playerHPmesh.transform.Translate(0f, -0.7f, 0f);

        playerAnim["walk"].wrapMode = WrapMode.Loop;

        playerAnim.Play("walk");


        while (player.transform.position.z > -10)
        {
            transform.Translate(TranslateSpeed.x * Time.deltaTime, TranslateSpeed.y * Time.deltaTime, TranslateSpeed.z * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        transform.Rotate(0, -90, 0);
        //playerHPmesh.transform.Rotate(0, 90, 0);
        //playerHPmesh.transform.Translate(0f, 0.7f, 0f);

        playerAnim["idle"].wrapMode = WrapMode.Loop;

        playerAnim.Play("idle");

        sinkku.ap -= 10;

        GameObject.Find("Spotlight2").GetComponent<Light>().enabled = true;
        GameObject.Find("Spotlight1").GetComponent<Light>().enabled = false;
        GameObject.Find("Spotlight3").GetComponent<Light>().enabled = true;


        Debug.Log("Sequence Completed");
        sinkku.isMoving = false;
        //TranslateSpeed.z *= -1;
    }

    IEnumerator MovePlayerDownToUp()
    {
        //TranslateSpeed.z *= -1;
        //yield return new WaitForSeconds(2.0f);

        transform.Rotate(0, -90, 0);
        //playerHPmesh.transform.Rotate(0, -90, 0);
        //playerHPmesh.transform.Translate(0f, -0.7f, 0f);

        playerAnim["walk"].wrapMode = WrapMode.Loop;

        playerAnim.Play("walk");


        while (player.transform.position.z < MaxZ)
        {
            transform.Translate(TranslateSpeed.x * Time.deltaTime, TranslateSpeed.y * Time.deltaTime, TranslateSpeed.z * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        transform.Rotate(0, 90, 0);
        //playerHPmesh.transform.Rotate(0, 90, 0);
        //playerHPmesh.transform.Translate(0f, 0.7f, 0f);

        playerAnim["idle"].wrapMode = WrapMode.Loop;

        playerAnim.Play("idle");

        sinkku.ap -= 10;

        GameObject.Find("Spotlight2").GetComponent<Light>().enabled = true;
        GameObject.Find("Spotlight1").GetComponent<Light>().enabled = true;
        GameObject.Find("Spotlight3").GetComponent<Light>().enabled = false;


        Debug.Log("Sequence Completed");
        sinkku.isMoving = false;
        //TranslateSpeed.z *= -1;
    }

    #endregion
}