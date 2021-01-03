using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BonusesItems : MonoBehaviour, IPointerClickHandler
{
	// Вся обработка бонусов сделана от пизды. Все держится на кастылях и замазано говнокодом ....
	// да можно было бы сделать все хорошо. Ноооо. Нужно все переписывать. А на это нет времени и желания. так что как то так 
	// Да Помолимся же великому шарпу. Богу кода нашего.
	// C# наш сущий в памяти!
	// да компилируется код Твой;
	// да приидет царствие Софта Твоего;
	// избавь же нас от багов глазом неуловимых 
	// Ентер
	//


	public GameObject buttonBuy;
	//public Text textDescription;
	public  TypeBonuses typeBonuses;
	private static TypeBonuses SelectedItem;

	public static bool isActive;

	// <>Upgrade data<>
	private static int priceBoomb;
	private static int increacePriceBoomb = 30;

	private static int priceSpawnBonusedBalls;
	private static int increacePriceSpawnBonusedBall = 60;

	private static int priceAddHpCount;
	private static int increaceAddPriceHpCount = 140;

	private static int priceMainBallHp;
	private static int increacePriceMainBallHp = 90;


	// </>UpgradeData</>

	private static bool isUpgrade = false;

	public enum TypeBonuses {Null,Boombs, SpawnBonusedBall, AddHpCount, MainBallHp};

	private void OnEnable()
	{
		isActive = true;
	}

	private void OnDisable()
	{
		isActive = false;
	}



	private void Start()
	{
		priceBoomb = Preservation("priceBonusesBoomb", 550);
		priceSpawnBonusedBalls = Preservation("priceChanseSpawnBonusedBall", 500);
		priceAddHpCount = Preservation("priceAddHpCount", 600);
		priceMainBallHp = Preservation("priceMainBallHp", 450);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		//print(" type " + typeBonuses.ToString() + " sh " + Game.ShanceSpawnBonusedBall + " bm " + Game.BoombDamage);
		gameObject.transform.GetChild(0).gameObject.SetActive(true);
		SelectedItem = typeBonuses;
		EventClick.isStoreItemSelected = true;
		buttonBuy.SetActive(true);
		switch (SelectedItem)
		{
			case TypeBonuses.Boombs:
				if (Game.BoombDamage >= 75) buttonBuy.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "MAX";
				else buttonBuy.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = priceBoomb.ToString();
				break;
			case TypeBonuses.SpawnBonusedBall:
				if (Game.ShanceSpawnBonusedBall >= 150) buttonBuy.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "MAX";
				else buttonBuy.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = priceSpawnBonusedBalls.ToString();
				break;
			case TypeBonuses.AddHpCount:
				if (Game.AddHpCount >= 5) buttonBuy.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "MAX";
				else buttonBuy.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = priceAddHpCount.ToString();
				break;
			case TypeBonuses.MainBallHp:
				if (Game.MainBallHillPoint >= 15) buttonBuy.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "MAX";
				else buttonBuy.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = priceMainBallHp.ToString();
				break;

		}
        PlayerPrefs.SetInt("Coins", Game.coins);
    }

	private void Update()
	{
		if (SelectedItem != typeBonuses) gameObject.transform.GetChild(0).gameObject.SetActive(false);
		else if (isUpgrade)
		{
			isUpgrade = false;
			OnPointerClick(null);
		}
		if (!EventClick.isStoreItemSelected) buttonBuy.SetActive(false);

		/*
		if (Input.GetKey(KeyCode.Q))
		{
			print("Bomb " + Game.BoombDamage);
			print("Shan " + Game.ShanceSpawnBonusedBall);
			print("Add " + Game.AddHpCount);
			print("Main " + Game.MainBallHillPoint);

		}
		*/
	}




	public static void UpgradeButton()
	{


		switch (SelectedItem)
		{
			case TypeBonuses.Boombs:
				if (Game.BoombDamage >= 75) break;
				if (Game.coins >= priceBoomb)
				{
					Game.BoombDamage += 25;
					Game.isSaveValueBonuses = true;
					Game.coins -= priceBoomb;
					priceBoomb += increacePriceBoomb;
					ResaveValue("priceBonusesBoomb", priceBoomb);
					isUpgrade = true;
				}
				break;

			case TypeBonuses.SpawnBonusedBall:
				if (Game.ShanceSpawnBonusedBall >= 150) break;
				if (Game.coins >= priceSpawnBonusedBalls)
				{
					Game.ShanceSpawnBonusedBall += 150;
					Game.isSaveValueBonuses = true;
					Game.coins -= priceSpawnBonusedBalls;
					priceSpawnBonusedBalls += increacePriceSpawnBonusedBall;
					ResaveValue("priceChanseSpawnBonusedBall", priceSpawnBonusedBalls);
					isUpgrade = true;
				}
				break;
			case TypeBonuses.AddHpCount:
				if (Game.AddHpCount >= 5) break;
				if (Game.coins >= priceAddHpCount)
				{
					Game.AddHpCount += 1;
					Game.isSaveValueBonuses = true;
					Game.coins -= priceAddHpCount;
					priceAddHpCount += increaceAddPriceHpCount;
					ResaveValue("priceAddHpCount", priceAddHpCount);
					isUpgrade = true;
				}
				break;

			case TypeBonuses.MainBallHp:
				if (Game.MainBallHillPoint >= 15) break;
				if (Game.coins >= priceMainBallHp)
				{
					Game.MainBallHillPoint += 10;
					Game.isSaveValueBonuses = true;
					Game.coins -= priceMainBallHp;
					priceMainBallHp += increacePriceMainBallHp;
					ResaveValue("priceMainBallHp", priceMainBallHp);
					isUpgrade = true;
				}

				break;
		}
        PlayerPrefs.SetInt("Coins", Game.coins);
    }


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

	private static void ResaveValue(string keyName, int value)
	{
		PlayerPrefs.SetInt(keyName, value);
	}

	

}
