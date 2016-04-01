using UnityEngine;
using System.Collections;
using System;

public enum JumpType
{
    IDLE, FLY_IDLE, BASIC, DASH, LEAP, POWER, 
}

public class PlayerCtrl : MonoBehaviour {

    public static float inputAxis = 0f;     // 입력 받는 키의 값
    public static bool isFocusRight = true; // 우측을 봐라보는 여부
    private bool isScript = false;          // 현재 대화중 확인
    public static bool isJumping = false;   // 현재 점프중인지 확인
    private bool isFlyingByRope = false;    // 날고 있는지
    private bool isCtrlAuthority = true;    // 플레이어의 조작권한이 있는지
    private string carryItemName = null;    // 들고 있는 아이템 이름
    
    private float currRadian;
    private float vx;
    private float vy;

    public float jumpHight = 3.0f;     // 기본 점프 높이
    public float dashJumpHight = 4.0f; // 대쉬 점프 높이
    //public float flyJumpHight = 2.0f;  // 날기 점프 높이
    public float LeapJumpHight = 3.0f; // 도약 점프 높이
    public float speed = 10f;          // 이동 속도
    public float moveResistant = 0f;   // 이동 저항력

    private float startTime = 0f;

    public Vector3 moveDir = Vector3.zero; // 이동 벡터
    public CharacterController controller; // 캐릭터컨트롤러
    public JumpType jumpState = JumpType.IDLE; // 점프 타입

    public Transform rayTr; // 레이캐스트 시작 위치
    private Animator anim;
    private SwitchObject switchState;
    private float hp = 10;
    private GameObject currInteraction;

    public Transform wahleTr;
    private Collider objColl = null;

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

        // 상호작용을 하기 위한 스위치
        switchState = gameObject.AddComponent<SwitchObject>();
        switchState.IsCanUseSwitch = false;
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
        if (Input.GetKeyDown(KeyCode.E)) { RidePet(); }

        // 플레이어에게 조작권한이 있다면 움직임
        if (isCtrlAuthority) Movement();

    }

    void FixedUpdate()
    {
        RopeWorker();

        // 플레이어
        //if (isCtrlAuthority) Movement();
        //else anim.SetFloat("Speed", 0f);

        //고래이동
        //if (WahleCtrl.moveType != WahleCtrl.Type.keybord && isCtrlAuthority) Movement();
        //else anim.SetFloat("Speed", 0f);

        //추락하여 사망 시
        if (transform.position.y <= -5.0f) {
            Debug.Log("Die");
            pData = PlayerData.Load();
            transform.position = pData.pPosition;
        }
    }

    // 권한 찾기
    void getCtrlAuthorityByRope()
    {
        //        currRadian = currInteraction.GetComponent<RopeCtrl>().getLowRopeTransform().eulerAngles.z
        currRadian = currInteraction.GetComponent<RopeCtrl>().getRadian();
        float speed = currInteraction.GetComponent<RopeCtrl>().getSpeed();
        vx = Mathf.Cos(currRadian * Mathf.Deg2Rad) * (speed * 10.0f);
        vy = Mathf.Sin(currRadian * Mathf.Deg2Rad) * (speed * 5.0f);

        moveDir.y = vy;
        isFlyingByRope = true;
        currInteraction = null;
    }

    void Movement()
    {
        // 키 입력
        inputAxis = Input.GetAxis("Horizontal");
        // 지상에 있을 시
        if (controller.isGrounded && !isScript)
        {
            jumpHight = 3f;
            //이동
            moveDir = Vector3.right * inputAxis;
            //anim.SetBool("Jump", false);
            anim.SetFloat("Speed", inputAxis);
            // 점프
            if (Input.GetKey(KeyCode.UpArrow) && Input.GetKeyDown(KeyCode.Space)) { Jump(JumpType.LEAP); }
            else if (Input.GetKey(KeyCode.Space)) { Jump(JumpType.BASIC); }

            
        }
        // 공중에 있을 시
        else if (!controller.isGrounded)
        {
            moveDir.x = inputAxis * 25f * Time.deltaTime;
            controller.Move(moveDir * Time.deltaTime);
            // 대쉬 점프
            if (Input.GetKey(KeyCode.DownArrow)) { Jump(JumpType.POWER); }
            else if (Input.GetKeyDown(KeyCode.Space)) { Jump(JumpType.DASH); }
        }

        //캐릭터 방향 회전
        if (inputAxis < 0 && isFocusRight) { TurnPlayer(); }
        else if (inputAxis > 0 && !isFocusRight) { TurnPlayer(); }

        moveDir += Physics.gravity * Time.deltaTime;

        controller.Move(moveDir * (speed - moveResistant) * Time.deltaTime);
    }

    //캐릭터가 봐라보는 방향 회전
    void TurnPlayer()
    {
        isFocusRight = !isFocusRight;
        transform.Rotate(new Vector3(0, 1, 0), 180.0f);
    }

    // 점프
    void Jump(JumpType isJump)
    {
        jumpState = isJump;
        switch (jumpState)
        {
            case JumpType.BASIC:
                isJumping = false;
                if (!isJumping) {
                    isJumping = true;
                    StartCoroutine(Jumping());  
                }
                break;
            case JumpType.DASH:
                if (isJumping) {
                    moveDir.y = dashJumpHight;
                    isJumping = false;
                }
                break;
            case JumpType.LEAP:
                moveDir.y = 5f;
                PlayerFunc.instance.FindObject();
                break;
            case JumpType.POWER:
                moveDir.y = -7f;
                StartCoroutine(WaitTime());
                break;
        }
    }
    IEnumerator WaitTime()
    {
        while (true)
        {
            if (controller.isGrounded) { break; }
            yield return null;
        }
        PlayerFunc.instance.SetPowerDamage();
        StopCoroutine(WaitTime());
    }

    IEnumerator Jumping()
    {
        float jumpTime = 0f; // 체공 시간

        while (Input.GetKey(KeyCode.Space) && jumpTime <= 0.18f)
        {
            moveDir.y = jumpHight;
            jumpTime += Time.deltaTime;

            yield return null;
        }
        StopCoroutine(Jumping());
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
    
    public void getRecovery(float recovery)
    {
        hp += recovery;
        if (hp >= 100)
        {
            hp = 100;
            Debug.Log(hp);
            return;
        }
        Debug.Log(hp);
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

    public bool IsJumping()
    {
        return isJumping;
    }

    // 로프의 움직임 관련
    void RopeWorker()
    {
        // 로프에서 권한을 찾아옴 ( 상호작용 하고 있는 로프의 조작 권한이 없어지면 )
        if (currInteraction != null && !isCtrlAuthority && !currInteraction.GetComponent<RopeCtrl>().isCtrlAuthority)
        {
            getCtrlAuthorityByRope();
        }

        // 로프를 타고 있을때
        if (!isCtrlAuthority && !isFlyingByRope && currInteraction != null)
        {
            Vector3 pos = currInteraction.GetComponent<RopeCtrl>().getLowRopeTransform().position;
            Quaternion rot = gameObject.transform.rotation;

            pos.y -= gameObject.transform.localScale.y;

            gameObject.transform.position = pos;
            gameObject.transform.rotation = rot;
        }
        // 로프를 이용해 날고 있을때
        if (isFlyingByRope)
        {

            controller.Move(Vector3.right * vx * Time.deltaTime);
            moveDir += Physics.gravity * Time.deltaTime;
            controller.Move(moveDir * 10.0f * Time.deltaTime);

            if (controller.isGrounded)
            {
                isFlyingByRope = false;
                isCtrlAuthority = true;
            }
        }
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.name == "Switch")
        {
            coll.GetComponent<SwitchObject>().IsCanUseSwitch = true;
            switchState = coll.GetComponent<SwitchObject>();
        }

        if (coll.tag == "Rope" && Input.GetKey(KeyCode.UpArrow) && isCtrlAuthority)
        {
            isCtrlAuthority = false;
            currInteraction = coll.transform.parent.gameObject;
            currInteraction.GetComponent<RopeCtrl>().setPlayerAuthority(Convert.ToInt32(coll.name));
        }
    }

    void OnTriggerExit(Collider coll)
    {
        if (coll.name == "Switch")
        {
            coll.GetComponent<SwitchObject>().IsCanUseSwitch = false;
            switchState = gameObject.AddComponent<SwitchObject>();
        }
    }
}
