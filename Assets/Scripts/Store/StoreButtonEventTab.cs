using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreButtonEventTab : MonoBehaviour {


	public GameObject[] scrollPanels;
	public StoreButtonBuyListener storeButtonBuyListener;


	public void OpenStore()
	{
		if (scrollPanels.Length <= 0) return;

		storeButtonBuyListener.DisableBuyButton();
		foreach (var item in scrollPanels)
		{
			item.SetActive(false);
		}
		scrollPanels[0].SetActive(true);
		

	}

	public void ButtonEvent(int numButton)
	{
		if (scrollPanels.Length <= 0)
		{
			Debug.LogError("ScrollPanels.Length = 0");
			return;	
		}
		if(numButton - 1 >= scrollPanels.Length )
		{
			Debug.LogError("NumButton:" + numButton + " >= ScrollPanels.Length :" + scrollPanels.Length);
			return;
		}

		storeButtonBuyListener.DisableBuyButton();
		foreach (var item in scrollPanels)
		{
			item.SetActive(false);
		}

		scrollPanels[numButton-1].SetActive(true);



	}

}
