using UnityEngine;
using System.Collections;

public class PlayerCtrl : MonoBehaviour {

    private float pushPower = 2f; // 미는 힘
    private float inputAxis = 0f; // 입력 받는 키의 값
    private bool focusRight = true; //우측을 봐라보는 여부
    private bool bJumping = false; //현재 점프중 확인(대쉬 점프)
    private bool bScript = false; // 현재 대화중 확인
    private bool bMoving = false; // 현재 이동중 확인

    //public float accel = 0.3f;    // 가속도
    //public float nalSpeed = 1.0f; // 기본 이동속도
    //public float maxSpeed = 10.0f; // 최대 이동속도
    //private float curSpeed = 5.0f; // 현재 이동속도

    public float jumpHight = 6.0f; // 기본 점프 높이
    public float dashJumpHight = 6.0f; //대쉬 점프 높이
    public float speed = 10f;

    public Transform rayTr; // 레이캐스트 시작 위치
    public Vector3 moveDir = Vector3.zero; // 이동 벡터
    public CharacterController controller; // 캐릭터컨트롤러
    private Animator anim;

    Data pData = new Data(); // 플레이어 데이터 저장을 위한 클래스 변수

    public static PlayerCtrl instance;

    void Awake()
    {
        instance = this;
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();        
    }

    public void Save()
    {
        pData.pPos = transform.position;
        PlayerData.Save(pData);
    }

    void FixedUpdate()
    {
        //이동
        Movement();
        //NPC와 대화
        if (Input.GetKeyDown(KeyCode.Return)) { ShotRay(); }
        //펫 타기
        else if (Input.GetKeyDown(KeyCode.E)) { RidePet(); }
    }

    void Movement()
    {
        inputAxis = Input.GetAxis("Horizontal");
        //캐릭터 방향 회전
        if (inputAxis < 0 && focusRight) { TurnPlayer(); }
        else if (inputAxis > 0 && !focusRight) { TurnPlayer(); }

        if (controller.isGrounded && !bScript){
            //이동
            moveDir = Vector3.right * inputAxis;
            //anim.SetBool("Jump", false);
            //점프
            if (Input.GetKeyDown(KeyCode.Space)) { Jump(true); }
            anim.SetFloat("Speed", inputAxis);
        }
        else if(!controller.isGrounded)
        {
            moveDir.x = inputAxis * 50f * Time.deltaTime;
            controller.Move(moveDir * Time.deltaTime);
            //대쉬 점프
            if (Input.GetKeyDown(KeyCode.Space)) { Jump(false); }
        }
        //중력 및 이동
        moveDir += Physics.gravity * Time.deltaTime;
        controller.Move(moveDir * speed * Time.deltaTime);
    }

    //캐릭터가 봐라보는 방향 회전
    void TurnPlayer()
    {
        focusRight = !focusRight;
        transform.Rotate(new Vector3(0, 1, 0), 180.0f);
    }

    //점프
    void Jump(bool bJump)
    {
        if (bJump) { // 짧은 점프
            moveDir.y = jumpHight;
            bJumping = true;
        }
        else if (!bJump && bJumping) {// 대쉬 점프
            moveDir.y = dashJumpHight;
            bJumping = false;
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
        if (Input.GetKey(KeyCode.LeftShift)){
            Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
            body.velocity = pushDir * pushPower;
        }
    }

    //레이캐스팅 발사
    void ShotRay()
    {
        RaycastHit hit;
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        if (Physics.Raycast(rayTr.position, forward, out hit, 1f)) { 
            //앞에 오를 수 있는 오브젝트 있을 시
            if(hit.collider.gameObject.tag == "Climb"){
                Debug.Log("Climb");
            }
            //NPC 체크 및 이름 확인
            else if (hit.collider.gameObject.tag == "NPC"){
                string name = hit.collider.gameObject.name;
                ShowScript(name);
            }
        }
    }

    //ScriptMgr에서 NPC이름을 찾아서 대화 생성
    void ShowScript(string name)
    { 
        if (name != null){
            //대화 중이면 true, 캐릭터 정지
            bScript = ScriptMgr.instance.GetScript(name);
            inputAxis = 0f;
        }
    }
    //펫 타기
    void RidePet()
    {
        Debug.Log("Riding Pet");
    }

    //IEnumerator Acceleration()
    //{
        
    //    while (true){
    //        if (nalSpeed < maxSpeed && bMoving) {
    //            nalSpeed += accel;
    //            curSpeed = nalSpeed;
    //        }
    //        yield return new WaitForSeconds(0.05f);
    //    }
    //}
}
