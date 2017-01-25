using UnityEngine;
using System.Collections;

public class OperationCanvas : MonoBehaviour {
	
	public Camera rotateCamera;

	void Start () {
        rotateCamera = Camera.main;
	}
	void Update () {
        var rcam = rotateCamera.transform.localEulerAngles;
        //transform.rotation = rotateCamera.transform.rotation;
        transform.rotation = Quaternion.Euler(0f, rcam.y - 180, 0f);
    }
    void Disable() {
		this.gameObject.SetActive(false);
	}

}
