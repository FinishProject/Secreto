using UnityEngine;
using System.Collections;

public class Kkokkali : FSMBase {

    private NavMeshAgent nvAgent;
    private Vector3 earlyPos;
    private Transform playerTr;

    public float traceDist = 10f;
    public float attackDist = 1f;
    public float attackSpeed = 5;
    public float damage = 16;
    


    // 몬스터 상태
    public enum EnemyStates
    {
        Dying = 0,      // 사망
        Chase = 1,      // 추적
        Attacking = 2,  // 공격
        Idle = 3,       // 대기

    }

    void Start()
    {
        earlyPos = transform.position;
        nvAgent = GetComponent<NavMeshAgent>();
        playerTr = PlayerCtrl.instance.transform;
    }

    #region 대기
    IEnumerator Idle_EnterState()
    {
        Debug.Log("Idle 상태 돌입");
        yield return null;
    }

    void Idle_Update()
    {
        nvAgent.destination = earlyPos;
        float distance = Vector3.Distance(playerTr.position, transform.position);
        if (distance <= attackDist) { curState = EnemyStates.Attacking; return; }
        if (distance <= traceDist) { curState = EnemyStates.Chase; return; }

    }
    #endregion

    #region 추적
    IEnumerator Chase_EnterState()
    {
        Debug.Log("Chase 상태 돌입");
        nvAgent.Resume();
        yield return null;
    }

    void Chase_Update()
    {
        nvAgent.destination = playerTr.position;

        float distance = Vector3.Distance(playerTr.position, transform.position);
        if (distance <= attackDist) { curState = EnemyStates.Attacking; return; }
        if (distance > traceDist) { curState = EnemyStates.Idle; return; }

    }
    #endregion

    #region 공격

    private float vx, vy;
    IEnumerator Attacking_EnterState()
    {
        Debug.Log("Attacking 상태 돌입");
        nvAgent.speed = 7f;
        yield return null;
    }

    void Attacking_Update()
    {
        nvAgent.destination = playerTr.position;

        float distance = Vector3.Distance(playerTr.position, transform.position);
        if (distance > attackDist) { curState = EnemyStates.Chase; return; }

    }
    
    IEnumerator Attacking_ExitState()
    {
        nvAgent.speed = 3.5f;
        yield return null;
    }

    void Attacking_OnTriggerEnter(Collider col)
    {
        if (col.tag.Equals("Player"))
            GiveDamage();
    }


    #endregion

    #region 사망
    IEnumerator Dying_EnterState()
    {
        Debug.Log("쮸금");
        GetComponent<ItemDrop>().DropItem();
        gameObject.SetActive(false);
        MonsterRespawnMgr.instance.Respawn(gameObject);
        yield return null;
    }

    void OnEnable()
    {
        curState = EnemyStates.Idle;
    }

    void OnDisable()
    {
        transform.position = earlyPos;
    }


    #endregion

    #region 기타 함수

    // 데미지 입었을때 ( 외부 호출 )
    public override void GetDamage(float damage)
    {
        base.GetDamage(damage);
        if (hp <= 0)
        {
            curState = EnemyStates.Dying;
        }
    }

    // 데미지 줄때
    public void GiveDamage()
    {
        PlayerCtrl.instance.getDamage(10);
    }
    #endregion
}
