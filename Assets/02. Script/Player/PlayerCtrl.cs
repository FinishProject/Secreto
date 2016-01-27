using UnityEngine;
using System.Collections;

public class PlayerCtrl : MonoBehaviour {

    private float gravity = 20.0f; // 중력
    private float pushPower = 2f; // 미는 힘
    private float inputAxis; // 입력 받는 키의 값
    private bool focusRight = true; //우측을 봐라보는 여부
    private float jumpTime = 0f; //스페이스 버튼 누르고 있는 시간
    private bool jumpType = true; // 긴점프와 짧은 점프 종류 
    private bool bScript = false; // 현재 대화중인지 확인

    public float speed = 3.0f; // 이동속도
    public float jumpHight = 6.0f; // 짧은 점프 높이
    public float longJumpHight = 8.0f; //긴 점프 높이

    public Transform rayTr; // 레이캐스트 시작 위치
    public Vector3 moveDir = Vector3.zero; // 이동 벡터
    public CharacterController controller; // 캐릭터컨트롤러
    private Animator anim; // 애니메이터

    Data playerData = new Data();

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        playerData.curPosition = this.transform.position;
        PlayerData.Save(playerData);
    }

    void FixedUpdate()
    {
        //이동
        Movement();
        //NPC와 대화
        if (Input.GetKeyDown(KeyCode.Return)) { ShotRay(); }
        //공중에서 스페이스바 클릭시 오브젝트 오름
        else if(!controller.isGrounded && Input.GetKeyDown(KeyCode.Space)) { ShotRay(); }
        //펫 타기
        else if (Input.GetKeyDown(KeyCode.E)) { RidePet(); }
    }

    void Movement()
    {
        if (controller.isGrounded && !bScript){
            //이동
            inputAxis = Input.GetAxis("Horizontal");
            moveDir = Vector3.forward * inputAxis;
            moveDir = transform.TransformDirection(moveDir);
            //anim.SetBool("Jump", false);
            //점프
            if (Input.GetKeyUp(KeyCode.Space)) { Jump(true); }
            //스페이스바 누르고 있던 시간에 따라 점프 상태 변경
            if (Input.GetKey(KeyCode.Space))
            {
                jumpTime += Time.deltaTime;
                if (jumpTime > 0.5f) { Jump(false); }
            }
        }
        //중력 및 이동, 애니메이션 재생
        moveDir.y -= gravity * Time.deltaTime;
        controller.Move(moveDir * speed * Time.deltaTime);
        anim.SetFloat("Speed", inputAxis);
    } 
    //점프
    void Jump(bool bJump)
    {
        if (bJump) // 짧은 점프
            moveDir.y = jumpHight;
        else // 긴 점프
            moveDir.y = longJumpHight;
        //anim.SetBool("Jump", true); // 점프 애니메이션
        jumpTime = 0f;
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
                Debug.Log("오르기");
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
        Debug.Log("Pet");
    }
}
