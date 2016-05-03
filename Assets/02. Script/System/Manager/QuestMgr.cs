﻿using UnityEngine;
using System.Collections;

public class QuestInfo
{
    public string targetName = null;
    public int completNum = 0;
    public int questType = 0;
    //public GameObject rewardItem;
}

public enum QuestType { HUNT, COLLECT, }


public class QuestMgr : MonoBehaviour {

    public static QuestInfo questInfo;
    public int curCompletNum = 0;
    private QuestType questType;

    public static QuestMgr instance;

    void Awake()
    {
        instance = this;
    }

    // 퀘스트 정보를 받아옴
    public void GetQuestInfo(QuestInfo info)
    {
        questInfo = info;
        questType = (QuestType)questInfo.questType;
        QuestTypes();
    }

    // 퀘스트 종료
    void QuestTypes()
    {
        switch (questType)
        {
            case QuestType.HUNT:
                Debug.Log("Hunt");
                break;
            case QuestType.COLLECT:
                StartCoroutine(CollectQuest());
                break;
        }
    }
    // 퀘스트 완료
    void Complete()
    {
        Debug.Log("Complete");
    }
    
    // 수집 퀘스트
    IEnumerator CollectQuest()
    {
        while (true)
        {
            // 목표 아이템 개수 이상 수집 시 퀘스트 완료
            if(questInfo.completNum <= curCompletNum)
            {
                Debug.Log("Complete");
                ScriptMgr.instance.isQuest = true;
                break;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}
