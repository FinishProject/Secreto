using UnityEngine;
using System.Collections;

public class MonsterMgr : MonoBehaviour {

    private enum State { idle, trace };

    private float posX = 1f;

    public GameObject gaObj;
    public float traceDist = 10f;

    private GameObject[] childMon = new GameObject[2];
    private Transform playerTr;
    private NavMeshAgent nvAgent;

    private State state = State.idle;

    Vector3 relativePos;

    void Start()
    {
        playerTr = GameObject.FindGameObjectWithTag("Player").transform;
        nvAgent = GetComponent<NavMeshAgent>();

        for (int i = 0; i < childMon.Length; i++)
        {
            childMon[i] = (GameObject)Instantiate(gaObj, transform.position, Quaternion.identity);
            childMon[i].SetActive(false);
        }
        StartCoroutine("FindTarget");
        //StartCoroutine("MoveMonster");
    }
    //몬스터 이동
    IEnumerator MoveMonster()
    {
        while (true)
        {
            switch (state)
            {
                case State.idle:
                    nvAgent.Stop();
                    break;
                case State.trace:
                    nvAgent.destination = playerTr.position;
                    nvAgent.Resume();
                    break;
            }
            yield return null;
        }
    }
    //타겟 탐색
    IEnumerator FindTarget()
    {
        while (true)
        {
            float distance = Vector3.Distance(playerTr.position, transform.position);

            if(distance <= traceDist) { state = State.trace; }
            else { state = State.idle; }

            yield return new WaitForSeconds(1f);
        }
    }

    void OnCollisionEnter(Collision coll)
    {
        Split();

        this.gameObject.transform.position = new Vector3(0, 100f, 0);
        //this.gameObject.SetActive(false);
    }

    void Split()
    {
        for (int i = 0; i < 2; i++)
        {
            childMon[i].SetActive(true);
            posX *= -1f;
            Vector3 posVec = new Vector3(transform.position.x + posX, transform.position.y,
                transform.position.z);
            childMon[i].transform.position = posVec;
        }
        StartCoroutine("Parabola");
    }

    IEnumerator Parabola()
    {
        float focus = 1f;
        float time = 0f;
        float vx, vy;
        float gr = 9.8f;

        Vector3 startPos = transform.position;
        Vector3 finishPos = new Vector3(startPos.x - 5f, 0, 0);

        vx = (finishPos.x - startPos.x) / 2f;
        vy = (finishPos.y - startPos.y + 2f * gr) / 2f;

        while (time <= 1.9f)
        {
            time += Time.deltaTime;
            for (int i = 0; i < childMon.Length; i++)
            {
                focus *= -1f;
                float sx = startPos.x + vx * time * focus;
                float sy = startPos.y + vy * time - 0.5f * gr * time * time;
                childMon[i].transform.position = new Vector3(sx, sy, 0f);
            }
            yield return null;
        }
    }

    //void Split()
    //{
    //    Collider[] colls = Physics.OverlapSphere(transform.position, 5f);

    //    foreach (Collider coll in colls)
    //    {
    //        Rigidbody rbody = coll.GetComponent<Rigidbody>();
    //        if(rbody != null)
    //        {
    //            rbody.mass = 1.0f;
    //            rbody.AddExplosionForce(500.0f, transform.position, 5.0f, 20.0f);
    //        }
    //    }
    //}


}
