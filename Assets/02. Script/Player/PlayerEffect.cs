using UnityEngine;
using System.Collections;

/*************************   정보   **************************

    플레이어의 이펙트를 관리하는 스크립트

    사용방법 :
    
    1. 플레이어에 스크립트를 추가
    2. 각 스킬 프리펩을 연결해주자

*************************************************************/

public class PlayerEffect : MonoBehaviour {

    private int jumpEffectCount = -1;

    public GameObject[] effects;
    private Transform playerTr;
    public GameObject[] jumpEffect;

    void Start()
    {
        playerTr = GetComponent<PlayerCtrl>().transform;

        for(int i=0; i < 4; i++)
        {
            jumpEffect[i] = (GameObject)Instantiate(effects[1], transform.position, Quaternion.identity);
            jumpEffect[i].SetActive(false);
        }
    }

    public void StartEffect(PlayerEffectList effectState)
    {
        StartCoroutine(ShowEffected(effectState));
    }

    IEnumerator ShowEffected(PlayerEffectList effectState)
    {
        Vector3 playerVec = this.transform.position;
        switch (effectState)
        {
            case PlayerEffectList.DIE:
                playerVec.y += 4f;
                effects[(int)effectState].transform.position = playerVec;

                effects[(int)effectState].SetActive(true);
                yield return new WaitForSeconds(1f);
                effects[(int)effectState].SetActive(false);
                break;
            case PlayerEffectList.BASIC_JUMP:

                jumpEffectCount++;
                if (jumpEffectCount >= jumpEffect.Length)
                {
                    jumpEffectCount = 0;
                }

                playerVec.y += 1f;
                jumpEffect[jumpEffectCount].SetActive(true);
                jumpEffect[jumpEffectCount].transform.position = playerVec;

                yield return new WaitForSeconds(3f);
                
                jumpEffect[jumpEffectCount].SetActive(false);
                break;
        }

        
    }
}
