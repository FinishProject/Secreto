using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillStruct
{
    public int id;
    public string name;
    public int function;
    public int value;
    public int precedeID;

    public void SetData(int id, string name, int function, int value, int precedeID)
    {
        this.id = id;
        this.name = name;
        this.function = function;
        this.value = value;
        this.precedeID = precedeID;
    }
}

public class ExpTable
{
    public int level;
    public int exp;

    public void SetData(int level, int exp)
    {
        this.level = level;
        this.exp = exp;
    }
}

public class SkillMgr : MonoBehaviour {

    public static SkillMgr instance; // 싱글톤 인스턴스

    ExpTable curLevelState;          // 현재 레벨 정보
    ExpTable nxtLevelState;          // 다음 레벨 정보

    private CSVParser ExpTable;      // 경험치 테이블
    private CSVParser SkillList;     // 스킬 리스트

    
    void Awake()
    {
        instance = this;
        
        ExpTable = new CSVParser("ExpTable");
        ExpTable.Load();
        SkillList = new CSVParser("SkillList");
        SkillList.Load();

        curLevelState = new ExpTable();
        curLevelState.SetData(1, 0);
        nxtLevelState = new ExpTable();
        ExpTable.ParseByLevel(nxtLevelState, curLevelState.level + 1);
    }

    // 경험치를 얻을때
    public void GetEXPoint(int exp)
    {
        curLevelState.exp += exp;

        // 경험치 비교, 레벨 업
        if (curLevelState.exp >= nxtLevelState.exp)
        {
            curLevelState.level++;
            // 다음 레벨 정보를 불러온다
            ExpTable.ParseByLevel(nxtLevelState, curLevelState.level + 1);
        }
        Debug.Log(curLevelState.level + "   " + curLevelState.exp);
    }

}
