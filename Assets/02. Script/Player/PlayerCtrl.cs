﻿using UnityEngine;
using System.Collections;

public class PlayerCtrl : MonoBehaviour, WorldObserver
{

    WorldSubject worldData;
    WeatherState weatherState;
    float weatherValue;

    public float inputAxis = 0f; // 입력 받는 키의 값
    public static bool isFocusRight = true; // 우측을 봐라보는 여부
    private bool isScript = false; // 현재 대화중 확인
    private bool isUsingLeaf = false; // 나뭇잎 쓰고 있니?
    private bool isJumping = false; // 현재 점프중인지 확인
    private string carryItemName = null;

    public float jumpHight = 6.0f; // 기본 점프 높이
    public float dashJumpHight = 6.0f; //대쉬 점프 높이
    public float speed = 10f; // 이동 속도

    public float LeafTimer = 10.0f;

    public Transform rayTr; // 레이캐스트 시작 위치
    public Vector3 moveDir = Vector3.zero; // 이동 벡터
    public CharacterController controller; // 캐릭터컨트롤러
    private Animator anim;
    private SwitchObject switchState;
    private float hp = 100;

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
        switchState = new SwitchObject();
        switchState.IsCanUseSwitch = false;

        // 옵저버 등록
        worldData = WorldCtrl.GetInstance().RetrunThis();
        worldData.registerObserver(this);
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
        if (Input.GetKeyDown(KeyCode.Z)) { switchState.IsSwitchOn = !switchState.IsSwitchOn; }
    }

    void FixedUpdate()
    {
        //이동
        if (WahleCtrl.moveType != WahleCtrl.Type.keybord) Movement();
        else anim.SetFloat("Speed", 0f);

        //NPC와 대화
        if (Input.GetKeyDown(KeyCode.Return)) { ShotRay(); }
        //펫 타기
        else if (Input.GetKeyDown(KeyCode.E)) { RidePet(); }


        //아래로 떨어졌을 시
        if (transform.position.y <= -5.0f)
        {
            Debug.Log("Die");
            pData = PlayerData.Load();
            transform.position = pData.pPosition;
        }
    }

    void Movement()
    {
        inputAxis = Input.GetAxis("Horizontal");
        //캐릭터 방향 회전
        if (inputAxis < 0 && isFocusRight) { TurnPlayer(); }
        else if (inputAxis > 0 && !isFocusRight) { TurnPlayer(); }


        {   // 날씨 효과
            if ((WeatherState.NONE & weatherState) == WeatherState.NONE)
            {

            }
            if ((WeatherState.WIND_LR & weatherState) == WeatherState.WIND_LR)
            {
                controller.Move(-Vector3.right * weatherValue * Time.deltaTime);
            }
            if ((WeatherState.WIND_UD & weatherState) == WeatherState.WIND_UD && isUsingLeaf)
            {
                moveDir.x = inputAxis * 50f * Time.deltaTime;
                controller.Move(moveDir * Time.deltaTime);

                if (Input.GetKeyDown(KeyCode.Space)) { moveDir.y = 2f; }

                moveDir += new Vector3(0.0f, -2.0f, 0.0f) * Time.deltaTime;
            }
            else
            {
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

                moveDir += Physics.gravity * Time.deltaTime;
            }
            if ((WeatherState.RAIN & weatherState) == WeatherState.RAIN && !isUsingLeaf)
            {
                controller.Move(moveDir * (speed - weatherValue) * Time.deltaTime);
            }
            else
            {
                controller.Move(moveDir * speed * Time.deltaTime);
            }
        }
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
        if (bJump)
        { // 짧은 점프
            moveDir.y = jumpHight;
            isJumping = true;
        }
        else if (!bJump && isJumping)
        { // 대쉬 점프
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

    public void updateObserver(WeatherState weatherState, float weatherValue)
    {
        this.weatherState = weatherState;
        this.weatherValue = weatherValue;
    }

    void OnGUI()
    {
        string tempText;
        tempText = "바람 : ";
        if ((WeatherState.WIND_LR & weatherState) == WeatherState.WIND_LR)
            tempText += "L/R  ";
        else if ((WeatherState.WIND_UD & weatherState) == WeatherState.WIND_UD)
            tempText += "U/D  ";
        else
            tempText += "OFF  ";

        tempText += "비  : ";
        if ((WeatherState.RAIN & weatherState) == WeatherState.RAIN)
            tempText += "ON   ";
        else
            tempText += "OFF  ";

        if (isUsingLeaf)
            tempText += "나뭇잎 사용 O";
        else
            tempText += "나뭇잎 사용 X";

        GUI.TextField(new Rect(0, 0, 300.0f, 30.0f), tempText);
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.name == "Leaf")
        {
            Destroy(coll.gameObject);
            isUsingLeaf = true;
            carryItemName = coll.name;
            StartCoroutine(LeafDestroy());
        }

        if (coll.tag == "Rope")
        {
            Debug.Log(coll.name);
        }

        if (coll.name == "Switch")
        {
            coll.GetComponent<SwitchObject>().IsCanUseSwitch = true;
            switchState = coll.GetComponent<SwitchObject>();
        }
    }

    void OnTriggerExit(Collider coll)
    {
        if (coll.name == "Switch")
        {
            coll.GetComponent<SwitchObject>().IsCanUseSwitch = false;
            switchState = new SwitchObject();
        }
    }


    // 임시
    IEnumerator LeafDestroy()
    {
        yield return new WaitForSeconds(LeafTimer);
        carryItemName = null;
        isUsingLeaf = false;
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
}
