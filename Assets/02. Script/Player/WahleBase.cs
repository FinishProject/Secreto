using UnityEngine;
using System.Collections;

public class WahleBase : MonoBehaviour {

    public float maxSpeed = 10f; // 최대 속도
    public float accel = 5f; // 가속도

    protected float initSpeed = 0f; // 초기 속도
    protected float distance = 0f; // 거리 차
    private bool isSearch = false; // 탐색 여부

    protected Transform targetPoint; // 대기 상태시 추격할 포인트 위치
    protected Vector3 relativePos; // 상대적 위치값
    protected Quaternion lookRot; // 봐라볼 방향

    public Transform playerTr;
    private Transform camTr;
    protected Animator anim;

    void Start()
    {
        anim = gameObject.GetComponentInChildren<Animator>();
        camTr = GameObject.FindGameObjectWithTag("MainCamera").transform;

        targetPoint = new GameObject().transform;
        targetPoint.name = "TargetPoint";
    }

    // 이동 속도 증가
    public float IncrementSpeed(float initSpeed, float maxSpeed, float accel)
    {
        if (initSpeed.Equals(maxSpeed))
            return initSpeed;
        else {
            initSpeed += accel * Time.deltaTime;
            // 기본 속도가 최고 속도를 넘을 시 음수가 되어 maxSpeed만 반환
            return (Mathf.Sign(maxSpeed - initSpeed).Equals(1)) ? initSpeed : maxSpeed;
        }
    }
    // 대기 상태 시 랜덤으로 포인트 위치 이동
    protected virtual Vector3 SetRandomPos()
    {
        float rndPointX = Random.Range(camTr.position.x - 5f, camTr.position.x + 5f);
        float rndPointY = Random.Range(playerTr.position.y - 1f, camTr.position.y + 2f);

        return new Vector3(rndPointX, rndPointY, playerTr.position.z);
    }

    // 레이캐스트 발사
    protected virtual void ShotRay()
    {
        // 양측 레이 발사 위치
        Vector3 rightRayPos = transform.position + (transform.right * 0.3f);
        Vector3 leftRayPos = transform.position - (transform.right * 0.3f);

        RaycastHit hit;
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        // 우측 레이캐스트
        if (Physics.Raycast(rightRayPos, forward, out hit, 3f) || Physics.Raycast(leftRayPos, forward, out hit, 3f))
        {
            // 플레이어, 벽, 땅이 있을 시 우회
            if (hit.collider.CompareTag("WALL") || hit.collider.CompareTag("Ground") || hit.collider.CompareTag("Player"))
            {
                relativePos += hit.normal * 50f;
            }
        }
    }
    // 카메라 밖으로 나갔는지 확인
    protected virtual bool CheckOutCamera()
    {
        Vector3 camVec = Camera.main.WorldToScreenPoint(transform.position);
        // 화면 밖으로 나갔을 시 true (오른쪽 || 왼쪽) 
        if (camVec.x >= Camera.main.pixelWidth || camVec.x <= 20f)
            return true;
        else
            return false;
    }

    // 랜덤 값 생성
    protected virtual int GetRandomValue(float[] values)
    {
        float total = 0f;
        // 전체 합을 구함
        for (int i = 0; i < values.Length; i++)
        {
            total += values[i];
        }
        // 전체 합의 임이의 0~1의 변수를 곱함
        float randomPoint = Random.value * total;

        for (int i = 0; i < values.Length; i++)
        {
            if (randomPoint < values[i])
            {
                return i;
            }
            else {
                randomPoint -= values[i];
            }
        }
        return values.Length - 1;
    }

}
