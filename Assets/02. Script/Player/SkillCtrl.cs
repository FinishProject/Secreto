using UnityEngine;
using System.Collections;

public class SkillCtrl : MonoBehaviour {

    public Transform shotTr;
    public GameObject bullet;

    private int count = 0;

    public GameObject[] goBullets = new GameObject[3];

    void Start()
    { 
        for (int i = 0; i < goBullets.Length; i++) {
            goBullets[i] = (GameObject)Instantiate(bullet, shotTr.position, Quaternion.identity);
            goBullets[i].SetActive(false);
        }
    }

    void Update()
    {
        //F키 입력 시 공격체 생성
        if (Input.GetKeyDown(KeyCode.F)) {   
            if (count >= goBullets.Length && !goBullets[0].activeSelf) { count = 0; }
            goBullets[count].SetActive(true);
            goBullets[count].SendMessage("GetFocusVector", this.transform.forward.normalized);
            goBullets[count].transform.position = shotTr.position;  
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
                goBullets[count].SendMessage("GetTarget", collider.transform);
                break;
            }
        }
    }
}
