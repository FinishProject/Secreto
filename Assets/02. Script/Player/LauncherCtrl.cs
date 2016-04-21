using UnityEngine;
using System.Collections;

public class LauncherCtrl : MonoBehaviour {

    private float speed = 8f;
    private float m_Time = 0f;
    private Transform targetTr;
    private Vector3 focusVec;

    public Material red;
    public Material blue;
    private bool isRed = true;
    public bool isPowerStrike = false;

    void FixedUpdate()
    {
        //타겟 없을 시
        if (targetTr == null) { transform.Translate(focusVec * speed * Time.deltaTime); }
        //타겟 있을 시
        else
        {
            Vector3 relativePos = this.targetTr.position - this.transform.position;
            //transform.position = Vector3.Lerp(transform.position, relativePos, speed * Time.deltaTime);
            transform.Translate(relativePos.normalized * speed * Time.deltaTime);
        }
        m_Time += Time.deltaTime;
        if (m_Time >= 1f) { gameObject.SetActive(false); }
    }
    void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "MONSTER")
        {
            var monster = coll.GetComponent<MonsterFSM>();
            if (monster.isRed == !isRed)
            {
                if(isPowerStrike) monster.getDamage(50);
                else monster.getDamage(15);
            }
            else if(!monster.isRed == isRed)
            {
                if (isPowerStrike) monster.getDamage(50);
                else monster.getDamage(15);
            }
                

            this.targetTr = null;
            gameObject.SetActive(false);

        }
    }
    void OnDisable()
    {
        targetTr = null;
        isPowerStrike = false;
        gameObject.transform.localScale = new Vector3(0.3256f, 0.3256f, 0.3256f);
    }

    void OnEnable()
    {
        m_Time = 0f;
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
            gameObject.transform.localScale = new Vector3(1,1,1);
    }

    public void GetFocusVector(Vector3 _focusVec)
    {
        focusVec = _focusVec;
    }

    void GetTarget(Transform _targetTr)
    {
        this.targetTr = _targetTr;
    }
}
