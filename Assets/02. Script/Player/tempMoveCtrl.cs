using UnityEngine;
using System.Collections;



public class tempMoveCtrl : MonoBehaviour
{
    public float inputAxis = 0f;     // 입력 받는 키의 값
    public bool isFocusRight = true; // 우측을 봐라보는 여부
    public bool isJumping = false;   // 현재 점프중인지 확인
    public bool lockJumping = false;    // 점프 잠금

    private float gravity = -5f;

    public float jumpHight = 3.0f;     // 기본 점프 높이
    public float dashJumpHight = 4.0f; // 대쉬 점프 높이
    public float speed = 10f;          // 이동 속도
    public float moveResistant = 0f;   // 이동 저항력

    public Vector3 moveDir = Vector3.zero; // 이동 벡터
    public CharacterController controller; // 캐릭터컨트롤러
    public JumpType jumpState = JumpType.IDLE; // 점프 타입

    private Animator anim;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        LengthOfRay = GetComponent<Collider>().bounds.extents.y;
        Debug.Log(LengthOfRay);
    }

    void Update()
    {
        // 점프
        if (Input.GetKeyDown(KeyCode.Space) && controller.isGrounded)       { Debug.Log("입력"); SetJumpState(JumpType.BASIC); }
        else if (Input.GetKeyDown(KeyCode.Space) && !controller.isGrounded) { SetJumpState(JumpType.DASH);  }

        // 걷기 애니메이션이 아직 없어 임시의 이동 애니메이션 재생을 위한 것
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
        {
            anim.SetBool("Run", true);
        }
        else
        {
            anim.SetBool("Run", false);
        }

    }

    void FixedUpdate()
    {
        
        Movement();
        Jump();
        controller.Move(moveDir * (speed - moveResistant) * Time.fixedDeltaTime);
    }

    // 좌우 이동
    void Movement()
    {
        // 키 입력
        inputAxis = Input.GetAxis("Horizontal");

        // 지상에 있을 시
        if (controller.isGrounded)
        {
            //이동
            moveDir = Vector3.right * inputAxis;

            anim.SetFloat("Speed", inputAxis);
            //애니메이션 임시 좌측 변수
            if (!isFocusRight)
                anim.SetFloat("Speed", inputAxis * -1f);

        }
        // 공중에 있을 시
        else if (!controller.isGrounded)
        {
            moveDir.x = inputAxis * 50f * Time.deltaTime;
        }

        //캐릭터 방향 회전
        if (inputAxis < 0 && isFocusRight) { TurnPlayer(); }
        else if (inputAxis > 0 && !isFocusRight) { TurnPlayer(); }

        
    }

    

    //캐릭터가 봐라보는 방향 회전
    void TurnPlayer()
    {
        isFocusRight = !isFocusRight;
        Vector3 scale = transform.localScale;
        scale.z *= -1f;
        transform.localScale = scale;
    }

    
    void SetJumpState(JumpType jumpType)
    {
        
        switch (jumpType)
        {
            case JumpType.BASIC:
                if (!isJumping)
                {
                    Debug.Log("점프");
                    moveDir.y = jumpHight;
                    controller.Move(moveDir * (speed - moveResistant) * Time.deltaTime);
                    anim.SetBool("Jump", true);
                    isJumping = true;
                }
                break;
            case JumpType.DASH:
                if (isJumping && !lockJumping)
                {
                    moveDir.y = dashJumpHight;
                    controller.Move(moveDir * (speed - moveResistant) * Time.deltaTime);
                    lockJumping = true;
                }
                break;
        }
        jumpState = jumpType;
    }

    // 점프
    void Jump()
    {
        if (controller.isGrounded && jumpState != JumpType.IDLE && isJumping)
        {
            Debug.Log("바닥");
            anim.SetBool("Jump", false);
            isJumping = false;
            lockJumping = false;
            jumpState = JumpType.IDLE;
            
            gravity = -20f;
        }
        else if(isJumping)
        {
            gravity = -6f;
        }

        moveDir.y += gravity * Time.fixedDeltaTime;

//        controller.Move(moveDir * (speed - moveResistant) * Time.deltaTime);
    }

    RaycastHit HitInfo;
    float LengthOfRay;
    Ray ray;

    /*
    bool isGround()
    {
        ray = new Ray(transform.position, Vector3.down);
        Debug.DrawRay(transform.position, Vector3.down, Color.yellow);
        if (Physics.Raycast(ray, out HitInfo, LengthOfRay))
        {
            if(HitInfo.collider.tag.Equals("Ground"))
            {
                Debug.Log("레이 : 땅");
                return true;               
            }
        }
        return false;
    }
    */
    /*
    bool isGround()
    {
        Debug.DrawRay(transform.position, Vector3.down * 1f, Color.red);
        if (Physics.Raycast(transform.position, Vector3.down, out HitInfo, 1f))
        {
            return true;
        }
        return false;
    }
    */

}




/*

public class tempMoveCtrl : MonoBehaviour
{
    public float inputAxis = 0f;     // 입력 받는 키의 값
    public bool isFocusRight = true; // 우측을 봐라보는 여부
    public bool isJumping = false;   // 현재 점프중인지 확인
    public bool lockJumping = false;    // 점프 잠금

    private float gravity = -5f;

    public float jumpHight = 3.0f;     // 기본 점프 높이
    public float dashJumpHight = 4.0f; // 대쉬 점프 높이
    public float speed = 10f;          // 이동 속도
    public float moveResistant = 0f;   // 이동 저항력

    public Vector3 moveDir = Vector3.zero; // 이동 벡터
    public CharacterController controller; // 캐릭터컨트롤러
    public JumpType jumpState = JumpType.IDLE; // 점프 타입

    private Animator anim;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }


    void Update()
    {
        // 점프
        if (Input.GetKeyDown(KeyCode.Space) && controller.isGrounded)       { isJumping = true; SetJumpState(JumpType.BASIC); }
        else if (Input.GetKeyDown(KeyCode.Space) && !controller.isGrounded) { isJumping = true; SetJumpState(JumpType.DASH);  }

        // 걷기 애니메이션이 아직 없어 임시의 이동 애니메이션 재생을 위한 것
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
        {
            anim.SetBool("Run", true);
        }
        else
        {
            anim.SetBool("Run", false);
        }

    }

    void FixedUpdate()
    {
        Jump();
        Movement();
        
    }

    void Movement()
    {
        // 키 입력
        inputAxis = Input.GetAxis("Horizontal");
    
        // 지상에 있을 시
        if (controller.isGrounded )
        {
            //이동
            moveDir = Vector3.right * inputAxis;
            
            anim.SetFloat("Speed", inputAxis);
            //애니메이션 임시 좌측 변수
            if (!isFocusRight)
                anim.SetFloat("Speed", inputAxis * -1f);

        }
        // 공중에 있을 시
        else if (!controller.isGrounded)
        {
            moveDir.x = inputAxis * 50f * Time.deltaTime;
        }

        //캐릭터 방향 회전
        if (inputAxis < 0 && isFocusRight) { TurnPlayer(); }
        else if (inputAxis > 0 && !isFocusRight) { TurnPlayer(); }

        controller.Move(moveDir * (speed - moveResistant) * Time.deltaTime);
    }

    //캐릭터가 봐라보는 방향 회전
    void TurnPlayer()
    {
        isFocusRight = !isFocusRight;
        Vector3 scale = transform.localScale;
        scale.z *= -1f;
        transform.localScale = scale;
    }

    
    void SetJumpState(JumpType jumpType)
    {
        anim.SetBool("Jump", true);
        jumpState = jumpType;
        switch (jumpState)
        {
            case JumpType.BASIC:
                if (isJumping)
                {
                    moveDir.y = jumpHight;   
                }
                break;
            case JumpType.DASH:
                if (isJumping && !lockJumping)
                {
                    moveDir.y = dashJumpHight;
                    lockJumping = true;
                }
                break;
        } 
    }

    // 점프
    void Jump()
    {

        if (controller.isGrounded)
        {
            isJumping = false;
            lockJumping = false;
            jumpState = JumpType.IDLE;
            anim.SetBool("Jump", false);

            gravity = -20f;
        }
        else
        {
            gravity = -5f;
        }
        moveDir.y += gravity * Time.deltaTime;
//        controller.Move(moveDir * (speed - moveResistant) * Time.deltaTime);

    }


}
*/


/*
public class tempMoveCtrl : MonoBehaviour {
    public static float inputAxis = 0f;     // 입력 받는 키의 값
    public static bool isFocusRight = true; // 우측을 봐라보는 여부
    public static bool isMove = true;       // 현재 이동 여부
    public static bool isJumping = false;   // 현재 점프중인지 확인

    private float gravity = 5f;

    public float jumpHight = 3.0f;     // 기본 점프 높이
    public float dashJumpHight = 4.0f; // 대쉬 점프 높이
    public float speed = 10f;          // 이동 속도
    public float moveResistant = 0f;   // 이동 저항력

    public Vector3 moveDir = Vector3.zero; // 이동 벡터
    public CharacterController controller; // 캐릭터컨트롤러
    public JumpType jumpState = JumpType.IDLE; // 점프 타입

    private Animator anim;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }


    void Update()
    {
        // 점프
        if (Input.GetKeyDown(KeyCode.Space) && controller.isGrounded)
        {
            //rb.AddForce(Vector3.up * 10f * Time.deltaTime);
            Jump(JumpType.BASIC);
        }
        else if (Input.GetKeyDown(KeyCode.Space) && !controller.isGrounded) { Jump(JumpType.DASH); }

        // 걷기 애니메이션이 아직 없어 임시의 이동 애니메이션 재생을 위한 것
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
        {
            anim.SetBool("Run", true);
        }
        else
        {
            anim.SetBool("Run", false);
        }

    }

    void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        // 키 입력
        inputAxis = Input.GetAxis("Horizontal");
        // 지상에 있을 시
        if (controller.isGrounded && isMove)
        {
            isJumping = false;
            gravity = 20f;
            //이동
            moveDir = Vector3.right * inputAxis;
            anim.SetBool("Jump", false);
            anim.SetFloat("Speed", inputAxis);
            //애니메이션 임시 좌측 변수
            if (!isFocusRight)
                anim.SetFloat("Speed", inputAxis * -1f);

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

        moveDir.y -= gravity * Time.deltaTime;


        controller.Move(moveDir * (speed - moveResistant) * Time.deltaTime);
    }

    //캐릭터가 봐라보는 방향 회전
    void TurnPlayer()
    {
        isFocusRight = !isFocusRight;
        Vector3 scale = transform.localScale;
        scale.z *= -1f;
        transform.localScale = scale;
        //transform.Rotate(new Vector3(0, 1, 0), 180.0f);
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
                    moveDir.y = jumpHight;
                    controller.Move(moveDir * (3f - moveResistant) * Time.deltaTime);

                }
                break;
            case JumpType.DASH:
                if (isJumping)
                {
                    moveDir.y = dashJumpHight;
                    controller.Move(moveDir * (3f - moveResistant) * Time.deltaTime);
                    isJumping = false;
                }
                break;
        }

    }


}
*/
