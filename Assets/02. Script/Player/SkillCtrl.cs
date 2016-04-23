using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillCtrl : MonoBehaviour {

    public Transform playerTr;
    public Transform shotTr;
    public GameObject bullet;
    
    private int count = 0;
    private GameObject[] objBullets = new GameObject[3];

    void Start()
    { 
        for (int i = 0; i < objBullets.Length; i++) {
            objBullets[i] = (GameObject)Instantiate(bullet, shotTr.position, Quaternion.identity);
            objBullets[i].SetActive(false);
        }
    }

    void Update()
    {
        //F키 입력 시 공격체 생성
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.A)) {
            if (count >= objBullets.Length) {
                if (!objBullets[0].activeSelf) count = 0;
            }
            else {
                objBullets[count].SetActive(true);
                objBullets[count].SendMessage("GetFocusVector", shotTr.right);
                objBullets[count].transform.position = shotTr.position;
                FindTarget();
                count++;
            }
        }
    }
    //타겟 탐색
    void FindTarget()
    {
        Collider[] hitCollider = Physics.OverlapSphere(playerTr.position, 8f);
        List<Transform> targetList = new List<Transform>();
        
        for(int i = 0; i < hitCollider.Length; i++) { 
            if (hitCollider[i].CompareTag("MONSTER")) {
                targetList.Add(hitCollider[i].transform);
            }
        }
        if(targetList.Count > 0)
            DistanceCompare(targetList);
    }
    // 가장 가까운 타겟의 위치를 찾아 타겟의 위치값을 넘겨줌
    void DistanceCompare(List<Transform> _target)
    {
        float nearDistance = (playerTr.position - _target[0].position).sqrMagnitude;
        int targetIndex = 0;
        // 각 타겟의 거리를 비교하여 가장 가까운 타겟을 찾음
        for (int i = 1; i < _target.Count; i++)
        {
            float curDistance = (playerTr.position - _target[i].position).sqrMagnitude;
            if (nearDistance > curDistance)
            {
                nearDistance = curDistance;
                targetIndex = i;
            }
        }
        // 현재 발사체에게 타겟 포지션을 알려줌
        objBullets[count].SendMessage("GetTarget", _target[targetIndex]);
    }
}
