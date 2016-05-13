using UnityEngine;
using System.Collections;

public class Slug : FSMBase
{
    private NavMeshAgent nvAgent;
    private Vector3 earlyPos;
    private Transform playerTr;

    public Transform shootTr;
    public float discoverDist = 12f;
    public float traceDist = 10f;
    public float attackDist = 2f;
    private float attackSpeed = 1;
    public float damage = 16;
    public float attackErrorRange = 1.5f;
    private System.Enum oldStates;

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

        if (distance <= attackDist)
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
    #endregion

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
            return;
        }

    }
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

    IEnumerator ConsecutiveShoot()
    {
        while (true)
        {
            anim.SetBool("Attack", true);
            yield return new WaitForSeconds(attackSpeed);
            //ShotBullet();

            anim.SetBool("Attack", false);
            yield return new WaitForSeconds(attackSpeed);
        }

    }

    void ShotBullet()
    {
        ObjectMgr.instance.GetParabolaBullet().UseItem().
                GetComponent<BulletObject_Parabola>().Moving(shootTr.position, attackErrorRange);
    }

    IEnumerator Attacking_ExitState()
    {
        StopAllCoroutines();
        anim.SetBool("Attack", false);
        nvAgent.Resume();
        yield return null;
    }

    //*******************************************************************************
    #endregion

    #region 피격
    //*******************************************************************************

    IEnumerator Attacked_EnterState()
    {
        Debug.Log("Attacked 상태 돌입");

        nvAgent.Stop();
        anim.SetTrigger("Attacked");
        yield return new WaitForSeconds(0.5f);
        if (curHp <= 0)
        {
            anim.SetBool("Attacked", false);
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

    //*******************************************************************************
    #endregion

    #region 사망
    IEnumerator Dying_EnterState()
    {
        Debug.Log("쮸금");
        anim.SetBool("Death", true);
        GetComponent<ItemDrop>().DropItem();

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

    #region 기타 함수
    //*******************************************************************************

    // 데미지 입었을때 ( 외부 호출 )
    public override void GetDamage(float damage)
    {
        base.GetDamage(damage);
        
        Debug.Log("피격");
        if(!curState.Equals(EnemyStates.Dying) && !curState.Equals(EnemyStates.Attacked))
        {
            oldStates = curState;
            curState = EnemyStates.Attacked;
        }
        
        
    }

    void StartRun()
    {
        if(curState.Equals(EnemyStates.Attacked))
        nvAgent.Resume();
    }

    // 데미지 줄때
    public void GiveDamage()
    {
        PlayerCtrl.instance.getDamage(10);
    }

    //*******************************************************************************
    #endregion
}


/*


public class Slug : FSMBase
{
    private NavMeshAgent nvAgent;
    private Vector3 earlyPos;
    private Transform playerTr;

    public Transform shootTr;
    public float traceDist = 10f;
    public float attackDist = 2f;
    private float attackSpeed = 1;
    public float damage = 16;
    public float attackErrorRange = 1.5f;
    private System.Enum oldStates;

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

    #region 대기
    //*******************************************************************************

    IEnumerator Idle_EnterState()
    {
        Debug.Log("Idle 상태 돌입");
        yield return null;
    }

    void Idle_Update()
    {
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
        anim.SetBool("Run", true);
        nvAgent.destination = playerTr.position;
        float distance = Vector3.Distance(playerTr.position, transform.position);
        if (distance <= attackDist) { curState = EnemyStates.Attacking; return; }
        if (distance > traceDist)   { curState = EnemyStates.ComBback; return; }

    }

    IEnumerator Chase_ExitState()
    {
        anim.SetBool("Run", false);
        yield return null;
    }

    //*******************************************************************************
    #endregion

    #region 복귀
    //*******************************************************************************
    IEnumerator ComBback_EnterState()
    {
        Debug.Log("ComBback 상태 돌입");
        yield return null;
    }

    void ComBback_Update()
    {
        anim.SetBool("Run", true);
        nvAgent.destination = earlyPos;

        float distance = Vector3.Distance(playerTr.position, transform.position);

        if (distance <= traceDist)
        {
            curState = EnemyStates.Chase;
            return;
        }

        if (Vector3.Distance(earlyPos, transform.position) < 0.5f)
        {
            Debug.Log(1111);
            anim.SetBool("Run", false);
            curState = EnemyStates.Idle;
            return;
        }

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

    IEnumerator ConsecutiveShoot()
    {
        while (true)
        {
            anim.SetBool("Attack", true);
            yield return new WaitForSeconds(attackSpeed);
            //ShotBullet();

            anim.SetBool("Attack", false);
            yield return new WaitForSeconds(attackSpeed);
        }

    }

    void ShotBullet()
    {
        ObjectMgr.instance.GetParabolaBullet().UseItem().
                GetComponent<BulletObject_Parabola>().Moving(shootTr.position, attackErrorRange);
    }

    IEnumerator Attacking_ExitState()
    {
        StopAllCoroutines();
        anim.SetBool("Attack", false);
        nvAgent.Resume();
        yield return null;
    }

    //*******************************************************************************
    #endregion

    #region 피격
    //*******************************************************************************

    IEnumerator Attacked_EnterState()
    {
        Debug.Log("Attacked 상태 돌입");

        nvAgent.Stop();
        anim.SetTrigger("Prceive");

        if (hp <= 0)
        {
            curState = EnemyStates.Dying;
        }
        else
        {
            curState = oldStates;
        }
            
        yield return null;
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
        if(!curState.Equals(EnemyStates.Dying))
        {
            oldStates = curState;
            curState = EnemyStates.Attacked;
        }
        
        
    }

    void StartRun()
    {
        if(curState.Equals(EnemyStates.Attacked))
        nvAgent.Resume();
    }

    // 데미지 줄때
    public void GiveDamage()
    {
        PlayerCtrl.instance.getDamage(10);
    }

    //*******************************************************************************
    #endregion
}




*/
