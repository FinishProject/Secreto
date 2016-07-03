using UnityEngine;
using System.Collections;

public class NpcMgr : MonoBehaviour {

    protected float distance = 0f; // 거리 차
    protected bool isSpeak = true;

    protected Transform playerTr;
    protected Animator anim;

    void Awake()
    {
        playerTr = GameObject.FindGameObjectWithTag("Player").transform;
    }
    // 자식 객체에서 초기화
    protected void Init()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        CurUpdate();
    }

    protected virtual void CurUpdate() { }

    // 플레이어와 거리 차이를 구함
    protected virtual float GetDistance(Vector3 npcPos)
    {
        return (playerTr.position - npcPos).sqrMagnitude;
    }

    protected virtual void SetScript(string name)
    {
        ScriptMgr.instance.GetScript(name);
        PlayerCtrl.instance.SetStopMove();
    }
}
