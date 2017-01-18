using UnityEngine;
using System.Collections;

public class CanvasFix : MonoBehaviour {

    public Transform character;
    public Transform Fixcamera;

    public float posx;
    public float posy;
    [SerializeField, Range(0.0f, 1.0f)]
    private float rate; // ※

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        var vec = character.position - Fixcamera.position;
        vec.x += posx;
        vec.y += posy;

        transform.position = Fixcamera.position + vec * rate;
    }
}
