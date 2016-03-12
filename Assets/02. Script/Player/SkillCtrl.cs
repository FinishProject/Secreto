using UnityEngine;
using System.Collections;

public class SkillCtrl : MonoBehaviour {

    private Transform targetTr;

    public Transform shotTr;
    public GameObject bullet;
 
    private int count = 0;

    public GameObject[] goBullets = new GameObject[3];

    void Start()
    {
        for (int i = 0; i < goBullets.Length; i++)
        {
            goBullets[i] = (GameObject)Instantiate(bullet, shotTr.position, Quaternion.identity);
            goBullets[i].SetActive(false);
        }
    }

    void Update()
    {
        //F키 입력 시 공격체 생성
        if (Input.GetKeyDown(KeyCode.F)) {
            FindTarget();

            if (count >= goBullets.Length && !goBullets[0].activeSelf) { count = 0; }
            goBullets[count].SetActive(true);
            goBullets[count].transform.position = shotTr.position;
            count++;
        }

    }

    void FindTarget()
    {
        Collider[] hitCollider = Physics.OverlapSphere(this.transform.position, 10f);
        foreach (Collider collider in hitCollider)
        {
            if (collider.gameObject.tag == "MONSTER")
            {
                LauncherCtrl.GetTarget(collider.transform);
                break;
            }
        }
    }
}
