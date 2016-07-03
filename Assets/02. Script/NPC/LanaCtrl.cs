using UnityEngine;
using System.Collections;

public class LanaCtrl : NpcMgr
{

    public float recognRange = 80f; // 인식 범위

    private bool isAppear = true;
//    private bool isSpeak = false;

    public Transform movePoint;
    public Transform CamPos;
    public Transform FocusPos;

    void Start()
    {
        Init();
    }

    // Update
    protected override void CurUpdate()
    {
        distance = GetDistance(this.transform.position);
        // 등장
        if (distance <= 80f && isAppear)
        {
            AppearNpc();
        }
        // 대화 시작
        if (distance <= 30f && !isAppear)
        {
            InGameUI.instance.CinematicView(true);
            Camera.main.GetComponent<CameraCtrl_4>().SetCinematicView(true, CamPos.position, FocusPos.position);
            anim.SetBool("Speak", true);
            Speak();
        }
        // 대화 종료
        else
        {
            InGameUI.instance.CinematicView(false);
            Camera.main.GetComponent<CameraCtrl_4>().SetCinematicView(false, Vector3.zero, Vector3.zero);
            anim.SetBool("Speak", false);
        }

    }

    void Speak()
    {
        // 대화한 적이 없다면
        SetScript(this.name);
        if (!ScriptMgr.instance.SpeakName(this.name))
        {
            SetScript(this.name);
        }
        // 대화한 적이 있고, 퀘스트 완료 시
        else if (ScriptMgr.instance.SpeakName(this.name) && ScriptMgr.instance.isQuest)
        {
            if (!isSpeak)
            {
                isSpeak = true;
                SetScript(this.name);
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
            anim.SetBool("Appear", false);
            isAppear = false;
        }
    }
}
