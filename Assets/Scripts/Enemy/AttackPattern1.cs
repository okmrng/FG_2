using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AttackPattern1 : MonoBehaviour
{
    // ゲームオブジェクト
    GameObject player;
    /// <summary> 攻撃アビリティオブジェクト </summary>
    public GameObject abilityAttack;
    /// <summary> 回復アビリティオブジェクト </summary>
    public GameObject abilityHeal;
    /// <summary> 敵HP </summary>
    GameObject bossHPGage;

    // スクリプト
    PlayerController playerCon;
    /// <summary> 攻撃アビリティスクリプト </summary>
    AbilityAttackRangeController attackRangeController;
    /// <summary> 回復アビリティスクリプト </summary>
    AbilityHealRangeController healRangeController;

    public float jumpForce = 10.0f;
    public float jumpCooldown = 2.0f; // ジャンプのクールダウン時間
    public float dashSpeed = 30.0f; // 突進速度
    public int maxJumps = 3; // 最大ジャンプ回数
    private bool dashAttackStop = false;

    private Rigidbody2D rb;
    private bool canJump = true;
    private int jumpCount = 0;
    private bool isDashing = false;
    private bool reachedEdge = false;
    private bool isDashingToWall = false;
    private Vector3 initialPosition;
    private Vector3 originalSize;
    
       // 追加: ResizeDuringDash スクリプトを保持する変数
    private ResizeDuringDash resizeScript;

    /// <summary> HPの最大値 </summary>
    public int HPMax = 1;

    /// <summary> プレイ開始までの時間 </summary>
    public float canPlayTime = 2;

    private void Start()
    {
        player = GameObject.Find("player");
        playerCon = player.GetComponent<PlayerController>();
        bossHPGage = GameObject.Find("bossGage");

        rb = GetComponent<Rigidbody2D>();
        initialPosition = transform.position;

        // 追加: ResizeDuringDash スクリプトを取得
        resizeScript = GetComponent<ResizeDuringDash>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (isDashing && resizeScript != null)
        {
            if (other.gameObject.tag == "Wall")
            {
                dashAttackStop = true;
                // 追加: サイズ変更処理を呼び出す
               resizeScript.StartResize();
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerAttack")
        {
           bossHPGage.GetComponent<Image>().fillAmount -= 0.05f;
        }
        if (collision.gameObject.tag == "PlayerBreakAttack")
        {
            bossHPGage.GetComponent<Image>().fillAmount -= 0.15f;
        }
        if (collision.gameObject.tag == "PlayerAbilityAttack")
        {
            bossHPGage.GetComponent<Image>().fillAmount -= 0.75f;
        }
        if (collision.gameObject.tag == "PlayerAbilityHeal")
        {
            if (bossHPGage.GetComponent<Image>().fillAmount < HPMax)
            {
                bossHPGage.GetComponent<Image>().fillAmount += 0.75f;
            }
        }
    }

    private void Update()
    {
        if (player != null)
        {
            if (playerCon.canPlay)
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
                        DashToEdge();
                        // 追加: サイズ変更処理を突進中に制御する
                        resizeScript.SetDashing(true);
                    }
                }
                else if (isDashing && !reachedEdge)
                {
                    if (dashAttackStop)
                    {
                        isDashing = false;
                        JumpBackToPlayer();
                        // 追加: サイズ変更処理を突進中に制御する
                        resizeScript.SetDashing(false);
                    }
                }
            }
            else
            {
                if(transform.position.y <= -1.15)
                {
                    canPlayTime -= Time.deltaTime;
                    if(canPlayTime <= 0)
                    {
                        playerCon.canPlay = true;
                    }
                }
            }

            if (bossHPGage.GetComponent<Image>().fillAmount > HPMax)
            {
                bossHPGage.GetComponent<Image>().fillAmount = HPMax;
            }

            if (bossHPGage.GetComponent<Image>().fillAmount <= 0)
            {
                SceneManager.LoadScene("ClearScene");
                Destroy(gameObject);
            }

            Debug.Log("dashAttackStop:" + dashAttackStop);
        }
    }

    void JumpTowardsPlayer()
    {
        Vector3 direction = new Vector3(player.transform.position.x - transform.position.x, 0, 0).normalized;
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
       //resizeScript.StartResize();
        Vector3 dashDirection = new Vector3(player.transform.position.x - transform.position.x, 0, 0).normalized; // 自機の方向に向かって突進

    rb.velocity = dashDirection * dashSpeed;

  
    }

    void JumpBackToPlayer()
    {
        reachedEdge = false;
        dashAttackStop = false;
        jumpCount = 0; // ジャンプ回数をリセット
        isDashing = false;

         // 追加: サイズ変更処理を停止
            resizeScript.SetDashing(false);
    }
}
