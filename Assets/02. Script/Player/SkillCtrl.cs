using UnityEngine;
using System.Collections;

public class SkillCtrl : MonoBehaviour {

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
            if (count >= objBullets.Length && !objBullets[0].activeSelf) { count = 0; }
            objBullets[count].SetActive(true);
            objBullets[count].SendMessage("GetFocusVector", this.transform.forward.normalized);
            objBullets[count].transform.position = shotTr.position;  
            FindTarget();
            count++;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            if (count >= objBullets.Length && !objBullets[0].activeSelf) { count = 0; }
            objBullets[count].GetComponent<LauncherCtrl>().isPowerStrike = true;
            objBullets[count].SetActive(true);           
            objBullets[count].SendMessage("GetFocusVector", this.transform.forward.normalized);
            objBullets[count].transform.position = shotTr.position;
            FindTarget();
            count++;
        }
    }
    //타겟 탐색
    void FindTarget()
    {
        Collider[] hitCollider = Physics.OverlapSphere(this.transform.position, 8f);
        for(int i = 0; i < hitCollider.Length; i++) { 
            if (hitCollider[i].gameObject.tag == "MONSTER") {
                objBullets[count].SendMessage("GetTarget", hitCollider[i].transform);
                break;
            }
        }
    }
}
