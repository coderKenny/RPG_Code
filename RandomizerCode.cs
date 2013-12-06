using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomizerCode : MonoBehaviour
{
    #region Variables

    public Singleton sinkku;
    public int enemiesInScene;
    public int enemiesInPool = 10;    // KEY VARIABLE !
    public Vector3[] enemyPositions;
    public Quaternion[] enemyQuaternions;
    public GameObject[] monster;

    public List<Transform> allEnemyPositions;

    public List<Transform> allHPPositions;
    public List<Transform> allAPPositions;


    #endregion


	void Start() 
    {

        countAllEnemyPositions();

        monster = new GameObject[enemiesInPool];
        enemyPositions = new Vector3[enemiesInPool];
        enemyQuaternions = new Quaternion[enemiesInPool];

        sinkku = Singleton.Instance;

        enemiesInScene = Random.Range(1, 4);

        switch (enemiesInScene)
        {
            case 1:
                instantiateEnemies(1);
                break;
            case 2:
               instantiateEnemies(2);
                break;
            case 3:
              instantiateEnemies(3);
                break;
        }
	}

    public void countAllEnemyPositions()
    {
        allEnemyPositions = new List<Transform>(enemiesInPool);

        // Three
        allEnemyPositions.Add(((GameObject)Resources.Load("Enemy1")).transform);
        allEnemyPositions.Add(((GameObject)Resources.Load("Enemy2")).transform);
        allEnemyPositions.Add(((GameObject)Resources.Load("Enemy3")).transform);

        if (enemiesInPool > 3)
        {
            for (int i = 4; i < enemiesInPool+1; i++)
            {
                allEnemyPositions.Add(((GameObject)Resources.Load("Enemies/Enemy"+(i).ToString())).transform);
            }
        }

        #region Text meshes

        allHPPositions = new List<Transform>(enemiesInPool);
        allAPPositions = new List<Transform>(enemiesInPool);

        allHPPositions.Add(((GameObject)Resources.Load("Enemy1")).transform.FindChild("EnemyHPMesh").transform);
        allHPPositions.Add(((GameObject)Resources.Load("Enemy2")).transform.FindChild("EnemyHPMesh").transform);
        allHPPositions.Add(((GameObject)Resources.Load("Enemy3")).transform.FindChild("EnemyHPMesh").transform);


        allAPPositions.Add(((GameObject)Resources.Load("Enemy1")).transform.FindChild("EnemyAPMesh").transform);
        allAPPositions.Add(((GameObject)Resources.Load("Enemy2")).transform.FindChild("EnemyAPMesh").transform);
        allAPPositions.Add(((GameObject)Resources.Load("Enemy3")).transform.FindChild("EnemyAPMesh").transform);

        #endregion

        #region Align upper stats better

        Vector3 tempVector = allAPPositions[0].position;
        tempVector.y = 2.4f;
        allAPPositions[0].position = tempVector;

        Vector3 tempVector2 = allHPPositions[0].position;
        tempVector2.y = 2.1f;
        tempVector2.x = 1.1f;
        allHPPositions[0].position = tempVector2;

        #endregion

        if (enemiesInPool > 3)
        {
            for (int i = 4; i < enemiesInPool + 1; i++)
            {
                allAPPositions.Add(((GameObject)Resources.Load("Enemies/Enemy" + i.ToString())).transform.FindChild("EnemyAPMesh").transform);
                allHPPositions.Add(((GameObject)Resources.Load("Enemies/Enemy" + i.ToString())).transform.FindChild("EnemyHPMesh").transform);
            }
        }


        //Debug.Log("Enemy1 transform : " + allHPPositions[0].position);
        //Debug.Log("Enemy2 transform : " + allHPPositions[1].position);
        //Debug.Log("Enemy3 transform : " + allHPPositions[2].position);

        //Debug.Log("Enemy1 transform : " + allAPPositions[0].position);
        //Debug.Log("Enemy2 transform : " + allAPPositions[1].position);
        //Debug.Log("Enemy3 transform : " + allAPPositions[2].position);

    }

 

    public void instantiateEnemies(int amount)
    {
        Debug.Log("Vihujen amount : " + amount);

        if (amount == 1)
        {

            int whichEnemy = Random.Range(1, enemiesInPool+1);

            if (whichEnemy == 1)
            {
                monster[0] = (GameObject)Instantiate(Resources.Load("Enemy1"));
                enemyPositions[0] = monster[0].transform.position;
                enemyQuaternions[0] = monster[0].transform.rotation;

                GameObject.FindGameObjectWithTag("ScriptiCamera").SendMessage("SetEnemy1", monster[0]);
                GameObject.FindGameObjectWithTag("ControlBlock").SendMessage("SetEnemy1", monster[0]);

                // Set HP & AP meshes
                monster[0].GetComponent<EnemyScript>().SendMessage("FindOwnAP");
                monster[0].GetComponent<EnemyScript>().SendMessage("FindOwnHP");
            }

            else if (whichEnemy == 2)
            {
                monster[0] = (GameObject)Instantiate(Resources.Load("Enemy2"));
                enemyPositions[1] = monster[0].transform.position;
                enemyQuaternions[1] = monster[0].transform.rotation;
                GameObject.FindGameObjectWithTag("ScriptiCamera").SendMessage("SetEnemy1", monster[0]);
                GameObject.FindGameObjectWithTag("ControlBlock").SendMessage("SetEnemy1", monster[0]);

                // Set HP & AP meshes
                monster[0].GetComponent<EnemyScript>().SendMessage("FindOwnAP");
                monster[0].GetComponent<EnemyScript>().SendMessage("FindOwnHP");
            }

            else if (whichEnemy == 3)
            {
                monster[0] = (GameObject)Instantiate(Resources.Load("Enemy3"));
                enemyPositions[2] = monster[0].transform.position;
                enemyQuaternions[2] = monster[0].transform.rotation;
                GameObject.FindGameObjectWithTag("ScriptiCamera").SendMessage("SetEnemy1", monster[0]);
                GameObject.FindGameObjectWithTag("ControlBlock").SendMessage("SetEnemy1", monster[0]);

                // Set HP & AP meshes
                monster[0].GetComponent<EnemyScript>().SendMessage("FindOwnAP");
                monster[0].GetComponent<EnemyScript>().SendMessage("FindOwnHP");
            }

            else if (whichEnemy == 4)
            {
                monster[0] = (GameObject)Instantiate(Resources.Load("Enemies/Enemy4"));
                enemyPositions[3] = monster[0].transform.position;
                enemyQuaternions[3] = monster[0].transform.rotation;
                GameObject.FindGameObjectWithTag("ScriptiCamera").SendMessage("SetEnemy1", monster[0]);
                GameObject.FindGameObjectWithTag("ControlBlock").SendMessage("SetEnemy1", monster[0]);

                // Set HP & AP meshes
                monster[0].GetComponent<EnemyScript>().SendMessage("FindOwnAP");
                monster[0].GetComponent<EnemyScript>().SendMessage("FindOwnHP");
            }

            // More than 4
            else
            {
                monster[0] = (GameObject)Instantiate(Resources.Load("Enemies/Enemy"+enemiesInPool.ToString()));
                enemyPositions[enemiesInPool-1] = monster[0].transform.position;
                enemyQuaternions[enemiesInPool - 1] = monster[0].transform.rotation;
                GameObject.FindGameObjectWithTag("ScriptiCamera").SendMessage("SetEnemy1", monster[0]);
                GameObject.FindGameObjectWithTag("ControlBlock").SendMessage("SetEnemy1", monster[0]);

                // Set HP & AP meshes
                monster[0].GetComponent<EnemyScript>().SendMessage("FindOwnAP");
                monster[0].GetComponent<EnemyScript>().SendMessage("FindOwnHP");
            }



            //randomizeLocation(monster[whichEnemy-1]);
            randomizeLocation(monster[0]);
        }

        else if (amount == 2)
        {
            int whichEnemy1 = Random.Range(1, enemiesInPool + 1);
            int whichEnemy2 = Random.Range(1, enemiesInPool + 1);

            while (whichEnemy1 == whichEnemy2)
                whichEnemy2 = Random.Range(1, enemiesInPool + 1);


            if (whichEnemy1 == 1)
            {
                spawnEnemy("Enemy1");
            }

            else if (whichEnemy1 == 2)
            {
                spawnEnemy("Enemy2");
            }

            else if (whichEnemy1 == 3)
            {
                spawnEnemy("Enemy3");
            }

            else if (whichEnemy1 == 4)
            {
                spawnEnemy("Enemies/Enemy4");
            }

            else
                spawnEnemy("Enemies/Enemy" + whichEnemy1.ToString());
            


            if (whichEnemy2 == 1)
            {
                spawnEnemy("Enemy1");
            }

            else if (whichEnemy2 == 2)
            {
                spawnEnemy("Enemy2");
            }

            else if (whichEnemy2 == 3)
            {
                spawnEnemy("Enemy3");
            }

            else if (whichEnemy2 == 4)
            {
                spawnEnemy("Enemies/Enemy4");
            }

            else
                spawnEnemy("Enemies/Enemy" + whichEnemy2.ToString());


            Debug.Log("Debug THIS !!!!!!!!!!!");
            randomizeLocation(monster[0], monster[1]);
    
        }

            
        // Three
        else
        {
            int whichEnemy1 = Random.Range(1, enemiesInPool + 1);
            int whichEnemy2 = Random.Range(1, enemiesInPool + 1);
            int whichEnemy3 = Random.Range(1, enemiesInPool + 1);

            while (whichEnemy1 == whichEnemy2)
                whichEnemy2 = Random.Range(1, enemiesInPool + 1);

            while (whichEnemy3 == whichEnemy1 || whichEnemy3 == whichEnemy2)
                whichEnemy3 = Random.Range(1, enemiesInPool + 1);

            Debug.Log("whichEnemy1 : " + whichEnemy1);
            Debug.Log("whichEnemy2 : " + whichEnemy2);
            Debug.Log("whichEnemy3 : " + whichEnemy3);



            #region One

            if (whichEnemy1 == 1)
            {
                spawnEnemy("Enemy1");
            }

            else if (whichEnemy1 == 2)
            {
                spawnEnemy("Enemy2");
            }

            else if (whichEnemy1 == 3)
            {
                spawnEnemy("Enemy3");
            }
               
            
            else
            {
                spawnEnemy("Enemies/Enemy" + whichEnemy1.ToString());
            }

         
            #endregion

            #region Two

            if (whichEnemy2 == 1)
            {
                spawnEnemy("Enemy1");
            }

            else if (whichEnemy2 == 2)
            {
                spawnEnemy("Enemy2");
            }

            else if (whichEnemy2 == 3)
            {
                spawnEnemy("Enemy3");
            }

            else
            {
                spawnEnemy("Enemies/Enemy" + whichEnemy2.ToString());
            }


            #endregion

            #region Three

            if (whichEnemy3 == 1)
            {
                spawnEnemy("Enemy1");
            }

            else if (whichEnemy3 == 2)
            {
                spawnEnemy("Enemy2");
            }

            else if (whichEnemy3 == 3)
            {
                spawnEnemy("Enemy3");
            }

            else
            {
                spawnEnemy("Enemies/Enemy" + whichEnemy3.ToString());
            }


            randomizeLocation(monster);

            #endregion
        }
    }

    public void spawnEnemy(string name)
    {   
        if (monster[0]==null)
        {
            monster[0] = (GameObject)Instantiate(Resources.Load(name));
            enemyPositions[0] = monster[0].transform.position;
            enemyQuaternions[0] = monster[0].transform.rotation;

            GameObject.FindGameObjectWithTag("ScriptiCamera").SendMessage("SetEnemy1", monster[0]);
            GameObject.FindGameObjectWithTag("ControlBlock").SendMessage("SetEnemy1", monster[0]);

            // Set HP & AP meshes
            monster[0].GetComponent<EnemyScript>().SendMessage("FindOwnAP");
            monster[0].GetComponent<EnemyScript>().SendMessage("FindOwnHP");
        }

        else if (monster[1] == null)
        {         
            monster[1] = (GameObject)Instantiate(Resources.Load(name));
            enemyPositions[1] = monster[1].transform.position;
            enemyQuaternions[1] = monster[1].transform.rotation;

            GameObject.FindGameObjectWithTag("ScriptiCamera").SendMessage("SetEnemy1", monster[1]);
            GameObject.FindGameObjectWithTag("ControlBlock").SendMessage("SetEnemy1", monster[1]);

            // Set HP & AP meshes
            monster[1].GetComponent<EnemyScript>().SendMessage("FindOwnAP");
            monster[1].GetComponent<EnemyScript>().SendMessage("FindOwnHP");
        }


        else if (monster[2] == null)
        {
            monster[2] = (GameObject)Instantiate(Resources.Load(name));
            enemyPositions[2] = monster[2].transform.position;
            enemyQuaternions[2] = monster[2].transform.rotation;

            GameObject.FindGameObjectWithTag("ScriptiCamera").SendMessage("SetEnemy1", monster[2]);
            GameObject.FindGameObjectWithTag("ControlBlock").SendMessage("SetEnemy1", monster[2]);

            // Set HP & AP meshes
            monster[2].GetComponent<EnemyScript>().SendMessage("FindOwnAP");
            monster[2].GetComponent<EnemyScript>().SendMessage("FindOwnHP");
        }      
    }


    #region Position randomization

    public void randomizeLocation(GameObject randomoitava)
    {

        int paikka = Random.Range(0, 3);

        randomoitava.transform.position = allEnemyPositions[paikka].position;

        randomoitava.transform.FindChild("EnemyAPMesh").transform.position = allAPPositions[paikka].position;
        randomoitava.transform.FindChild("EnemyHPMesh").transform.position = allHPPositions[paikka].position;

        GameObject.FindGameObjectWithTag("ControlBlock").SendMessage("ResetColliders", 1);

    }

    public void randomizeLocation(GameObject randomoitava, GameObject toinen)
    {
        int paikka = Random.Range(0, 3);
        int paikka2 = Random.Range(0, 3);

        while (paikka == paikka2)
            paikka2 = Random.Range(0, 3);

        randomoitava.transform.position = allEnemyPositions[paikka].position;

        randomoitava.transform.FindChild("EnemyAPMesh").transform.position = allAPPositions[paikka].position;
        randomoitava.transform.FindChild("EnemyHPMesh").transform.position = allHPPositions[paikka].position;


        toinen.transform.position = allEnemyPositions[paikka2].position;


        toinen.transform.FindChild("EnemyAPMesh").transform.position = allAPPositions[paikka2].position;
        toinen.transform.FindChild("EnemyHPMesh").transform.position = allHPPositions[paikka2].position;




        //GameObject.FindGameObjectWithTag("ControlBlock").SendMessage("ResetColliders", monster[0]);
        GameObject.FindGameObjectWithTag("ControlBlock").GetComponent<ButtonReactor>().ResetColliders(monster[0], monster[1]);

    }

    public void randomizeLocation(GameObject[] randomoitavat)
    {
        int paikka1 = Random.Range(0, 3);
        int paikka2 = Random.Range(0, 3);
        int paikka3 = Random.Range(0, 3);

        while (paikka1 == paikka2)
            paikka2 = Random.Range(0, 3);

        while (paikka3 == paikka1 || paikka3 == paikka2)
            paikka3 = Random.Range(0, 3);


        randomoitavat[0].transform.position = allEnemyPositions[paikka1].position;

        randomoitavat[0].transform.FindChild("EnemyAPMesh").transform.position = allAPPositions[paikka1].position;
        randomoitavat[0].transform.FindChild("EnemyHPMesh").transform.position = allHPPositions[paikka1].position;

        randomoitavat[1].transform.position = allEnemyPositions[paikka2].position;

        randomoitavat[1].transform.FindChild("EnemyAPMesh").transform.position = allAPPositions[paikka2].position;
        randomoitavat[1].transform.FindChild("EnemyHPMesh").transform.position = allHPPositions[paikka2].position;

        randomoitavat[2].transform.position = allEnemyPositions[paikka3].position;

        randomoitavat[2].transform.FindChild("EnemyAPMesh").transform.position = allAPPositions[paikka3].position;
        randomoitavat[2].transform.FindChild("EnemyHPMesh").transform.position = allHPPositions[paikka3].position;



        //GameObject.FindGameObjectWithTag("ControlBlock").SendMessage("ResetColliders", monster[0]);
        GameObject.FindGameObjectWithTag("ControlBlock").GetComponent<ButtonReactor>().ResetColliders(randomoitavat);

    }

    #endregion

}
