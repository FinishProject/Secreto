using UnityEngine;
using System.Collections;

/*************************   정보   **************************

    아이템 드랍을 해주는 클래스

    사용방법 :
    변수 옆 주석 확인

*************************************************************/

public class ItemDrap : MonoBehaviour {

    public bool isRandomNumberDrap;     // 갯수 랜덤
    [System.Serializable]
    public struct DarpItemInfo
    {
        public bool HpRecover;          // 회복 아이템 드랍 할 것인가?
        public int  HpRecoverNumber;    // 드랍할 회복 아이템 개수
        public bool Exp;                // 경험치 
        public int  ExpNumber;          // 경험치 개수
    }
    public DarpItemInfo darpItemList;
    public float drapRange;             // 드랍 범위

	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void SetDarpPos(Transform targetPos)
    {
        ItemMgr.instance.GetItem(ItemFunction.HpRecovery).UseItem();
    }

}
