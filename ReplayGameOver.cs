﻿using UnityEngine;
using System.Collections;

public class ReplayGameOver : MonoBehaviour 
{

    public Singleton sinkku;

    void Start()
    {
        sinkku = Singleton.Instance;
    }

    void OnClick()
    {
        // Resetters -->
        sinkku.hp = sinkku.hpMax;

        sinkku.playerTurn = true;

        sinkku.enemyTurn = false;

        Singleton.points = 0;

        Invoke("DelayedLoading", 1f);
        gameObject.GetComponent<UIButtonSound>().enabled = false;
        //Invoke("toggleVisible", 5);
        //if (Application.loadedLevelName == "Peli")
        //    Application.LoadLevel("Peli");

        //if (Application.loadedLevelName == "Battle2")
        //    Application.LoadLevel("Battle2");

        //if (Application.loadedLevelName == "Battle3")
        //    Application.LoadLevel("Battle3");

        //if (Application.loadedLevelName == "Battle4")
        //    Application.LoadLevel("Battle4");

        //if (Application.loadedLevelName == "Battle5")
        //    Application.LoadLevel("Battle5");

        //if (Application.loadedLevelName == "KolmeTavista")
        //    Application.LoadLevel("KolmeTavista");

        //if (Application.loadedLevelName == "RandomTavikset")
            

        //if (Application.loadedLevelName == "Random1")
        //    Application.LoadLevel("Random1");

    }

    public void DelayedLoading()
    {
        Application.LoadLevel("RandomTavikset");
    }
}
