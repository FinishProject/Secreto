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

    void Start () {

        for(int i=0; i<effects.Length; i++)
        {
            effects[i] = Instantiate(effects[i]);
            effects[i].SetActive(false);
        }
        //effect2 = Instantiate(effect2);
        //effect2.SetActive(false);
    }

    public void StartEffect(PlayerEffectList effectState)
    {
        StartCoroutine(ShowEffected(effectState));
        //switch(effectState)
        //{
            //case PlayerEffectList.BASIC_JUMP : StartCoroutine(effected());   break;
            //case PlayerEffectList.DASH_JUMP  : StartCoroutine(effected_2()); break;
        //}
    }

    IEnumerator ShowEffected(PlayerEffectList effectState)
    {
        Vector3 tempPos = transform.position;
        tempPos.y += 1.0f;
        effects[(int)effectState].transform.position = tempPos;
        effects[(int)effectState].SetActive(true);
        yield return new WaitForSeconds(0.5f);
        effects[(int)effectState].SetActive(false);
    }

    //IEnumerator effected_2()
    //{
    //    Vector3 tempPos = gameObject.transform.position;
    //    tempPos.y += 1.0f;
    //    effect2.transform.position = tempPos;
    //    effect2.SetActive(true);
    //    yield return new WaitForSeconds(0.5f);
    //    effect2.SetActive(false);

    //}
    //IEnumerator effected()
    //{
    //    Vector3 tempPos = gameObject.transform.position;
    //    tempPos.y += 1.0f;
    //    effect1.transform.position = tempPos;
    //    effect1.SetActive(true);
    //    yield return new WaitForSeconds(0.5f);
    //    effect1.SetActive(false);
    //}
}
