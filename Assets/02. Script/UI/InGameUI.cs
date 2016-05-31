﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/****************************   정보   ****************************

    인게임 UI 클래스
    HP, 현재 속성, 인핸스 수치(MP) 등 표시

    사용방법 :

    1. 컴포넌트를 연결
    2. ChangeAttribute(), ChangeHpBar() 등 변경될때 마다 외부에서 호출 
    
******************************************************************/

public class InGameUI : MonoBehaviour {

    [System.Serializable]
    public struct AttributeImage
    {
        public Image backGroundAttribute;
        public Image redAttribute;
        public Image blueAttribute;
    }

    public AttributeImage attributeImage;

    /*
    public GameObject hpbar;
    public GameObject attribute;
    public GameObject enhanceGauge;
    */
    public Image hpBar;
    public Image enhanceGauge;

    public static InGameUI instance;

    void Awake()
    {
        instance = this;


    }

    void Start()
    {
        InitUIValue();
    }

    // UI들의 수치값을 최신화
    public void InitUIValue()
    {
        ChangeHpBar();
//        ChangeAttribute();
    }

    // HP바 수치를 바꿔줌 (외부 호출)
    public void ChangeHpBar()
    {
        hpBar.fillAmount = PlayerCtrl.instance.ProportionHP;
    }


    // 인핸스 수치 변경 (외부 호출)
    public void ChangeEnhance()
    {
        enhanceGauge.fillAmount = SkillCtrl.instance.ProportionEnhance;
    }


    // 속성을 바꾼다 (외부 호출)
    public void ChangeAttribute()
    {
        attributeImage.redAttribute.gameObject.SetActive(true);
        switch (SkillCtrl.instance.curAttribute)
        {
            case AttributeState.noraml:
                attributeImage.redAttribute.gameObject.SetActive(false);
                attributeImage.blueAttribute.gameObject.SetActive(false);
                attributeImage.backGroundAttribute.fillAmount = 1f;
                break;
            case AttributeState.red:
                attributeImage.redAttribute.gameObject.SetActive(true);
                attributeImage.blueAttribute.gameObject.SetActive(false);
                break;
            case AttributeState.blue:
                attributeImage.redAttribute.gameObject.SetActive(false);
                attributeImage.blueAttribute.gameObject.SetActive(true);
                break;
        }
    }

    // 기존 속성 삭제, 속성 변경 (외부 호출)
    public void ChangeCountDownForAttributeBar()
    {
        StopAllCoroutines();
        StartCoroutine( ChangeAttributeBar());
    }

    // 속성 변경
    IEnumerator ChangeAttributeBar()
    {
        float countDown = 1f;
        while (true)
        {
            countDown -= 0.05f;
            yield return new WaitForSeconds(0.02f);
            attributeImage.backGroundAttribute.fillAmount = countDown;
            if(countDown < 0.05f)
            {
                break;
            }
        }
    }

    
}
