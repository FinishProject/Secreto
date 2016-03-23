using UnityEngine;
using System.Collections;

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

    // Use this for initialization
    void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
