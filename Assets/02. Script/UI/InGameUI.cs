using UnityEngine;
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
    public struct HpBarImage
    {
        public Image fullHpBar;
        public Image curHpBar;
    }
    [System.Serializable]
    public struct AttributeImage
    {
        public Image backGroundAttribute;
        public Image redAttribute;
        public Image blueAttribute;
    }
    public HpBarImage hpBarImage;
    public AttributeImage attributeImage;

    public GameObject hpbar;
    public GameObject attribute;
    public GameObject enhanceGauge;
    private Vector3 hpbarPos;
    private Vector3 attributePos;
    private Vector3 enhanceGaugePos;

    public static InGameUI instance;

    // UI를 배치 했을때 해상도
    private float baseWidth = 600;
    private float baseHeight = 450;
    // 현재 해상도와 UI를 배치했을때 해상도의 비율
    private float proportionWidth;
    private float proportionHeight;

    void Awake()
    {
        instance = this;

        GetUIPos();
        attributeImage.redAttribute.gameObject.SetActive(false);
        attributeImage.blueAttribute.gameObject.SetActive(false);
    }

    void Start()
    {
        changeAllUIByResolution();
        InitUIValue();
    }

    // UI들의 수치값을 최신화
    public void InitUIValue()
    {
        ChangeHpBar();
        ChangeAttribute();
        ChangeEnhance();
    }

    // UI 기본 위치 저장
    void GetUIPos()
    {
        hpbarPos = hpbar.transform.localPosition;
        attributePos = attribute.transform.localPosition;
        enhanceGaugePos = enhanceGauge.transform.localPosition;
    }

    // 해상도에 따른 모든 UI 위치, 크기 변경
    void changeAllUIByResolution()
    {
        proportionWidth = baseWidth / Screen.width;
        proportionHeight = baseHeight / Screen.height;

        ChangeUIByResolution(hpbar, hpbarPos);
        ChangeUIByResolution(attribute, attributePos);
        ChangeUIByResolution(enhanceGauge, enhanceGaugePos);
    }

    // 해상도에 따른 UI 위치, 크기 변경
    void ChangeUIByResolution(GameObject changeUI, Vector3 basePos)
    {
        Vector3 tempSize = new Vector3();
        Vector3 tempPos = new Vector3();

        // 크기 비율 맞춤
        tempSize.x = 1 / proportionWidth;
        tempSize.y = 1 / proportionHeight;

        // 위치 비율 맞춤
        tempPos.x = basePos.x / proportionWidth;
        tempPos.y = basePos.y / proportionHeight;

        // 적용
        changeUI.transform.position = tempPos;
        changeUI.transform.localScale = tempSize;

    }

    // HP바 수치를 바꿔줌 (외부 호출)
    public void ChangeHpBar()
    {
        hpBarImage.curHpBar.fillAmount = PlayerCtrl.instance.ProportionHP;
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

    // 인핸스 수치 변경 (외부 호출)
    public void ChangeEnhance()
    {
        for(int i = 1; i <= SkillCtrl.instance.maxEnhance; i++)
        {
            var temp = enhanceGauge.transform.Find(i.ToString()).gameObject;

            if(i <= SkillCtrl.instance.curEnhance)
                temp.transform.Find("Full").gameObject.SetActive(true);
            else
                temp.transform.Find("Full").gameObject.SetActive(false);
        }
    }
}
