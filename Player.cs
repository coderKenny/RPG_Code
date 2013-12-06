using UnityEngine;
using System.Collections;
using System;

[RequireComponent (typeof(Animation))]
[RequireComponent(typeof(AudioSource))]

public class Player : MonoBehaviour
{
    #region Variables

    public GameObject[] silmaluvut;
    public GameObject DamageNoppa;
    tk2dTextMesh textMesh, stats;
    public Singleton sinkku;
    public AudioClip swing;
    private Vector3 splashPosition;
    private bool started = false;
    public GameObject hitSplash;
    public Animation playerAnim;
    public Material materiaali;
   

    public enum Weapons
    {
        sword,
        axe,
        dagger
    }
    public Weapons ase;

    private bool isAlive = true;

    #endregion

    #region Unity methods

    void Start()
    {
        loadDiceResources();

        playerAnim = GameObject.FindGameObjectWithTag("Pelaaja").GetComponent<Animation>();
        sinkku = Singleton.Instance;

        textMesh = GameObject.FindGameObjectWithTag("HPPlayer").GetComponent<tk2dTextMesh>();

        textMesh.text = sinkku.hp.ToString();

        ase = Weapons.sword;
        sinkku.weapon = Singleton.Weapons.sword;

        stats = GameObject.FindGameObjectWithTag("Stats").GetComponent<tk2dTextMesh>();

        sinkku.ap = 30;

        sinkku.armor = Singleton.Armors.light;

        Singleton.playerTorsoMaterial = Resources.Load("Dwarf_01") as Material;

        Singleton.playerHeadMaterial = Resources.Load("Dwarf_12") as Material;

        Singleton.playerShoulderMaterial = Resources.Load("Dwarf_weapons_01") as Material;

        //transform.FindChild("dwarf_skn_body").GetComponent<SkinnedMeshRenderer>().material = Singleton.playerTorsoMaterial;

        //transform.FindChild("dwarf_head_01").GetComponent<SkinnedMeshRenderer>().material = Singleton.playerHeadMaterial;

        //transform.FindChild("dwarf_shoulder pad_02").GetComponent<SkinnedMeshRenderer>().material = Singleton.playerShoulderMaterial;
    }

    void Update()
    {
        // if weapon is changed in editor

        if (ase == Weapons.axe)
            sinkku.weapon = Singleton.Weapons.axe;

        if (ase == Weapons.dagger)
            sinkku.weapon = Singleton.Weapons.dagger;

        if (ase == Weapons.sword)
            sinkku.weapon = Singleton.Weapons.sword;

        //Debug.Log("Sinkun ase : "+sinkku.weapon);

        //string teksti = stats.text;
        
        /*
        teksti = teksti.Remove(30);

        teksti = teksti + sinkku.ap;
        stats.text = teksti;
        */

        stats.text = "^3HP : "+sinkku.hp+"\n^1Mana : 200\n^2AP : " + sinkku.ap;


        string teksti2 = sinkku.hp + " / " + sinkku.hpMax + " points";

        textMesh.text = teksti2;


        if (sinkku.ap == 0 && !started)
        {
            resolveAPzeroSituation();
        }
    }

    #endregion

    #region Other methods

    public bool isItAlive()
    {
        return isAlive;
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


        GameObject noppaklooni = GameObject.FindGameObjectWithTag("DamageToPlayer");


        Destroy(noppaklooni);

        //if (noppaklooni != null)


        noppaklooni = Instantiate(silmaluvut[silmaLuku], paikka, rotaatio) as GameObject;

        Destroy(noppaklooni, 2f);

        noppaklooni.transform.localScale = koko;
        noppaklooni.tag = "DamageToPlayer";

        StartCoroutine(Vilkuta(0.2f,noppaklooni));

        noppaklooni.renderer.material = materiaali;
    }

    public int GetArmorInt()
    {
        int armorInt = 0;

        if (sinkku.armor == Singleton.Armors.light)
            armorInt = 2;
        if (sinkku.armor == Singleton.Armors.medium)
            armorInt = 5;
        if (sinkku.armor == Singleton.Armors.heavy)
            armorInt = 8;

        return armorInt;
    }

    public void toggleStartedBool()
    {
        started = !started;
    }

    public  void loadDiceResources()
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

        DamageNoppa = Resources.Load("d10_low_4_playerDamage") as GameObject;

        materiaali = Resources.Load("Materials/d10/d10-white") as Material;

    }

    public void resolveAPzeroSituation()
    {
        if (sinkku.killCount <= 2)
        {
            started = true;
            // Enemyturn
            sinkku.playerTurn = false;
            sinkku.enemyTurn = true;
        } // else all dead !

    }

    public void RestorePlayerAPs()
    {
        sinkku.ap = 30;
    }

    public void dealDamage(int amount)
    {
        if (amount != 0)  // Causes nagative damage, array index mess
            rendaaNoppa(amount - 1);

        //Debug.Log("Monster's damage to player : " + amount);

        splashPosition = transform.position;
        splashPosition.y += 1;
        Instantiate(hitSplash, splashPosition, Quaternion.identity);

        sinkku.hp -= amount;

        if (sinkku.hp < 0)
            sinkku.hp = 0;

        textMesh.text = sinkku.hp.ToString();

        if (sinkku.hp <= 0)
        {
            die();
        }
    }

    public void die()
    {
        StopAllCoroutines();
        playerAnim["die1"].wrapMode = WrapMode.ClampForever;
        playerAnim.Play("die1");

        isAlive = false;

        //Paralyze -->
        GameObject.FindGameObjectWithTag("ControlBlock").GetComponent<ButtonReactor>().ToggleCooled();

        #region Try to find existing enemy, really stupid way :(

        try
        {
            GameObject.FindGameObjectWithTag("Boss").SendMessage("instantiateEndingBanner", "Battle lost !!!");
            //return;
        }

        catch (Exception e)
        {
            Debug.Log("Exception caught : " + e);
        }


        try
        {
            GameObject.FindGameObjectWithTag("Troll2").SendMessage("instantiateEndingBanner", "Battle lost !!!");
            //return;
        }

        catch (Exception e)
        {
            Debug.Log("Exception caught : " + e);
            //GameObject.FindGameObjectWithTag("Troll2").SendMessage("instantiateEndingBanner", "Battle lost !!!");
            //return;
        }

        // General try
        try
        {
            GameObject.FindGameObjectWithTag("Enemy1").SendMessage("instantiateEndingBanner", "Battle lost !!!");
            //return;
        }

        catch (Exception e)
        {
            Debug.Log("Exception caught : " + e);
            //GameObject.FindGameObjectWithTag("Troll2").SendMessage("instantiateEndingBanner", "Battle lost !!!");
            //return;
        }


        try
        {
            GameObject.FindGameObjectWithTag("Troll").SendMessage("instantiateEndingBanner", "Battle lost !!!");
            //return;
        }

        catch (Exception e)
        {
            Debug.Log("Exception caught : " + e);
        }

        // IF above else fails ->

        GameObject.Find("Randomizer").GetComponent<RandomizerCode>().monster[0].GetComponent<EnemyScript>().instantiateEndingBanner("Battle lost !!!");

        #endregion



        #region Store HighScore


        int oldHighScore = PlayerPrefs.GetInt("RopeHighScore", 0);

        if (oldHighScore < Singleton.points)
        {
            PlayerPrefs.SetInt("RopeHighScore", Singleton.points);
        }

        #endregion

        StartCoroutine(LatausViiveella(5.0f, "GameOver"));

    }

    public IEnumerator Vilkuta(float waitTime,GameObject what)
    {
        for (int i = 0; i < 1; i++)
        {
            yield return new WaitForSeconds(waitTime);

            if(what !=null)
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

    public IEnumerator LatausViiveella(float waitTime, String levelName)
    {
        yield return new WaitForSeconds(waitTime);

        Application.LoadLevel(levelName);
    }

    #endregion
}
