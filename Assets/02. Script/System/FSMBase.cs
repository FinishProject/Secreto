﻿using UnityEngine;
using System.Collections;
using System.Reflection;
using System;

/****************************   정보   ****************************

    FSM 기본 틀

    사용방법 :

    1. 부모 클래스, 자식 클래스에서 상속 받자

    2. 자식클래스에서 Awake 을 쓸때 OnAwake를 오버라이딩해서 사용

    3. 자식클래스에서 아래 함수들을 쓸때 앞에 "상태_" 를 붙여주자
       ex) "Idle_Update"   "Chase_FixedUpdate"

    "Update"             "LateUpdate"        "FixedUpdate"
    "OnTriggerEnter"     "OnTriggerExir"     "OnTriggerEnter"    
    "OnCollisionEnter"   "OnCollisionExit"   "OnCollisionStay"
    "EnterState"         "ExitState"      
    
      
******************************************************************/

public class FSMBase : MonoBehaviour {

    // 현재 상태 저장
    private Enum _curState;
    public Enum curState
    {
        get
        {
            return _curState;
        }
        set
        {
            _curState = value;
            ChangState();
        }
    }

    // 원래 있는 변수들을 덮어 씌우기 위해 new 를 붙임
    protected new Transform transform;
    protected new Rigidbody rigidbody;
    protected new Animation animation;
    protected new GameObject gameObject;
    protected new Collider collider;

    public Animator anim;

    public float hp = 40;
    public AttributeState curAttibute;

    // 자식 클래스에서 초기화 ( Awake 함수 쓰기 위해 )
    protected virtual void OnAwake() { }
    void Awake()
    {
        transform = base.transform;
        gameObject = base.gameObject;
        
        rigidbody = GetComponent<Rigidbody>();
        animation = GetComponent<Animation>();     
        collider = GetComponent<Collider>();
        anim = GetComponent<Animator>();
        OnAwake();
    }
    void Update() { DoUpdate(); }
    void LateUpdate() { DoLateUpdate(); }
    void FixedUpdate() { DoFixedUpdate(); }
    void OnTriggerEnter(Collider col) { DoOnTriggerEnter(col); }
    void OnTriggerExit(Collider col) { DoOnTriggerExit(col); }
    void OnTriggerStay(Collider col) { DoOnTriggerStay(col); }
    void OnCollisionEnter(Collision col) { DoOnCollisionEnter(col); }
    void OnCollisionExit(Collision col) { DoOnCollisionExit(col); }
    void OnCollisionStay(Collision col) { DoOnCollisionStay(col); }
 
    static void DoNothing() { }
    static void DoNothingCollider(Collider col) { }
    static void DoNothingCollision(Collision col) { }
    static IEnumerator DoNothingCoroutine() { yield return null; }
     
    public Action DoUpdate = DoNothing;
    public Action DoLateUpdate = DoNothing;
    public Action DoFixedUpdate = DoNothing;
    public Action<Collider> DoOnTriggerEnter = DoNothingCollider;
    public Action<Collider> DoOnTriggerExit = DoNothingCollider;
    public Action<Collider> DoOnTriggerStay = DoNothingCollider;
    public Action<Collision> DoOnCollisionEnter = DoNothingCollision;
    public Action<Collision> DoOnCollisionExit = DoNothingCollision;
    public Action<Collision> DoOnCollisionStay = DoNothingCollision;
    public Func<IEnumerator> ExitState = DoNothingCoroutine;

    // 델리게이트 상태 변환 시켜줌
    T ChagneDelegate<T>(string methodName, T Default) where T : class
    {
        // _curState.ToString() + "_" + methodName 이름의 함수 검색
        var warkMethod = GetType().GetMethod(_curState.ToString() + "_" + methodName,
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod); //  검색 조건

        if (warkMethod != null)
        {
            //일반적인 인스턴스가 필요로하는 타입의 델리게이트를 만들고
            //이를 T타입으로 캐스팅 한다.                     
            return Delegate.CreateDelegate(typeof(T), this, warkMethod) as T;
        }
        else
        {
            return Default;
        }
    }

    // 상태를 바꿔줌
    void ChangState()
    {
        if (ExitState != null)
        {
            StartCoroutine(ExitState());
        }

        DoUpdate = ChagneDelegate<Action>("Update", DoNothing);
        DoLateUpdate = ChagneDelegate<Action>("LateUpdate", DoNothing);
        DoFixedUpdate = ChagneDelegate<Action>("FixedUpdate", DoNothing);

        DoOnTriggerEnter = ChagneDelegate<Action<Collider>>("OnTriggerEnter", DoNothingCollider);
        DoOnTriggerExit = ChagneDelegate<Action<Collider>>("OnTriggerExir", DoNothingCollider);
        DoOnTriggerStay = ChagneDelegate<Action<Collider>>("OnTriggerStay", DoNothingCollider);

        DoOnCollisionEnter = ChagneDelegate<Action<Collision>>("OnCollisionEnter", DoNothingCollision);
        DoOnCollisionExit = ChagneDelegate<Action<Collision>>("OnCollisionExit", DoNothingCollision);
        DoOnCollisionStay = ChagneDelegate<Action<Collision>>("OnCollisionStay", DoNothingCollision);

        Func<IEnumerator> EnterState = ChagneDelegate<Func<IEnumerator>>("EnterState", DoNothingCoroutine);
        ExitState = ChagneDelegate<Func<IEnumerator>>("ExitState", DoNothingCoroutine);

        //현재 상태를 시작한다.
        StartCoroutine(EnterState());  
    }

    public IEnumerator WaitForAnimation(string name, float ratio)
    {
        var state = animation[name];
        return WaitForAnimation(state, ratio);
    }

    public static IEnumerator WaitForAnimation(AnimationState state, float ratio)
    {
        state.wrapMode = WrapMode.ClampForever;
        state.enabled = true;
        state.speed = state.speed == 0 ? 1 : state.speed;
        while (state.normalizedTime < ratio - float.Epsilon)
        {
            yield return null;
        }
    }

    public IEnumerator PlayAnimation(string name)
    {
        var state = animation[name];
        return PlayAnimation(state);
    }

    public static IEnumerator PlayAnimation(AnimationState state)
    {
        state.time = 0;
        state.weight = 1;
        state.speed = 1;
        state.enabled = true;
        var wait = WaitForAnimation(state, 1.0f);
        while (wait.MoveNext())
        {
            yield return null;
        }
        state.weight = 0; 
    }

    // 데미지 입을때
    virtual public void GetDamage(float damage)
    {
        hp -= damage;
    }

}
