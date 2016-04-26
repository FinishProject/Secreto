using UnityEngine;
using System.Collections;

public class LauncherCtrl : MonoBehaviour {

    private float speed = 15f;
    private float m_Time = 0f;
    private GameObject target;
    private Vector3 focusVec;

    public Material red;
    public Material blue;
    private bool isRed = true;
    public bool isPowerStrike = false;

    void FixedUpdate()
    {
        //타겟 없을 시
        if (target == null) { transform.Translate(focusVec * speed * Time.deltaTime); }
        //타겟 있을 시
        else
        {
            Vector3 relativePos = this.target.transform.position - this.transform.position;
            transform.Translate(relativePos.normalized * speed * Time.deltaTime);
            // 타겟이 사라졌을 시 탄환 사라짐
            if(!target.activeSelf) {
                target = null;
                gameObject.SetActive(false);
            }
        }
    }
    void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("MONSTER"))
        {
            var monster = coll.GetComponent<MonsterFSM>();
            if (monster.isRed == !isRed)
            {
                if (isPowerStrike) monster.getDamage(50);
                else monster.getDamage(15);
            }
            else if (!monster.isRed == isRed)
            {
                if (isPowerStrike) monster.getDamage(50);
                else monster.getDamage(15);
            }

            this.target = null;
            gameObject.SetActive(false);
        }
        
    }
    void OnDisable()
    {
        target = null;
        isPowerStrike = false;
        gameObject.transform.localScale = new Vector3(0.3256f, 0.3256f, 0.3256f);
    }

    void OnEnable()
    {
        StartCoroutine(HoldingTime());
        if (PlayerCtrl.instance.isRed)
        {
            gameObject.GetComponent<MeshRenderer>().material = red;
            isRed = true;
        }
        else
        {
            gameObject.GetComponent<MeshRenderer>().material = blue;
            isRed = false;
        }

        if (isPowerStrike)
            gameObject.transform.localScale = new Vector3(1, 1, 1);
    }

    public void GetFocusVector(Vector3 _focusVec)
    {
        focusVec = _focusVec;
    }

    void GetTarget(GameObject _target)
    {
        this.target = _target;
    }
    //유지시간
    IEnumerator HoldingTime()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
