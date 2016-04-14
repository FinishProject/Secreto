using UnityEngine;
using System.Collections;

public class QuestInfo
{
    public string targetName = null;
    public int completNum = 0;
    public int questType = 0;
    public GameObject rewardItem;
}

public enum QuestType { HUNT, COLLECT, }


public class QuestMgr : MonoBehaviour {

    private QuestInfo questInfo;
    private QuestType questType;

    public static QuestMgr instance;

    void Awake()
    {
        instance = this;
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
                Debug.Log("COLLECT");
                break;
        }
    }
    // 퀘스트 완료
    void Complete()
    {
        Debug.Log("Complete");
    }
    // 퀘스트 정보를 받아옴
    public void GetQuestInfo(QuestInfo info)
    {
        questInfo = info;
        questType = (QuestType)questInfo.questType;
    }
}
