﻿using UnityEngine;
using System.Collections;

public class LauncherCtrl : MonoBehaviour {

    public float speed = 10f;
    public float durationTime = 1f;

    private GameObject target;
    private Transform traceTargetTr;

    public Material matNormal;
    public Material matRed;
    public Material matBlue;

    private AttributeState _curAttibute;
    public bool isPowerStrike = false;

    private int index = 0;

    void FixedUpdate()
    {
        //타겟 없을 시
        if (target == null)
        {
            transform.position = Vector3.Lerp(this.transform.position, traceTargetTr.position, Time.time);
        }
        //타겟 있을 시
        if (target != null)
        {
            //Vector3 relativePos = this.target.transform.position - this.transform.position;
            //transform.position = Vector3.Lerp(this.transform.position, target.transform.position, speed * Time.deltaTime);

            Vector3 center = (target.transform.position + this.transform.position) * 0.5f;
            center -= new Vector3(1, 1, 1);
            Vector3 fromRelCenter = this.transform.position - center;
            Vector3 toRelCenter = target.transform.position - center;
            transform.position = Vector3.Slerp(fromRelCenter, toRelCenter, speed * Time.deltaTime);
            transform.position += center;

            if (!target.activeSelf) {
                target = null;
                SkillCtrl.instance.StartReset(index);
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
            SkillCtrl.instance.StartReset(index);
        }
    }

    void OnDisable()
    {
        target = null;
        isPowerStrike = false;        
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
    public void GetTarget(GameObject _target, int _index)
    {
        this.target = _target;
        this.index = _index;

        StartCoroutine(Duration());
    }

    void GetTraceTarget(Transform targetTr)
    {
        traceTargetTr = targetTr;
    }
    // 탄환이 날아가면서 지속되는 시간
    IEnumerator Duration()
    {
        yield return new WaitForSeconds(durationTime);
        SkillCtrl.instance.StartReset(index);
        StopCoroutine(Duration());
    }
}
