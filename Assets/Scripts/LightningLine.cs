using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningLine : MonoBehaviour {

    public LineRenderer lineRender;
	public GameObject generator;
	private GameObject randomObj;

	// Use this for initialization
	void Start () {
		

	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.L))
        {
            lineRender.positionCount = 3;
            lineRender.SetPosition(2, new Vector3(400,600,1));
            //lineRende
        }

		if (Input.GetKeyDown(KeyCode.H))
		{
			bool isFind = false;
			while (!isFind) {
				randomObj = generator.transform.GetChild(Random.RandomRange(0, generator.transform.childCount)).gameObject;
				if (randomObj.transform.childCount > 0)
				{
					randomObj = randomObj.transform.GetChild(Random.RandomRange(0, randomObj.transform.childCount)).gameObject;
					if (randomObj.transform.childCount > 0 && randomObj.transform.parent.gameObject.active)
					{
						isFind = true;
						randomObj.transform.GetComponent<SpriteRenderer>().color = new Color32(18, 255, 2, 255);
					}
				}
				
			}

			


		}
	}

    

}
