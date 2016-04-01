using UnityEngine;
using System.Collections;

/*************************   정보   **************************

    회복 아이템

    나중에 작성해야지~

*************************************************************/

public class HpRecoveryItem : MonoBehaviour{

    private ItemStruct itemData;
    private Transform thisTr;
    private bool isGetPossible = false;
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
        if(col.tag.Equals("Player") && isGetPossible)
        {
            col.GetComponent<PlayerCtrl>().getRecovery(itemData.value);
            StopAllCoroutines();
            isGetPossible = false;
            gameObject.SetActive(false);
        }
        if(col.tag.Equals("Ground"))
        {
            StartCoroutine(OffActive());
        }
    }

    public void Dropped(Vector3 StartPos, Vector3 tagetPos)
    {
        gameObject.SetActive(true);
        StartCoroutine(Moving(StartPos, tagetPos));
    }

    IEnumerator Moving(Vector3 StartPos, Vector3 tagetPos)
    {
        thisTr = GetComponent<Transform>();
        thisTr.position = StartPos;

        float oldTime = 0;
        while(true)
        {          
            if (oldTime >= 0.1f)
            {
                isGetPossible = true;
                break;   
            }
            else if (Vector3.Distance(thisTr.position, tagetPos) > 0.5f)
            {
                thisTr.position = Vector3.Lerp(thisTr.position, tagetPos, Time.deltaTime);
            }
            else
            {
                oldTime += Time.deltaTime;
            }
            yield return null;
        }
    }

    IEnumerator OffActive()
    {
        yield return new WaitForSeconds(2.0f);
        gameObject.SetActive(false);
    }

}
