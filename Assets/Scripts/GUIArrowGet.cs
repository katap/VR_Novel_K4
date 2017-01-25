using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIArrowGet : MonoBehaviour
{

    public GameObject centralControllerPlace;
    public Camera targetCamera;
    public Image icon;

    private Rect canvasRect;

    CentralController animCom;

    void Start()
    {
        animCom = centralControllerPlace.GetComponent<CentralController>();
        // UIがはみ出ないようにする
        canvasRect = ((RectTransform)icon.canvas.transform).rect;

        var irtr = icon.rectTransform.rect;

        canvasRect.Set(
            canvasRect.x + irtr.width * 0.5f,
            canvasRect.y + irtr.height * 0.5f,
            canvasRect.width - irtr.width,
            canvasRect.height - irtr.height
        );
        icon.enabled = false;
    }

    private void Update()
    {
        if (animCom.spawnEventActive)
        {
            if (animCom.aActive[animCom.y - 1]) {
                iconEnable();
            }
        }
        else
        {
            iconEnable();
        }
    }

    private void iconEnable()
    {
        var viewport = targetCamera.WorldToViewportPoint(animCom.setAllCanvas()[animCom.nowC].transform.position);
        if (animCom.canvasActive[animCom.nowC]._isRendered)
        {
            //Debug.Log("カメラに映ってるよ！");
            icon.enabled = false;
        }
        else
        {
            //Debug.Log("カメラに映ってないよ！");

            icon.enabled = true;
            /*
            // 画面内で対象を追跡
            // ココを変える
            viewport.x = Mathf.Clamp01(viewport.x);
            viewport.y = Mathf.Clamp01(viewport.y);
            icon.rectTransform.anchoredPosition = Rect.NormalizedToPoint(canvasRect, viewport);
            */
        }
    }
}