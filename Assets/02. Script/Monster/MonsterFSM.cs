using UnityEngine;
using System.Collections;

/****************************   정보   ****************************

    ( 임시 ) 몬스터 FSM

    사용방법 :

    부모 클래스 참고
      
******************************************************************/

public class MonsterFSM : FSMBase {

    private float hp = 50;

    public AttributeState curAttibute;
    public Material matNormal;
    public Material matRed;
    public Material matBlue;

    // 몬스터 상태
    public enum EnemyStates
    {
        Idle = 0,       // 대기
        Chase = 1,      // 추적
        Attacking = 2,  // 공격
        Dying = 3,      // 사망
    }

	void Start () {
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
        if (Input.GetKeyDown(KeyCode.M))
            curState = EnemyStates.Chase;
    }
    #endregion

    #region 추적
    IEnumerator Chase_EnterState()
    {
        Debug.Log("Chase 상태 돌입");
        yield return null;
    }

    void Chase_Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            curState = EnemyStates.Attacking;
    }
    #endregion

    #region 공격
    IEnumerator Attacking_EnterState()
    {
        Debug.Log("Attacking 상태 돌입");
        yield return null;
    }

    void Attacking_Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            curState = EnemyStates.Dying;
    }
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

    public void getDamage(float damage)
    {
        hp -= damage;
        if(hp <= 0)
        {
            curState = EnemyStates.Dying;
        }
    }
}
