using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AdjustSize : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    void Update()
    {
        Text text = GetComponent<Text>();
        text.rectTransform.sizeDelta = new Vector2(text.preferredWidth + 30f, text.preferredHeight);
    }
}
