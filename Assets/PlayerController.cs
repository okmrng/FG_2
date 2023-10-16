using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D ridid2d;
    public float jumpForce = 500.0f;

    // Start is called before the first frame update
    void Start()
    {
        ridid2d = GetComponent<Rigidbody2D>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ridid2d.AddForce(transform.up * jumpForce);
        }
    }
}
