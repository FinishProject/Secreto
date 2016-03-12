using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

    float gr = 9.8f;
    float vx, vy;
    float time;

    Vector3 startPos;
    Vector3 destPos;


    void Start()
    {
        startPos = transform.position;
        destPos = new Vector3(startPos.x - 5f, 0, 0);

        vx = (destPos.x - startPos.x) / 2f;
        vy = (destPos.y - startPos.y + 2 * 9.8f) / 2f;
    }

    void Update()
    {
        if (time <= 1.9f)
        {
            time += Time.deltaTime;
            float sx = startPos.x + vx * time;
            float sy = startPos.y + vy * time - 0.5f * gr * time * time;

            transform.position = new Vector3(sx, sy, 0f);
        }
        
    }
}
