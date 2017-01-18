using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class UIExecute : MonoBehaviour {

    Image image;
    public GameObject cursor;
    //public float timeToSelect = 2.0f;
    //private float countDown;
    private GameObject currentButton;
    //private Clicker clicker = new Clicker ();
    public bool gageMax;

    void Start()
    {
        image = cursor.GetComponent<Image>();
        image.fillAmount = 0;
    }

	// Update is called once per frame
	void Update ()
    {
        Transform camera = Camera.main.transform;
        Ray ray = new Ray(camera.position, camera.rotation *
            Vector3.forward);
        RaycastHit hit;
        GameObject hitButton = null;
        PointerEventData data = new PointerEventData
            (EventSystem.current);

        if (image.fillAmount >= 1) {
            gageMax = true;
        } else {
            gageMax = false;
        }

        if (Physics.Raycast (ray, out hit)) {
            if (hit.transform.gameObject.tag == "Button") {
                image.fillAmount += 0.02f;
                hitButton = hit.transform.parent.gameObject;
                //image.fillAmount += 0.02f;
            } else {
                image.fillAmount -= 0.02f;
            }
        } else {
            image.fillAmount -= 0.02f;
        }


        if (currentButton != hitButton) {
            if (currentButton != null){//ハイライトを外す
                ExecuteEvents.Execute<IPointerExitHandler>
                    (currentButton, data, ExecuteEvents.pointerExitHandler);
                //image.fillAmount += 0.02f;
            }
            currentButton = hitButton;
            if (currentButton != null){//ハイライトする
                ExecuteEvents.Execute<IPointerEnterHandler>
                    (currentButton, data, ExecuteEvents.pointerEnterHandler);
                //countDown = timeToSelect;
                //image.fillAmount += 0.02f;
            }
        }
        /*
        if (currentButton != null)
        {
            countDown -= Time.deltaTime;
            if (clicker.clicked() || countDown < 0.0f)
            {
                ExecuteEvents.Execute<IPointerClickHandler>
                    (currentButton, data, ExecuteEvents.pointerClickHandler);
                countDown = timeToSelect;
                image.fillAmount += 0.02f;
            }
        }*/
    }
}
