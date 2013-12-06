using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


using Vectrosity; // Drawing

public class ButtonReactor : MonoBehaviour
{
    #region Variables

    public Animation cubeAnim; 
    public Singleton sinkku;
    public AudioClip osumaSoundi;
    public GameObject pelaaja;
	public Camera playerCamera;
	private bool cooled = false;
    public GameObject[] silmaluvut;
    public GameObject damageIndicator;
    private float hitLifeSpan = 2.0f;
    public Transform Vihu1;
    public GameObject Vihu2;
    private RaycastHit hit;
    private int enemyToHit;
    public int weaponTurbo = 0;
    private bool bossHit = false;
    private int bossMask = 1 << 15;
    private int layerMask1 = 1 << 11;
    private int layerMask2 = 1 << 12;
    private int layerMask3 = 1 << 13;
    private int finalMask;
    private int generalMask;
    public List<GameObject> ActiveColliders;
    public List<GameObject> Enemies;
    public int general_enemy_amount=0;



    #endregion

    #region Unity methods

    void Start()
    {

        // Default
        finalMask = layerMask1;

        if (Application.loadedLevelName == "KolmeTavista")
        {
            foreach (GameObject enemy in Enemies)
            {
                general_enemy_amount++;
            }
        }



        if (general_enemy_amount == 3)
        {
            generalMask = layerMask1 | layerMask2 | layerMask3;
        }


        // Jos vain yksi normivihu kehissä
        if (Singleton.enemyCount == 1 && general_enemy_amount == 0)
        {
            finalMask = layerMask1;
        }


        // Kax normia kehissä ?
        if (Singleton.enemyCount == 2 && general_enemy_amount == 0)
        {


            finalMask = layerMask2 | layerMask3;
        }

        // Kax normia kehissä + boss ?
        if (Singleton.enemyCount == 3 && general_enemy_amount == 0)
        {
            finalMask = layerMask2 | layerMask3;
        }

 
    }

    void Awake()
    {
        loadDiceResources();
        sinkku = Singleton.Instance;
        playerCamera = Camera.main;
        pelaaja = GameObject.FindGameObjectWithTag("Pelaaja"); 
	}

    void Update()
    {
        
        if (!pelaaja.GetComponent<Player>().isItAlive())
            GameObject.FindGameObjectWithTag("HitCollider1").collider.enabled = false;


        if (sinkku.playerTurn && !sinkku.isMoving)
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

            if (Input.GetMouseButtonDown(0) && !cooled)
            {
                Physics.Raycast(ray, out hit, Mathf.Infinity);
                Debug.DrawRay(ray.origin, ray.direction * 20, Color.blue, 1f);
                //Debug.Log("Ray info :  " + ray);
                Vector3 temp = ray.origin;
                temp.x -= 0.2f;
                //temp.y -= 0.5f;

                //VectorLine.SetRay(Color.yellow, 5f, temp, ray.direction * 20);

                //Debug.Log("MAski on, boss mask : " + finalMask + " , " + bossMask);

                // PääVihun kurmootus
                if (sinkku.ap != 0 && Physics.Raycast(ray, out hit, 50f, bossMask))
                {
                    Debug.Log("Out info boss haarassa :  " + hit.collider.tag);
                    punishTheBoss();
                }


                // Geneerinen kurmootus
                if (sinkku.ap != 0 && Physics.Raycast(ray, out hit, 50f, generalMask) && general_enemy_amount != 0)
                {
                    Debug.Log("Out info general haarassa :  " + hit.collider.tag);

                    GameObject enemy=null;
                    
                    // In case there are three enemies this is fine
                    if(Singleton.enemyCount == 3 && general_enemy_amount==0)
                    {
                        if (hit.collider.tag == "HitCollider1")
                            enemy = Enemies[0];

                        if (hit.collider.tag == "HitCollider2")
                            enemy = Enemies[1];

                        if (hit.collider.tag == "HitCollider3")
                            enemy = Enemies[2];


                        punishGeneric(enemy);

                    }

                    else
                    {

                        if (Enemies.Count == 1)
                        {
                            enemy = Enemies[0];
                            punishGeneric(enemy);
                        }

                        else if (Enemies.Count == 2)
                        {

                            if (hit.collider.tag == ActiveColliders[0].tag)
                            {
                                enemy = Enemies[0];
                                punishGeneric(enemy);
                            }

                            else
                            {
                                enemy = Enemies[1];
                                punishGeneric(enemy);
                            }
                        }


                        //three
                        else
                        {
                            if (hit.collider.tag == ActiveColliders[0].tag)
                            {
                                enemy = Enemies[0];
                                punishGeneric(enemy);
                            }

                            else if (hit.collider.tag == ActiveColliders[1].tag)
                            {
                                enemy = Enemies[1];
                                punishGeneric(enemy);
                            }

                            else
                            {
                                enemy = Enemies[2];
                                punishGeneric(enemy);
                            }
                        }
                    }
                }



                if (sinkku.ap != 0 && Physics.Raycast(ray, out hit, 50f, finalMask) && !cooled && general_enemy_amount == 0)
                {
                    Debug.Log("Out info haarassa kovakoodattu :  " + hit.collider.tag);

                    enemyToHit = 1;

                    if (hit.collider.tag == "HitCollider2")
                        enemyToHit = 2;

                    if (hit.collider.tag == "HitCollider3")
                        enemyToHit = 3;

                    cooled = true;
                    cubeAnim = GameObject.FindGameObjectWithTag("Pelaaja").GetComponent<Animation>();


                    cubeAnim["attack1"].wrapMode = WrapMode.Once;
                    cubeAnim.Play("attack1");

                    StartCoroutine(WaitAndPrint(0.95f));
                }
            }
        }
    }


    #endregion

    #region Custom methods

    void ResetColliders(int vihuja)
    {
        if (vihuja == 1)
        {
            Ray ray = new Ray();
            ray.origin = Enemies[0].transform.position;
            ray.direction = -transform.up;
            Physics.Raycast(ray, out hit, Mathf.Infinity);
            ActiveColliders[0] = hit.collider.gameObject;

            generalMask = 1 << hit.collider.gameObject.layer;
        }
    
    }


    public void ResetColliders(GameObject first, GameObject second)
    {           
        Ray ray = new Ray();            
        ray.origin = first.transform.position;           
        ray.direction = -transform.up;            
        Physics.Raycast(ray, out hit, Mathf.Infinity);            
        ActiveColliders[0] = hit.collider.gameObject;    
        generalMask = 1 << hit.collider.gameObject.layer;



        ray = new Ray();
        ray.origin = second.transform.position;
        ray.direction = -transform.up;
        Physics.Raycast(ray, out hit, Mathf.Infinity);
        ActiveColliders[1] = hit.collider.gameObject;
        generalMask =generalMask | (1 << hit.collider.gameObject.layer);


    }

    public void ResetColliders(GameObject[] vihutaulu)
    {
        Ray ray = new Ray();
        ray.origin = vihutaulu[0].transform.position;
        ray.direction = -transform.up;
        Physics.Raycast(ray, out hit, Mathf.Infinity);
        ActiveColliders[0] = hit.collider.gameObject;
        generalMask = 1 << hit.collider.gameObject.layer;

        ray = new Ray();
        ray.origin = vihutaulu[1].transform.position;
        ray.direction = -transform.up;
        Physics.Raycast(ray, out hit, Mathf.Infinity);
        ActiveColliders[1] = hit.collider.gameObject;
        generalMask = generalMask | (1 << hit.collider.gameObject.layer);

        ray = new Ray();
        ray.origin = vihutaulu[2].transform.position;
        ray.direction = -transform.up;
        Physics.Raycast(ray, out hit, Mathf.Infinity);
        ActiveColliders[2] = hit.collider.gameObject;
        generalMask = generalMask | (1 << hit.collider.gameObject.layer);


    }



    public void SetEnemy1(GameObject vihu)
    {


        // When to create new
        if (Enemies.Count == 3 && general_enemy_amount == 0)
        {
            Enemies = new List<GameObject>(Singleton.enemyCount);
            ActiveColliders = new List<GameObject>(Singleton.enemyCount);
        }

        Enemies.Add(vihu);

     


        GameObject collider;
        RaycastHit hit;

        Ray ray = new Ray(vihu.transform.position, -transform.up);


        if (Physics.Raycast(ray, out hit, 100))
        {
            collider = hit.transform.gameObject;

            Debug.DrawLine(ray.origin, hit.point);
            ActiveColliders.Add(collider);
        }
        general_enemy_amount++;



        // One general
        if (general_enemy_amount == 1)
        {
            generalMask = 1 << ActiveColliders[0].gameObject.layer;

        }

        // Two general
        if (general_enemy_amount == 2)
        {
            generalMask = 1 << ActiveColliders[0].gameObject.layer;
            int tempMask = 1 << ActiveColliders[1].gameObject.layer;

            generalMask = generalMask | tempMask;
        }

        // Three general
        if (general_enemy_amount == 3)
        {
            generalMask = 1 << ActiveColliders[0].gameObject.layer;
            int tempMask = 1 << ActiveColliders[1].gameObject.layer;
            int tempMask2 = 1 << ActiveColliders[2].gameObject.layer;

            generalMask = generalMask | tempMask | tempMask2;
        }

    }

    public GameObject[] FindGameObjectsWithLayer(int layer)
    {
        GameObject[] goArray = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        List<GameObject> goList = new List<GameObject>();
        for (int i = 0; i < goArray.Length; i++)
        {
            if (goArray[i].layer == layer)
            {
                goList.Add(goArray[i]);
            }
        }
        if (goList.Count == 0)
        {
            return null;
        }
        return goList.ToArray();
    }

    public void countEnemyAmount()
    {
        GameObject[] vihuTaulu = FindGameObjectsWithLayer(14);
        //Singleton.enemyCount = 3;

        Debug.Log("Vihutaulun koko : " + vihuTaulu.Length);  
        Singleton.enemyCount = vihuTaulu.Length;
    }
    

    public void ToggleCooled()
    {
        cooled = !cooled;
    }

    void punishTheBoss()
    {
        bossHit = true;
        cooled = true;
        //Debug.Log("Out info :  " + hit.collider.tag);

        cubeAnim = GameObject.FindGameObjectWithTag("Pelaaja").GetComponent<Animation>();


        cubeAnim["attack1"].wrapMode = WrapMode.Once;
        cubeAnim.Play("attack1");

        StartCoroutine(Venaa(0.95f));
    }

    void punishGeneric(GameObject enemy)
    {
        cooled = true;

        cubeAnim = GameObject.FindGameObjectWithTag("Pelaaja").GetComponent<Animation>();


        cubeAnim["attack1"].wrapMode = WrapMode.Once;
        cubeAnim.Play("attack1");

        StartCoroutine(Venaa(0.95f,enemy));

    }

    // Venaa overloads -->
    public IEnumerator Venaa(float time)
    {
        countPossibleHit();
        yield return new WaitForSeconds(time);

        cubeAnim["idle"].wrapMode = WrapMode.Loop;
        cubeAnim.Play("idle");

        // Consume AP ->

        sinkku.ap -= 5;
        if (sinkku.ap <= 0)
            sinkku.ap = 0;

        cooled = false;
        bossHit = false;
    }
    public IEnumerator Venaa(float time,GameObject vihu)
    {
        countPossibleGenericHit(vihu);
        yield return new WaitForSeconds(time);

        cubeAnim["idle"].wrapMode = WrapMode.Loop;
        cubeAnim.Play("idle");

        // Consume AP ->

        sinkku.ap -= 5;
        if (sinkku.ap <= 0)
            sinkku.ap = 0;

        cooled = false;
    }


    public void rendaaNoppa(int silmaLuku)
    {
        Vector3 paikka = Vector3.zero;

        Quaternion rotaatio;

        paikka = silmaluvut[3].transform.position;
        rotaatio = silmaluvut[silmaLuku].transform.rotation;


        GameObject noppaklooni=GameObject.FindGameObjectWithTag("Noppaklooni");

        Destroy(noppaklooni);

        //if (noppaklooni != null)


        noppaklooni = Instantiate(silmaluvut[silmaLuku],paikka,rotaatio) as GameObject;

        noppaklooni.tag = "Noppaklooni";
    }
   
    public IEnumerator WaitAndPrint(float waitTime)
    {
        countPossibleHit();
        yield return new WaitForSeconds(waitTime);
        //GameObject.FindGameObjectWithTag("DodgeButton").collider.enabled = true;
        //GameObject.FindGameObjectWithTag("HitCollider1").collider.enabled = true;
        cubeAnim["idle"].wrapMode = WrapMode.Loop;
        cubeAnim.Play("idle");

        // Consume AP ->

        sinkku.ap -= 5;
        if (sinkku.ap <= 0)
            sinkku.ap = 0;
		
        
		cooled = false;
        
    }

    public void loadDiceResources()
    {
        silmaluvut = new GameObject[10];

        silmaluvut[0] = Resources.Load("d10_low_1") as GameObject;
        silmaluvut[1] = Resources.Load("d10_low_2") as GameObject;
        silmaluvut[2] = Resources.Load("d10_low_3") as GameObject;
        silmaluvut[3] = Resources.Load("d10_low_4") as GameObject;
        silmaluvut[4] = Resources.Load("d10_low_5") as GameObject;
        silmaluvut[5] = Resources.Load("d10_low_6") as GameObject;
        silmaluvut[6] = Resources.Load("d10_low_7") as GameObject;
        silmaluvut[7] = Resources.Load("d10_low_8") as GameObject;
        silmaluvut[8] = Resources.Load("d10_low_9") as GameObject;
        silmaluvut[9] = Resources.Load("d10_low_0") as GameObject;

        //DamageNoppa = Resources.Load("d10_low_4_enemyDamage") as GameObject;

    }

    public bool countPossibleHit()
    {
        int dice = UnityEngine.Random.Range(1, 11);

        rendaaNoppa(dice - 1);

        //int result;
        string info;
        int damageInflicted = 0;
        int weaponHit = 5;
        int weaponDamage = 5;

        //result = dice + sinkku.dex - 3;

        pelaaja.audio.Play();

        if (sinkku.weapon == Singleton.Weapons.sword)
        {
            weaponHit = 2;
            weaponDamage = 5;
        }

        if (sinkku.weapon == Singleton.Weapons.dagger)
        {
            weaponHit = 0;
            weaponDamage = 3;
        }

        if (sinkku.weapon == Singleton.Weapons.axe)
        {
            weaponHit = 4;
            weaponDamage = 7;
        }


        info = "^3 Player hit math : " + dice + " <= " + sinkku.dex + " + " + weaponHit;


        if (dice <= sinkku.dex + weaponHit)
        {
            // Deal damage !!

            //info = "Matikka : " + dice + " + " + sinkku.dex +" - "+ weaponHit + " -> HIT !!";
            AudioSource.PlayClipAtPoint(osumaSoundi, pelaaja.transform.position);

            // Initially Troll (1)
            GameObject vihu = GameObject.FindGameObjectWithTag("Troll");



            if (enemyToHit == 1)
                vihu = GameObject.FindGameObjectWithTag("Troll");

            if (enemyToHit == 3)
                vihu = GameObject.FindGameObjectWithTag("Troll2");

            if (enemyToHit == 2)
                vihu = GameObject.FindGameObjectWithTag("Troll3");

            if (bossHit)
                vihu = GameObject.FindGameObjectWithTag("Boss");



            int hitDice = UnityEngine.Random.Range(1, 11);



            int armorInt = vihu.GetComponent<EnemyScript>().GetArmorInt();

            hitDice = hitDice - armorInt;



            damageInflicted = hitDice;

            // Can be a negative damage !

            if (damageInflicted <= 0)
                damageInflicted = 0;

            if (hitDice > sinkku.str + weaponDamage)
            {
                damageInflicted = sinkku.str + weaponDamage;
            }



            vihu.GetComponent<EnemyScript>().dealDamage(damageInflicted + weaponTurbo);

            GameObject.FindGameObjectWithTag("HitInfo").SendMessage("UpdateInfo", info);


            GameObject kahva = null;


            // POSSIBLY EXPENSIVE !!!!
            Destroy(GameObject.Find("DamageIndicator(Clone)"));


            damageIndicator.GetComponent<tk2dTextMesh>().text = damageInflicted.ToString();

            Vector3 temp = vihu.transform.position;

            temp.z -= 1;
            temp.x -= 1.5f;

            kahva = Instantiate(damageIndicator, temp, transform.rotation) as GameObject;

            kahva.transform.Rotate(90, 0, 0);

            Destroy(kahva.gameObject, hitLifeSpan);

            return true;
        }

        else
        {
            //info = "Matikka : " + dice + " + " + sinkku.dex + " - " + weaponHit + " -> MISS !!";
            GameObject.FindGameObjectWithTag("HitInfo").SendMessage("UpdateInfo", info);
            return false;
        }
    }

    public bool countPossibleGenericHit(GameObject vihu)
    {
        int dice = UnityEngine.Random.Range(1, 11);

        rendaaNoppa(dice - 1);

        //int result;
        string info;
        int damageInflicted = 0;
        int weaponHit = 5;
        int weaponDamage = 5;

        //result = dice + sinkku.dex - 3;

        pelaaja.audio.Play();

        if (sinkku.weapon == Singleton.Weapons.sword)
        {
            weaponHit = 2;
            weaponDamage = 5;
        }

        if (sinkku.weapon == Singleton.Weapons.dagger)
        {
            weaponHit = 0;
            weaponDamage = 3;
        }

        if (sinkku.weapon == Singleton.Weapons.axe)
        {
            weaponHit = 4;
            weaponDamage = 7;
        }


        info = "^3 Player hit math : " + dice + " <= " + sinkku.dex + " + " + weaponHit;


        if (dice <= sinkku.dex + weaponHit)
        {
            // Deal damage !!

            //info = "Matikka : " + dice + " + " + sinkku.dex +" - "+ weaponHit + " -> HIT !!";
            AudioSource.PlayClipAtPoint(osumaSoundi, pelaaja.transform.position);


            int hitDice = UnityEngine.Random.Range(1, 11);



            int armorInt = vihu.GetComponent<EnemyScript>().GetArmorInt();

            hitDice = hitDice - armorInt;



            damageInflicted = hitDice;

            // Can be a negative damage !

            if (damageInflicted <= 0)
                damageInflicted = 0;

            if (hitDice > sinkku.str + weaponDamage)
            {
                damageInflicted = sinkku.str + weaponDamage;
            }



            vihu.GetComponent<EnemyScript>().dealDamage(damageInflicted + weaponTurbo);

            GameObject.FindGameObjectWithTag("HitInfo").SendMessage("UpdateInfo", info);


            GameObject kahva = null;


            // POSSIBLY EXPENSIVE !!!!
            Destroy(GameObject.Find("DamageIndicator(Clone)"));


            damageIndicator.GetComponent<tk2dTextMesh>().text = damageInflicted.ToString();

            Vector3 temp = vihu.transform.position;

            temp.z -= 1;
            temp.x -= 1.5f;

            kahva = Instantiate(damageIndicator, temp, transform.rotation) as GameObject;

            kahva.transform.Rotate(90, 0, 0);

            Destroy(kahva.gameObject, hitLifeSpan);

            return true;
        }

        else
        {
            //info = "Matikka : " + dice + " + " + sinkku.dex + " - " + weaponHit + " -> MISS !!";
            GameObject.FindGameObjectWithTag("HitInfo").SendMessage("UpdateInfo", info);
            return false;
        }
    }

    #endregion
}
