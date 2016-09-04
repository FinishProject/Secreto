using UnityEngine;
using System.Collections;
using System;

public enum JumpType
{
    IDLE, FLY_IDLE, BASIC, DASH, LEAP, POWER,
}
public enum PlayerEffectList
{
    DIE, BASIC_JUMP, DASH_JUMP,
}

public class PlayerCtrl : MonoBehaviour
{
    public static JumpType jumpState = JumpType.IDLE; // 점프 타입

    // 플레이어 이동 변수
    public float moveSpeed = 10f; // 이동 속도
    public float basicJumpHight = 3.0f; // 기본 점프 높이
    public float dashJumpHight = 4.0f; // 대쉬 점프 높이
    public float dropGravity = 5f; // 공중에 있을 때의 중력값
    private float curGravity; // 현재 중력값

    public static float inputAxis = 0f;     // 입력 받는 키의 값
    public static bool isFocusRight = true; // 우측을 봐라보는 여부
    private float amorTime = 0.5f;

    [System.NonSerialized]
    public float moveResistant = 0f;   // 이동 저항력
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

    public Transform rayTr; // 레이캐스트 시작 위치
    public static Vector3 moveDir = Vector3.zero; // 이동 벡터
    public static CharacterController controller; // 캐릭터컨트롤러
    private SwitchObject switchState;
    private GameObject currInteraction;
    private Animator anim;

    public Cloth cloth;
    public GameObject lunaModel;
    //private Data pData = new Data(); // 플레이어 데이터 저장을 위한 클래스 변수
    private PlayerEffect pEffect;
    private WahleMove wahleMove;

    public static PlayerCtrl instance;

    void Awake()
    {
        instance = this;
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        pEffect = GetComponent<PlayerEffect>();
        wahleMove = GameObject.FindGameObjectWithTag("WAHLE").GetComponent<WahleMove>();

        // 상호작용을 하기 위한 스위치
        switchState = gameObject.AddComponent<SwitchObject>();
        switchState.IsCanUseSwitch = false;
    }

    void Start()
    {
        //GetPlayerData();
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
        //cloth.damping = 0.6f;
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
            curGravity = 50f;
            anim.SetBool("Jump", false);
            anim.SetBool("Dash", false);
            anim.SetBool("Idle", false);

            //이동
            moveDir = Vector3.right * inputAxis;

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
            else
            {
                anim.SetBool("Run", false);
            }
        }
        // 공중에 있을 시
        else if (!controller.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump(JumpType.DASH);
            }
            else
            {
                curGravity = dropGravity;
                moveDir.x = inputAxis;
            }
        }

        moveDir.y -= curGravity * Time.deltaTime;
        controller.Move(moveDir * moveSpeed * Time.deltaTime);
    }

    //캐릭터 방향 회전
    void TurnPlayer()
    {
        isFocusRight = !isFocusRight;
        //transform.Rotate(new Vector3(0, 1, 0), 180);
        //focusRight *= -1f;
        //cloth.damping = 1f;

        Vector3 localScale = transform.localScale;
        localScale.z *= -1f;
        transform.localScale = localScale;

        wahleMove.ResetSpeed();
        if (!controller.isGrounded) { moveDir.x *= -1f; }
    }

    // 점프
    void Jump(JumpType curJumpState)
    {
        //cloth.worldAccelerationScale = 1f;
        curGravity = dropGravity;
        switch (curJumpState)
        {
            case JumpType.BASIC:
                anim.SetBool("Jump", true);
                isJumping = true;
                pEffect.StartEffect(PlayerEffectList.BASIC_JUMP);
                moveDir.y = basicJumpHight;
                break;
            case JumpType.DASH:
                if (isJumping)
                {
                    anim.SetBool("Dash", true);
                    isJumping = false;
                    //pEffect.StartEffect(PlayerEffectList.DASH_JUMP);
                    moveDir.y = dashJumpHight;
                }
                break;
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
        if (!hasSuperArmor)
        {
            StartCoroutine(SuperArmor());
            curHp -= damage;
            InGameUI.instance.ChangeHpBar();
            anim.SetTrigger("Hit");
            if (curHp <= 0)
            {
                PlayerDie(false);
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
            PlayerDie(true);
        }

        else if(coll.CompareTag("Finish"))
        {
            InGameUI_2.instance.GameEnd();
        }
    }

    void OnTriggerExit(Collider coll)
    {
        if (coll.name == "Switch")
        {
            switchState = gameObject.AddComponent<SwitchObject>();
        }
    }

    public void PlayerDie(bool isFall)
    {
        if (isFall)
        {
            GetPlayerData();
        }
        else if (!isFall)
            StartCoroutine(ResetPlayer());
    }

    IEnumerator ResetPlayer()
    {
        isMove = false;
        lunaModel.SetActive(false);
        pEffect.StartEffect(PlayerEffectList.DIE);

        yield return new WaitForSeconds(1.3f);

        GetPlayerData();
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
