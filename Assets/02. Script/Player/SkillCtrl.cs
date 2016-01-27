﻿using UnityEngine;
using System.Collections;

[System.Serializable]
public class Skill
{
    public int skillNum = 0;
    public int skillType = 0;
}

public class SkillCtrl : MonoBehaviour {

    enum SkillNumber
    {
        one = 0,
        two = 1,
        three = 2,
        four = 3,
    };

    public Transform shotTr;
    public GameObject bullet;
    Skill skill = new Skill();

    private GameObject goBullet;

    void Start()
    {
        goBullet = (GameObject)Instantiate(bullet, shotTr.position, Quaternion.identity);
        goBullet.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)){
            goBullet.SetActive(true);
            goBullet.transform.position = shotTr.position;
        }
        if (Input.GetKeyDown(KeyCode.Tab)) { SkillTypeChagne(); }
        SkillNum();
    }

    void SkillTypeChagne()
    {
        skill.skillType++;
        if(skill.skillType >= 4) { skill.skillType = 0; }
    }

    void SkillNum()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            skill.skillNum = (int)SkillNumber.one;
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2)){
            skill.skillNum = (int)SkillNumber.two;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            skill.skillNum = (int)SkillNumber.three;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            skill.skillNum = (int)SkillNumber.four;
        }
    }
}
