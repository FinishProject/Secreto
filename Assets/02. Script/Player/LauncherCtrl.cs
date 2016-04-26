using UnityEngine;
using System.Collections;

public class LauncherCtrl : MonoBehaviour {

    public float speed = 15f;
    public float durationTime = 1f;
    private GameObject target;
    private Vector3 focusVec;

    public Transform targetTr;
    public Material matNormal;
    public Material matRed;
    public Material matBlue;

    private AttributeState _curAttibute;
    public bool isPowerStrike = false;

    int count = 0;

    void FixedUpdate()
    {
        //타겟 없을 시
        if (target == null) {
            transform.RotateAround(targetTr.position, Vector3.right, 200f * Time.deltaTime);
        }
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
        SkillCtrl.instance.StartReset(count);
        target = null;
        isPowerStrike = false;
        gameObject.transform.localScale = new Vector3(0.3256f, 0.3256f, 0.3256f);
        
    }

    void OnEnable()
    {
        StartCoroutine(Duration());
        _curAttibute = SkillCtrl.instance.curAttribute;
        switch (_curAttibute)
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

    void GetTarget(GameObject _target)
    {
        this.target = _target;
    }

    void GetIndex(int index)
    {
        count = index;
    }

    IEnumerator Duration()
    {
        yield return new WaitForSeconds(durationTime);
        this.gameObject.SetActive(false);
    }
}
