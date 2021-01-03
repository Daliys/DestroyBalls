using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StoreButtonBuyListener : MonoBehaviour , IPointerClickHandler, IPointerUpHandler {
	public GameObject buttonBuy1;
	public GameObject buttonBuy2;
	public GameObject buttonBuy3;
	public GameObject buttonBuy4;

	public void OnPointerClick(PointerEventData eventData)
	{
		DisableBuyButton();
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		DisableBuyButton();
	}

	public void DisableBuyButton()
	{
		buttonBuy1.SetActive(false);
		buttonBuy2.SetActive(false);
		buttonBuy3.SetActive(false);
		buttonBuy4.SetActive(false);
		EventClick.isStoreItemSelected = false;

	}
}
