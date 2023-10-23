using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D ridid2d;
    GameObject director;
    GameObject playerAttack;
    GameObject playerBreakAttack;

    public float moveSpeed = 5.0f; // 移動速度

    // ダッシュ
    float dashSpeed = 0.0f;         // ダッシュの速度
    public float dashPower = 20.0f; // ダッシュの速度
    bool canDash = false;
    float dashTimer = 0;
    public float dashTimerStatus = 0.5f;
    float dashCoolTime = 0;
    public float dashCoolTimeStatus = 0.5f;
    bool canPushDash = true;

    // ジャンプ
    public float jumpForce = 10.0f; // ジャンプ
    bool canPushJanp = true;
    bool isGround = false;

    // HP
    public int HP = 5;
    int damage = 0;

    // 攻撃
    bool isAttack = false;                // 攻撃フラグ
    public int attackPower = 5;           // パワー
    float attackEndTime = 0;              // 攻撃時間
    public float attackEndTimeStatus = 1; // 攻撃時間

    // 破壊攻撃
    bool isBreak = false;                   // 破壊攻撃フラグ
    public int breakPower = 10;             // 破壊パワー
    float breakEndTime = 0;                 // 破壊時間
    public float breakEndTimeStatus = 1.2f; // 破壊時間

    // 向き
    float distance = 0;

    void Start()
    {
        this.ridid2d = GetComponent<Rigidbody2D>();
        director = GameObject.Find("gameDirector");
        playerAttack = GameObject.Find("playerAttack");
        playerBreakAttack = GameObject.Find("playerBreakAttack");
    }

    // Update is called once per frame
    void Update()
    {
        // 移動
        Move();

        // ジャンプ
        isGround = false;
        isGround = Physics2D.Raycast(transform.position, Vector2.down, 1.0f, LayerMask.GetMask("Ground"));

        if (canPushJanp)
        {
            if (Input.GetKeyDown(KeyCode.Space) && isGround)
            {
                ridid2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }

        // HPアイコン
        director.GetComponent<GameDirector>().HpIcons(damage);

        // 攻撃
        Attack();
        //Break();

        // 向き
        Distance();

        // デバッグ
        Debugg();
    }


    void Move()
    {
        // 左右のベクトルを取得
        float horizontalInput = Input.GetAxis("Horizontal");

        // ダッシュ
        if (canPushDash)
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                canPushDash = false;

                if (dashCoolTime < 0)
                {
                    canDash = true;
                }
            }
            else
            {
                canDash = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            canPushDash = true;
        }

        if (canDash)
        {
            canPushJanp = false;
            dashSpeed = dashPower;
            ridid2d.velocity = new Vector2(horizontalInput * dashSpeed, 0);

            // ダッシュ終わり
            dashTimer -= Time.deltaTime;
            if (dashTimer < 0)
            {
                dashCoolTime = dashCoolTimeStatus;
                canDash = false;
            }
        }
        else
        {
            canPushJanp = true;
            dashTimer = dashTimerStatus;
            dashCoolTime -= Time.deltaTime;
            // 左右移動
            ridid2d.velocity = new Vector2(horizontalInput * moveSpeed, ridid2d.velocity.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            HP -= 1;
            damage += 1;
        }
    }

    // 攻撃メソッド
    void Attack()
    {
        playerAttack.transform.position = 
            new Vector3(transform.position.x + 0.8f, transform.position.y, transform.position.z);
        
        if (Input.GetKeyUp(KeyCode.Z))
        {
            isAttack = true;
        }

        if (isAttack)
        {
            playerAttack.SetActive(true);

            attackEndTime -= Time.deltaTime;
            if (attackEndTime < 0)
            {
                isAttack = false;
            }
        }
        else
        {
            playerAttack.SetActive(false);
            attackEndTime = attackEndTimeStatus;
        }
    }

    // 破壊攻撃メソッド
    void Break()
    {
        playerBreakAttack.transform.position =
           new Vector3(transform.position.x + 0.95f, transform.position.y + 0.3f, transform.position.z);

        if (Input.GetKeyUp(KeyCode.Z))
        {
            isBreak = true;
        }

        if (isBreak)
        {
            playerBreakAttack.SetActive(true);

            breakEndTime -= Time.deltaTime;
            if (breakEndTime < 0)
            {
                isBreak = false;
            }
        }
        else
        {
            playerBreakAttack.SetActive(false);
            breakEndTime = breakEndTimeStatus;
        }
    }

    // 向き
    void Distance()
    {
        // キー入力で左右の向きか取得
        if (Input.GetKey(KeyCode.RightArrow)) distance = 0.8f;
        if (Input.GetKey(KeyCode.LeftArrow)) distance = -0.8f;

        if (distance != 0)
        {
            transform.localScale = new Vector3(distance, 1.5f, 1);
        }

        // 攻撃の向き
        //if (distance == 0.8f)
        //{
        //    playerAttack.transform.position = new Vector3(playerAttack.transform.position.x,
        //        playerAttack.transform.position.y, playerAttack.transform.position.z);
        //    playerBreakAttack.transform.position = new Vector3(playerBreakAttack.transform.position.x,
        //        playerBreakAttack.transform.position.y, playerBreakAttack.transform.position.z);
        //}
        //else if (distance == -0.8f)
        //{
        //    playerAttack.transform.position = new Vector3(playerAttack.transform.position.x - 1.58f,
        //    playerAttack.transform.position.y, playerAttack.transform.position.z);
        //    playerBreakAttack.transform.position =
        //        new Vector3(playerBreakAttack.transform.position.x - 1.9f,
        //        playerBreakAttack.transform.position.y, playerBreakAttack.transform.position.z);
        //}
    }

    // デバッグ
    void Debugg()
    {
        // デバッグ
        Debug.Log(dashTimer);
        Debug.Log("ダメージ" + damage);
        Debug.Log("向き変数" +  distance);
    }
}