using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour {
    [SerializeField]
    [Header("Paying list")]
    public PayBalls[] PayBallsList;
    public PayBackgraunds[] PayBackgraundsList;

    public static bool isGenerate;      // true - when creating and moving new balls and false - when player can throw the ball
    public static bool isGameOver;
    public static bool isPayMainBalls;
    public static bool isPayBackgraunds;
    public static bool getToCheckpoint;
    public static bool isSaveValueBonuses; // preservation of value bonuses after improvement
    public static bool isChekingClearField;
    public static bool isQuestGameMode;

    public static int SpawnBallHillPoint;
    public static int counter;
    public static int record;
    public static int coins;
    public static int payIdBalls;
  //  public static int idBallsForQuest;
    public static int payIdBackgraunds;

    //upgrade parametres
    public static int MainBallHillPoint;
    public static int AddHpCount = 1; // + 1 hp
    public static int BoombDamage = 50; // 0% - 100% piece of subtraction
    public static int ShanceSpawnBonusedBall = 0;

    [Header("Main elements")]
    public GameObject MainBall;
    public GameObject MainBackgraunds;
    [Space]
    public Text DebugText;

    private void Awake()
    {
        getToCheckpoint = false;
        MainBallHillPoint = 5;
        SpawnBallHillPoint = Preservation("SpawnBallHillPoint", 1);
        SpawnBallHillPoint = 1; // + 2; =  8 + 2 = 10;
    }

    private void Start()
    {
        counter = 0;
        //ShanceSpawnBonusedBall = Preservation("ShanceSpawnBonusedBall", 0);
        //BoombDamage = Preservation("BoombDamage", 50);
        //AddHpCount = Preservation("AddHpCount", 1);
        payIdBalls = Preservation("MainBall", 0);
        payIdBackgraunds = Preservation("Backgraunds", 0);
        MainBallHillPoint = Preservation("MainBallHillPoint", 5);

        coins = Preservation("Coins", 0);
        record = Preservation("Record", 0);

        isGameOver = false;
        isChekingClearField = false;

        Physics2D.IgnoreLayerCollision(8, 11);
        Physics2D.IgnoreLayerCollision(11, 10);
        PayingBalls();
        PayingBackgraunds();
    }

    private void Update()
    {
        if (isChekingClearField)
        {
            isChekingClearField = false;
            StartCoroutine(CheckClearField());
        }
        DebugText.text = ShanceSpawnBonusedBall.ToString();
        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.LogError("VALUE " + ShanceSpawnBonusedBall);
        }
        if (Input.GetKeyDown(KeyCode.R)) {
            isPayMainBalls = true;
            payIdBalls++;
        }
        if (Input.GetKeyDown(KeyCode.T)) {
            PlayerPrefs.DeleteAll();
        }
        if (isPayMainBalls)
        {
            isPayMainBalls = false;
            PayingBalls();
        }
        if (isPayBackgraunds)
        {
            isPayBackgraunds = false;
            PayingBackgraunds();
        }

        if (isSaveValueBonuses)
        {
            isSaveValueBonuses = !isSaveValueBonuses;
            ResaveValue("BoombDamage", BoombDamage);
            ResaveValue("ShanceSpawnBonusedBall", ShanceSpawnBonusedBall);
            ResaveValue("AddHpCount", AddHpCount);
            ResaveValue("MainBallHillPoint", MainBallHillPoint);

        }


    }
    /// <summary>
    /// Return value with key name and if hasn't key - set first value
    /// </summary>
    /// <param name="keyName"></param>
    /// <param name="value"></param>
    int Preservation(string keyName, int firstValue)
    {
        int value;
        if (!PlayerPrefs.HasKey(keyName))
        {
            PlayerPrefs.SetInt(keyName, firstValue);
            value = firstValue;
        }
        else value = PlayerPrefs.GetInt(keyName);
        return value;
    }
    float Preservation(string keyName, float firstValue)
    {
        float value;
        if (!PlayerPrefs.HasKey(keyName))
        {
            PlayerPrefs.SetFloat(keyName, firstValue);
            value = firstValue;
        }
        else value = PlayerPrefs.GetFloat(keyName);
        return value;
    }

    void ResaveValue(string keyName, float value)
    {
        PlayerPrefs.SetFloat(keyName, value);
    }

    void ResaveValue(string keyName, int value)
    {
        PlayerPrefs.SetInt(keyName, value);
    }

    void PayingBalls()
    {
        PlayerPrefs.SetInt("MainBall", payIdBalls);
        Destroy(MainBall);
        MainBall = Instantiate(PayBallsList[payIdBalls].gameObject, MainBall.transform.position, MainBall.transform.rotation, null);
       
    }

    void PayingBackgraunds()
    {
        PlayerPrefs.SetInt("Backgraunds", payIdBackgraunds);
        Destroy(MainBackgraunds);
        MainBackgraunds = Instantiate(PayBackgraundsList[payIdBackgraunds].gameObject, MainBackgraunds.transform.position, MainBackgraunds.transform.rotation, transform);
    }

    IEnumerator CheckClearField()
    {
        yield return new WaitForFixedUpdate();
        if (!Ball.fly && GameObject.FindGameObjectsWithTag("BallDouble").Length == 0)
        {
            isGenerate = true;
        }
    }

    [System.Serializable]
    public class PayBalls
    {
        [SerializeField]
        public int idKey;
        [SerializeField]
        public GameObject gameObject;
    }

    [System.Serializable]
    public class PayBackgraunds
    {
        [SerializeField]
        public int idKey;
        [SerializeField]
        public GameObject gameObject;
    }
}
