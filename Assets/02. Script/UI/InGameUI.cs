using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour {
    [System.Serializable]
    public struct AttributeImage
    {
        public Image backGroundAttribute;
        public Image redAttribute;
        public Image blueAttribute;
    }
    public Image curHpBar;
    public AttributeImage attributeImage;
    public GameObject enhanceGauge;
    public static InGameUI instance;
    private int WindowWidth;
    private int windowHeight;
    void Awake()
    {
//        Screen.width;
        instance = this;
        attributeImage.redAttribute.gameObject.SetActive(false);
        attributeImage.blueAttribute.gameObject.SetActive(false);
    }

    public void ChangeHpBar()
    {
        curHpBar.fillAmount = PlayerCtrl.instance.ProportionHP;
    }

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

    public void ChangeCountDownForAttributeBar()
    {
        StopAllCoroutines();
        StartCoroutine( ChangeAttributeBar());
    }

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
