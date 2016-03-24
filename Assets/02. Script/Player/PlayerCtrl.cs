﻿using UnityEngine;
using System.Collections;
using System;

public class PlayerCtrl : MonoBehaviour {

    private float inputAxis = 0f; // 입력 받는 키의 값
    public static bool isFocusRight = true; // 우측을 봐라보는 여부
    private bool isScript = false; // 현재 대화중 확인
    private bool isJumping = false; // 현재 점프중인지 확인
    private bool isFlying = false;  // 날고 있는지
    private bool isCtrlAuthority = true; // 조작권한 (로프 조종)
    private string carryItemName = null;

    private float currRadian;
    private float vx;
    private float vy;

    public float jumpHight = 6.0f; // 기본 점프 높이
    public float dashJumpHight = 4.0f; //대쉬 점프 높이
    public float speed = 10f; // 이동 속도

    public Vector3 moveDir = Vector3.zero; // 이동 벡터
    public CharacterController controller; // 캐릭터컨트롤러

    public Transform rayTr; // 레이캐스트 시작 위치
    private Animator anim;
    private SwitchObject switchState;
    private float hp = 100;
    private GameObject currInteraction;

    Data pData = new Data(); // 플레이어 데이터 저장을 위한 클래스 변수

    public static PlayerCtrl instance;

    public string getCarryItemName()
    {
        return carryItemName;
    }

    void Awake()
    {
        instance = this;
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>(); 
    }

    //void Start()
    //{
    //    pData = PlayerData.Load();
    //    transform.position = pData.pPosition;
    //}

    //플레이어 데이터 저장
    public void Save()
    {
        pData.pPosition = transform.position;
        PlayerData.Save();
    }

    void Update()
    {

        // 상호작용 (버튼 조작)
        if (Input.GetKeyDown(KeyCode.Z)) { switchState.IsSwitchOn = !switchState.IsSwitchOn; }

        //NPC와 대화
        if (Input.GetKeyDown(KeyCode.Return)) { ShotRay(); }

        //펫 타기
        else if (Input.GetKeyDown(KeyCode.E)) { RidePet(); }

        if (currInteraction!= null && !currInteraction.GetComponent<RopeCtrl>().isCtrlAuthority)
        {
            getCtrlAuthority();
            isCtrlAuthority = true;
        }
             
    }

    void FixedUpdate()
    {
        // 로프
        if (!isCtrlAuthority && currInteraction != null)
        {
            Vector3 pos = currInteraction.GetComponent<RopeCtrl>().getLowRopeTransform().position;
            Quaternion rot = gameObject.transform.rotation;

            pos.y -= gameObject.transform.localScale.y;
            //            rot.x = currInteraction.GetComponent<RopeCtrl>().getLowRopeTransform().eulerAngles.z;

            gameObject.transform.position = pos;
            gameObject.transform.rotation = rot;
        }

        if (isFlying)
        {
            moveDir += Physics.gravity * Time.deltaTime;
            controller.Move(((Vector3.right * vx) + moveDir) * Time.deltaTime / 10f);
            /*
            moveDir.x += vx;
            moveDir += Physics.gravity * Time.deltaTime;
            controller.Move(moveDir * (speed - weatherValue) * Time.deltaTime);
            */
            if (controller.isGrounded)
            {
                /*
                Quaternion rot = gameObject.GetComponent<RectTransform>().rotation;
                rot.x = 0.0f;
                rot.y = 90.0f;
                rot.z = 0.0f;
                gameObject.GetComponent<RectTransform>().rotation = baseRot;
                */
                isFlying = false;
            }

        }

        //이동
        if (WahleCtrl.moveType != WahleCtrl.Type.keybord && isCtrlAuthority) Movement();
        else anim.SetFloat("Speed", 0f);

        //추락하여 사망 시
        if (transform.position.y <= -5.0f) {
            Debug.Log("Die");
            pData = PlayerData.Load();
            transform.position = pData.pPosition;
        }
    }

    // 권한 찾기
    void getCtrlAuthority()
    {
        currRadian = currInteraction.GetComponent<RopeCtrl>().getLowRopeTransform().eulerAngles.z;
        float speed = currInteraction.GetComponent<RopeCtrl>().getSpeed();
        vx = Mathf.Cos(currRadian * Mathf.Deg2Rad) * (-speed * 15.0f);
        vy = Mathf.Sin(currRadian * Mathf.Deg2Rad) * (-speed * 15.0f);

        Debug.Log(-speed);
        Debug.Log(currRadian);
        Debug.Log(vy);
        moveDir.y = vy;
        isFlying = true;
    }

    void Movement()
    {
        // 키 입력
        inputAxis = Input.GetAxis("Horizontal");
        if (controller.isGrounded && !isScript)
        {
            //이동
            moveDir = Vector3.right * inputAxis;
            //anim.SetBool("Jump", false);
            //점프
            if (Input.GetKeyDown(KeyCode.Space)) { Jump(true); }
            anim.SetFloat("Speed", inputAxis);
        }
        else if (!controller.isGrounded)
        {
            moveDir.x = inputAxis * 50f * Time.deltaTime;
            controller.Move(moveDir * Time.deltaTime);
            //대쉬 점프
            if (Input.GetKeyDown(KeyCode.Space)) { Jump(false); }
        }

        //캐릭터 방향 회전
        if (inputAxis < 0 && isFocusRight) { TurnPlayer(); }
        else if (inputAxis > 0 && !isFocusRight) { TurnPlayer(); }

        moveDir += Physics.gravity * Time.deltaTime;
        controller.Move(moveDir * speed * Time.deltaTime);
    }

    //캐릭터가 봐라보는 방향 회전
    void TurnPlayer()
    {
        isFocusRight = !isFocusRight;
        transform.Rotate(new Vector3(0, 1, 0), 180.0f);
    }

    //점프
    void Jump(bool bJump)
    {
        if (bJump) { // 짧은 점프
            moveDir.y = jumpHight;
            isJumping = true;
        }
        else if (!bJump && isJumping) { // 대쉬 점프
            moveDir.y = dashJumpHight;
            isJumping = false;
        }
    }

    //캐릭터 컨트롤러 충돌
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body == null || body.isKinematic)
            return;
        if (hit.moveDirection.y < -0.3F)
            return;
        //오브젝트 밀기
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
            body.velocity = pushDir * 2f;
        }

    }

    //레이캐스팅 발사
    void ShotRay()
    {
        RaycastHit hit;
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        if (Physics.Raycast(rayTr.position, forward, out hit, 3f))
        {
            //앞에 오를 수 있는 오브젝트 있을 시
            if (hit.collider.gameObject.tag == "Climb")
            {
                Debug.Log("Climb");
            }
            //NPC 체크 및 이름 확인
            else if (hit.collider.gameObject.tag == "NPC")
            {
                string name = hit.collider.gameObject.name;
                ShowScript(name);
            }
        }
    }

    //ScriptMgr에서 NPC이름을 찾아서 대화 생성
    void ShowScript(string name)
    {
        if (name != null)
        {
            //대화 중이면 true, 캐릭터 정지
            isScript = ScriptMgr.instance.GetScript(name);
            inputAxis = 0f;
        }
    }

    //펫 타기
    void RidePet()
    {
        Debug.Log("Riding Pet");
    }

    public void getDamage(float damage)
    {
        hp -= damage;
        if(hp <= 0)
        {
            Debug.Log("사망");
            return;
        }
        Debug.Log(hp);
    }


    void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "Rope" && Input.GetKey(KeyCode.UpArrow) && isCtrlAuthority)
        {
            isCtrlAuthority = false;
            currInteraction = coll.transform.parent.gameObject;
            currInteraction.GetComponent<RopeCtrl>().setPlayerAuthority(Convert.ToInt32(coll.name));
        }
    }


}
