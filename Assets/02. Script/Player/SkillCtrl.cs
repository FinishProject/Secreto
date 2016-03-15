using UnityEngine;
using System.Collections;

public class SkillCtrl : MonoBehaviour {

    public Transform shotTr;
    public GameObject bullet;

    private int count = 0;

    private GameObject[] bulletObjs = new GameObject[3];

    void Start()
    {
        for (int i = 0; i < bulletObjs.Length; i++) {
            bulletObjs[i] = (GameObject)Instantiate(bullet, shotTr.position, Quaternion.identity);
            bulletObjs[i].SetActive(false);
        }
    }

    void Update()
    {
        //F키 입력 시 공격체 생성
        if (Input.GetKeyDown(KeyCode.F)) {   
            if (count >= bulletObjs.Length && !bulletObjs[0].activeSelf) { count = 0; }
            bulletObjs[count].SetActive(true);
            bulletObjs[count].transform.position = shotTr.position;
            bulletObjs[count].SendMessage("GetFocusVector", this.transform.forward.normalized);
            FindTarget();
            count++;
        }
    }
    //타겟 탐색
    void FindTarget()
    {
        Collider[] hitCollider = Physics.OverlapSphere(this.transform.position, 10f);
        foreach (Collider collider in hitCollider) {
            if (collider.gameObject.tag == "MONSTER") {
                bulletObjs[count].SendMessage("GetTarget", collider.transform);
                break;
            }
        }
    }
}
