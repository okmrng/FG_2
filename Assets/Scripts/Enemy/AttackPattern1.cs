using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPattern1 : MonoBehaviour
{
    GameObject player;
    PlayerController playerCon;
     public float jumpForce = 10.0f;
    public float jumpCooldown = 2.0f; // ジャンプのクールダウン時間
    public float dashSpeed = 20.0f; // 突進速度
    public int maxJumps = 3; // 最大ジャンプ回数
    //public BoxCollider2D collider; // 当たり判定コライダー

    private Rigidbody2D rb;
    private bool canJump = true;
    private int jumpCount = 0;
    private bool isDashing = false;
    private bool reachedEdge = false;
    private Vector3 initialPosition;

    private void Start()
    {
        player = GameObject.Find("player");
       playerCon = GetComponent<PlayerController>();
      
        rb = GetComponent<Rigidbody2D>();
        initialPosition = transform.position;
    }

      private void Update()
    {
        if (player != null)
        {
            if (!isDashing)
            {
                if (canJump)
                {
                    JumpTowardsPlayer();
                }
                if (jumpCount >= maxJumps)
                {
                    isDashing = true;
                   // collider.size = new Vector2(collider.size.x, collider.size.y / 2f); // 縦の当たり判定を半分に
                    DashToEdge();
                }
            }
            else if (isDashing && !reachedEdge)
            {
                // ボスが画面端に到達したら突進から横移動に切り替える
                if (transform.position.x >= Screen.width)
                {
                    reachedEdge = true;
                    HorizontalMovement();
                }
            }
        }
    }

    void JumpTowardsPlayer()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        rb.velocity = Vector2.zero; // ジャンプする前に速度をリセット
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        rb.AddForce(direction * jumpForce, ForceMode2D.Impulse);
        canJump = false;
        jumpCount++;
        StartCoroutine(JumpCooldown());
    }

    IEnumerator JumpCooldown()
    {
        yield return new WaitForSeconds(jumpCooldown);
        canJump = true;
    }

    void DashToEdge()
    {
        Vector3 dashDirection = Vector3.right; // 画面端まで突進する方向（例として右に設定）
        rb.velocity = dashDirection * dashSpeed;
    }

    void HorizontalMovement()
    {
        Vector3 horizontalDirection = Vector3.right; // 横移動の方向（例として右に設定）
        rb.velocity = horizontalDirection * dashSpeed;
    }

}

