using UnityEngine;
using System.Collections;

/*************************   정보   **************************

    회복 아이템

    나중에 작성해야지~

*************************************************************/

public class HpRecoveryItem : MonoBehaviour {

    private ItemStruct itemData;
    public bool isBulkRecovery;

    void Start () {
        itemData = new ItemStruct();
        initData();
    }

    void initData()
    {
        if(isBulkRecovery)
            ItemMgr.instance.GetItem().ParseByID(itemData, 1); // 엑셀 시트 ID값 참조
        else
            ItemMgr.instance.GetItem().ParseByID(itemData, 0);
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.tag.Equals("Player"))
        {
            col.GetComponent<PlayerCtrl>().getRecovery(itemData.value);
            gameObject.SetActive(false);
        }
    }
	
}
