using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour
{
    //public Vector3 pos;
    private GameObject obj;
    public float hight = 1;

    public float gravity = 10.0f;//重力加速度

    // Use this for initialization
    void Start()
    {
        //Vector3 pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;

        if (Input.GetKey("up") || Input.GetKey("w"))
        {
            pos += transform.forward * 0.1f;
        }
        if (Input.GetKey("down") || Input.GetKey("s"))
        {
            pos -= transform.forward * 0.1f;
        }
        if (Input.GetKey("right") || Input.GetKey("d"))
        {
            pos += transform.right * 0.1f;
        }
        if (Input.GetKey("left") || Input.GetKey("a"))
        {
            pos -= transform.right * 0.1f;
        }

        //pos.y -= gravity * 0.1f;//重力設定

        //pos -= transform.up * 0.1f;

        pos.y = hight;
        transform.position = pos;

    }
}