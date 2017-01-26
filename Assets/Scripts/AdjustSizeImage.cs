using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AdjustSizeImage : MonoBehaviour {

    public Text tx;
    public float marginWidth;
    public float marginHeight;
    Image image;

    // Use this for initialization
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        image.rectTransform.sizeDelta = new Vector2(tx.rectTransform.sizeDelta.x + marginWidth, tx.rectTransform.sizeDelta.y + marginHeight);
    }
}
