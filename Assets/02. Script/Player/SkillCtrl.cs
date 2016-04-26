using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum AttributeState
{
    noraml = 0,
    red = 1,
    blue = 2
}

public class SkillCtrl : MonoBehaviour {

    struct BulletInfo {
        public GameObject Bullet;
        public Vector3 originPos;
        public bool isFire;
    }

    public Transform[] rotateTr;

    private BulletInfo[] bulletInfo;

    public Transform playerTr;
    public Transform shotTr;
    public GameObject normalBullet;
    
    private int count = 0;
    public GameObject[] objBullets;

    [System.NonSerialized]
    public float curEnhance = 0;
    public float maxEnhance = 10;
    [System.NonSerialized]
    public AttributeState curAttribute;         // 속성   
    public float attributeDuration = 5.0f;      // 속성 지속시간 (시간이 끝나면 noraml)
    public static SkillCtrl instance;
    private float _countDownForAttribute;

    public int bulletNum = 3;
    public float initTime = 3f;

    void Awake()
    {
        instance = this;
        curAttribute = AttributeState.noraml;
        _countDownForAttribute = attributeDuration;
    }

    void Start()
    {
        bulletInfo = new BulletInfo[objBullets.Length];

        for (int i=0; i< bulletInfo.Length; i++)
        {
            bulletInfo[i].Bullet = objBullets[i];
            bulletInfo[i].originPos = objBullets[i].transform.position;
            bulletInfo[i].isFire = true;
            //bulletInfo[i].Bullet.SetActive(false);
        }
    }

    void Update()
    {
        for (int i=0; i<bulletInfo.Length; i++)
        {
            
                //bulletInfo[i].Bullet.transform.position = Vector3.Lerp(
                //    bulletInfo[i].Bullet.transform.position, shotTr.position, 10f * Time.deltaTime);
                rotateTr[i].transform.RotateAround(transform.position, Vector3.right, 200f * Time.deltaTime);
  
        }

        // F키 입력 시 공격체 생성
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.A)) {
            if (count >= bulletInfo.Length) {
                 count = 0;
            }
            else {
                if (bulletInfo[count].isFire)
                {
                    bulletInfo[count].Bullet.SetActive(true);
                    bulletInfo[count].Bullet.transform.position = shotTr.position;
                    FindTarget();
                    count++;
                }
            }
        }

        // G키 입력, 인핸스가 있으면 공격체 생성
        if (Input.GetKeyDown(KeyCode.G) && curEnhance >= maxEnhance)
        {
            if (count >= objBullets.Length && !objBullets[0].activeSelf) { count = 0; }
            curEnhance = 0;
            objBullets[count].GetComponent<LauncherCtrl>().isPowerStrike = true;
            objBullets[count].SetActive(true);
            objBullets[count].transform.position = shotTr.position;
            FindTarget();
            count++;
        }
    }

    //타겟 탐색
    void FindTarget()
    {
        Collider[] hitCollider = Physics.OverlapSphere(playerTr.position, 8f);
        List<Transform> targetList = new List<Transform>();
        
        for(int i = 0; i < hitCollider.Length; i++) { 
            if (hitCollider[i].CompareTag("MONSTER")) {
                targetList.Add(hitCollider[i].transform);
            }
        }
        if (targetList.Count > 0)
        {
            
            DistanceCompare(targetList);
        }
    }
    // 가장 가까운 타겟의 위치를 찾아 타겟의 위치값을 넘겨줌
    void DistanceCompare(List<Transform> _target)
    {
        float nearDistance = (playerTr.position - _target[0].position).sqrMagnitude;
        int targetIndex = 0;
        // 각 타겟의 거리를 비교하여 가장 가까운 타겟을 찾음
        for (int i = 1; i < _target.Count; i++)
        {
            float curDistance = (playerTr.position - _target[i].position).sqrMagnitude;
            if (nearDistance > curDistance)
            {
                nearDistance = curDistance;
                targetIndex = i;
            }
        }
        // 현재 발사체에게 타겟 포지션을 알려줌
        LauncherCtrl launcher = bulletInfo[count].Bullet.GetComponent<LauncherCtrl>();
        launcher.GetTarget(_target[targetIndex].gameObject, count);
        bulletInfo[count].isFire = false;
    }

    public void ChangeAttribute(AttributeState attribute)
    {
        StartCoroutine(StartChangeAttribute(attribute));
    }

    // 속성 지속
    IEnumerator StartChangeAttribute(AttributeState attribute)
    {
        curAttribute = attribute;
        while(true)
        {
            yield return new WaitForSeconds(1f);
            _countDownForAttribute -= 1f;

            if(_countDownForAttribute < 1f)
            {
                _countDownForAttribute = attributeDuration;
                curAttribute = AttributeState.noraml;
                yield return null;
            }
        }
        
    }

    // 인핸스 증가 ( 마나 )
    public void AddEnhance()
    {
        curEnhance+= 1;
        if(curEnhance > maxEnhance)
        {
            curEnhance = maxEnhance;
        }
    }

    void OnGUI()
    {
        string tempText;
        tempText = "현재 인핸스 : " + curEnhance.ToString();
        tempText += "\n적용 속성  : " + curAttribute.ToString();


        if(!curAttribute.Equals(AttributeState.noraml))
        {
            tempText += "\n남은 시간 : " + Mathf.Round(_countDownForAttribute).ToString();
        }

        GUI.TextField(new Rect(0, 0, 300.0f, 60.0f), tempText);
    }

    public void StartReset(int index)
    {
        StartCoroutine(ResetBullet(index));
    }

    IEnumerator ResetBullet(int index)
    {
        yield return new WaitForSeconds(initTime);
        bulletInfo[index].Bullet.SetActive(true);
        bulletInfo[index].isFire = true;
        bulletInfo[index].Bullet.transform.position = rotateTr[index].position;
    }
}
