using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EventClick : MonoBehaviour, IPointerClickHandler
{

	public static bool isStoreItemSelected;

	public void OnPointerClick(PointerEventData eventData)
	{
		if (!BonusesItems.isActive)
		{
			transform.parent.gameObject.GetComponent<StoreItemsDataBase>().SelectItem(gameObject);
			isStoreItemSelected = false;
			StartCoroutine(WaitNextUpdate());
		}
		else
		{
			isStoreItemSelected = false;
			StartCoroutine(WaitNextUpdate());
		}

	}


	IEnumerator WaitNextUpdate()
	{
		yield return new WaitForFixedUpdate();
		transform.GetChild(0).gameObject.SetActive(true);
		isStoreItemSelected = true;
	}



	private void FixedUpdate()
	{
		if(!isStoreItemSelected) transform.GetChild(0).gameObject.SetActive(false);
	}


}
