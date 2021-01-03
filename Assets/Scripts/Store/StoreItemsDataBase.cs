using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.IO;

public class StoreItemsDataBase : MonoBehaviour
{

	public GameObject prefab;                                       // Keep store cell prefab
	List<GameObject> InstanseObj = new List<GameObject>();          // Array of store cells
	public Text coinText;                                           // Text of price the selected cell 
	public string pathToPrefabs;                                    // string - path to prefabs location
	GameObject selectedItem;                                        // prefab ball of the selected cell
	GameObject[] storeGameObjects;                                       // All prefabs 
	public GameObject buttonBuy;

	public GameObject PableBuy;
	public Canvas CanvasInterface;

	public enum TypeTabStorage { PlayerBalls, Backgrounds, Bonuses }
	public TypeTabStorage typeTab;



	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.B))
		{
			Generated();
			AddBonuses();
		}
		
	}

	private void Awake()
	{
		Generated();
		
	}

	public void Start()
	{
		if(typeTab == TypeTabStorage.Bonuses)
		AddBonuses();
	}

	public void AddBonuses()
	{
		
		if (PlayerPrefs.HasKey("StoteBoughtID" + typeTab + "0"))
		{
			Game.BoombDamage += storeGameObjects[0].GetComponent<StoreInformation>().AddBonus;
			print("Bomb " + Game.BoombDamage);
		}
		if (PlayerPrefs.HasKey("StoteBoughtID" + typeTab + "1"))
		{
			Game.ShanceSpawnBonusedBall += storeGameObjects[1].GetComponent<StoreInformation>().AddBonus;
			print("Chance " + Game.ShanceSpawnBonusedBall);
		}
		if (PlayerPrefs.HasKey("StoteBoughtID" + typeTab + "2"))
		{
			Game.MainBallHillPoint += storeGameObjects[2].GetComponent<StoreInformation>().AddBonus;
			print("HP " + Game.MainBallHillPoint);
		}

		gameObject.transform.parent.gameObject.SetActive(false);

	}

	public void Generated()
	{
		if (typeTab == TypeTabStorage.Bonuses)
		{
			storeGameObjects = Resources.LoadAll<GameObject>(pathToPrefabs);
			//print(storeGameObjects.Length + " -----------");
			foreach (var item in storeGameObjects)
			{
				GameObject obj = Instantiate(prefab, transform);
				obj.GetComponent<Image>().sprite = item.GetComponent<Image>().sprite;

				InstanseObj.Add(obj);
			}
			return;
		}
		storeGameObjects = Resources.LoadAll<GameObject>(pathToPrefabs);
		foreach (var item in storeGameObjects)
		{
			GameObject obj = Instantiate(prefab, transform);
			obj.GetComponent<Image>().sprite = item.GetComponent<SpriteRenderer>().sprite;


			InstanseObj.Add(obj);
		}

	}


	public void SelectItem(GameObject item)
	{
		int index = InstanseObj.FindIndex(s => s.Equals(item));
		selectedItem = storeGameObjects[index].gameObject;

		if (!PlayerPrefs.HasKey("StoteBoughtID" + typeTab + selectedItem.GetComponent<StoreInformation>().ID) && storeGameObjects[index].GetComponent<StoreInformation>().price != 0)
		{
			coinText.text = storeGameObjects[index].GetComponent<StoreInformation>().price.ToString();
		}
		else
		{
			coinText.text = "Choose";
			if (typeTab == TypeTabStorage.Bonuses) coinText.text = "MAX";

		}
		buttonBuy.SetActive(true);

	}


	public void BuyItemButton()
	{

		if (!PlayerPrefs.HasKey("StoteBoughtID" + typeTab + selectedItem.GetComponent<StoreInformation>().ID))
		{
			if (selectedItem.GetComponent<StoreInformation>().price <= Game.coins)
			{
				
				Game.coins -= selectedItem.GetComponent<StoreInformation>().price;
				GameObject tj = Instantiate(PableBuy, CanvasInterface.transform.position, PableBuy.transform.rotation, CanvasInterface.transform);
				Destroy(tj, 1.3000f);
				buttonBuy.SetActive(false);
				EventClick.isStoreItemSelected = false;

				switch (typeTab)
				{
					case TypeTabStorage.PlayerBalls:
						Game.payIdBalls = selectedItem.GetComponent<StoreInformation>().ID;
						Game.isPayMainBalls = true;
						PlayerPrefs.SetInt("StoteBoughtID" + typeTab + selectedItem.GetComponent<StoreInformation>().ID, selectedItem.GetComponent<StoreInformation>().ID);
						break;

					case TypeTabStorage.Backgrounds:
						Game.isPayBackgraunds = true;
						Game.payIdBackgraunds = selectedItem.GetComponent<StoreInformation>().ID;
						PlayerPrefs.SetInt("StoteBoughtID" + typeTab + selectedItem.GetComponent<StoreInformation>().ID, selectedItem.GetComponent<StoreInformation>().ID);
						break;

					case TypeTabStorage.Bonuses:
						if(selectedItem.GetComponent<StoreInformation>().ID == 0)
						{
							Game.BoombDamage += selectedItem.GetComponent<StoreInformation>().AddBonus;
						}
						else if (selectedItem.GetComponent<StoreInformation>().ID == 1)
						{
							Game.ShanceSpawnBonusedBall += selectedItem.GetComponent<StoreInformation>().AddBonus;
						}
						else if(selectedItem.GetComponent<StoreInformation>().ID == 2)
						{
							Game.MainBallHillPoint += selectedItem.GetComponent<StoreInformation>().AddBonus;
						}
						PlayerPrefs.SetInt("StoteBoughtID" + typeTab + selectedItem.GetComponent<StoreInformation>().ID, selectedItem.GetComponent<StoreInformation>().ID);


						break;
				}

				buttonBuy.SetActive(false);
				EventClick.isStoreItemSelected = false;
			}
		}
		else
		{
			buttonBuy.SetActive(false);
			EventClick.isStoreItemSelected = false;
		}

		
		PlayerPrefs.SetInt("Coins", Game.coins);

		Debug.Log("Item " + selectedItem + " successfully purchased");
	}

	
}

	


