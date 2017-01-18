using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIArrowActive : MonoBehaviour {

    //メインカメラに付いているタグ名
    private const string MAIN_CAMERA_TAG_NAME = "MainCamera";

    //カメラに表示されているか
    public bool _isRendered = true;
    
   // public GameObject GUIArrow;

    private void Update()
    {
        //var viewport = targetCamera.WorldToViewportPoint(target.position);
        if (_isRendered)
        {
            //Debug.Log("カメラに映ってるよ！");
            //GUIArrow.SetActive(false);

        } else {
            //Debug.Log("カメラに映ってないよ！");
            //GUIArrow.SetActive(true);

        }
        _isRendered = false;
    }

    //カメラに映ってる間に呼ばれる
    private void OnWillRenderObject()
    {
        //メインカメラに映った時だけ_isRenderedを有効に
        if (Camera.current.tag == MAIN_CAMERA_TAG_NAME)
        {
            _isRendered = true;
        }
    }
}