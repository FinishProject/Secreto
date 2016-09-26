using UnityEngine;
using System.Collections;

public class AltarCtrl : MonoBehaviour {

    public Renderer[] render = new Renderer[3];
    private Color offColor;
    private Color originColor;
    private Color drawColor;

    private bool isDraw = false;
    private bool isClear = false;
    private bool isOnBox = false;

    Color targetColor = new Color(0f, 0.8117652f, 1.5f);

    public GameObject altarEffect;
    public Transform stepHold;

    private Vector3 originPos, finishPos;

    public float length = 0.5f;
    public float speed = 1f;

    public GameObject[] hold = new GameObject[4];
    public Renderer[] holdRender = new Renderer[4];

    void Start()
    {
        //originColor = new Color(0f, 0.8117652f, 1.5f);
        originColor = new Color(0.322f, 0.322f, 0.322f);
        for (int i = 0; i < render.Length; i++)
        {
            render[i].material.SetColor("_EmissionColor", originColor);
        }

        originPos = stepHold.position;
        finishPos = originPos;
        finishPos.y -= length;

        //StartCoroutine(ClearColor());
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.collider.CompareTag("OBJECT"))
        {
            isDraw = true;
            isClear = false;
            StartCoroutine(DrawColor());
            altarEffect.SetActive(true);
            isOnBox = true;

            if (hold[0] != null)
                StartCoroutine(SetOnObject());
        }
    }

  
    void OnCollisionExit(Collision col)
    {
        if (col.collider.CompareTag("OBJECT") && isOnBox)
        {
            isDraw = false;
            isClear = true;
            altarEffect.SetActive(false);
            StartCoroutine(ClearColor());
            isOnBox = false;
            if(hold[0] != null)
                StartCoroutine(SetOffObject());
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            isDraw = true;
            isClear = false;
            StartCoroutine(DrawColor());
        }

    }


    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player") && !isOnBox)
        {
            isDraw = false;
            isClear = true;
            StartCoroutine(ClearColor());
        }
    }

    IEnumerator DrawColor()
    {
        drawColor = render[0].material.GetColor("_EmissionColor");
        while (isDraw)
        {
            if (drawColor.r >= targetColor.r)
                drawColor.r -= 0.05f;
            if (drawColor.g <= targetColor.g)
                drawColor.g += 0.02f;
            if (drawColor.b <= targetColor.b)
                drawColor.b += 0.03f;

            for (int i = 0; i < render.Length; i++)
            {
                render[i].material.SetColor("_EmissionColor", drawColor);
            }

            stepHold.position = Vector3.MoveTowards(stepHold.position, finishPos, speed * Time.deltaTime);

            if (drawColor.r <= targetColor.r && drawColor.g >= targetColor.g
                && drawColor.b >= targetColor.b)
                break;

            yield return null;
        }
    }

    IEnumerator ClearColor()
    {
        drawColor = render[0].material.GetColor("_EmissionColor");
        while (isClear)
        {
            if (drawColor.r <= originColor.r)
                drawColor.r += 0.05f;
            if (drawColor.g >= originColor.g)
                drawColor.g -= 0.02f;
            if (drawColor.b >= originColor.b)
                drawColor.b -= 0.03f;

            for (int i = 0; i < render.Length; i++)
            {
                render[i].material.SetColor("_EmissionColor", drawColor);
            }

            stepHold.position = Vector3.MoveTowards(stepHold.position, originPos, speed * Time.deltaTime);

            if (drawColor.r >= originColor.r && drawColor.g <= originColor.g && 
                drawColor.b <= originColor.b)
                break;

            yield return null;
        }
    }

    IEnumerator SetOffObject()
    {
        Color color = holdRender[0].material.color;
        while (!isOnBox)
        {
            color.a -= 1f * Time.deltaTime;
            for (int i = 0; i < hold.Length; i++)
            {
                holdRender[i].material.color = color;
                if (color.a <= 0f)
                    hold[i].SetActive(false);
            }

            if (color.a <= 0f)
                break;

            yield return null;
        }
    }

    IEnumerator SetOnObject()
    {
        Color color = holdRender[0].material.color;
        while (isOnBox)
        {
            color.a += 1f * Time.deltaTime;
            for (int i = 0; i < hold.Length; i++)
            {
                hold[i].SetActive(true);
                holdRender[i].material.color = color;
            }

            if (color.a >= 1f)
                break;

            yield return null;
        }
    }
}
