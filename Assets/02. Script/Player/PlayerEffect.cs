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

    //void Start () {

    //    for(int i=0; i<effects.Length; i++)
    //    {
    //        effects[i] = Instantiate(effects[i]);
    //        effects[i].SetActive(false);
    //    }
    //}

    public void StartEffect(PlayerEffectList effectState)
    {
        StartCoroutine(ShowEffected(effectState));
    }


    IEnumerator ShowEffected(PlayerEffectList effectState)
    {
        //Vector3 tempPos = transform.position;
        //tempPos.y += 1.0f;
        //effects[(int)effectState].transform.position = tempPos;
        effects[(int)effectState].SetActive(true);
        yield return new WaitForSeconds(1f);
        effects[(int)effectState].SetActive(false);
    }
}
