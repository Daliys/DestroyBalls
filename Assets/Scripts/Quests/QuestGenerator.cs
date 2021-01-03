using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGenerator : MonoBehaviour
{
    [SerializeField]
    public GameObject[] ballsPrefabs;

    public GameObject generatorPositionByScreen;
    public GameObject lastLineGameOver;
    public Vector2 ballsSize;
    public int numBallsInRow;
    public int numBallsInCol;
    public float speedMoveUp;
    public Vector2 startPostion;

    private bool isNeedMoveUp = false;
    private int numMoveUpPerCircle = 0;
    private List<LineBalls> rowLines;

    void Start()
    {
        StartCoroutine(SetGamePosition());
    }

    IEnumerator SetGamePosition()
    {
        for (int i = 0; i < 4; i++) yield return new WaitForFixedUpdate();

        Vector3 pos = Camera.main.ScreenToWorldPoint(generatorPositionByScreen.transform.position);
        startPostion = new Vector2(startPostion.x, pos.y + ballsSize.y - ballsSize.y * 0.45f);
        lastLineGameOver.transform.position = new Vector3(0, pos.y + (numBallsInCol) * ballsSize.y + ballsSize.y * 0.25f, 0);
    }

    public void SetQuest(string questLevel)
    {
       
        Game.isQuestGameMode = true;
        Game.isGenerate = false;
        QuestLevelIntialized(questLevel);
    }

    private void FixedUpdate()
    {
        if (isNeedMoveUp)
        {
            MoveLinesUp();
        }
    }

    void Update()
    {
        if (!Game.isQuestGameMode) return;

        if (numMoveUpPerCircle != 0)
        {
            if (Game.isGenerate)
            {
                isNeedMoveUp = true;
            }
        }
        else
        {
            Game.isGenerate = false;
            RemoveEmptyLines();

        }
    }

    private void MoveLinesUp()
    {
        float addMoveUp = speedMoveUp;
        bool isAddNumLinePosition = false;

        if (rowLines.Count != 0)
        {
            float currentPositionY = rowLines[0].gameLine.transform.position.y;
            float positionFinishY = ((rowLines[0].positionLine + numMoveUpPerCircle) * ballsSize.y) + startPostion.y;
        
            if ((positionFinishY - currentPositionY) <= addMoveUp)
            {
                addMoveUp = positionFinishY - currentPositionY;
                isNeedMoveUp = false;
                isAddNumLinePosition = true;
            }
        }
        else
        {
            isNeedMoveUp = false;       
            return;
        }
       
        for (int i = 0; i < rowLines.Count; i++)
        {
            rowLines[i].gameLine.gameObject.transform.position = new Vector3(
            rowLines[i].gameLine.gameObject.transform.position.x,
            rowLines[i].gameLine.gameObject.transform.position.y + addMoveUp,
            rowLines[i].gameLine.gameObject.transform.position.z);

            if (isAddNumLinePosition)
            {
                rowLines[i].positionLine += numMoveUpPerCircle;
            }
        }

        if (!isNeedMoveUp) RemoveEmptyLines();
    }

    private void RemoveEmptyLines()
    {
        
        for (int i = 0; i < rowLines.Count; i++)
        {
            if (rowLines[i].gameLine.transform.childCount == 0)
            {
                Destroy(rowLines[i].gameLine);
                rowLines.RemoveAt(i);
                i--;
            }
            else break;
        }
       // if the position of the topmost element is equal to the position of the line to death, then means game over
        if (rowLines.Count > 0 && rowLines[0].positionLine >= numBallsInCol) Game.isGameOver = true;
        if (rowLines.Count == 0) print("WIN");


        if (numMoveUpPerCircle != 0)
        {
            if (rowLines.Count > 0 && rowLines[0].positionLine >= numBallsInCol - numMoveUpPerCircle) lastLineGameOver.transform.GetComponent<GameOverLine>().SetActiveColor();
            else lastLineGameOver.transform.GetComponent<GameOverLine>().SetDisableColor();
        }
        Game.isGenerate = false;
    }



    public void QuestLevelIntialized(string str)
    {
        rowLines = new List<LineBalls>();

        string[] rows = str.Split('\n');

        for (int i = 0; i < numBallsInCol; i++)
        {
            GameObject line = new GameObject();
            line.transform.parent = this.transform;
            line.gameObject.name = "BallLines";
            line.transform.position = new Vector2(0, startPostion.y + ballsSize.x * (numBallsInCol-1 - i));
            line.transform.localScale = new Vector3(1, 1, 1);

            string[] cols = rows[i].Split(' ');

            for (int j = 0; j < numBallsInRow; j++)
            {
                string[] info = cols[j].Split('_');
                if ((int.Parse(info[0]) == 0))continue;
                else
                {
                    GameObject obj = Instantiate(ballsPrefabs[int.Parse(info[0]) - 1], new Vector2(0, 0), Quaternion.Euler(0, 0, 0), line.transform);
                    obj.transform.localPosition = new Vector3(startPostion.x + (ballsSize.x * j), 0, 0);
                    obj.transform.localScale = new Vector2(ballsSize.x, ballsSize.y);
                    obj.transform.GetComponent<BallDestroy>().isNeedTakeHpFromGame = false;
                    obj.transform.GetComponent<BallDestroy>().hillPoint = int.Parse(info[1]);
                }
            }

            rowLines.Add(new LineBalls(line, (numBallsInCol - 1 - i)));
        }
        RemoveEmptyLines();

        string[] typeMaiBall = rows[numBallsInCol ].Split();
//        Game.idBallsForQuest = int.Parse(typeMaiBall[1]);

        string[] xpMain = rows[numBallsInCol +1].Split();
        Game.MainBallHillPoint = int.Parse(xpMain[1]);
       
        string[] moveUp = rows[numBallsInCol + 2].Split();
        numMoveUpPerCircle = int.Parse(moveUp[1]);
    }


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
