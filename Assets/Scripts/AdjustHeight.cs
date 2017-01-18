using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AdjustHeight : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Text text = GetComponent<Text>();
        text.rectTransform.sizeDelta = new Vector2(text.rectTransform.sizeDelta.x, text.preferredHeight);

    }
}
