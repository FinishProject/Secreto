using UnityEngine;
using System.Collections;

public class Slug : FSMBase
{
    private NavMeshAgent nvAgent;
    private Vector3 earlyPos;
    private Transform playerTr;

    public Transform shootTr;
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
    //*******************************************************************************

    IEnumerator Idle_EnterState()
    {
        Debug.Log("Idle 상태 돌입");
        yield return null;
    }

    void Idle_Update()
    {
        transform.position = Vector3.Lerp(transform.position, earlyPos, Time.deltaTime);
        float distance = Vector3.Distance(playerTr.position, transform.position);
        if (distance <= attackDist) { curState = EnemyStates.Attacking; return; }
        if (distance <= traceDist) { curState = EnemyStates.Chase; return; }

    }

    //*******************************************************************************
    #endregion

    #region 추적
    //*******************************************************************************

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
        if (distance <= attackDist) { nvAgent.Stop(); curState = EnemyStates.Attacking; return; }
        if (distance > traceDist)   { nvAgent.Stop(); curState = EnemyStates.Idle; return; }

    }

    //*******************************************************************************
    #endregion

    #region 공격
    //*******************************************************************************

    IEnumerator Attacking_EnterState()
    {
        Debug.Log("Attacking 상태 돌입");
        StartCoroutine(ConsecutiveShoot());
        yield return null;
    }

    void Attacking_Update()
    {
        
        float distance = Vector3.Distance(playerTr.position, transform.position);
        if (distance > attackDist) { curState = EnemyStates.Chase; return; }

    }

    IEnumerator Attacking_ExitState()
    {
        StopAllCoroutines();
        yield return null;
    }

    IEnumerator ConsecutiveShoot()
    {
        while(true)
        {
            ObjectMgr.instance.GetParabolaBullet().UseItem().
                GetComponent<BulletObject_Parabola>().Moving(shootTr.position);

            yield return new WaitForSeconds(0.5f);
        }
        
    }
    //*******************************************************************************
    #endregion

    #region 사망
    //*******************************************************************************

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

    //*******************************************************************************

    #endregion

    #region 기타 함수
    //*******************************************************************************

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

    //*******************************************************************************
    #endregion
}
