using UnityEngine;
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

    private int count = 0;

    public Transform shotTr;
    public GameObject bullet;
    Skill skill = new Skill();

    public GameObject[] goBullets = new GameObject[3];

    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            goBullets[i] = (GameObject)Instantiate(bullet, shotTr.position, Quaternion.identity);
            goBullets[i].SetActive(false);
        }
    }

    void Update()
    {
        //F키 입력 시 공격체 생성
        if (Input.GetKeyDown(KeyCode.F)){
            //배열의 길이보다 작고 0인덱스의 게임오브젝트가 꺼져있을 때
            if (count >= goBullets.Length && !goBullets[0].activeSelf) { count = 0; }
            goBullets[count].SetActive(true);
            goBullets[count].transform.position = shotTr.position;
            count++;
        }
        //if (Input.GetKeyDown(KeyCode.Tab)) { SkillTypeChagne(); }
        //SkillNum();
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
