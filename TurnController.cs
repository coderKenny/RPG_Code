using UnityEngine;        
using System.Collections;

public class TurnController : MonoBehaviour
{
    #region Variables

    public Singleton sinkku;
    public AudioClip playerHitted;
    public AudioClip playerMissed;
    public GameObject vihu1;
    public GameObject vihu2;
    public GameObject vihu3;
    public GameObject pelaaja;
    private Animation enemyAnim;
    private tk2dTextMesh turn;
    public GameObject enemyDamageIndicator;
    private bool enemyTurnStarted = false;
    public GameObject[] silmaluvut;
    public Material materiaali;
    private bool attackOrder = true;
    private bool SingleBoss = true;
    public GameObject[] attackers;
    public bool generalAttack = false;
    private Component vihuHalo;

    private bool reCreatedTable = false;

    #endregion

    #region Methods called by Unity engine

    void Start()
    {

        loadDiceResources();

        loadAudioResources();

        pelaaja = GameObject.FindGameObjectWithTag("Pelaaja");

        sinkku = Singleton.Instance;
        turn = GameObject.Find("TurnMesh").GetComponent<tk2dTextMesh>();
        //enemyAnim = vihu1.GetComponent<Animation>();
        //enemyAnim["idle"].wrapMode = WrapMode.Loop;
        //enemyAnim.Play("idle");


        if (Singleton.enemyCount == 1 && vihu1.tag == "Boss")
            SingleBoss = true;

        else
            SingleBoss = false;
	}
	
	void Update () 
    {
        //Debug.Log("KillCount : " + sinkku.killCount);
        if (sinkku.enemyTurn && pelaaja.GetComponent<Player>().isItAlive() && !enemyTurnStarted && sinkku.killCount < Singleton.enemyCount)
        {

            // Randomoidaan alotusjärjestys -->
            int order = Random.Range(1, 11);

            if (order < 6)
                attackOrder = true;
            else if (order > 5)
                attackOrder = false;

            // ----------------------------------
            Debug.Log("KillCount : " + sinkku.killCount);
            Debug.Log("AttackOrder : " + attackOrder);

            enemyTurnStarted = !enemyTurnStarted;
            (pelaaja.GetComponent("Halo") as Behaviour).enabled = false;

            if(!SingleBoss)
                turn.text = "Enemy turn";


            if (generalAttack)
            {
                Debug.Log("GENERAL ATTACK !");
                StartCoroutine(Attack(2f, attackers, pelaaja));
            }


            // Yksi vastus
            if (Singleton.enemyCount == 1 && vihu1.GetComponent<EnemyScript>().isItAlive() && !SingleBoss && !generalAttack)
            {
                StartCoroutine(WaitAndPrint(2f));
            }


            // Yksi Pomo
            else if (Singleton.enemyCount == 1 && SingleBoss && vihu1.GetComponent<EnemyScript>().isItAlive() && !generalAttack)
            {
                StartCoroutine(WaitAndPrint3(2f,vihu1));
            }


            else if(!generalAttack)
            {
                // Another dead
                if (!vihu1.GetComponent<EnemyScript>().isItAlive() || !vihu2.GetComponent<EnemyScript>().isItAlive())
                {
                    // Kumpi ?
                    if (!vihu1.GetComponent<EnemyScript>().isItAlive())
                        StartCoroutine(WaitAndPrint2(2f));

                    else
                        StartCoroutine(Reversed2(2f));

                }

                // Both alive
                if (vihu1.GetComponent<EnemyScript>().isItAlive() && vihu2.GetComponent<EnemyScript>().isItAlive())
                {
                    if (attackOrder)
                    {
                        StartCoroutine(Reversed(2f,vihu2));
                    }

                    else
                    {
                        StartCoroutine(WaitAndPrint(2f));
                    }
                }
            }
        }
           
        if (sinkku.playerTurn && pelaaja.GetComponent<Player>().isItAlive() && !Singleton.checkingPlayer)
        {
                (pelaaja.GetComponent("Halo") as Behaviour).enabled = true;
                turn.text = "Player turn";
        }
	}

    #endregion

    #region unStatic common methods


    public void loadAudioResources()
    {
        playerMissed = Resources.Load("EnemyMiss") as AudioClip;
    }

    public void loadDiceResources()
    {
        silmaluvut = new GameObject[10];

        silmaluvut[0] = Resources.Load("d10_low_1") as GameObject;
        silmaluvut[1] = Resources.Load("d10_low_2") as GameObject;
        silmaluvut[2] = Resources.Load("d10_low_3") as GameObject;
        silmaluvut[3] = Resources.Load("d10_low_4_enemy") as GameObject;
        silmaluvut[4] = Resources.Load("d10_low_5") as GameObject;
        silmaluvut[5] = Resources.Load("d10_low_6") as GameObject;
        silmaluvut[6] = Resources.Load("d10_low_7") as GameObject;
        silmaluvut[7] = Resources.Load("d10_low_8") as GameObject;
        silmaluvut[8] = Resources.Load("d10_low_9") as GameObject;
        silmaluvut[9] = Resources.Load("d10_low_0") as GameObject;

        //DamageNoppa = Resources.Load("d10_low_4_enemyDamage") as GameObject;

        materiaali = Resources.Load("Materials/d10/d10-red") as Material;
    }

    public void rendaaNoppa(int silmaLuku)
    {
        Vector3 paikka = Vector3.zero;

        Quaternion rotaatio;

        paikka = silmaluvut[3].transform.position;
        rotaatio = silmaluvut[silmaLuku].transform.rotation;


        GameObject noppaklooni = GameObject.FindGameObjectWithTag("EnemyNoppaklooni");


        Destroy(noppaklooni);

        //if (noppaklooni != null)


        noppaklooni = Instantiate(silmaluvut[silmaLuku], paikka, rotaatio) as GameObject;
        noppaklooni.tag = "EnemyNoppaklooni";
        noppaklooni.renderer.material = materiaali;
    }

    public int countDamage(GameObject pelaaja, GameObject enemy)
    {
        int hitDice = Random.Range(1, 11);

        int damageInflicted;
        int armorInt = pelaaja.GetComponent<Player>().GetArmorInt();

        //Debug.Log("ArmorInt arvo : " + armorInt);

        hitDice = hitDice - armorInt;

        damageInflicted = hitDice;

        if (hitDice <= 0)
            damageInflicted = 0;

        else
            damageInflicted = hitDice;

        if (hitDice > enemy.GetComponent<EnemyScript>().Str + enemy.GetComponent<EnemyScript>().WeaponDamage)
            damageInflicted = sinkku.str + enemy.GetComponent<EnemyScript>().WeaponDamage;

        return damageInflicted;
    }

    public void InstatiateDamageDecal(int dammu)
    {
        GameObject kahvaKuula = null;
        Vector3 correction = Vector3.zero;

        correction = pelaaja.transform.position;

        correction.x += 1.237f;

        correction.z -= 1.28f;

        kahvaKuula = Instantiate(enemyDamageIndicator, correction, enemyDamageIndicator.transform.rotation) as GameObject;


        //int damage = countDamage(pelaaja, vihu1);
        kahvaKuula.GetComponent<tk2dTextMesh>().text = dammu.ToString();

        Destroy(kahvaKuula.gameObject, 2f);

    }

    public void SetEnemy1(GameObject vihu)
    {
        vihu1 = vihu;
        
        if (!reCreatedTable)
        {
            attackers = new GameObject[1];
            reCreatedTable = true;
            attackers[0]=vihu;
        }

        else if (reCreatedTable && attackers.Length==1)
        {
            GameObject temp = attackers[0];

            attackers = new GameObject[2];

            attackers[0] = temp;
            attackers[1] = vihu;
        }

        else if (reCreatedTable && attackers.Length == 2)
        {
            GameObject temp1 = attackers[0];
            GameObject temp2 = attackers[1];

            attackers = new GameObject[3];

            attackers[0] = temp1;
            attackers[1] = temp2;
            attackers[2] = vihu;
        }


        
    }

    #endregion

    // Coroutines -->

    #region EkaRutiini

    public IEnumerator WaitAndPrint(float waitTime)
    {
        //(vihu1.GetComponent("Halo") as Behaviour).enabled = true;

        //(vihu1.GetComponent("Halo") as MonoBehaviour).enabled = true;

        vihuHalo = vihu1.GetComponent("Halo");

        vihuHalo.GetType().GetProperty("enabled").SetValue(vihuHalo, true, null);

       

        yield return new WaitForSeconds(waitTime-1);
        turn.text = "Enemy turn";
        yield return new WaitForSeconds(waitTime);

        enemyAnim = vihu1.GetComponent<Animation>();
        enemyAnim["attack1"].wrapMode = WrapMode.Once;
        enemyAnim.Play("attack1");

        vihu1.GetComponent<EnemyScript>().reduceActionPoints(5);
        int dice = Random.Range(1, 11);

        rendaaNoppa(dice - 1);

        string info = "Enemy1 hit math : " + dice + " <= " + vihu1.GetComponent<EnemyScript>().getDex() + " + " + vihu1.GetComponent<EnemyScript>().GetWeaponHit();

        bool osuma = dice <= vihu1.GetComponent<EnemyScript>().getDex() + vihu1.GetComponent<EnemyScript>().GetWeaponHit();


        if (osuma)
        {
            AudioSource.PlayClipAtPoint(playerHitted, pelaaja.transform.position);

            int damage = countDamage(pelaaja, vihu1);

            InstatiateDamageDecal(damage);

            pelaaja.GetComponent<Player>().dealDamage(damage);
        }

        GameObject.FindGameObjectWithTag("HitInfo").SendMessage("UpdateInfo", info);

        yield return new WaitForSeconds(1f);
        enemyAnim["idle"].wrapMode = WrapMode.Loop;
        enemyAnim.Play("idle");



        if (!pelaaja.GetComponent<Player>().isItAlive())
            StopAllCoroutines();

        yield return new WaitForSeconds(waitTime);
        enemyAnim = vihu1.GetComponent<Animation>();
        enemyAnim["attack1"].wrapMode = WrapMode.Once;
        enemyAnim.Play("attack1");
        dice = Random.Range(1, 11);

        rendaaNoppa(dice - 1);

        vihu1.GetComponent<EnemyScript>().reduceActionPoints(5);

        info = "Enemy1 hit math : " + dice + " <= " + vihu1.GetComponent<EnemyScript>().getDex() + " + " + vihu1.GetComponent<EnemyScript>().GetWeaponHit();

        osuma = dice <= vihu1.GetComponent<EnemyScript>().getDex() + vihu1.GetComponent<EnemyScript>().GetWeaponHit();


       

        if (osuma)
        {
            AudioSource.PlayClipAtPoint(playerHitted, pelaaja.transform.position);
            
            int damage = countDamage(pelaaja, vihu1);
            InstatiateDamageDecal(damage);
            pelaaja.GetComponent<Player>().dealDamage(damage);
        }




        GameObject.FindGameObjectWithTag("HitInfo").SendMessage("UpdateInfo", info);

        yield return new WaitForSeconds(1f);
        enemyAnim["idle"].wrapMode = WrapMode.Loop;
        enemyAnim.Play("idle");
       
            
        (vihu1.GetComponent("Halo") as Behaviour).enabled = false;
      
        if (Singleton.enemyCount>=2)
        {
            if (vihu2.GetComponent<EnemyScript>().isItAlive())
                StartCoroutine(WaitAndPrint2(2f));

            else
            {
                GameObject.FindGameObjectWithTag("Pelaaja").SendMessage("toggleStartedBool");
                enemyTurnStarted = false;

                GameObject.FindGameObjectWithTag("Pelaaja").SendMessage("RestorePlayerAPs");

                sinkku.enemyTurn = false;
                sinkku.playerTurn = true;

                StopAllCoroutines();
            }
        }

        else
        {
            sinkku.enemyTurn = false;
            sinkku.playerTurn = true;

            GameObject.FindGameObjectWithTag("Pelaaja").SendMessage("toggleStartedBool");
            enemyTurnStarted = false;

            GameObject.FindGameObjectWithTag("Pelaaja").SendMessage("RestorePlayerAPs");
        }   
    }

    #endregion

    #region TokaRutiini

    public IEnumerator WaitAndPrint2(float waitTime)
    {
        if (!pelaaja.GetComponent<Player>().isItAlive())
            StopAllCoroutines();
        
            
        (vihu2.GetComponent("Halo") as Behaviour).enabled = true;

        yield return new WaitForSeconds(waitTime - 1);
        turn.text = "Enemy turn";
        yield return new WaitForSeconds(waitTime);

        enemyAnim = vihu2.GetComponent<Animation>();
        enemyAnim["attack1"].wrapMode = WrapMode.Once;
        enemyAnim.Play("attack1");

        vihu2.GetComponent<EnemyScript>().reduceActionPoints(5);
        int dice = Random.Range(1, 11);

        rendaaNoppa(dice - 1);
        

        string info = "Enemy2 hit math : " + dice + " <= " + vihu2.GetComponent<EnemyScript>().getDex() + " + " + vihu2.GetComponent<EnemyScript>().GetWeaponHit();

        bool osuma = dice <= vihu2.GetComponent<EnemyScript>().getDex() + vihu2.GetComponent<EnemyScript>().GetWeaponHit();


        if (osuma)
        {
            AudioSource.PlayClipAtPoint(playerHitted, pelaaja.transform.position);          

            int damage = countDamage(pelaaja, vihu2);
      
            InstatiateDamageDecal(damage);

            pelaaja.GetComponent<Player>().dealDamage(damage);
        }

        GameObject.FindGameObjectWithTag("HitInfo").SendMessage("UpdateInfo", info);

        yield return new WaitForSeconds(1f);
        enemyAnim["idle"].wrapMode = WrapMode.Loop;
        enemyAnim.Play("idle");

        // Second attack 

        if (!pelaaja.GetComponent<Player>().isItAlive())
            StopAllCoroutines();

     

        yield return new WaitForSeconds(waitTime);
        enemyAnim = vihu2.GetComponent<Animation>();
        enemyAnim["attack1"].wrapMode = WrapMode.Once;
        enemyAnim.Play("attack1");
        dice = Random.Range(1, 11);


        rendaaNoppa(dice - 1);

        vihu1.GetComponent<EnemyScript>().reduceActionPoints(5);

        info = "Enemy2 hit math : " + dice + " <= " + vihu2.GetComponent<EnemyScript>().getDex() + " + " + vihu2.GetComponent<EnemyScript>().GetWeaponHit();

        osuma = dice <= vihu2.GetComponent<EnemyScript>().getDex() + vihu2.GetComponent<EnemyScript>().GetWeaponHit();



        if (osuma)
        {
            AudioSource.PlayClipAtPoint(playerHitted, pelaaja.transform.position);
            

            int damage = countDamage(pelaaja, vihu2);
            InstatiateDamageDecal(damage);

            pelaaja.GetComponent<Player>().dealDamage(damage);
        }



        //info += "... Osuma? " + osuma;

        GameObject.FindGameObjectWithTag("HitInfo").SendMessage("UpdateInfo", info);

        yield return new WaitForSeconds(1f);
        enemyAnim["idle"].wrapMode = WrapMode.Loop;
        enemyAnim.Play("idle");


        //if (pelaaja.GetComponent<Player>().isItAlive())
            
        (vihu2.GetComponent("Halo") as Behaviour).enabled = false;

        if (vihu3 != null)
        {
            if (vihu3.tag.Equals("Boss"))
            {
                if (vihu3.GetComponent<EnemyScript>().isItAlive())
                    StartCoroutine(WaitAndPrint3(2f,vihu3));

                else
                {
                    GameObject.FindGameObjectWithTag("Pelaaja").SendMessage("toggleStartedBool");
                    enemyTurnStarted = false;

                    GameObject.FindGameObjectWithTag("Pelaaja").SendMessage("RestorePlayerAPs");

                    

                    sinkku.enemyTurn = false;
                    sinkku.playerTurn = true;

                    StopAllCoroutines();
                }
            }

        }

        else
        {
            sinkku.enemyTurn = false;
            sinkku.playerTurn = true;

            GameObject.FindGameObjectWithTag("Pelaaja").SendMessage("toggleStartedBool");
            enemyTurnStarted = false;

            GameObject.FindGameObjectWithTag("Pelaaja").SendMessage("RestorePlayerAPs");
        }   
    }

    #endregion

    #region KolmasRutiini

    public IEnumerator WaitAndPrint3(float waitTime, GameObject attacker)
    {
        if (!pelaaja.GetComponent<Player>().isItAlive())
            StopAllCoroutines();

     
        (attacker.GetComponent("Halo") as Behaviour).enabled = true;

        yield return new WaitForSeconds(waitTime - 1);
        turn.text = "BOSS TURN !!";
        yield return new WaitForSeconds(waitTime);

        enemyAnim = attacker.GetComponentInChildren<Animation>();
        enemyAnim["attack1"].wrapMode = WrapMode.Once;
        enemyAnim.Play("attack1");

        attacker.GetComponent<EnemyScript>().reduceActionPoints(5);
        int dice = Random.Range(1, 11);

        rendaaNoppa(dice - 1);


        string info = "Enemy3 hit math : " + dice + " <= " + attacker.GetComponent<EnemyScript>().getDex() + " + " + attacker.GetComponent<EnemyScript>().GetWeaponHit();

        bool osuma = dice <= attacker.GetComponent<EnemyScript>().getDex() + attacker.GetComponent<EnemyScript>().GetWeaponHit();


      

        if (osuma)
        {
            AudioSource.PlayClipAtPoint(playerHitted, pelaaja.transform.position);
            

            int damage = countDamage(pelaaja, attacker);
            InstatiateDamageDecal(damage);

            pelaaja.GetComponent<Player>().dealDamage(damage);
        }

        GameObject.FindGameObjectWithTag("HitInfo").SendMessage("UpdateInfo", info);

        yield return new WaitForSeconds(1f);
        enemyAnim["idle"].wrapMode = WrapMode.Loop;
        enemyAnim.Play("idle");

        // Second attack 

        if (!pelaaja.GetComponent<Player>().isItAlive())
            StopAllCoroutines();



        yield return new WaitForSeconds(waitTime);
        enemyAnim["attack1"].wrapMode = WrapMode.Once;
        enemyAnim.Play("attack1");
        dice = Random.Range(1, 11);

        rendaaNoppa(dice - 1);


        info = "Enemy3 hit math : " + dice + " <= " + attacker.GetComponent<EnemyScript>().getDex() + " + " + attacker.GetComponent<EnemyScript>().GetWeaponHit();

        osuma = dice <= attacker.GetComponent<EnemyScript>().getDex() + attacker.GetComponent<EnemyScript>().GetWeaponHit();


        if (osuma)
        {
            AudioSource.PlayClipAtPoint(playerHitted, pelaaja.transform.position);
            
   

            int damage = countDamage(pelaaja, attacker);
            InstatiateDamageDecal(damage);

            pelaaja.GetComponent<Player>().dealDamage(damage);
        }



        //info += "... Osuma? " + osuma;

        GameObject.FindGameObjectWithTag("HitInfo").SendMessage("UpdateInfo", info);

        yield return new WaitForSeconds(1f);
        enemyAnim["idle"].wrapMode = WrapMode.Loop;
        enemyAnim.Play("idle");


        sinkku.enemyTurn = false;
        sinkku.playerTurn = true;

        GameObject.FindGameObjectWithTag("Pelaaja").SendMessage("toggleStartedBool");
        enemyTurnStarted = false;

        GameObject.FindGameObjectWithTag("Pelaaja").SendMessage("RestorePlayerAPs");
		
		//if(pelaaja.GetComponent<Player>().isItAlive())
        	
		(attacker.GetComponent("Halo") as Behaviour).enabled = false;
    }

    #endregion


    #region Reversed1

    public IEnumerator Reversed(float waitTime, GameObject attacker)
    {
        (attacker.GetComponent("Halo") as Behaviour).enabled = true;

        yield return new WaitForSeconds(waitTime - 1);
        turn.text = "Enemy turn";
        yield return new WaitForSeconds(waitTime);

        enemyAnim = attacker.GetComponent<Animation>();
        enemyAnim["attack1"].wrapMode = WrapMode.Once;
        enemyAnim.Play("attack1");

        attacker.GetComponent<EnemyScript>().reduceActionPoints(5);
        int dice = Random.Range(1, 11);

        rendaaNoppa(dice - 1);

        string info = "Enemy1 hit math : " + dice + " <= " + attacker.GetComponent<EnemyScript>().getDex() + " + " + attacker.GetComponent<EnemyScript>().GetWeaponHit();

        bool osuma = dice <= attacker.GetComponent<EnemyScript>().getDex() + attacker.GetComponent<EnemyScript>().GetWeaponHit();


        if (osuma)
        {
            AudioSource.PlayClipAtPoint(playerHitted, pelaaja.transform.position);


            int damage = countDamage(pelaaja, attacker);
            InstatiateDamageDecal(damage);

            pelaaja.GetComponent<Player>().dealDamage(damage);
        }

        GameObject.FindGameObjectWithTag("HitInfo").SendMessage("UpdateInfo", info);

        yield return new WaitForSeconds(1f);
        enemyAnim["idle"].wrapMode = WrapMode.Loop;
        enemyAnim.Play("idle");



        if (!pelaaja.GetComponent<Player>().isItAlive())
            StopAllCoroutines();

        yield return new WaitForSeconds(waitTime);
        enemyAnim["attack1"].wrapMode = WrapMode.Once;
        enemyAnim.Play("attack1");
        dice = Random.Range(1, 11);

        rendaaNoppa(dice - 1);

        attacker.GetComponent<EnemyScript>().reduceActionPoints(5);

        info = "Enemy1 hit math : " + dice + " <= " + attacker.GetComponent<EnemyScript>().getDex() + " + " + attacker.GetComponent<EnemyScript>().GetWeaponHit();

        osuma = dice <= attacker.GetComponent<EnemyScript>().getDex() + attacker.GetComponent<EnemyScript>().GetWeaponHit();



        if (osuma)
        {
            AudioSource.PlayClipAtPoint(playerHitted, pelaaja.transform.position);

            int damage = countDamage(pelaaja, attacker);
            InstatiateDamageDecal(damage);

            pelaaja.GetComponent<Player>().dealDamage(damage);
        }


        GameObject.FindGameObjectWithTag("HitInfo").SendMessage("UpdateInfo", info);

        yield return new WaitForSeconds(1f);
        enemyAnim["idle"].wrapMode = WrapMode.Loop;
        enemyAnim.Play("idle");

        (attacker.GetComponent("Halo") as Behaviour).enabled = false;

        if (Singleton.enemyCount>=2)
        {
            if (vihu1.GetComponent<EnemyScript>().isItAlive())
            {
                StartCoroutine(Reversed2(2f));
            }

            else
            {
                GameObject.FindGameObjectWithTag("Pelaaja").SendMessage("toggleStartedBool");
                enemyTurnStarted = false;

                GameObject.FindGameObjectWithTag("Pelaaja").SendMessage("RestorePlayerAPs");
             
                sinkku.enemyTurn = false;
                sinkku.playerTurn = true;

                StopAllCoroutines();
            }
        }

        else
        {
            sinkku.enemyTurn = false;
            sinkku.playerTurn = true;

            GameObject.FindGameObjectWithTag("Pelaaja").SendMessage("toggleStartedBool");
            enemyTurnStarted = false;

            GameObject.FindGameObjectWithTag("Pelaaja").SendMessage("RestorePlayerAPs");
        }
    }

    #endregion

    #region Reversed2

    public IEnumerator Reversed2(float waitTime)
    {
        if (!pelaaja.GetComponent<Player>().isItAlive())
            StopAllCoroutines();


        (vihu1.GetComponent("Halo") as Behaviour).enabled = true;
        

        yield return new WaitForSeconds(waitTime - 1);
        turn.text = "Enemy turn";
        yield return new WaitForSeconds(waitTime);

        enemyAnim = vihu1.GetComponent<Animation>();
        enemyAnim["attack1"].wrapMode = WrapMode.Once;
        enemyAnim.Play("attack1");

        vihu1.GetComponent<EnemyScript>().reduceActionPoints(5);
        int dice = Random.Range(1, 11);

        rendaaNoppa(dice - 1);

     
        string info = "Enemy2 hit math : " + dice + " <= " + vihu1.GetComponent<EnemyScript>().getDex() + " + " + vihu1.GetComponent<EnemyScript>().GetWeaponHit();

        bool osuma = dice <= vihu1.GetComponent<EnemyScript>().getDex() + vihu1.GetComponent<EnemyScript>().GetWeaponHit();


        if (osuma)
        {
            AudioSource.PlayClipAtPoint(playerHitted, pelaaja.transform.position);
           


            int damage = countDamage(pelaaja, vihu1);
            InstatiateDamageDecal(damage);

            pelaaja.GetComponent<Player>().dealDamage(damage);
        }

        GameObject.FindGameObjectWithTag("HitInfo").SendMessage("UpdateInfo", info);

        yield return new WaitForSeconds(1f);
        enemyAnim["idle"].wrapMode = WrapMode.Loop;
        enemyAnim.Play("idle");

        // Second attack 

        if (!pelaaja.GetComponent<Player>().isItAlive())
            StopAllCoroutines();



        yield return new WaitForSeconds(waitTime);
        enemyAnim = vihu1.GetComponent<Animation>();
        enemyAnim["attack1"].wrapMode = WrapMode.Once;
        enemyAnim.Play("attack1");
        dice = Random.Range(1, 11);


        rendaaNoppa(dice - 1);

        vihu1.GetComponent<EnemyScript>().reduceActionPoints(5);

        info = "Enemy2 hit math : " + dice + " <= " + vihu1.GetComponent<EnemyScript>().getDex() + " + " + vihu1.GetComponent<EnemyScript>().GetWeaponHit();

        osuma = dice <= vihu1.GetComponent<EnemyScript>().getDex() + vihu1.GetComponent<EnemyScript>().GetWeaponHit();


        if (osuma)
        {
            AudioSource.PlayClipAtPoint(playerHitted, pelaaja.transform.position);


            int damage = countDamage(pelaaja, vihu1);
            InstatiateDamageDecal(damage);

            pelaaja.GetComponent<Player>().dealDamage(damage);
        }


        GameObject.FindGameObjectWithTag("HitInfo").SendMessage("UpdateInfo", info);

        yield return new WaitForSeconds(1f);
        enemyAnim["idle"].wrapMode = WrapMode.Loop;
        enemyAnim.Play("idle");

        (vihu1.GetComponent("Halo") as Behaviour).enabled = false;

        if (vihu3 != null)
        {
            if (vihu3.tag.Equals("Boss"))
            {
                if (vihu3.GetComponent<EnemyScript>().isItAlive())
                    StartCoroutine(WaitAndPrint3(2f,vihu3));

                else
                {
                    GameObject.FindGameObjectWithTag("Pelaaja").SendMessage("toggleStartedBool");
                    enemyTurnStarted = false;

                    GameObject.FindGameObjectWithTag("Pelaaja").SendMessage("RestorePlayerAPs");

                    

                    sinkku.enemyTurn = false;
                    sinkku.playerTurn = true;

                    StopAllCoroutines();
                }
            }

        }

        else
        {
            (vihu1.GetComponent("Halo") as Behaviour).enabled = false;
            (vihu2.GetComponent("Halo") as Behaviour).enabled = false;

            sinkku.enemyTurn = false;
            sinkku.playerTurn = true;

            GameObject.FindGameObjectWithTag("Pelaaja").SendMessage("toggleStartedBool");
            enemyTurnStarted = false;

            GameObject.FindGameObjectWithTag("Pelaaja").SendMessage("RestorePlayerAPs");

           

            if (vihu3 != null)
                (vihu3.GetComponent("Halo") as Behaviour).enabled = false;

        }
    }

    #endregion


    #region Generalisoitu rutiini

    public IEnumerator Attack(float waitTime, GameObject[] attackers, GameObject defender)
    {
        int numberOfAttackers = attackers.Length;
        int[] order = CalculateOrder(numberOfAttackers);

        if (attackers[order[0]].GetComponent<EnemyScript>().isItAlive())
        {
			StartCoroutine(GeneralAttack(2f, attackers[order[0]], pelaaja));
			Debug.Log ("Attack done state : "+attackers[order[0]].GetComponent<EnemyScript>().attackDone );

            while (!attackers[order[0]].GetComponent<EnemyScript>().attackDone)
            {
				//Debug.Log ("Attack done state : "+attackers[order[0]].GetComponent<EnemyScript>().attackDone +"  by  "+attackers[order[0]].name);
                yield return null;
            }
        }


        if (attackers.Length >= 2)
        {

            if (attackers[order[1]].GetComponent<EnemyScript>().isItAlive())
            {
                StartCoroutine(GeneralAttack(2f, attackers[order[1]], pelaaja));

                while (!attackers[order[1]].GetComponent<EnemyScript>().attackDone)
                {
                    //Debug.Log ("Attack done state : "+attackers[order[1]].GetComponent<EnemyScript>().attackDone +"  by  "+attackers[order[1]].name);
                    yield return null;
                }
            }
        }


        if (attackers.Length >= 3)
        {

            if (attackers[order[2]].GetComponent<EnemyScript>().isItAlive())
            {
                StartCoroutine(GeneralAttack(2f, attackers[order[2]], pelaaja));

                while (!attackers[order[2]].GetComponent<EnemyScript>().attackDone)
                {
                    //Debug.Log ("Attack done state : "+attackers[order[2]].GetComponent<EnemyScript>().attackDone +"  by  "+attackers[order[2]].name);
                    yield return null;
                }
            }
        }




        sinkku.enemyTurn = false;
        sinkku.playerTurn = true;

        GameObject.FindGameObjectWithTag("Pelaaja").SendMessage("toggleStartedBool");
        enemyTurnStarted = false;

        GameObject.FindGameObjectWithTag("Pelaaja").SendMessage("RestorePlayerAPs");

        attackers[order[0]].GetComponent<EnemyScript>().attackDone = false;
        
        if(attackers.Length >= 2)
            attackers[order[1]].GetComponent<EnemyScript>().attackDone = false;

        if (attackers.Length >= 3)
            attackers[order[2]].GetComponent<EnemyScript>().attackDone = false;

    }

    public int[] CalculateOrder(int alkioita)
    {
        int[] sinkkutaulu = { 0 };

        if (alkioita == 3)
        {
            int[] order = { 0, 0, 0 };
            int luku1 = 0;
            int luku2 = 0;
            int luku3 = 0;
           

            luku1 = Random.Range(0, 3);
            luku2 = Random.Range(0, 3);
            luku3 = Random.Range(0, 3);

            while (luku2 == luku1)
                luku2 = Random.Range(0, 3);

            while (luku3 == luku1 || luku3 == luku2)
                luku3 = Random.Range(0, 3);


            order[0] = luku1;
            order[1] = luku2;
            order[2] = luku3;

            return order;
        }

        if (alkioita == 2)
        {
            int[] order = { 0, 0 };
            int luku1 = 0;
            int luku2 = 0;

            luku1 = Random.Range(0, 2);
            luku2 = Random.Range(0, 2);
           

            while (luku2 == luku1)
                luku2 = Random.Range(0, 2);

            order[0] = luku1;
            order[1] = luku2;

            return order;
        }

        // Yksi mörkö

        return sinkkutaulu;
            
    }

    public IEnumerator GeneralAttack(float waitTime, GameObject attacker, GameObject defender)
    {
        if (!defender.GetComponent<Player>().isItAlive())
            StopAllCoroutines();


        (attacker.GetComponent("Halo") as Behaviour).enabled = true;


        yield return new WaitForSeconds(waitTime - 1);

        if(attacker.tag=="Boss")
            turn.text = "BOSS TURN !!";
        else
            turn.text = "Enemy turn";

        

        yield return new WaitForSeconds(waitTime);

        enemyAnim = attacker.GetComponent<Animation>();
        enemyAnim["attack1"].wrapMode = WrapMode.Once;
        enemyAnim.Play("attack1");

        AudioSource.PlayClipAtPoint(playerMissed, defender.transform.position);

        attacker.GetComponent<EnemyScript>().reduceActionPoints(5);
        int dice = Random.Range(1, 11);

        rendaaNoppa(dice - 1);


        string info = attacker.name + " hit math : " + dice + " <= " + attacker.GetComponent<EnemyScript>().getDex() + " + " + attacker.GetComponent<EnemyScript>().GetWeaponHit();

        bool osuma = dice <= attacker.GetComponent<EnemyScript>().getDex() + attacker.GetComponent<EnemyScript>().GetWeaponHit();


        if (osuma)
        {
            AudioSource.PlayClipAtPoint(playerHitted, defender.transform.position);

            int damage = countDamage(defender, attacker);
            InstatiateDamageDecal(damage);

            defender.GetComponent<Player>().dealDamage(damage);
        }

        GameObject.FindGameObjectWithTag("HitInfo").SendMessage("UpdateInfo", info);

        yield return new WaitForSeconds(1f);
        enemyAnim["idle"].wrapMode = WrapMode.Loop;
        enemyAnim.Play("idle");

        // Second attack 

        if (!defender.GetComponent<Player>().isItAlive())
            StopAllCoroutines();



        yield return new WaitForSeconds(waitTime);
        enemyAnim = attacker.GetComponent<Animation>();
        enemyAnim["attack1"].wrapMode = WrapMode.Once;
        enemyAnim.Play("attack1");

        AudioSource.PlayClipAtPoint(playerMissed, defender.transform.position);

        dice = Random.Range(1, 11);


        rendaaNoppa(dice - 1);

        attacker.GetComponent<EnemyScript>().reduceActionPoints(5);

        info = attacker.name + " hit math : " + dice + " <= " + vihu1.GetComponent<EnemyScript>().getDex() + " + " + vihu1.GetComponent<EnemyScript>().GetWeaponHit();

        osuma = dice <= attacker.GetComponent<EnemyScript>().getDex() + attacker.GetComponent<EnemyScript>().GetWeaponHit();


        if (osuma)
        {
            AudioSource.PlayClipAtPoint(playerHitted, defender.transform.position);


            int damage = countDamage(defender, attacker);
            InstatiateDamageDecal(damage);

            defender.GetComponent<Player>().dealDamage(damage);
        }


        GameObject.FindGameObjectWithTag("HitInfo").SendMessage("UpdateInfo", info);

        yield return new WaitForSeconds(1f);
        enemyAnim["idle"].wrapMode = WrapMode.Loop;
        enemyAnim.Play("idle");

        (attacker.GetComponent("Halo") as Behaviour).enabled = false;

        attacker.GetComponent<EnemyScript>().attackDone = true;
    }

    #endregion
}
