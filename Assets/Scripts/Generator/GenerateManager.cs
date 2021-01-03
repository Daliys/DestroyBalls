using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateManager : MonoBehaviour {

    
    public TypesBalls[] typesBalls;     //  All types of balls generated
    public int numBallsRow = 7;         //  Number of balls in a row
    public Vector3 sizeOfBall = new Vector3(1.5f, 1.5f, 1.5f);
    public Vector3 startingPosition = new Vector3(-2.5f, -5f, 1);
    public Vector2 distanceBetweenBalls = new Vector2(0.6f, 0.6f);
    public float speedMoveUp = 0.02f;   
    public int linesToDeath = 16 + 2;   //10 - gameField and 2 - below gameField
	public GameObject generatePosition;
	public GameObject lastLineGameOver;

	//public static Vector3 positionGameOverLine;

    private List<LineBalls> listLines;  // list that stores all the lines with balls and position number of the line
    private bool isNeedMoveUp = false;  
	private List<int> pastGenerationLineIndexs;

	// Use this for initialization
	void Start () {

		listLines = new List<LineBalls>();
		pastGenerationLineIndexs = new List<int>();
		PreGenerateLine();

		StartCoroutine(SetGamePosition());
	}

	IEnumerator SetGamePosition()
	{
		for(int i = 0; i < 4; i++) yield return new WaitForFixedUpdate();

		Vector3 pos = Camera.main.ScreenToWorldPoint(generatePosition.transform.position);
		startingPosition = new Vector3(startingPosition.x, pos.y - distanceBetweenBalls.y * 0.45f, startingPosition.z);
		lastLineGameOver.transform.position = new Vector3(0, pos.y + (linesToDeath-1) * distanceBetweenBalls.x + distanceBetweenBalls.y * 0.25f, 0);
	}

	private void FixedUpdate()
	{
		
		if (isNeedMoveUp)
		{
			MoveLinesUp();
		}
	}


	void Update () {
		//print(Camera.main.ScreenToWorldPoint(generatePosition.transform.position));
		// for debugging
		{
            if (Input.GetKeyDown(KeyCode.P))
            {
                if (!isNeedMoveUp) PreGenerateLine();
            }

            if (Input.GetKeyDown(KeyCode.M))
            {
                isNeedMoveUp = true;
            }	
        }

		if (!isNeedMoveUp && listLines.Count != 0 && listLines[listLines.Count - 1].positionLine != 0)
		{
		//	PreGenerateLine();
		}

		if (Game.isGenerate && !Game.isQuestGameMode)
		{
			isNeedMoveUp = true;
		}
	
	}

	private void PreGenerateLine()
	{
		List<int> indexGenerateBalls = new List<int>();
		int numNullBalls = 0;		//number of empty balls

		// cycle for regeneration if the line of past balls conincides with new line
		for (int i = 0; i < 3; i++)	
		{
			numNullBalls = 0;
			indexGenerateBalls.Clear();
			for (int a = 0; a < numBallsRow; a++)
			{
				float random = Random.value;
				float sumChances = 0f;
				for(int r = 0; r < typesBalls.Length; r++)
				{
					sumChances += typesBalls[r].chance;
					if(random < sumChances)
					{
						indexGenerateBalls.Add(r);
						break;
					}
				}
				if(random >= sumChances)
				{
					indexGenerateBalls.Add(-1);
					numNullBalls++;
				}
			}
				if (!pastGenerationLineIndexs.Equals(indexGenerateBalls) && numNullBalls != numBallsRow) break;
		}
		// this is a check for at least one ball in a row
		if(numNullBalls == numBallsRow && typesBalls.Length != 0)
		{
			indexGenerateBalls[Random.Range(0, indexGenerateBalls.Count)] = Random.Range(0, typesBalls.Length);
		}

		pastGenerationLineIndexs = indexGenerateBalls;
		AddChanseBalls();
		GenerateLine(indexGenerateBalls);
	}


	// create ball objects (GameObject) on scene
    private void GenerateLine(List<int> indexGeneration)
    {
		// Line generation with balls 
        GameObject line = new GameObject();
        line.transform.parent = this.transform;
        line.gameObject.name = "BallLines";
        //line.transform.position = new Vector3(0, startingPosition.y - (listLines.Count * distanceBetweenBalls.y), startingPosition.z);	
        line.transform.position = new Vector3(0, startingPosition.y , startingPosition.z);	
        line.transform.localScale = new Vector3(1, 1, 1);

		line.SetActive(false);
		// balls generation 
        for (int i = 0; i < indexGeneration.Count; i++)
        {
			if (indexGeneration[i] == -1) continue;
			GameObject game = Instantiate(typesBalls[indexGeneration[i]].prefab, new Vector3(0, 0, 0), Quaternion.identity);
            game.transform.parent = line.transform;
            game.transform.localPosition = new Vector3(startingPosition.x + (distanceBetweenBalls.x * i) ,0 ,0);
            game.transform.localScale = new Vector3(sizeOfBall.x , sizeOfBall.y , sizeOfBall.z);
        }

        listLines.Add(new LineBalls(line,0));
        RemoveEmptyLines();
        Game.SpawnBallHillPoint++;
		//	print("After " + Game.SpawnBallHillPoint);
    }

	private void AddChanseBalls()
	{
		float chanseSpawn = 0f;
		for(int i = 0; i < typesBalls.Length; i++)
		{
			chanseSpawn += typesBalls[i].chance;
		}

		if(chanseSpawn < 0.8f)
		{
			for (int i = 0; i < typesBalls.Length; i++)
			{
				typesBalls[i].chance += typesBalls[i].AddValue;
			}
		}

	}


	private void RemoveEmptyLines()
	{
		for (int i = 0; i < listLines.Count; i++)
		{
			if (listLines[i].gameLine.transform.childCount == 0)
			{
				Destroy(listLines[i].gameLine);
				listLines.RemoveAt(i);
				i--;
			}
			else
			{
				break;
			}
		}
		//if the position of the topmost element is equal to the position of the line to death, then means game over
		if (listLines.Count > 0 && listLines[0].positionLine == linesToDeath) Game.isGameOver = true;

		if (listLines.Count > 0 && listLines[0].positionLine >= linesToDeath - 1) lastLineGameOver.transform.GetComponent<GameOverLine>().SetActiveColor();
		else lastLineGameOver.transform.GetComponent<GameOverLine>().SetDisableColor();
	}

    //move All lines up 1 position
    private void MoveLinesUp()
    {
		float addMoveUp = speedMoveUp;
		bool isAddNumLinePosition = false;

		if (listLines.Count != 0)
		{
            float currentPositionY = listLines[0].gameLine.transform.position.y;
			float positionFinishY = ((listLines[0].positionLine + 1) * distanceBetweenBalls.y) + startingPosition.y;

			if((positionFinishY - currentPositionY) <= addMoveUp)
			{
				addMoveUp = positionFinishY - currentPositionY;
				isNeedMoveUp = false;
				isAddNumLinePosition = true;
			
				if(listLines.Count >= 1) Game.isGenerate = false;

			}
		}
		else {
			isNeedMoveUp = false;
			PreGenerateLine();
			return;
		}

		for (int i = 0; i < listLines.Count; i++)
		{
				listLines[i].gameLine.gameObject.transform.position = new Vector3(
				listLines[i].gameLine.gameObject.transform.position.x,
				listLines[i].gameLine.gameObject.transform.position.y + addMoveUp,
				listLines[i].gameLine.gameObject.transform.position.z);

			if (isAddNumLinePosition)
			{
				listLines[i].positionLine++;
			}	
		}

		if (listLines.Count >= 1) listLines[listLines.Count - 1].gameLine.SetActive(true);
		if (!isNeedMoveUp) PreGenerateLine();
	}

	/// <summary>
	/// Class contains fild gameLine (GameObject) and positionLine (int)
	/// </summary>
    private class LineBalls
    {
        public GameObject gameLine;
        public int positionLine;

        public LineBalls(GameObject line, int position)
        {
            gameLine = line;
            positionLine = position;
        }
    }
}
