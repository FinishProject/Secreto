using UnityEngine;
using System.Collections;

public class LauncherCtrl : MonoBehaviour {

    private float speed = 15f;
    private GameObject target;
    private Vector3 focusVec;

    public Material matNormal;
    public Material matRed;
    public Material matBlue;

    private AttributeState _curAttibute;
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

            if (!target.activeSelf)
            {
                target = null;
                this.gameObject.SetActive(false);
            }
        }
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("MONSTER"))
        {
            var monster = coll.GetComponent<MonsterFSM>();

            switch(monster.curAttibute)
            {
                // 속성 상관 없이 데미지
                case AttributeState.noraml:
                                        
                    if (isPowerStrike) monster.getDamage(50);
                    else monster.getDamage(15);
                    break;

                // 몬스터 빨강 / 발사체 파랑일 때 데미지
                case AttributeState.red:

                    if(_curAttibute.Equals(AttributeState.blue))
                    {
                        if (isPowerStrike) monster.getDamage(50);
                        else monster.getDamage(15);
                    }
                    break;

                // 몬스터 파랑 / 발사체 빨강일 때 데미지
                case AttributeState.blue:

                    if (_curAttibute.Equals(AttributeState.red))
                    {
                        if (isPowerStrike) monster.getDamage(50);
                        else monster.getDamage(15);
                    }
                    break;
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
        StartCoroutine(TimeDuration());
        _curAttibute = SkillCtrl.instance.curAttribute;
        switch(_curAttibute)
        {
            case AttributeState.noraml:
                gameObject.GetComponent<MeshRenderer>().material = matNormal;
                break;
            case AttributeState.red:
                gameObject.GetComponent<MeshRenderer>().material = matRed;
                break;
            case AttributeState.blue:
                gameObject.GetComponent<MeshRenderer>().material = matBlue;
                break;
        }

        if (isPowerStrike)
            gameObject.transform.localScale = new Vector3(1,1,1);
    }

    public void GetFocusVector(Vector3 _focusVec)
    {
        focusVec = _focusVec;
    }

    void GetTarget(GameObject _targetTr)
    {
        this.target = _targetTr;
    }

    IEnumerator TimeDuration()
    {
        yield return new WaitForSeconds(1f);
        this.gameObject.SetActive(false);
    }
}
