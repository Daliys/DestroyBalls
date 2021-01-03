using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestUIPanel : MonoBehaviour
{
    public GameObject QuestGenerator;
    public TextAsset quest1;

    void Start()
    {
        
    }

    public void StartQuest(int id)
    {
        QuestGenerator.GetComponent<QuestGenerator>().SetQuest(quest1.text);
        //quest1.text


        gameObject.SetActive(false);
    }
    
}
