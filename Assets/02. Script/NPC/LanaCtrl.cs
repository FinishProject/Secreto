using UnityEngine;
using System.Collections;

public class LanaCtrl : NpcMgr {

    public float recognRange = 80f; // 인식 범위

    private bool isAppear = true;
    private bool isSpeak = false;

    public Transform movePoint;

    void Start()
    {
        Init();
    }

    // Update
    protected override void CurUpdate()
    {
        distance = GetDistance(this.transform.position);

        if(distance <= 80f && isAppear)
        {
            AppearNpc();
        }

        if(distance <= 60f)
        {
            // 대화한 적이 없다면
            if (!ScriptMgr.instance.SpeakName(this.name))
            {
                SetScript(this.name);
            }
            // 대화한 적이 있고, 퀘스트 완료 시
            else if(ScriptMgr.instance.SpeakName(this.name) && ScriptMgr.instance.isQuest) {
                if (!isSpeak)
                {
                    isSpeak = true;
                    SetScript(this.name);
                }
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

    void OnTriggerEnter(Collider col)
    {
        if (col.name.Equals(movePoint.name))
        {
            isAppear = false;
        }
    }
}
