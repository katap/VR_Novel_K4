using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIArrow : MonoBehaviour {

    [SerializeField]
    Transform target;
    [SerializeField]
    Camera targetCamera;
    [SerializeField]
    Image icon;

    private Rect rect = new Rect(0, 0, 1, 1);

    private Rect canvasRect;

    void Start()
    {
        // UIがはみ出ないようにする
        canvasRect = ((RectTransform)icon.canvas.transform).rect;
        canvasRect.Set(
            canvasRect.x + icon.rectTransform.rect.width * 0.5f,
            canvasRect.y + icon.rectTransform.rect.height * 0.5f,
            canvasRect.width - icon.rectTransform.rect.width,
            canvasRect.height - icon.rectTransform.rect.height
        );
    }

    void Update()
    {
        var viewport = targetCamera.WorldToViewportPoint(target.position);
        if (rect.Contains(viewport))
        {
            Debug.Log("カメラに映ってるよ！");
            icon.enabled = false;
        }
        else
        {
            Debug.Log("カメラに映ってないよ！");
            icon.enabled = true;

            // 画面内で対象を追跡
            viewport.x = Mathf.Clamp01(viewport.x);
            viewport.y = Mathf.Clamp01(viewport.y);
            icon.rectTransform.anchoredPosition = Rect.NormalizedToPoint(canvasRect, viewport);
        }
    }
}