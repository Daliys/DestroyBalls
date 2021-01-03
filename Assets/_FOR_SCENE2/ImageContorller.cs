using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ImageContorller : MonoBehaviour
{
    [System.Serializable]
    public struct ID{
        public int x, y;

        public ID(int _x , int _y)
        {
            x = _x;
            y = _y;
        }
    }

    public ID id;

    public void ImageDown()
    {
        transform.parent.GetComponent<_MM>().PressImageListener(id);
        
        GetComponent<Image>().sprite = GENERAL_MOUSE.currentImage.sprite;
        if(GENERAL_MOUSE.CurrentSelectedID == 0) transform.GetChild(0).GetComponent<Text>().text = 0 + "";
        else transform.GetChild(0).GetComponent<Text>().text = GENERAL_MOUSE.XpBalls.ToString();
    }
}
