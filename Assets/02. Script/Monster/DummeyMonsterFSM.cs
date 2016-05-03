using UnityEngine;
using System.Collections;

/****************************   정보   ****************************

    몬스터 더미

    사용방법 :

    FSMBase 참고,
    오브젝트에 추가 후 컴포넌트를 수정해 주자
    
******************************************************************/

public class DummeyMonsterFSM : FSMBase
{
    private NavMeshAgent nvAgent;
    private Transform playerTr;
    public float traceDist = 10f;
    public float attackDist = 7f;
    public float jumpHeight = 10;
    public float damage = 16;

    public Material matNormal;
    public Material matRed;
    public Material matBlue;

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
        nvAgent = GetComponent<NavMeshAgent>();
        playerTr = PlayerCtrl.instance.transform;

        switch (curAttibute)
        {
            case AttributeState.noraml:
                gameObject.GetComponent<MeshRenderer>().material = matNormal;
                break;
            case AttributeState.red:
                gameObject.GetComponent<MeshRenderer>().material = matRed;
                break;
            case AttributeState.blue:
                gameObject.GetComponent<MeshRenderer>().material = matBlue;
                break;
        }

        curState = EnemyStates.Idle;
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
        if (distance <= attackDist) { nvAgent.Stop(); curState = EnemyStates.Attacking; return; }
        if (distance  > traceDist ) { nvAgent.Stop(); curState = EnemyStates.Idle;     return; }

    }
    #endregion

    #region 공격

    IEnumerator Attacking_EnterState()
    {
        Debug.Log("Attacking 상태 돌입");
        StartCoroutine(JumpAttack());
        yield return null;
    }

    void Attacking_OnTriggerEnter(Collider col)
    {
        if (col.tag.Equals("Player"))
            GiveDamage();
    }

    IEnumerator JumpAttack()
    {
        nvAgent.enabled = false;
        float angle = 70f;
  
        var targetPos = playerTr.position;
        targetPos.y += PlayerCtrl.instance.transform.localScale.y;
        var dir = targetPos - transform.position;  // 방향 벡터
        var h = dir.y;                             // 높이 차이
        dir.y = 0;                                 // 수평방향만 유지
        var dist = dir.magnitude;                  // 수평 거리
        var a = angle * Mathf.Deg2Rad;             // 각도를 라디안으로 변환
        dir.y = dist * Mathf.Tan(a);               // 방향을 앙각으로 맞춤( 지평면과 포신 방향이 이루는 각)
        dist += h / Mathf.Tan(a);                  // 정확한 높이 차이를 위해             
        var vel = Mathf.Sqrt(dist * Physics.gravity.magnitude / Mathf.Sin(2 * a)); // 속도를 계산
        var ballistic = vel * dir.normalized;

        while (true)
        {
            ballistic += Physics.gravity * Time.deltaTime;
            transform.position += ballistic * Time.deltaTime;

            if (isCollisionByRaycast() && !isCollisionByRaycast("MONSTER"))
            {
                nvAgent.enabled = true;
                nvAgent.destination = playerTr.position;
                curState = EnemyStates.Chase;
                break;
            }
            yield return null;
        }
        
    }
    
    /*
    IEnumerator JumpAttack()
    {
        nvAgent.enabled = false;
        Vector3 moveDir = new Vector3();
        int xDir = 0;
        float distance = Vector3.Distance(transform.position, playerTr.position);
        float speed = 0;
        float deg = 45f;
        float gr = -7f;
        xDir = (transform.position.x - playerTr.position.x) > 1 ? -1 : 1;
        speed = Mathf.Sin((2 * deg)* Mathf.Deg2Rad) / gr * distance;
        speed = 1 / (speed * speed);
        moveDir.x = Mathf.Cos(deg * Mathf.Deg2Rad) * speed * xDir;
        moveDir.y = Mathf.Sin(deg * Mathf.Deg2Rad) * speed ;
        
        while (true)
        {
            moveDir += new Vector3(0f, gr, 0f) * Time.deltaTime;   // 중력
            controller.Move(moveDir * 10 * Time.deltaTime);

            if (controller.isGrounded)
            {
                nvAgent.enabled = true;
                break;
            }
            yield return null;
        }
        Debug.Log(controller.isGrounded ? "GROUNDED" : "NOT GROUNDED");
        nvAgent.enabled = true;
        nvAgent.destination = playerTr.position;
    }
    */
    #endregion

    #region 사망
    IEnumerator Dying_EnterState()
    {
        Debug.Log("쮸금");
        GetComponent<ItemDrop>().DropItem();
        gameObject.SetActive(false);

        yield return null;
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

    RaycastHit hit; // 레이
    // 충돌 됬는지 체크
    bool isCollisionByRaycast()
    {
        Debug.DrawRay(transform.position, Vector3.down * 0.5f, Color.red);
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.5f))
        {
            return true;
        }
        return false;
    }

    // 어떤 오브젝트랑 충돌 됬는지 체크
    bool isCollisionByRaycast(string obj)
    {
        Debug.DrawRay(transform.position, Vector3.down * 0.5f, Color.red);
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.5f))
        {
            if(hit.transform.tag.Equals(obj))
                return true;
        }
        return false;
    }

    //충돌 오브젝트 이름 리턴
    string GetNameCollisionByRaycast()
    {
        if (Physics.Raycast(transform.position, transform.up, out hit, 0.5f))
        {
            return hit.transform.tag;
        }
        return null;
    }
    #endregion
}
