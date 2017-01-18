using UnityEngine;
using System.Collections;

/// <summary>
/// SmoothLookを試すスクリプト
/// </summary>
public class SmoothLookTest : MonoBehaviour
{

    // 目線を向けるターゲットとなる青玉と赤玉
    public Transform targetBlue;
    public Transform targetRed;

    // 目線制御モード
    public Mode mode;
    public enum Mode
    {
        NoLook,     // 目線制御OFF
        LookBlue,   // 青玉見てる
        LookRed     // 赤玉見てる
    }

    SmoothLook smoothLook;

    #region unity

    void Awake()
    {
        smoothLook = GetComponent<SmoothLook>();
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(100, 300, 150, 40), "mode : " + mode.ToString()))
        {
            switch (mode)
            {
                case Mode.NoLook:
                    mode = Mode.LookBlue;
                    smoothLook.Look(targetBlue);
                    break;

                case Mode.LookBlue:
                    mode = Mode.LookRed;
                    smoothLook.Look(targetRed);
                    break;

                case Mode.LookRed:
                    mode = Mode.NoLook;
                    smoothLook.EndLook();
                    break;
            }
        }
    }

    #endregion
}