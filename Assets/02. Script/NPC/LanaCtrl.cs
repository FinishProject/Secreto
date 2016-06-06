using UnityEngine;
using System.Collections;

public class LanaCtrl : NpcMgr {

    public float recognRange = 36f; // 인식 범위

    public Transform movePoint;

    void Start()
    {
        Init();
    }

    // Upate
    protected override void CurUpdate()
    {
        distance = GetDistance(this.transform.position);

        if(distance <= 80f)
        {
            AppearNpc();
        }

        if(distance <= 60f)
        {
            if (!ScriptMgr.instance.SpeakName(this.name))
            {
                SetScript(this.name);
            }
            else if(ScriptMgr.instance.SpeakName(this.name) && ScriptMgr.instance.isQuest) {
                Debug.Log("!!");
            }
        }
        
    }



    // 포물선을 그리며 플레이어 앞으로 등장 함수
    void AppearNpc()
    {
        anim.SetBool("Appear", true);

        Vector3 center = (movePoint.position + this.transform.position) * 0.5f;
        center -= new Vector3(0, 1, 1);
        Vector3 fromRelCenter = this.transform.position - center;
        Vector3 toRelCenter = movePoint.position - center;
        transform.position = Vector3.Slerp(fromRelCenter, toRelCenter, 3f * Time.deltaTime);
        transform.position += center;

        Vector3 lookTarget = new Vector3(0f, playerTr.position.y, playerTr.position.z);
    }
}
