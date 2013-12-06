using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic; // List<>


public class Singleton
{

    #region Variables

    public enum Weapons
    {
        sword,
        axe,
        dagger
    }

    public enum Armors
    {
        light,
        medium,
        heavy
    }

    public Armors armor=Armors.medium;

    public Weapons weapon;

    public bool playerTurn = true;

    public bool enemyTurn = false;


    public static int enemyCount;

    public static int points=0;

    private static Singleton instance;

    private static int pisteet;

    public int killCount;

    private static bool paused;

    private static GameObject RuntimeSpawn;

    private static bool muted = true;

    private static int indexi;

    private static int levelStatus=0;

    private static bool etuNapinState = false;

    private static bool takaNapinState = false;

    private static bool successFullJumpDone;

    private static float jumpTime;

    private static float totalTime;

    public static float[] userFloatArray;

    public static List<float> userFloatList = new List<float>();

    public static ArrayList userArrayList = new ArrayList();


    public int dex = 5;

    public int str = 5;

    public int wsd = 5;

    public int hp = 20;

    public int hpMax = 20;

    public int ap = 0;

    public bool isMoving = false;

    public static Material playerTorsoMaterial;

    public static Material playerHeadMaterial;

    public static Material playerShoulderMaterial;

    public static bool checkingPlayer=false;

    public static Material originalMaterial;

    public static Quaternion alkuRotaatio;

    public static Material currentCycledMaterial;

    public static bool arrowCollidersAlive = true;

    public static string labelText;

    public static Material originalFaceMaterial;

    public static Material originalHairMaterial;

    public enum BodyShapes
    {
        dwarf_skn_body,
        dwarf_str_body,
        dwarf_fat_body
    }

    public static BodyShapes bodyshape = BodyShapes.dwarf_str_body;



    #endregion

    #region GetSet_ocean

    public int getLevelState()
    {
        return levelStatus;
    }

    public void setLevelState(int i)
    {
        levelStatus = i;
    }


    public bool getHypynState()
    {
        return successFullJumpDone;
    }

    public void setHypynState(bool i)
    {
        successFullJumpDone = i;
    }



    public bool getEtuNapinState()
    {
        return etuNapinState;
    }

    public void setEtuNapinState(bool i)
    {
        etuNapinState = i;
    }


    public bool getTakaNapinState()
    {
        return takaNapinState;
    }

    public void setTakaNapinState(bool i)
    {
        takaNapinState = i;
    }


    public int getIndex()
    {
        return indexi;
    }

    public void setIndex(int i)
    {
        indexi = i;
    }


    public void addPoints(int i)
    {
        pisteet = i;
    }

    public int givePoints()
    {
        return pisteet;
    }

    // Constructor -->
    private Singleton() { }


    public static Singleton Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Singleton();
            }
            return instance;
        }
    }


    public void pause()
    {
        paused = true;
    }


    public void resume()
    {
        paused = false;
    }

    public bool isPaused()
    {
        return paused;
    }

    public void CreateGui(GameObject Gui)
    {
        RuntimeSpawn = Gui;
    }

    public void DestroyGui()
    {
        GameObject.Destroy(RuntimeSpawn);
    }

    public void toggleMute()
    {
        muted = !muted;
        //Debug.Log("Sinkku muted arvo on : " + muted);
    }

    public bool giveMuteState()
    {
        return muted;
    }


    public float getHypynAika()
    {
        return jumpTime;
    }

    public void setHypynAika(float i)
    {
        jumpTime = i;
    }

    public float getTotalTime()
    {
        return totalTime;
    }

    public void setTotalTime(float i)
    {
        totalTime = i;
    }

    #endregion
}