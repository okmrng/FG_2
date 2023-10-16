using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D ridid2d;

    public float moveSpeed = 5.0f;  // 移動速度
    public float jumpForce = 10.0f; // ジャンプの力
    public int HP = 5;

    void Start()
    {
        this.ridid2d = GetComponent<Rigidbody2D>(); 
    }

    // Update is called once per frame
    void Update()
    {
        // 左右の移動
        float horizontalInput = Input.GetAxis("Horizontal");
        ridid2d.velocity = new Vector2(horizontalInput * moveSpeed, ridid2d.velocity.y);

        // ジャンプ
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ridid2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        // デバッグ
        Debug.Log(HP);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            HP -= 1;
        }
    }
}
