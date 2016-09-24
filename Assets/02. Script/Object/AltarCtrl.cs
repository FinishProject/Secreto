﻿using UnityEngine;
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

    void Start()
    {
        //originColor = new Color(0f, 0.8117652f, 1.5f);
        originColor = new Color(0.322f, 0.322f, 0.322f);
        for (int i = 0; i < 2; i++)
        {
            render[i].sharedMaterial.SetColor("_EmissionColor", originColor);
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
        drawColor = render[0].sharedMaterial.GetColor("_EmissionColor");
        while (isDraw)
        {
            if (drawColor.r >= targetColor.r)
                drawColor.r -= 0.05f;
            if (drawColor.g <= targetColor.g)
                drawColor.g += 0.02f;
            if (drawColor.b <= targetColor.b)
                drawColor.b += 0.03f;

            for (int i = 0; i < 2; i++)
            {
                render[i].sharedMaterial.SetColor("_EmissionColor", drawColor);
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
        drawColor = render[0].sharedMaterial.GetColor("_EmissionColor");
        while (isClear)
        {
            if (drawColor.r <= originColor.r)
                drawColor.r += 0.05f;
            if (drawColor.g >= originColor.g)
                drawColor.g -= 0.02f;
            if (drawColor.b >= originColor.b)
                drawColor.b -= 0.03f;

            for (int i = 0; i < 2; i++)
            {
                render[i].sharedMaterial.SetColor("_EmissionColor", drawColor);
            }

            stepHold.position = Vector3.MoveTowards(stepHold.position, originPos, speed * Time.deltaTime);

            if (drawColor.r >= originColor.r && drawColor.g <= originColor.g && 
                drawColor.b <= originColor.b)
                break;

            yield return null;
        }
    }
}
