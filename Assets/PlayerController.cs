using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f;  // �ړ����x
    public float jumpForce = 10.0f; // �W�����v�̗�
    Rigidbody2D ridid2d;

    void Start()
    {
        this.ridid2d = GetComponent<Rigidbody2D>(); 
    }

    // Update is called once per frame
    void Update()
    {
        // ���E�̈ړ�
        float horizontalInput = Input.GetAxis("Horizontal");
        ridid2d.velocity = new Vector2(horizontalInput * moveSpeed, ridid2d.velocity.y);

        // �W�����v
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ridid2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
}
