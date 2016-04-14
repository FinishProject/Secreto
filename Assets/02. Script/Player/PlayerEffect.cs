using UnityEngine;
using System.Collections;

/*************************   정보   **************************

    플레이어의 이펙트를 관리하는 스크립트

    사용방법 :
    
    1. 플레이어에 스크립트를 추가
    2. 각 스킬 프리펩을 연결해주자

*************************************************************/

public class PlayerEffect : MonoBehaviour {

    public GameObject Effect1;
    public GameObject Effect2;

    // Use this for initialization
    void Start () {
        Effect1 = Instantiate(Effect1);
        Effect1.SetActive(false);

        Effect2 = Instantiate(Effect2);
        Effect2.SetActive(false);
    }

    public void StartEffect(PlayerEffectList EffectState)
    {
        switch(EffectState)
        {
            case PlayerEffectList.BASIC_JUMP : StartCoroutine(Effected());   break;
            case PlayerEffectList.DASH_JUMP  : StartCoroutine(Effected_2()); break;
        }
    }

    IEnumerator Effected_2()
    {
        Vector3 tempPos = gameObject.transform.position;
        tempPos.y += 1.0f;
        Effect2.transform.position = tempPos;
        Effect2.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        Effect2.SetActive(false);

    }
    IEnumerator Effected()
    {
        Vector3 tempPos = gameObject.transform.position;
        tempPos.y += 1.0f;
        Effect1.transform.position = tempPos;
        Effect1.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        Effect1.SetActive(false);
    }
}
