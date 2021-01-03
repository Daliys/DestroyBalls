using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddHpText : MonoBehaviour {

    Text text;

	void Start () {
        text = GetComponent<Text>();
        text.text = "+"+Game.AddHpCount;
    }
	

	void Update () {
        text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - 0.005f);
        if (text.color.a <= 0) Destroy(gameObject);
	}
}
