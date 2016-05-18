﻿using UnityEngine;
using System.Collections;

public class Slug : FSMBase
{
    private NavMeshAgent nvAgent;
    private Vector3 earlyPos;
    private Transform playerTr;

    public Transform shootTr;
    private bool isPlayerLiftSide;          // 플레이어가 좌측에 있는지
    private bool isLookLiftSide;            // 좌측 방향을 바라 보는지
    private float discoverDist = 10f;       // 발견 범위
    private float traceDist = 8f;           // 추적 범위
    private float attackDist = 6f;          // 공격 범위
    private float attackSpeed = 2f;         // 공격 속도
    public float damage = 16;               // 공격력
    public float attackErrorRange = 1.5f;   // 공격 오차 범위
    private System.Enum oldStates;          // 기존 스테이트 저장용

    // 몬스터 상태
    public enum EnemyStates
    {
        Dying = 0,      // 사망
        Attacked = 2,   // 피격
        Discover = 3,   // 발견
        Chase = 4,      // 추적
        ComBback = 5,   // 복귀
        Attacking = 6,  // 공격
        Idle = 7,       // 대기
    }

    void Start()
    {
        earlyPos = transform.position;
        nvAgent = GetComponent<NavMeshAgent>();
        playerTr = PlayerCtrl.instance.transform;
    }

    //*******************************************************************************

    #region 대기
    IEnumerator Idle_EnterState()
    {
        Debug.Log("Idle 상태 돌입");
        yield return null;
    }

    void Idle_Update()
    {
        float distance = Vector3.Distance(playerTr.position, transform.position);
        if (distance <= discoverDist)
        {
            anim.SetBool("Prepare", true);
            curState = EnemyStates.Discover;
        }
        else
            anim.SetBool("Prepare", false);
    }
    #endregion

    //*******************************************************************************

    #region 발견
    IEnumerator Discover_EnterState()
    {
        Debug.Log("Discover 상태 돌입");
        yield return null;
    }

    void Discover_Update()
    {
        float distance = Vector3.Distance(playerTr.position, transform.position);
        if (distance <= traceDist)
        {
            anim.SetBool("Run", true);
            anim.SetBool("Prepare", false);
            curState = EnemyStates.Chase;
            return;
        }
        else if (distance > discoverDist)
        {
            anim.SetBool("Prepare", false);
            curState = EnemyStates.Idle;
        }

    }
    #endregion

    //*******************************************************************************

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

        if (distance <= attackDist && isAttackAngle())
        {
            anim.SetBool("Attack", true);
            anim.SetBool("Run", false);
            curState = EnemyStates.Attacking;
            return;
        }

        if (distance > traceDist)
        {
            curState = EnemyStates.ComBback;
            return;
        }

    }

    // 공격 가능한 회전 각인지 체크하는 함수 ( 회전을 마치고 공격을 하기위해 )
    bool isAttackAngle()
    {
        if (Mathf.Abs(transform.localEulerAngles.y - 270f) < 5 && isPlayerLiftSide ||
            Mathf.Abs(transform.localEulerAngles.y - 90f) < 5 && !isPlayerLiftSide)
            return true;
        return false;
    }
    #endregion

    //*******************************************************************************

    #region 복귀
    IEnumerator ComBback_EnterState()
    {
        Debug.Log("ComBback 상태 돌입");
        yield return null;
    }

    void ComBback_Update()
    {
        nvAgent.destination = earlyPos;

        float distance = Vector3.Distance(playerTr.position, transform.position);

        if (distance <= traceDist)
        {
            curState = EnemyStates.Chase;
            return;
        }

        if (Vector3.Distance(earlyPos, transform.position) < 0.5f)
        {
            anim.SetBool("Prepare", true);
            anim.SetBool("Run", false);
            curState = EnemyStates.Idle;
            nvAgent.Stop();
            return;
        }

    }
    #endregion

    //*******************************************************************************

    #region 공격

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
        if (distance > attackDist)
        {
            anim.SetBool("Run", true);
            anim.SetBool("Attack", false);
            curState = EnemyStates.Chase;
            return;
        }
    }

    IEnumerator ConsecutiveShoot()
    {
        while (true)
        {
            anim.SetBool("Attack", true);
            yield return new WaitForSeconds(attackSpeed);
        }

    }

    IEnumerator Attacking_ExitState()
    {
        StopAllCoroutines();
        nvAgent.Resume();
        yield return null;
    }

    // 발사를 위한 함수 ( 애니메이션 끝난 후 이벤트로 호출할 함수)
    void ShotBullet()
    {
        ObjectMgr.instance.GetParabolaBullet().UseItem().
                GetComponent<BulletObject_Parabola>().Moving(shootTr.position, attackErrorRange);
    }

    #endregion

    //*******************************************************************************

    #region 피격

    IEnumerator Attacked_EnterState()
    {
        Debug.Log("Attacked 상태 돌입");

        nvAgent.Stop();
        anim.SetTrigger("Attacked");
        yield return new WaitForSeconds(0.5f);

        if (curHp <= 0)
        {
            anim.SetBool("Run", false);
            anim.SetBool("Attack", false);
            curState = EnemyStates.Dying;
        }
        else
        {
            curState = oldStates;
        }

        yield return null;
    }

    #endregion

    //*******************************************************************************

    #region 사망
    IEnumerator Dying_EnterState()
    {
        WahleCtrl.instance.isDie = true;
        WahleCtrl.state = WahleState.IDLE;
        
        Debug.Log("쮸금");
        anim.SetBool("Death", true);
        GetComponent<ItemDrop>().DropItem();

        //Destroy(this.gameObject, 3f);
        yield return new WaitForSeconds(3f);

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
        curHp = oldHp;
        transform.position = earlyPos;
    }


    #endregion

    //*******************************************************************************

    #region 기타 함수

    // 데미지 입었을때 ( 외부 호출 )
    public override void GetDamage(float damage)
    {
        base.GetDamage(damage);

        Debug.Log("피격");
        if (!curState.Equals(EnemyStates.Dying) && !curState.Equals(EnemyStates.Attacked))
        {
            oldStates = curState;
            curState = EnemyStates.Attacked;
        }


    }

    // 업데이트 함수 ( 모든 상태일때 적용 할 )
    public override void ChildUpdate()
    {
        isPlayerLiftSide = Mathf.Sign(playerTr.position.x - transform.position.x) == -1 ? true : false;
    }

    void StartRun()
    {
        if (curState.Equals(EnemyStates.Attacked))
            nvAgent.Resume();
    }

    // 데미지 줄때
    public void GiveDamage()
    {
        PlayerCtrl.instance.getDamage(10);
    }

    #endregion
}