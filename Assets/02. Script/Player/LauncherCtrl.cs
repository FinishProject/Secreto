using UnityEngine;
using System.Collections;

public class LauncherCtrl : MonoBehaviour {

    public float speed = 15f;
    public float durationTime = 1f;
    private GameObject target;
    private Vector3 focusVec;
    public Transform olaTr;

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
        if (target == null)
        {
            transform.position = Vector3.Lerp(this.transform.position, olaTr.position, 20f * Time.deltaTime);
        }
        //타겟 있을 시
        if (target != null)
        {
            Vector3 relativePos = this.target.transform.position - this.transform.position;
            transform.position = Vector3.Lerp(this.transform.position, target.transform.position, speed * Time.deltaTime);
            //transform.Translate(new Vector3(0f, relativePos.y * speed * Time.deltaTime, relativePos.y * speed * Time.deltaTime));
            //transform.LookAt(new Vector3(target.transform.position.x, target.transform.position.y, 0f));

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
        //_curAttibute = SkillCtrl.instance.curAttribute;
        //switch (_curAttibute)
        //{
        //    case AttributeState.noraml:
        //        gameObject.GetComponent<MeshRenderer>().material = matNormal;
        //        break;
        //    case AttributeState.red:
        //        gameObject.GetComponent<MeshRenderer>().material = matRed;
        //        break;
        //    case AttributeState.blue:
        //        gameObject.GetComponent<MeshRenderer>().material = matBlue;
        //        break;
        //}
        
        if (isPowerStrike)
            gameObject.transform.localScale = new Vector3(1,1,1);
    }
    public void GetTarget(GameObject _target, int index)
    {
        this.target = _target;
        this.count = index;
        StartCoroutine(Duration());
    }

    void GetIndex(int index)
    {
        count = index;
    }

    IEnumerator Duration()
    {
        yield return new WaitForSeconds(durationTime);
        SkillCtrl.instance.StartReset(count);
        this.gameObject.SetActive(false);
    }
}
