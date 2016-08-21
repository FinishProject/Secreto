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
    public JumpType jumpState = JumpType.IDLE; // 점프 타입

    public static float inputAxis = 0f;     // 입력 받는 키의 값
    public static bool isFocusRight = true; // 우측을 봐라보는 여부
    public float jumpHight = 3.0f;     // 기본 점프 높이
    public float dashJumpHight = 4.0f; // 대쉬 점프 높이
    public float speed = 10f;          // 이동 속도
    private float amorTime = 0.5f;
    public float pushPower = 2f;

    [System.NonSerialized]
    public float moveResistant = 0f;   // 이동 저항력
    [System.NonSerialized]
    public bool isMove = true;       // 현재 이동 여부
    [System.NonSerialized]
    public bool isJumping = false;   // 현재 점프중인지 확인

    private bool isFlyingByRope = false;    // 날고 있는지
    private bool isCtrlAuthority = true;    // 플레이어의 조작권한이 있는지
    private string carryItemName = null;    // 들고 있는 아이템 이름
    private bool isClimb = false; // 벽 오르기 확인

    private float gravity = 5f; // 중력값
	public float gr = 5;
    private float fullHp = 100; // 체력
    private float curHp = 100;
    public static float focusRight = 1f;
    private float lockPosZ = 0f;

    private float currRadian;
    private float vx;
    private float vy;

    private bool hasSuperArmor = false;

    public float ProportionHP
    {
        get { return curHp / fullHp; }
    }
   
    public Transform rayTr; // 레이캐스트 시작 위치
    public static Vector3 moveDir = Vector3.zero; // 이동 벡터
    public static CharacterController controller; // 캐릭터컨트롤러
    private SwitchObject switchState;
    private GameObject currInteraction;

    private Animator anim;
    public Cloth cloth;

    private Data pData = new Data(); // 플레이어 데이터 저장을 위한 클래스 변수
    private PlayerEffect pEffect;
    private PlayerFunc pFunc;
    private WahleMove wahleMove;

    public static PlayerCtrl instance;

    void Awake()
    {
        instance = this;
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        pEffect = GetComponent<PlayerEffect>();
        pFunc = GetComponent<PlayerFunc>();
        wahleMove = GameObject.FindGameObjectWithTag("WAHLE").GetComponent<WahleMove>();

        // 상호작용을 하기 위한 스위치
        switchState = gameObject.AddComponent<SwitchObject>();
        switchState.IsCanUseSwitch = false;
    }

    void Start()
    {
        pData = DataSaveLoad.Load();
        if (pData != null)
        {
            curHp = fullHp;
            transform.position = pData.pPosition;
            lockPosZ = pData.pPosition.z;
        }
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, lockPosZ);
        // 플레이어에게 조작권한이 있다면 움직임
        if (isCtrlAuthority) Movement();
        else RopeWorker();

        //NPC와 대화
        //if (Input.GetKeyDown(KeyCode.Return)) { ShotRay(); }
        // 상호작용 (버튼 조작)
        if (Input.GetKeyDown(KeyCode.KeypadEnter)) { switchState.IsSwitchOn = !switchState.IsSwitchOn; }
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

        if (isMove)
        {
            // 지상에 있을 시
            if (controller.isGrounded)
            {
                gravity = 50f;

                anim.SetBool("Jump", false);
                anim.SetBool("Dash", false);
                anim.SetBool("Idle", false);

                //이동
                moveDir = Vector3.right * inputAxis;

                //cloth.worldAccelerationScale = 2f;
                // 점프
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Jump(JumpType.BASIC);
                    anim.SetBool("Run", false);
                }
                // 키 입력 시 달리기 애니메이션 재생
                if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow) ||
                    Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
                {
                    anim.SetBool("Run", true);
                }
                else {
                    anim.SetBool("Run", false);
                }
            }
            // 공중에 있을 시
            else if (!controller.isGrounded)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Jump(JumpType.DASH);
                    //cloth.damping = 0.4f;
                }
                gravity = gr;
                moveDir.x = inputAxis;
            }

            //캐릭터 방향 회전
            if (inputAxis < 0 && isFocusRight) { TurnPlayer(); }
            else if (inputAxis > 0 && !isFocusRight) { TurnPlayer(); }
        }

        //if (!isClimb) { moveDir.y -= gravity * Time.deltaTime; }
        //// 벽에 메달릴 시
        //else if (isClimb)
        //{
        //    inputAxis = Input.GetAxis("Vertical");
        //    moveDir = Vector3.up * inputAxis;
        //}

        moveDir.y -= gravity * Time.deltaTime;
        controller.Move(moveDir * (speed) * Time.deltaTime);
    }

    //캐릭터 방향 회전
    void TurnPlayer()
    {
        isFocusRight = !isFocusRight;
        transform.Rotate(new Vector3(0, 1, 0), 180);
        focusRight *= -1f;
        wahleMove.ResetSpeed();
        if (!controller.isGrounded)
        {
            moveDir.x *= -1f;
        }
    }

    // 점프
    void Jump(JumpType curJumpState)
    {
        //cloth.worldAccelerationScale = 1f;
        gravity = 5f;
        switch (curJumpState)
        {
            case JumpType.BASIC:
                anim.SetBool("Jump", true);
                //anim.CrossFade("Basic_Jump", 0f);
                isJumping = true;
                pEffect.StartEffect(PlayerEffectList.BASIC_JUMP);
                moveDir.y = jumpHight;
                break;
            case JumpType.DASH:
                if (isJumping)
                {
                    anim.SetBool("Dash", true);
                    isJumping = false;
                    //pEffect.StartEffect(PlayerEffectList.DASH_JUMP);
                    moveDir.y = jumpHight;
                    //cloth.damping = 1f;
                }
                break;
        }
    }

    ////레이캐스팅 발사
    void ShotRay()
    {
        RaycastHit hit;
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 shotPoint = new Vector3(rayTr.position.x, rayTr.position.y, rayTr.position.z);
        if (Physics.Raycast(shotPoint, forward, out hit, 10f))
        {
            Debug.DrawRay(shotPoint, forward, Color.red, 2f);
            Debug.Log(hit.collider.name);
            ////NPC 체크 및 이름 확인
            //if (hit.collider.CompareTag("NPC"))
            //{
            //    pFunc.ShowScript(hit.collider.name);
            //    anim.SetBool("Run", false);
            //}
        }
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
        if(!hasSuperArmor)
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

    // 로프에서 권한 찾기
    public void GetCtrlAuthorityByRope()
    {
        currRadian = currInteraction.GetComponent<RopeCtrl>().getRadian();
        float speed = currInteraction.GetComponent<RopeCtrl>().getSpeed();
        vx = Mathf.Cos(currRadian * Mathf.Deg2Rad) * (speed * 1.2f);
        vy = Mathf.Sin(currRadian * Mathf.Deg2Rad) * (speed * 0.3f);

        moveDir.y = vy;
        isFlyingByRope = true;
        isJumping = true;
        currInteraction = null;
    }
    public string getCarryItemName()
    {
        return carryItemName;
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

    void OnTriggerStay(Collider coll)
    {
        if (coll.CompareTag("DeadLine"))
        {
            PlayerDie();
        }
        // 퀘스트 아이템 습득
        if (QuestMgr.isQuest)
        {
            if (coll.CompareTag("ITEM"))
            {
                if (QuestMgr.questInfo.targetName == coll.name)
                {
                    QuestMgr.instance.curCompletNum++;
                    coll.gameObject.SetActive(false);
                }
            }
        }

        else if(coll.name == "Switch")
        {
            coll.GetComponent<SwitchObject>().IsCanUseSwitch = true;
            switchState = coll.GetComponent<SwitchObject>();
        }

        // 로프와 충돌
        else if(coll.CompareTag("Rope") && Input.GetKey(KeyCode.UpArrow) && isJumping)
        {
            isCtrlAuthority = false;
            isJumping = false;
            isFlyingByRope = false;
            currInteraction = coll.transform.parent.gameObject;
            currInteraction.GetComponent<RopeCtrl>().
            setAuthority(Convert.ToInt32(coll.name), isFocusRight); // 조작권한을 넘겨줌
        }
        else if (coll.CompareTag("WALL"))
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
        else if (coll.CompareTag("WALL"))
        {
            isClimb = false;
        }
    }

    public void PlayerDie()
    {
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
