﻿using UnityEngine;
using System.Collections;

public class MentalItem : MonoBehaviour {

    public float removeTime = 5.0f;         // 아이템이 사라지는데 걸리는 시간
    private ItemStruct itemData;            // 아이템의 정보
    private Transform thisTr;               // 위치 확인
    private bool isGetPossible = false;     // 플레이어가 아이템을 습득 가능한지 판단
    private bool isCollSomething = false; 
    private float gr = 5f;

    void initData(bool isBulkMental)
    {
        itemData = new ItemStruct();

        // 경험치 아이템의 종류를 구분, 정보를 불러온다.
        if (isBulkMental)
            ItemMgr.instance.GetItem().ParseByID(itemData, 2); // 엑셀 시트 ID값 참조
        else
            ItemMgr.instance.GetItem().ParseByID(itemData, 3);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag.Equals("Player") && isGetPossible)
        {
            SkillCtrl.instance.AddEnhance();
            StopAllCoroutines();                        // 사용중인 코루틴 정지
            isGetPossible = false;
            gameObject.SetActive(false);
        }

        if (col.tag.Equals("Ground") || col.tag.Equals("OBJECT"))
        {
            isCollSomething = true;
        }
    }

    // 드랍 해주는 함수       시작 위치        목표 위치       회복 아이템 종류
    public void Dropped(Vector3 StartPos, Vector3 tagetPos, bool isBulkMental)
    {
        gameObject.SetActive(true);
        initData(isBulkMental);
        StartCoroutine(Moving(StartPos, tagetPos));
        StartCoroutine(OffActive());
    }

    // 목표지점까지 움직임
    IEnumerator Moving(Vector3 StartPos, Vector3 tagetPos)
    {
        thisTr = GetComponent<Transform>();
        thisTr.position = StartPos;

        float oldTime = 0;
        while (true)
        {
            oldTime += Time.deltaTime;
            if (oldTime >= 0.4f)
            {
                isGetPossible = true;
                StartCoroutine(fall());
                break;
            }
            if (Vector3.Distance(thisTr.position, tagetPos) > 0.5f)
            {
                thisTr.position = Vector3.Lerp(thisTr.position, tagetPos, Time.deltaTime);
            }

            yield return null;
        }
    }

    // 바닥으로 떨어짐
    IEnumerator fall()
    {
        yield return new WaitForSeconds(1.0f);
        thisTr = GetComponent<Transform>();
        Vector3 tempPos = thisTr.position;
        while (true)
        {
            if (isCollSomething)
                break;

            tempPos.y -= gr * Time.deltaTime;
            thisTr.position = Vector3.Lerp(thisTr.position, tempPos, Time.deltaTime);

            yield return null;
        }
    }

    // 시간내 먹지않으면 사라짐
    IEnumerator OffActive()
    {
        yield return new WaitForSeconds(removeTime);
        gameObject.SetActive(false);
    }

}