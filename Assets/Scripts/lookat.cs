using UnityEngine;
using System.Collections;

public class lookat : MonoBehaviour
{
    
    public Camera rotateCamera;

    [SerializeField]
    public GameObject target = null;

    // ゴール
    [SerializeField]
    //Transform target;

    void Start()
    {
        rotateCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(target.transform);
        transform.rotation = rotateCamera.transform.rotation;

    }

}