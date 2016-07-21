using UnityEngine;
using System.Collections;

public class Test2 : MonoBehaviour
{
    [System.Serializable]
    public struct BackGround
    {
        public GameObject bg_F;
        public GameObject bg_M;
        public GameObject bg_E;
    }
    public BackGround bg1, bg2;

    [System.Serializable]
    public struct BackGround_Speed
    {
        public float bg_F;
        public float bg_M;
        public float bg_E;
    }
    public BackGround_Speed bg_value;

    float CamPosZ;
    Transform camTr;
    void Start()
    {
        camTr = GameObject.FindGameObjectWithTag("MainCamera").transform;
        CamPosZ = camTr.position.z;

    }
    void Update()
    {
        
        MoveBg_X(bg1.bg_F, bg_value.bg_F);
        MoveBg_X(bg2.bg_F, bg_value.bg_F);

        MoveBg_X(bg1.bg_M, bg_value.bg_M);
        MoveBg_X(bg2.bg_M, bg_value.bg_M);

        MoveBg_X(bg1.bg_E, bg_value.bg_E);
        MoveBg_X(bg2.bg_E, bg_value.bg_E);

//        Debug.Log("CamPosZ " + CamPosZ + "    camTr.position.z " + camTr.position.z);
        
        /*
        if (Mathf.Abs(CamPosZ - camTr.position.z) > 0.5f)
        {
            Debug.Log(111111);
            float val = CamPosZ - camTr.position.z;

            MoveBg_Z(bg1.bg_F, val);
            MoveBg_Z(bg2.bg_F, val);

            MoveBg_Z(bg1.bg_M, val);
            MoveBg_Z(bg2.bg_M, val);

            MoveBg_Z(bg1.bg_E, val);
            MoveBg_Z(bg2.bg_E, val);

            CamPosZ -= val;
        }             
        */           
    }

    Vector3 tempPos;
    void MoveBg_X(GameObject gameObj, float moveValue)
    {
        tempPos = gameObj.transform.position;
        tempPos.x -= PlayerCtrl.controller.velocity.x * moveValue;
        gameObj.transform.position = tempPos;
    }

    void MoveBg_Z(GameObject gameObj, float moveValue)
    {
        tempPos = gameObj.transform.localScale;
        tempPos.z += moveValue;
        gameObj.transform.position = tempPos;
    }


}