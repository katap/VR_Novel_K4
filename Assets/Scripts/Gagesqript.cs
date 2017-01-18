using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Gagesqript : MonoBehaviour
{
    Image image;
    public bool GageMax;
    private GameObject circle;

    void Start()
    {
        image = GetComponent<Image>();
        image.fillAmount = 0;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            image.fillAmount += 0.02f;
            if (image.fillAmount >= 1)
            {
                GageMax = true;
                image.fillAmount = 0;
            }
        } else {
            image.fillAmount -= 0.02f;
        }

    }
}