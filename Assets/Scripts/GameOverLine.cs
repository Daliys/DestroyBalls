using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverLine : MonoBehaviour {

	public void SetActiveColor()
	{
		transform.GetComponent<SpriteRenderer>().color = new Color32(255, 0, 23, 255);
	}
	public void SetDisableColor()
	{
		transform.GetComponent<SpriteRenderer>().color = new Color32(255, 0, 23, 50);

	}
}
