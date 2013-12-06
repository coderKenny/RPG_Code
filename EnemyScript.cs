
#region NameSpaces

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

#endregion

public class EnemyScript : MonoBehaviour
{
    #region Variables

    private bool isAlive = true;
    public bool isItAlive()
    {
        return isAlive;
    }
    public tk2dTextMesh hp;
    public tk2dTextMesh ap;
    //public tk2dTextMesh hp2;
    //public tk2dTextMesh ap2;
    //public tk2dTextMesh hp3;
    //public tk2dTextMesh ap3;
    private int hitPoints;
    private int actionPoints = 10;
    private Vector3 splashPosition;
    public GameObject hitSplash;
    public GameObject winBanner;
    public Animation enemyAnim;
    public AudioClip deathAudio;
    public AudioClip swooshSound; 
    private int maxHP;
    private int weaponHit;
    public int GetWeaponHit()
    {
        return weaponHit;
    }
    private int dex=0;
    private int str;   // the name field
    public int Str   // the Name property
    {
        get 
        {
            return this.str; 
        }

        set
        {
            this.str = value;
        }
    }
    public int getDex()
    {
        return dex;
    }
    public enum EnemyArmors
    {
        light=2,
        medium=5,
        heavy=8
    }
    public void reduceActionPoints(int amount)
    {
        actionPoints -= amount;
    }
    public EnemyArmors armor;
    public int armorInt;
    public int GetArmorInt()
    {
        return armorInt;
    }
    private int weaponDamage;   // the name field
    public int WeaponDamage   // the Name property
    {
        get
        {
            return this.weaponDamage;
        }

        set
        {
            this.weaponDamage = value;
        }
    }
    public Singleton sinkku;
    public GameObject[] silmaluvut;
    public GameObject DamageNoppa;
    public Material materiaali;
    public bool attackDone = false;

    public static GameObject WinBanner;
    public static AudioClip SwooshSound;

    #endregion

    #region Unity methods


    void Awake()
    {
        countEnemyAmount();
    }
    
    
    void Start () 
    {
        loadDiceResources();

        loadSoundResources();

        loadOtherResources();

        winBanner = Resources.Load("WinBanner") as GameObject;
        WinBanner = Resources.Load("WinBanner") as GameObject;

        if (gameObject.tag == "Boss")
            LoadBossMeshes();


        sinkku = Singleton.Instance;

        sinkku.killCount = 0;
        sinkku.enemyTurn = false;
        sinkku.playerTurn = true;


        enemyAnim = gameObject.GetComponent<Animation>();

        enemyAnim = this.gameObject.GetComponent<Animation>();

        if (enemyAnim == null)
        {
            // If attached tagged to parent / child
            enemyAnim = GameObject.FindGameObjectWithTag("EnemyModel").GetComponent<Animation>();
        }

        enemyAnim["idle"].wrapMode = WrapMode.Loop;

        enemyAnim.Play("idle");


        #region EnemyStatsCollection

        // Eka trolli -----------------------------------------------------------------------
        if (gameObject.name == "Troll")
        {
            hp = GameObject.FindGameObjectWithTag("HPTroll").GetComponent<tk2dTextMesh>();
            ap = GameObject.FindGameObjectWithTag("APTroll1").GetComponent<tk2dTextMesh>();

            dex = 3;
            armor = EnemyArmors.medium;
            Str = 3;

            hitPoints = 15;
            maxHP = 15;

            WeaponDamage = 5;
            weaponHit = 3;

            (gameObject.GetComponent("Halo") as Behaviour).enabled = false;
            
        }

        // Boss (bonusta statseihin)----------------------------------------------------------------------

        if (gameObject.tag == "Boss")
        {
            enemyAnim = this.GetComponentInChildren<Animation>();

            if(hp==null)
                hp = GameObject.FindGameObjectWithTag("HPBoss").GetComponent<tk2dTextMesh>();

            if(ap==null)
                ap = GameObject.FindGameObjectWithTag("APBoss").GetComponent<tk2dTextMesh>();

            dex = 5;
            armor = EnemyArmors.heavy;
            Str = 5;

            hitPoints = 22;
            maxHP = 22;

            WeaponDamage = 7;
            weaponHit = 5;

            (gameObject.GetComponent("Halo") as Behaviour).enabled = false;
        }


        if (gameObject.name == "Troll 2")
        {
            hp = GameObject.FindGameObjectWithTag("HPTroll2").GetComponent<tk2dTextMesh>();
            ap = GameObject.FindGameObjectWithTag("APTroll2").GetComponent<tk2dTextMesh>();

            dex = 3;
            armor = EnemyArmors.heavy;
            Str = 3;

            hitPoints = 15;
            maxHP = 15;

            WeaponDamage = 5;
            weaponHit = 3;

            (gameObject.GetComponent("Halo") as Behaviour).enabled = false;
        }

        if (gameObject.name == "Troll 3")
        {
            hp = GameObject.FindGameObjectWithTag("HPTroll3").GetComponent<tk2dTextMesh>();
            ap = GameObject.FindGameObjectWithTag("APTroll3").GetComponent<tk2dTextMesh>();

            dex = 3;
            armor = EnemyArmors.heavy;
            Str = 3;

            hitPoints = 15;
            maxHP = 15;

            WeaponDamage = 5;
            weaponHit = 3;

            (gameObject.GetComponent("Halo") as Behaviour).enabled = false;
        }

        if (gameObject.name == "Enemy1" || gameObject.name == "Enemy1(Clone)")
        {
            //hp = GameObject.FindGameObjectWithTag("HPTroll3").GetComponent<tk2dTextMesh>();
            //ap = GameObject.FindGameObjectWithTag("APTroll3").GetComponent<tk2dTextMesh>();

            dex = 3;
            armor = EnemyArmors.light;
            Str = 3;

            hitPoints = 15;
            maxHP = 15;

            WeaponDamage = 5;
            weaponHit = 3;

            (gameObject.GetComponent("Halo") as Behaviour).enabled = false;
        }


        if (gameObject.name == "Enemy2" || gameObject.name == "Enemy2(Clone)")
        {
            //hp = GameObject.FindGameObjectWithTag("HPTroll3").GetComponent<tk2dTextMesh>();
            //ap = GameObject.FindGameObjectWithTag("APTroll3").GetComponent<tk2dTextMesh>();

            dex = 3;
            armor = EnemyArmors.light;
            Str = 3;

            hitPoints = 15;
            maxHP = 15;

            WeaponDamage = 5;
            weaponHit = 3;

            (gameObject.GetComponent("Halo") as Behaviour).enabled = false;
        }

        if (gameObject.name == "Enemy3" || gameObject.name == "Enemy3(Clone)")
        {
            //hp = GameObject.FindGameObjectWithTag("HPTroll3").GetComponent<tk2dTextMesh>();
            //ap = GameObject.FindGameObjectWithTag("APTroll3").GetComponent<tk2dTextMesh>();

            dex = 3;
            armor = EnemyArmors.light;
            Str = 3;

            hitPoints = 15;
            maxHP = 15;

            WeaponDamage = 5;
            weaponHit = 3;

            (gameObject.GetComponent("Halo") as Behaviour).enabled = false;
        }

        if (gameObject.name == "Enemy4" || gameObject.name == "Enemy4(Clone)")
        {
            //hp = GameObject.FindGameObjectWithTag("HPTroll3").GetComponent<tk2dTextMesh>();
            //ap = GameObject.FindGameObjectWithTag("APTroll3").GetComponent<tk2dTextMesh>();

            dex = 3;
            armor = EnemyArmors.light;
            Str = 3;

            hitPoints = 20;
            maxHP = 20;

            WeaponDamage = 5;
            weaponHit = 3;

            (gameObject.GetComponent("Halo") as Behaviour).enabled = false;
        }



        if (gameObject.tag == "Troll" && dex == 0)
        {
            dex = 3;
            armor = EnemyArmors.light;
            Str = 3;

            hitPoints = 7;
            maxHP = 7;

            WeaponDamage = 5;
            weaponHit = 3;

            (gameObject.GetComponent("Halo") as Behaviour).enabled = false;

        }


        // Default -->
        if(dex==0) // if not found yet
        {
        //if (gameObject.name == "Enemy" + GameObject.Find("Randomizer").GetComponent<RandomizerCode>().enemiesInPool || gameObject.name == "Enemy" + GameObject.Find("Randomizer").GetComponent<RandomizerCode>().enemiesInPool + "(Clone)")
        //{
            //hp = GameObject.FindGameObjectWithTag("HPTroll3").GetComponent<tk2dTextMesh>();
            //ap = GameObject.FindGameObjectWithTag("APTroll3").GetComponent<tk2dTextMesh>();

            dex = 3;
            armor = EnemyArmors.light;
            Str = 3;

            hitPoints = 10;
            maxHP = 10;

            WeaponDamage = 5;
            weaponHit = 3;

            (gameObject.GetComponent("Halo") as Behaviour).enabled = false;
        //}
        }



        #endregion;

        if (armor == EnemyArmors.light)
            armorInt = 0;
        if (armor == EnemyArmors.medium)
            armorInt = 1;
        if (armor == EnemyArmors.heavy)
            armorInt = 3;

        hp.text = hitPoints.ToString();

        

	}
	
	void Update () 
    {

        ap.text = "AC "+GetArmorInt().ToString();

        string teksti2 = hitPoints + " / "+maxHP+" points";

        hp.text = teksti2;

        Debug.DrawRay(transform.position, transform.up * -10, Color.green, 1f);
	}

    #endregion
    
    #region Other methods

    public void FindOwnHP()
    {
        hp = gameObject.transform.Find("EnemyHPMesh").GetComponent<tk2dTextMesh>();
    }

    public void FindOwnAP()
    {
        ap = gameObject.transform.Find("EnemyAPMesh").GetComponent<tk2dTextMesh>();
    }

    public void loadSoundResources()
    {
        deathAudio=Resources.Load("EnemyDeathSound") as AudioClip;
        swooshSound = Resources.Load("1_KKE") as AudioClip;

        SwooshSound = Resources.Load("1_KKE") as AudioClip;
    }

    public void loadOtherResources()
    {
        hitSplash = Resources.Load("HitFX_Ice") as GameObject;       
    }



    public void dealDamage(int amount)
    {
        
        if(amount != 0)  // Causes nagative damage, array index mess
            rendaaNoppa(amount - 1);

        //Debug.Log("Damage to enemy is : " + amount);
        splashPosition = transform.position;
        splashPosition.y += 2;
        Instantiate(hitSplash,splashPosition,Quaternion.identity);

        hitPoints -= amount;

        if (hitPoints < 0)
            hitPoints = 0;

        hp.text = hitPoints.ToString();

        if (hitPoints <= 0)
        {
            die();
        }
    }

    public void die()
    {

        Singleton.points += this.maxHP;
        isAlive = false;
        enemyAnim["die"].wrapMode = WrapMode.ClampForever;
        enemyAnim.Play("die");

        AudioSource.PlayClipAtPoint(deathAudio, transform.position);

        hp.renderer.enabled = false;
        ap.renderer.enabled = false;

        Debug.Log("Tapetun tagi on : " + this.gameObject.tag);
		
		sinkku.killCount++;
		
		#region Collider killing


        GameObject collider;
        RaycastHit hit;
        Ray ray = new Ray(transform.position, -transform.up);


        if (Physics.Raycast(ray, out hit, 100))
        {
            collider = hit.transform.gameObject;
            Debug.Log("Hitted : "+hit.transform.gameObject.name);
            Debug.DrawLine(ray.origin, hit.point);

            collider.GetComponent<BoxCollider>().enabled = false ;
            
        }
        


        /*


		if(this.gameObject.tag.Equals("Boss"))
        	GameObject.FindGameObjectWithTag("HitCollider1").collider.enabled = false;
        
        
        if(this.gameObject.tag.Equals("Troll2"))
            GameObject.FindGameObjectWithTag("HitCollider3").collider.enabled = false;

        if (this.gameObject.tag.Equals("Troll3"))
            GameObject.FindGameObjectWithTag("HitCollider2").collider.enabled = false;




        if (this.gameObject.tag.Equals("Enemy2"))
            GameObject.FindGameObjectWithTag("HitCollider1").collider.enabled = false;


        if (this.gameObject.tag.Equals("Enemy1"))
            GameObject.FindGameObjectWithTag("HitCollider3").collider.enabled = false;

        if (this.gameObject.tag.Equals("Enemy3"))
            GameObject.FindGameObjectWithTag("HitCollider2").collider.enabled = false;


        */







		
		#endregion
		

        GameObject[] vihut = FindGameObjectsWithLayer(14);

        //for(int i=0;i<20;i++)
            Debug.Log("Vihujen maara  : "+vihut.Length+"\nKillCount : "+sinkku.killCount);

        if(vihut.Length == 1)
            instantiateEndingBanner(null);

        if(vihut.Length == 2 && sinkku.killCount>=2)
            instantiateEndingBanner(null);

        if (vihut.Length == 3 && sinkku.killCount >= 3)
            instantiateEndingBanner(null);

    }

    public void instantiateEndingBanner(string text)
    {
        if (text == null)
        {
            AudioSource.PlayClipAtPoint(swooshSound, winBanner.transform.position);
            Instantiate(winBanner, winBanner.transform.position, winBanner.transform.rotation);

            if (Application.loadedLevelName == "RandomTavikset")
                loadNextLevel("RandomTavikset");

            else
                loadNextLevel();
        }

        else
        {
            string tempTextToStore = winBanner.GetComponent<tk2dTextMesh>().text;
            winBanner.GetComponent<tk2dTextMesh>().text = text;
            Instantiate(winBanner, winBanner.transform.position, winBanner.transform.rotation);
            AudioSource.PlayClipAtPoint(swooshSound, winBanner.transform.position);
            winBanner.GetComponent<tk2dTextMesh>().text = tempTextToStore;
        }
    }




    public void loadNextLevel()
    {
        // Resetters -->
        //sinkku.hp = 20;
        sinkku.playerTurn = true;

        sinkku.enemyTurn = false;
        StartCoroutine(LatausViiveella(5.0f));   
    }


    public void loadNextLevel(String name)
    {
        // Resetters -->
        //sinkku.hp = 20;
        sinkku.playerTurn = true;

        sinkku.enemyTurn = false;
        StartCoroutine(LatausViiveella(5.0f,name));
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


    public void rendaaNoppa(int silmaLuku)
    {
        Vector3 paikka = Vector3.zero;
        Vector3 koko = Vector3.zero;


        Quaternion rotaatio;

        paikka = DamageNoppa.transform.position;
        koko = new Vector3(0.25f, 0.25f, 0.25f);

        //Debug.Log("Koko on : " + koko);

        rotaatio = silmaluvut[silmaLuku].transform.rotation;


        GameObject noppaklooni = GameObject.FindGameObjectWithTag("DamageToEnemy");


        Destroy(noppaklooni);

        //if (noppaklooni != null)


        noppaklooni = Instantiate(silmaluvut[silmaLuku], paikka, rotaatio) as GameObject;

        Destroy(noppaklooni, 2f);

        noppaklooni.transform.localScale = koko;
        noppaklooni.tag = "DamageToEnemy";

        StartCoroutine(Vilkuta(0.2f, noppaklooni));

        noppaklooni.renderer.material = materiaali;
    }



    public void LoadBossMeshes()
    {
        if(ap==null)
            ap = GameObject.FindGameObjectWithTag("APBoss").GetComponent<tk2dTextMesh>();

        if(hp==null)
            hp = GameObject.FindGameObjectWithTag("HPBoss").GetComponent<tk2dTextMesh>();

    }

    void loadDiceResources()
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

        DamageNoppa = Resources.Load("d10_low_4_enemyDamage") as GameObject;

        materiaali = Resources.Load("d10-yellow") as Material;

    }


    #endregion

    #region CoRoutines

    public IEnumerator Vilkuta(float waitTime, GameObject what)
    {
        for (int i = 0; i < 1; i++)
        {
            yield return new WaitForSeconds(waitTime);

            if (what != null)
                what.renderer.enabled = false;
            yield return new WaitForSeconds(waitTime);

            if (what != null)
                what.renderer.enabled = true;
            yield return new WaitForSeconds(waitTime);

            if (what != null)
                what.renderer.enabled = false;
            yield return new WaitForSeconds(waitTime);

            if (what != null)
                what.renderer.enabled = true;
            yield return new WaitForSeconds(waitTime);

            if (what != null)
                what.renderer.enabled = false;
            yield return new WaitForSeconds(waitTime);

            if (what != null)
                what.renderer.enabled = true;
            yield return new WaitForSeconds(waitTime);

            if (what != null)
                what.renderer.enabled = false;
            yield return new WaitForSeconds(waitTime);

            if (what != null)
                what.renderer.enabled = true;
            yield return new WaitForSeconds(waitTime);

            if (what != null)
                what.renderer.enabled = false;
        }
    }

    public IEnumerator LatausViiveella(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        int loadIndex = Application.loadedLevel;
        loadIndex++;

        if (loadIndex <= Application.levelCount)
        {
            Application.LoadLevel(loadIndex);
        }
    }


    public IEnumerator LatausViiveella(float waitTime,String levelName)
    {
        yield return new WaitForSeconds(waitTime);
  
        Application.LoadLevel(levelName);
        
    }


    #endregion
}
