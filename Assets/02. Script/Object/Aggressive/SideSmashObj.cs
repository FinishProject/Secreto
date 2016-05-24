using UnityEngine;
using System.Collections;

public class SideSmashObj : MonoBehaviour {

    public Transform upWall, downWall;
    public float speed;
    public float length;
    public float waitTime = 1.5f; // 충돌 후 대기 시간
    public bool isRight = true; // 오른쪽에 위치한 오브젝트인지 확인
  
    private bool isSmash = false; // 충돌 했는 지
    private bool isMove = false; // 대기 시간 지난 후 이동 체크
    private Vector3 upOrigin, downOrigin;
    private Vector3 upTarget, downTarget;

    Vector3 center;

    void Start()
    {
        //originPos = this.transform.position;
        //center = (upWall.position - downWall.position);
        //center.y = (upWall.position.y - downWall.position.y) * 0.5f;
        //center = Vector3.Cross(upWall.position, downWall.position);
        //center *= 0.5f;
        if (upWall != null)
        {
            upOrigin = upWall.position;
            upTarget = upWall.position;
            upTarget.y += length;
        }

        downOrigin = downWall.position;
        downTarget = downWall.position;
        downTarget.y -= length;
    }

    void Update()
    {
        if(downWall.position.y <= downTarget.y + 0.2f)
        {
            Debug.Log("11");
            isSmash = true;
        }
        else if(downWall.position.y >= downOrigin.y - 0.2f)
        {
            Debug.Log("22");
            isSmash = false;
        }

        if (!isSmash)
        {
            if (upWall != null)
                upWall.position = Vector3.Lerp(upWall.position, upTarget, 2f * Time.deltaTime);
            downWall.position = Vector3.Lerp(downWall.position, downTarget, 2f * Time.deltaTime);
        }
        else
        {
            if (upWall != null)
                upWall.position = Vector3.Lerp(upWall.position, upOrigin, Time.deltaTime);
            downWall.position = Vector3.Lerp(downWall.position, downOrigin, Time.deltaTime);
        }
    }
    // 충돌 시 이동 중지
    //void OnTriggerEnter(Collider col)
    //{
    //    if (col.CompareTag("SmashObj"))
    //    {
    //        isSmash = true;
    //        StartCoroutine(WaitMove(false));
    //    }
    //}
    // 잠시 대기 후 다시 이동
    IEnumerator WaitMove(bool isType)
    {
        yield return new WaitForSeconds(waitTime);
        if (isType) // 충돌 하기 위해 이동
        {
            isSmash = false;
        }
        else { // 돌아가기 위해 이동
            isMove = true;
        }
        StopCoroutine("WaitMove");
    }
}
