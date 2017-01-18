﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIArrowGet : MonoBehaviour
{

    public GameObject animCommandPlace;
    
    [SerializeField]
    Camera targetCamera;
    public Image icon;

    private Rect rect = new Rect(0, 0, 1, 1);

    private Rect canvasRect;

    Animation_Command animCom;

    void Start()
    {
        animCom = animCommandPlace.GetComponent<Animation_Command>();
        // UIがはみ出ないようにする
        canvasRect = ((RectTransform)icon.canvas.transform).rect;

        var irtr = icon.rectTransform.rect;

        canvasRect.Set(
            canvasRect.x + irtr.width * 0.5f,
            canvasRect.y + irtr.height * 0.5f,
            canvasRect.width - irtr.width,
            canvasRect.height - irtr.height
        );
    }

    private void Update()
    {
        var viewport = targetCamera.WorldToViewportPoint(animCom.setAllCanvas()[animCom.nowC].transform.position);
        if (animCom.canvasAcrive[animCom.nowC]._isRendered)
        {
            //Debug.Log("カメラに映ってるよ！");
            icon.enabled = false;
        }
        else
        {
            //Debug.Log("カメラに映ってないよ！");

            icon.enabled = true;

            // 画面内で対象を追跡
            // ココを変える
            viewport.x = Mathf.Clamp01(viewport.x);
            viewport.y = Mathf.Clamp01(viewport.y);
            icon.rectTransform.anchoredPosition = Rect.NormalizedToPoint(canvasRect, viewport);

        }
    }
}