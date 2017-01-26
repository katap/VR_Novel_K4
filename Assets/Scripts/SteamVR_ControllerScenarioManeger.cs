using UnityEngine;
using System.Collections;

public class SteamVR_ControllerScenarioManeger : MonoBehaviour {

    SteamVR_TrackedObject trackedObj;

    public GameObject centralControllerPlace;

    CentralController animCom;

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        animCom = centralControllerPlace.GetComponent<CentralController>();
    }

    void Update()
    {

        var device = SteamVR_Controller.Input((int)trackedObj.index);
        
        #region 一応とっておく
        /*
        if (animCom.scenarioNum == animCom.moveTransition[animCom.j])
        {
            //  ChoiceSelect が 0 以外ならチョイス画面に飛ぶ
            if (animCom.moveChoiceNoSelect[animCom.j] != 0)
            {
                //  tChoiceの処理
                if (animCom.choiceswitch != 999)
                {
                    animCom.choiceCanvasControll();
                    if (animCom.cBack.Count != 1)
                    {
                        if (device.GetPressUp(SteamVR_Controller.ButtonMask.ApplicationMenu))
                        {
                            animCom.k = animCom.cBack[animCom.cBack.Count - 2];
                            animCom.cBack.RemoveAt(animCom.cBack.Count - 1);
                            animCom.cBackSize.RemoveAt(animCom.cBack.Count - 1);
                        }
                    }
                }
            }
        }
        */
        #endregion

        // fontSizeCanvasの終了処理
        if (animCom.fontSizeCanvas.activeInHierarchy && animCom.fontSizeText.fontSize >= 1)
        {
            if (device.GetPressUp(SteamVR_Controller.ButtonMask.ApplicationMenu))
            {
                animCom.endFontSizeCanvas();
            }
        }

        // fontSizeCanvasのフォントの大きさを変える処理
        if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Touchpad))
        {
            Vector2 touchPosition = device.GetAxis();
            if (touchPosition.y / touchPosition.x > 1 || touchPosition.y / touchPosition.x < -1)
            {
                if (touchPosition.y > 0)
                {
                    //タッチパッド上をクリックした場合の処理
                    animCom.fontSizeText.fontSize += 1;
                    Invoke("delayTouch", 0.5f);
                }
                else
                {
                    if (animCom.fontSizeText.fontSize != 1)
                    {
                        //タッチパッド下をクリックした場合の処理
                        animCom.fontSizeText.fontSize -= 1;
                        Invoke("delayTouch", 0.5f);
                    }
                }
            }
        }

        
        
        // 文字送り
        if (animCom.textControllerPlace.activeInHierarchy || animCom.fontSizeCanvas.activeInHierarchy == false) {
            if (animCom.sMan.m_textController.IsCompleteDisplayText)
            {
                if (animCom.sMan.m_currentLine < animCom.sMan.m_scenarios.Length)
                {
                    if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
                    {
                        animCom.sMan.RequestNextLine();
                    }
                    if (animCom.sMan.m_textController.maneger != 1)
                    {
                        if (device.GetPressUp(SteamVR_Controller.ButtonMask.ApplicationMenu))
                        {
                            animCom.sMan.m_textController.maneger -= 2;
                            animCom.sMan.m_currentLine -= 2;
                            animCom.sMan.RequestNextLine();
                        }
                    }
                }
            }
            else
            {
                if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
                {
                    animCom.sMan.m_textController.ForceCompleteDisplayText();
                }
            }
        }
    }

    void delayTouch()
    {
        var device = SteamVR_Controller.Input((int)trackedObj.index);

        // fontSizeCanvasのフォントの大きさを変える処理
        if (device.GetTouch(SteamVR_Controller.ButtonMask.Touchpad))
        {
            Vector2 touchPosition = device.GetAxis();
            if (touchPosition.y / touchPosition.x > 1 || touchPosition.y / touchPosition.x < -1)
            {
                if (touchPosition.y > 0)
                {
                    //タッチパッド上をクリックした場合の処理
                    animCom.fontSizeText.fontSize += 1;
                }
                else
                {
                    if (animCom.fontSizeText.fontSize != 1)
                    {
                        //タッチパッド下をクリックした場合の処理
                        animCom.fontSizeText.fontSize -= 1;
                    }
                }
            }
        }
    }
}
