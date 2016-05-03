using UnityEngine;
using System.Collections;
using System;

public enum JumpType
{
    IDLE, FLY_IDLE, BASIC, DASH, LEAP, POWER, 
}
public enum PlayerEffectList
{
    BASIC_JUMP, DASH_JUMP, 
}


public class PlayerCtrl : MonoBehaviour
{

    public static float inputAxis = 0f;     // 입력 받는 키의 값
    public static bool isFocusRight = true; // 우측을 봐라보는 여부
    public static bool isMove = true;       // 현재 이동 여부
    public static bool isJumping = false;   // 현재 점프중인지 확인
    private bool isFlyingByRope = false;    // 날고 있는지
    private bool isCtrlAuthority = true;    // 플레이어의 조작권한이 있는지
    private string carryItemName = null;    // 들고 있는 아이템 이름

    private float hp = 10; // 체력
    private float gravity = 5f;


    private float curHp = 10; // 체력
    private float fullHp = 100; // 체력
    public float ProportionHP
    {
        get { return curHp / fullHp; }
    }

    private float currRadian;
    private float vx;
    private float vy;

    public float jumpHight = 3.0f;     // 기본 점프 높이
    public float dashJumpHight = 4.0f; // 대쉬 점프 높이
    public float speed = 10f;          // 이동 속도
    public float moveResistant = 0f;   // 이동 저항력

    public Vector3 moveDir = Vector3.zero; // 이동 벡터
    public CharacterController controller; // 캐릭터컨트롤러
    public JumpType jumpState = JumpType.IDLE; // 점프 타입

    public Transform rayTr; // 레이캐스트 시작 위치
    private Animator anim;
    private SwitchObject switchState;

    private GameObject currInteraction;
	private Vector3 originPos;

	public float reloadValue;
   
    //private Collider objColl = null;

    bool isClimb = false;

    Data pData = new Data(); // 플레이어 데이터 저장을 위한 클래스 변수

    public static PlayerCtrl instance;

    Rigidbody rb;

    public string getCarryItemName()
    {
        return carryItemName;
    }

    void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        // 상호작용을 하기 위한 스위치
        switchState = gameObject.AddComponent<SwitchObject>();
        switchState.IsCanUseSwitch = false;

		originPos = this.transform.position;
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
        // 점프
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.X)) && controller.isGrounded) {
            //rb.AddForce(Vector3.up * 10f * Time.deltaTime);
            Jump(JumpType.BASIC); 
        }
        else if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.X)) && !controller.isGrounded) { Jump(JumpType.DASH); }
        // 상호작용 (버튼 조작)
        else if (Input.GetKeyDown(KeyCode.KeypadEnter)) { switchState.IsSwitchOn = !switchState.IsSwitchOn; }
        //NPC와 대화
        else if (Input.GetKeyDown(KeyCode.Return)) { ShotRay(); }
        //펫 타기
        else if (Input.GetKeyDown(KeyCode.E)) { PlayerFunc.instance.RidePet(); }

        // 걷기 애니메이션이 아직 없어 임시의 이동 애니메이션 재생을 위한 것
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow)) {
            anim.SetBool("Run", true);
        } else {
            anim.SetBool("Run", false);
        }

		if (transform.position.y <= -5f) {
			transform.position = originPos;
		} 
    }

    void FixedUpdate()
    {
        // 플레이어에게 조작권한이 있다면 움직임
        if (isCtrlAuthority) Movement();
        else RopeWorker();
    }

    void Movement()
    {
        // 키 입력
        inputAxis = Input.GetAxis("Horizontal");
        // 지상에 있을 시
        if (controller.isGrounded && isMove)
        {
            isJumping = false;
            gravity = 14f;
            //이동
            moveDir = Vector3.right * inputAxis;
            anim.SetBool("Jump", false);
            //anim.SetFloat("Speed", inputAxis);
        }
        // 공중에 있을 시
        else if (!controller.isGrounded)
        {
            gravity = 5f;
            moveDir.x = inputAxis * 50f * Time.deltaTime;
            controller.Move(moveDir * Time.deltaTime);
        }

        //캐릭터 방향 회전
        if (inputAxis < 0 && isFocusRight) { TurnPlayer(); }
        else if (inputAxis > 0 && !isFocusRight) { TurnPlayer(); }

        if (!isClimb)
        {
           moveDir.y -= gravity * Time.deltaTime;
        }

        else if (isClimb)
        {
            inputAxis = Input.GetAxis("Vertical");
            moveDir = Vector3.up * inputAxis;
        }

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
        anim.SetBool("Jump", true);
        switch (jumpState)
        {
            case JumpType.BASIC:
                isJumping = false;
                if (!isJumping)
                {
                    isJumping = true;
                    ///////////////////////////////////////////////
                    //gameObject.GetComponent<PlayerEffect>().StartEffect(PlayerEffectList.BASIC_JUMP);
                    moveDir.y = jumpHight;
                    controller.Move(moveDir * (3f - moveResistant) * Time.deltaTime);

                }
                break;
            case JumpType.DASH:
                if (isJumping)
                {
                    //////////////////////////////////////////////
                    //gameObject.GetComponent<PlayerEffect>().StartEffect(PlayerEffectList.DASH_JUMP);
                    moveDir.y = dashJumpHight;
                    controller.Move(moveDir * (3f - moveResistant) * Time.deltaTime);
                    isJumping = false;
                }
                break;
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
        if (Physics.Raycast(rayTr.position, forward, out hit, 8f))
        {
            //앞에 오를 수 있는 오브젝트 있을 시
            if (hit.collider.gameObject.tag == "WALL")
            {
                Debug.Log("Climb");
            }
            //NPC 체크 및 이름 확인
            else if (hit.collider.CompareTag("NPC"))
            {
                string name = hit.collider.gameObject.name;
                hit.collider.transform.Rotate(Vector3.up, 180f);
                PlayerFunc.instance.ShowScript(name);
            }
        }
    }

    public void getRecovery(float recovery)
    {
        curHp += recovery;
        InGameUI.instance.ChangeHpBar();
        if (curHp >= 100)
        {
            curHp = 100;
            Debug.Log(curHp);
            return;
        }
        Debug.Log(curHp);
    }

    public void getDamage(float damage)
    {
        curHp -= damage;
        //InGameUI.instance.ChangeHpBar();
        if (curHp <= 0)
        {
            //PlayerDie();
            Debug.Log("Player Die");
            return;
        }
    }

    public bool IsJumping()
    {
        return isJumping;
    }

    // 로프에서 권한 찾기
    public void GetCtrlAuthorityByRope()
    {
        currRadian = currInteraction.GetComponent<RopeCtrl>().getRadian();
        float speed = currInteraction.GetComponent<RopeCtrl>().getSpeed();
        vx = Mathf.Cos(currRadian * Mathf.Deg2Rad) * (speed * 1.2f);
        vy = Mathf.Sin(currRadian * Mathf.Deg2Rad) * (speed * 0.3f);

        /*
        Debug.Log(" 각속도 : " + speed);
        Debug.Log("  각도 : " + currRadian);
        Debug.Log(" X벡터 : " + vx);
        Debug.Log(" Y벡터 : " + vy);
        */

        moveDir.y = vy;
        isFlyingByRope = true;
        isJumping = true;
        currInteraction = null;
    }

    // 로프의 움직임 관련
    void RopeWorker()
    {
        // 로프에서 권한을 찾아옴 ( 상호작용 하고 있는 로프의 조작 권한이 없어지면 )
        if (currInteraction != null && !isCtrlAuthority && !currInteraction.GetComponent<RopeCtrl>().isCtrlAuthority)
        {
            GetCtrlAuthorityByRope();
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
            moveDir += new Vector3(0f, -7f, 0f) * Time.deltaTime;   // 중력
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
        // 퀘스트 아이템 습득
        if (coll.CompareTag("ITEM"))
        {
            if(QuestMgr.questInfo.targetName == coll.name){
                QuestMgr.instance.curCompletNum++;
            }
        }

        if (coll.name == "Switch")
        {
            coll.GetComponent<SwitchObject>().IsCanUseSwitch = true;
            switchState = coll.GetComponent<SwitchObject>();
        }

        // 로프와 충돌
        if (coll.tag == "Rope" && Input.GetKey(KeyCode.UpArrow) && isJumping)
        {
            isCtrlAuthority = false;
            isJumping = false;
            isFlyingByRope = false;
            currInteraction = coll.transform.parent.gameObject;
            currInteraction.GetComponent<RopeCtrl>().
            setAuthority(Convert.ToInt32(coll.name), isFocusRight); // 조작권한을 넘겨줌
        }
    }

    void OnTriggerStay(Collider coll)
    {
        if (coll.tag == "WALL")
        {
            isClimb = true;
        }
    }

    void OnTriggerExit(Collider coll)
    {
        if (coll.name == "Switch")
        {
            switchState = gameObject.AddComponent<SwitchObject>();
        }
        isClimb = false;
    }


    void PlayerDie()
    {
        pData = PlayerData.Load();
        transform.position = pData.pPosition;
    }

}
