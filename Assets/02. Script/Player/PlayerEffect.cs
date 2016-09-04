using UnityEngine;
using System.Collections;

/*************************   정보   **************************

    플레이어의 이펙트를 관리하는 스크립트

    사용방법 :
    
    1. 플레이어에 스크립트를 추가
    2. 각 스킬 프리펩을 연결해주자

*************************************************************/

public class PlayerEffect : MonoBehaviour {

    public GameObject[] effects;

    private Transform playerTr;

    void Start()
    {
        playerTr = GetComponent<PlayerCtrl>().transform;
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
                break;
            case PlayerEffectList.BASIC_JUMP:
                playerVec.y += 1f;
                effects[(int)effectState].transform.position = playerVec;

                if (effects[(int)effectState].activeSelf)
                    effects[(int)effectState].SetActive(false);
                break;
        }

        effects[(int)effectState].SetActive(true);
        yield return new WaitForSeconds(1f);
        effects[(int)effectState].SetActive(false);
    }
}
