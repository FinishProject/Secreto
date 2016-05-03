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
    private float attackSpeed = 1;
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
        nvAgent.destination = earlyPos;
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
        anim.SetBool("Move", true);
        nvAgent.destination = playerTr.position;
        float distance = Vector3.Distance(playerTr.position, transform.position);
        if (distance <= attackDist) { curState = EnemyStates.Attacking; return; }
        if (distance > traceDist)   { curState = EnemyStates.Idle; return; }

    }

    IEnumerator Chase_ExitState()
    {
        anim.SetBool("Move", false);
        yield return null;
    }

    //*******************************************************************************
    #endregion

    #region 공격
    //*******************************************************************************

    IEnumerator Attacking_EnterState()
    {
        Debug.Log("Attacking 상태 돌입");
        nvAgent.Stop();
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
            anim.SetBool("Attack", true);
            yield return new WaitForSeconds(attackSpeed);
            ObjectMgr.instance.GetParabolaBullet().UseItem().
                GetComponent<BulletObject_Parabola>().Moving(shootTr.position);

            anim.SetBool("Attack", false);
            yield return new WaitForSeconds(attackSpeed);
        }
        
    }
    //*******************************************************************************
    #endregion

    #region 사망
    //*******************************************************************************

    IEnumerator Dying_EnterState()
    {
        Debug.Log("쮸금");
        anim.SetBool("Death", true);
        yield return new WaitForSeconds(2.0f);

        GetComponent<ItemDrop>().DropItem();
        gameObject.SetActive(false);
        MonsterRespawnMgr.instance.Respawn(gameObject);
        yield return null;
    }

    void OnEnable()
    {
        hp = 30;
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
        Debug.Log("피격");
        anim.SetTrigger("Prceive");

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
