using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.IO;

public class _MM : MonoBehaviour
{

    public Vector2 mapSize;
    public GameObject ImgaePrefab;

    [SerializeField]
    public int[,] mapItemType;
    public int[,] mapItemXP;

    void Start()
    {
        GetComponent<GridLayoutGroup>().constraintCount = (int)mapSize.x;
        mapItemType = new int[(int)mapSize.x, (int)mapSize.y];
        mapItemXP = new int[(int)mapSize.x, (int)mapSize.y];

        for (int i = 0; i < mapItemType.GetLength(0); i++)
        {
            for (int j = 0; j < mapItemType.GetLength(1); j++)
            {
                mapItemType[i, j] = 0;
                mapItemXP[i, j] = 0;
            }
        }


        GenerateGrid();
    }

    public void GenerateGrid()
    {
        for (int i = 0; i < mapSize.y; i++)
        {
            for (int j = 0; j < mapSize.x; j++)
            {
                GameObject gb = Instantiate(ImgaePrefab, transform, false);
                gb.transform.parent = transform;
                gb.GetComponent<ImageContorller>().id = new ImageContorller.ID(j, i);
            }
        }
    }

    public void PressImageListener(ImageContorller.ID id)
    {
        mapItemType[id.x, id.y] = GENERAL_MOUSE.CurrentSelectedID;
        if (GENERAL_MOUSE.CurrentSelectedID == 0) mapItemXP[id.x, id.y] = 0;
        else mapItemXP[id.x, id.y] = GENERAL_MOUSE.XpBalls;
    }

    public void SaveData()
    {
        string path = "Assets/_FOR_SCENE2/Output.txt";
        int itter = 0;
        while(File.Exists(path))
        {
            path = "Assets/_FOR_SCENE2/Output" + itter + ".txt";
            itter++;
        }
        //FileStream stream = new FileStream(path, FileMode.CreateNew);

        using (StreamWriter sr = new StreamWriter(path))
        {
            for (int j = 0;j < mapSize.y; j++)
            {
                for (int i = 0; i < mapSize.x; i++)
                {
                    sr.Write(mapItemType[i, j].ToString());

                    if (mapItemType[i, j] == 6 || mapItemType[i, j] == 4 || mapItemType[i, j] == 5 || mapItemType[i,j] == 8)
                    {
                        sr.Write("_1 ");
                    }
                    else
                    {
                        sr.Write("_" + mapItemXP[i, j].ToString() + " ");
                    }                                     
                }
                sr.WriteLine();
            }

            sr.WriteLine("MainBall: " + GENERAL_MOUSE.CurrentSelectedMainBallID);
            sr.WriteLine("MainBallXP: " + GENERAL_MOUSE.XPMainBall);
            sr.WriteLine("MoveUp: " + GENERAL_MOUSE.IncreaseUp);
            sr.WriteLine("NumThrows: " + GENERAL_MOUSE.numOfThrows);
            
            sr.Close();

        }
    }
}
