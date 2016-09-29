﻿using UnityEngine;
using System.Collections;
using System;

public enum PlayerEffectList
{
    DIE, BASIC_JUMP, DASH_JUMP,
}

public class PlayerCtrl : MonoBehaviour
{
    // 플레이어 이동 변수
    public float moveSpeed = 10f; // 이동 속도
    public float maxJumpHight = 10f;
    public float basicJumpHight = 3.0f; // 기본 점프 높이
    public float dashJumpHight = 4.0f; // 대쉬 점프 높이
    public float jumpAccel = 1f;
    public float upGravity = 1f; // 점프 시 중력 값
    public float dropGravity = 5f; // 공중에 있을 때의 중력값
    public static float curGravity; // 현재 중력값

    public static float inputAxis = 0f;     // 입력 받는 키의 값
    public static bool isFocusRight = true; // 우측을 봐라보는 여부
    private float amorTime = 0.5f;

    [System.NonSerialized]
    public bool isMove = true;       // 현재 이동 여부
    [System.NonSerialized]
    public bool isJumping = false;   // 현재 점프중인지 확인

    private float fullHp = 100; // 체력
    private float curHp = 100;
    public static float focusRight = 1f;
    private float lockPosZ = 0f;

    private bool hasSuperArmor = false;

    public float ProportionHP
    {
        get { return curHp / fullHp; }
    }

    public static Vector3 moveDir = Vector3.zero; // 이동 벡터
    public static CharacterController controller; // 캐릭터컨트롤러
    private Animator anim;

    public Cloth cloth;
    public GameObject lunaModel;
    private PlayerEffect pEffect;
    private WahleMove wahleMove;
    public Transform headPoint;

    public static PlayerCtrl instance;

    void Awake()
    {
        instance = this;
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        pEffect = GetComponent<PlayerEffect>();
        wahleMove = GameObject.FindGameObjectWithTag("WAHLE").GetComponent<WahleMove>();
    }

    void Start()
    {
        //GetPlayerData();
        curGravity = dropGravity;

        lockPosZ = transform.position.z;
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, lockPosZ);
        // 플레이어에게 조작권한이 있다면 움직임
        if (isMove) Movement();
        //캐릭터 방향 회전
        if (inputAxis < 0 && isFocusRight) { TurnPlayer(); }
        else if (inputAxis > 0 && !isFocusRight) { TurnPlayer(); }

    }

    void Movement()
    {
        inputAxis = Input.GetAxis("Horizontal"); // 키 입력
        anim.SetFloat("Velocity", controller.velocity.y);

        // 좌우 동시 입력을 막기위함
        if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow) ||
            Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
        {
            inputAxis = 0f;
            anim.SetBool("Run", false);
        }

        // 지상에 있을 시
        if (controller.isGrounded)
        {
            //curGravity = 50f;
            anim.SetBool("Jump", false);
            anim.SetBool("Dash", false);
            anim.SetBool("Idle", false);

            //이동
            moveDir = Vector3.right * inputAxis;

            // 점프
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(BasicJump());
            }

            // 키 입력 시 달리기 애니메이션 재생
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow) ||
                Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) {
                anim.SetBool("Run", true);
            }
            else {
                anim.SetBool("Run", false);
            }
        }
        // 공중에 있을 시
        else if (!controller.isGrounded)
        {
            moveDir.x = inputAxis;

            if (Input.GetKeyDown(KeyCode.Space) && isJumping)
                DashJump();

            if (controller.velocity.y <= -0.1)
            {
                curGravity = dropGravity;
            }
        }

        moveDir.y -= curGravity * Time.deltaTime;
        controller.Move(moveDir * moveSpeed * Time.deltaTime);
    }

    IEnumerator BasicJump()
    {
        curGravity = upGravity;
        isJumping = true;
        anim.SetBool("Jump", true);
        pEffect.StartEffect(PlayerEffectList.BASIC_JUMP);
        float jumpTime = 0f;
        
        //moveDir.y = basicJumpHight;
        //moveDir.y -= curGravity * Time.deltaTime;
        //controller.Move(moveDir * moveSpeed * Time.deltaTime);

        while (Input.GetKey(KeyCode.Space))
        {
            if (jumpTime >= maxJumpHight)
                break;
            moveDir.y = jumpAccel;
            jumpTime += Time.deltaTime;

            yield return null;
        }
    }

    void DashJump()
    {
        curGravity = upGravity;
        anim.SetBool("Dash", true);
        isJumping = false;
        moveDir.y = dashJumpHight;
    }

    //캐릭터 방향 회전
    public void TurnPlayer()
    {
        isFocusRight = !isFocusRight;
        focusRight *= -1f;
    
        transform.Rotate(new Vector3(0, 1, 0), 180);

        wahleMove.ResetSpeed();
        if (!controller.isGrounded) { moveDir.x *= -1f; }

        //Vector3 localScale = transform.localScale;
        //localScale.z *= -1f;
        //transform.localScale = localScale;
    }

    public void getRecovery(float recovery)
    {
        curHp += recovery;
        InGameUI.instance.ChangeHpBar();
        if (curHp >= 100)
        {
            curHp = 100;
            return;
        }
    }

    public void getDamage(float damage)
    {
        if (!hasSuperArmor)
        {
            StartCoroutine(SuperArmor());
            curHp -= damage;
            InGameUI.instance.ChangeHpBar();
            anim.SetTrigger("Hit");
            if (curHp <= 0)
            {
                PlayerDie();
                return;
            }
        }
    }

    IEnumerator SuperArmor()
    {
        hasSuperArmor = true;
        yield return new WaitForSeconds(amorTime);
        hasSuperArmor = false;
    }

    void OnTriggerStay(Collider coll)
    {
        if (coll.CompareTag("DeadLine"))
        {
            PlayerDie();
        }

        else if (coll.CompareTag("Finish"))
        {
            InGameUI_2.instance.GameEnd();
        }
        //else if (coll.CompareTag("OBJECT"))
        //{
        //    if(Input.GetKey(KeyCode.LeftShift) && inputAxis != 0 &&
        //        transform.position.y <= coll.transform.position.y)
        //    {
        //        anim.SetBool("Push", true);
        //        coll.gameObject.GetComponent<PushBox>().PushObject(this.transform);
        //    }
        //    else
        //        anim.SetBool("Push", false);
        //}
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("OBJECT"))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                hit.gameObject.GetComponent<PushBox>().PushObject(this.transform, isFocusRight);
            }
        }
    }

    //void OnTriggerExit(Collider col)
    //{
    //    if (col.CompareTag("OBJECT"))
    //    {
    //        anim.SetBool("Push", false);
    //    }
    //}

    string GetObjectTag()
    {
        RaycastHit hit;
        Vector3 forward = transform.TransformDirection(Vector3.forward);

        // 우측 레이캐스트
        if (Physics.Raycast(headPoint.position, forward, out hit, 1f))
        {
            return hit.collider.tag;
        }
        else
            return null;
    }

    public void PlayerDie()
    {
        StartCoroutine(ResetPlayer());
    }
    
    public void animReset()
    {
        anim.SetFloat("Velocity", 0);
        anim.SetBool("Run", false);
        anim.SetBool("Jump", false);
        anim.SetBool("Dash", false);
        anim.SetBool("Idle", true);
    }

    IEnumerator ResetPlayer()
    {
        FadeInOut.instance.StartFadeInOut(1, 2, 3);
        isMove = false;
        cloth.gameObject.SetActive(false);
        lunaModel.SetActive(false);
        pEffect.StartEffect(PlayerEffectList.DIE);

        yield return new WaitForSeconds(1.3f);

        GetPlayerData();
        cloth.gameObject.SetActive(true);
        lunaModel.SetActive(true);
        isMove = true;
    }

    void GetPlayerData()
    {
        Data pData = new Data(); // 플레이어 데이터 저장을 위한 클래스 변수
        pData = DataSaveLoad.Load();
        transform.position = pData.pPosition;
    }

    void OnEnable()
    {
        WayPoint.OnSave += Save;
    }

    //플레이어 데이터 저장
    void Save()
    {
        Data pData = new Data();
        pData.pPosition = transform.position;
        pData.hp = curHp;
        DataSaveLoad.Save(pData);
    }


    // 2단 점프 끝났을 때 실행
    void SetEndAnim()
    {
        anim.SetBool("Dash", false);
    }

    public void SetStopMove()
    {
        isMove = false;
        moveDir.x = 0f;
        anim.SetBool("Idle", true);
    }

    public void SetPushAnim(bool isPush)
    {
        anim.SetBool("Push", isPush);
    }
}