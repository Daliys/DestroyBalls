using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GENERAL_MOUSE : MonoBehaviour
{
    public GameObject CurrentSelecterd;
    public GameObject CurrentSelecterdMainBall;
    public static int CurrentSelectedID;
    public static int CurrentSelectedMainBallID;
    public static Image currentImage;
    public static int XpBalls = 1;
    public static int XPMainBall = 1;
    public static int IncreaseUp = 0;
    public static int numOfThrows = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // для выбора рисования шаров
    public void ClickByTools(int index)
    {
        CurrentSelectedID = index;
    }

    public void ChangeSelected(GameObject gm)
    {
        CurrentSelecterd.GetComponent<Image>().sprite = gm.GetComponent<Image>().sprite;
        currentImage = gm.GetComponent<Image>();
    }

    // измененние хп шарov
    public void ChangeXPBalls(InputField field)
    {
        XpBalls = int.Parse(field.text);
    }

    // изменения основного шара
    public void ClickMainBalls(int index)
    {
        CurrentSelectedMainBallID = index;
    }
    public void ChangeSelectedMainBall(GameObject gm)
    {
        CurrentSelecterdMainBall.GetComponent<Image>().sprite = gm.GetComponent<Image>().sprite;
    }

    public void ChangeXPMainBalls(InputField field)
    {
        XPMainBall = int.Parse(field.text);
    }

    public void IncreaceUpBalls(InputField field)
    {
        IncreaseUp = int.Parse(field.text);
    }

    public void NumberThrows(InputField field)
    {
        numOfThrows = int.Parse(field.text);
    }
}
