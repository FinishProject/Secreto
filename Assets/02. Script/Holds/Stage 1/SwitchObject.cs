using UnityEngine;
using System.Collections;

/*************************   정보   **************************

    스위치

    사용방법 :
    
    1. 
*************************************************************/

public class SwitchObject : MonoBehaviour {
    private bool isSwitchOn;
    public bool IsSwitchOn
    {
        get { return isSwitchOn; }
        set
        {
            if (IsCanUseSwitch)
                isSwitchOn = value;
        }
        
    }
    public bool IsCanUseSwitch { set; get; }
}
