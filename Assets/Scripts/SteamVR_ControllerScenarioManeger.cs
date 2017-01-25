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
        
        if (animCom.sMan.m_textController.IsCompleteDisplayText)
        {
            if (animCom.sMan.m_currentLine < animCom.sMan.m_scenarios.Length)
            {
                if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
                {
                    animCom.sMan.m_textController.maneger -= 2;
                    animCom.sMan.m_currentLine -= 2;
                    animCom.sMan.RequestNextLine();
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
