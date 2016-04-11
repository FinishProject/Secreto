using UnityEngine;
using System.Collections;

/*************************   정보   **************************

    센서
    (범위 내에 충돌체크)

    사용방법 :
    
    1. 센서를 달고 싶은 오브젝트에 빈 자식오브젝트를 생성
    2. 빈 자식오브젝트에 이 스크립트를 추가
    3. 부모 스크립트에 Sensorable 인터페이스를 이용해 센서입력을 받자!

    ※ 부모 오브젝트에서 ActiveSensor를 실행시켜,
    반환한 값이 true이면 
*************************************************************/

// 센서를 작동, 작동되면 true, 작동이 안되면 false 반환
public interface Sensorable
{
    bool ActiveSensor(int index);
}

public class Sensor : MonoBehaviour {
    public int index = 0; // 여러개의 센서가 들어가는 부모 오브젝트를 위한 index
    void OnTriggerEnter(Collider col)
    {
        if (col.tag.Equals("Player"))
        {
            if (transform.parent.GetComponent<Sensorable>().ActiveSensor(index))
                gameObject.SetActive(false);
        }
    }
}
