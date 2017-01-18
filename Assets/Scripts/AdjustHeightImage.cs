﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AdjustHeightImage : MonoBehaviour {

    public Text tx;
    Image image;

    // Use this for initialization
    void Start () {
        image = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
	image.rectTransform.sizeDelta = new Vector2(image.rectTransform.sizeDelta.x, tx.rectTransform.sizeDelta.y + 40f);
    }
}