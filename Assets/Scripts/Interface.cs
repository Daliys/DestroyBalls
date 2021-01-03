using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Interface : MonoBehaviour
{
    public Toggle Inverse;


    public GameObject ResetGameChoosePanel;
    public GameObject ContinueAdsButton;
    public GameObject GameCanvas;
    public GameObject Generate;
    public GameObject StartWithCheckpointText;
    public GameObject CheckpointText;
    public GameObject ChangeScale;
    public GameObject PauseButton;
    public GameObject StoreButton;
    public GameObject SettingsOneButton;
    public GameObject SettingsTwoButton;
    public GameObject ConinueButtonPanel;
    public GameObject x2;
    public GameObject ButtonSoundOn;
    public GameObject ButtonSoundOff;
    public GameObject PlayButton;
    public GameObject ReturnButton;
    public GameObject RewardedPanel;

    public Animator InterfaceAnimator;

    public static bool pause;
    public static bool start;
    public static bool continueGameWithAds;

    float scaleChekpoint;
    int mainBallRecordHP;
    int spawnBallRecordHP;

    public Text CheckPointCount;
    public Text Counter;
    public Text Record;
    public Text Coins;
    public Text BefCheckText;
    GameObject TextText;
    GameObject Gener;
    void Start()
    {

        pause = false;
        start = false;
        continueGameWithAds = false;
        //Gener = Instantiate(Generate, Generate.transform.position, Generate.transform.rotation, GameCanvas.transform);

        scaleChekpoint = 5000;
        Record.text = "Record: " + PlayerPrefs.GetInt("Record");
        if (!PlayerPrefs.HasKey("Inverse"))
        {
            PlayerPrefs.SetInt("Inverse", -1);
            Ball.inverse = PlayerPrefs.GetInt("Inverse");
        }
        if (PlayerPrefs.GetInt("Inverse") == -1) Inverse.isOn = true;
        else Inverse.isOn = false;
        PauseButton.SetActive(false);
    }

    private void Update()
    {
        ScaleBar();
        GameOver();
        Coins.text = Game.coins.ToString();
        Counter.text = Game.counter.ToString();

        /*if (Ball.balldouble) x2.SetActive(true);
        else x2.SetActive(false);*/
    }

    void GameOver()
    {
        if (Game.isGameOver && InterfaceAnimator.GetInteger("State") != 2 && !ContinueAdsButton.activeSelf)
        {

            if (Game.counter > PlayerPrefs.GetInt("Record"))
            {
                PlayerPrefs.SetInt("Record", Game.counter);
                Game.record = PlayerPrefs.GetInt("Record");
                Record.text = "Record: " + Game.record.ToString();
            }
            if (Game.getToCheckpoint)
            {
                //Показ рекламы 
                //Advertising.showAd = true;

                Game.isGameOver = false;
                StartCoroutine(BeforeCheckpointText());
                Game.getToCheckpoint = false;
                Game.MainBallHillPoint = mainBallRecordHP;
                Game.SpawnBallHillPoint = spawnBallRecordHP;
                Destroy(Gener);
                Gener = new GameObject();
                Gener = Instantiate(Generate, Generate.transform.position, Generate.transform.rotation, GameCanvas.transform);
            }
            else if(start)
            {
                // Показ рекламы 
               // Advertising.showAd = true;
                //Продолжение игры за рекламу 
                /*if (Advertising.ad && !continueGameWithAds)
                {
                    print("Start");
                    continueGameWithAds = true;
                    start = false;
                    ContinueAdsButton.SetActive(true);
                    StartCoroutine(ContinueForAd());
                    
                }
                else*/
                //{
                start = false;
                    StopPlay();
                    Pause();
                //}
            }
        }
    }

    void ScaleBar()
    {
        if (Game.counter >= scaleChekpoint)
        {
            Game.getToCheckpoint = true;
            mainBallRecordHP = Game.MainBallHillPoint;
            spawnBallRecordHP = Game.SpawnBallHillPoint;
            scaleChekpoint *= 2;
            ChangeScale.transform.localScale = new Vector3(ChangeScale.transform.localScale.x, 0);
            GameObject t = Instantiate(CheckpointText, CheckpointText.transform.position, ChangeScale.transform.rotation, transform);
            Destroy(t, 1);
            if (scaleChekpoint < 1000000) { CheckPointCount.text = scaleChekpoint / 1000 + "K"; }
            else if (scaleChekpoint >= 1000000 && scaleChekpoint < 1000000000) { CheckPointCount.text = scaleChekpoint / 1000 + "M"; }
            else { CheckPointCount.text = scaleChekpoint / 1000 + "B"; }
        }
        else
        {
            ChangeScale.transform.localScale = new Vector3(ChangeScale.transform.localScale.x, Game.counter / scaleChekpoint);
        }
    }

    public void CloseRewardedPanel()
    {
        RewardedPanel.SetActive(false);
    }
    public void PlayGameStart()
    {
        PauseButton.SetActive(true);
        start = true;
        if(!Game.isQuestGameMode)
            Game.isGenerate = true;
        InterfaceAnimator.SetInteger("State", 1);
    }
    public void Pause()
    {

        StoreButton.SetActive(false);
        SettingsOneButton.SetActive(false);
        SettingsTwoButton.SetActive(true);
        start = false;
        InterfaceAnimator.SetInteger("State", 0);
    }
    public void Contunie()
    {
        ConinueButtonPanel.SetActive(false);
        start = true;
        InterfaceAnimator.SetInteger("State", 1);
    }
    public void StopPlay()
    {
        ConinueButtonPanel.SetActive(true);
        start = false;
        PlayButton.SetActive(false);
        ReturnButton.SetActive(true);
    }
    public void RestartButton()
    {
        SceneManager.LoadScene("Main");
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void StoreMarket()
    {
        InterfaceAnimator.SetInteger("State", 3);
    }
    public void StoreMarketBack()
    {
        InterfaceAnimator.SetInteger("State", 4);
    }
    public void Sound()
    {
        if (ButtonSoundOn.activeSelf)
        {
            ButtonSoundOn.SetActive(false);
            ButtonSoundOff.SetActive(true);
        }
        else
        {
            ButtonSoundOn.SetActive(true);
            ButtonSoundOff.SetActive(false);
        }
    }
    public void ResetGameButton()
    {
        ResetGameChoosePanel.SetActive(true);
    }
    public void CloseResetGamePanel()
    {
        ResetGameChoosePanel.SetActive(false);
    }
    public void ResetGame()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("Main");
    }
    public void Settings()
    {
        InterfaceAnimator.SetInteger("State", 2);
    }
    public void BackSettings()
    {
        InterfaceAnimator.SetInteger("State", 0);
    }
    public void OnToggle(bool selected)
    {
        if (selected) PlayerPrefs.SetInt("Inverse", -1);
        else PlayerPrefs.SetInt("Inverse", 1);
        Ball.inverse = PlayerPrefs.GetInt("Inverse");
    }
    /*public void ContinueForAdsButton()
    {
        Advertising.showAd = true;
        StopCoroutine(ContinueForAd());
        start = true;
        ContinueAdsButton.SetActive(false);
        Destroy(TextText);
    }*/
    public void AdVideo()
    {
      //  Advertising.showAdVideo = true;
    }

    IEnumerator BeforeCheckpointText()
    {
        int k = 3;
        GameObject textT = Instantiate(BefCheckText.gameObject, BefCheckText.gameObject.transform.position, BefCheckText.transform.rotation, transform);
        Destroy(textT, 3);
        Text te = textT.GetComponent<Text>();
        while (k > 0)
        {
            te.text = k.ToString();
            k--;
            yield return new WaitForSeconds(1);
        }
        Game.isGenerate = true;
        GameObject text = Instantiate(StartWithCheckpointText, StartWithCheckpointText.transform.position, StartWithCheckpointText.transform.rotation, transform);
        Destroy(text, 2);
    }
    /*IEnumerator ContinueForAd()
    {
        int k = 5;
        TextText = new GameObject(); 
        TextText = Instantiate(BefCheckText.gameObject, BefCheckText.gameObject.transform.position, BefCheckText.transform.rotation, transform);
        Destroy(TextText, 3);
        Text te = TextText.GetComponent<Text>();
        while (k > 0 && TextText != null)
        {
            te.text = k.ToString();
            k--;
            yield return new WaitForSeconds(1);
        }
        Pause();
        ContinueAdsButton.SetActive(false);
        GameOver();
        start = false;
    }*/
}
