using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D ridid2d;
    GameObject director;
    public GameObject playerAttackPrefab;
    public GameObject playerBreakAttackPrefab;
    public GameObject[] abilityStopObj;
    GameObject abilityChoose;
    public GameObject abilityAttackPrefab;
    public GameObject abilityHealPrefab;
    GameObject abilityGage;
    GameDirector gameDirectorScript;

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
    public bool isAttack = false;         // 攻撃フラグ
    public int attackPower = 5;           // パワー

    // 破壊攻撃
    public bool isBreak = false;     // 破壊攻撃フラグ
    public int breakPower = 10;      // 破壊パワー
    public float backLashStatus = 3; // 反動
    float backlash = 0;              // 反動
    bool isBacklash = false;         // 反動

    int attackMode = 0;           // 0 = 無し、1 = 通常攻撃、2 = 破壊攻撃
    bool attackModeChange = true; // 攻撃変化フラグ

    public float attackChargeMax = 3; // チャージ時間
    float attackCharge = 0;           // チャージ時間

    // 向き
    public float distance = 0;

    // アビリティ
    public bool onAbility = false;       // アビリティ発動
    bool isAbilityAttack = false; // 攻撃効果
    bool isAbilityHeal = false;   // 回復効果

    bool isKinematicInitially; // 初期のisKinematicの状態を保存

    float horizontalInput;

    float abilityChooseTime = 0;
    public float abilityChooseTimeStatus = 0.1f;
    float choose = 0;

    void Start()
    {
        this.ridid2d = GetComponent<Rigidbody2D>();
        director = GameObject.Find("gameDirector");
        abilityChoose = GameObject.Find("abilityChoose");
        abilityChoose.SetActive(false);
        abilityGage = GameObject.Find("abilityGage");
        gameDirectorScript = GetComponent<GameDirector>();

        isKinematicInitially = ridid2d.isKinematic;
    }

    // Update is called once per frame
    void Update()
    {
        if (!onAbility)
        {
            // 移動
            Move();

            // 向き
            Distance();

            if (canPushJanp)
            {
                if ((Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump")) && isGround)
                {
                    ridid2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                }
            }

            ridid2d.GetComponent<Rigidbody2D>().velocity = ridid2d.GetComponent<Rigidbody2D>().velocity;
            ridid2d.GetComponent<Rigidbody2D>().isKinematic = false;
        }
        else
        {
            ridid2d.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            ridid2d.GetComponent<Rigidbody2D>().isKinematic = true;
        }

        if(backlash <= 0)
        {
            isBacklash = false;
        }
        if (backlash > 0 && !isBreak)
        {
            backlash -= Time.deltaTime;
        }

        // ジャンプ
        isGround = false;
        isGround = Physics2D.Raycast(transform.position, Vector2.down, 1.0f, LayerMask.GetMask("Ground"));

        // HPアイコン
        director.GetComponent<GameDirector>().HpIcons(damage);

        // 攻撃
        if (attackMode == 1)
        {
            if (!isBacklash)
            {
                Attack();
            }
        }
        else if (attackMode == 2)
        {
            Break();
        }
        else if (attackMode == 0)
        {
            //playerBreakAttack.SetActive(false);
        }

        // チャージ
        if (attackModeChange)
        {
            if (Input.GetKey(KeyCode.Z) || Input.GetButton("Attack"))
            {
                attackCharge += Time.deltaTime;
            }
            else { attackCharge = 0; }

            if (attackCharge < attackChargeMax)
            {
                // 通常攻撃
                attackMode = 1;
            }
            else if (attackCharge >= attackChargeMax)
            {
                // 破壊攻撃
                attackMode = 2;
            }
            else
            {
                // 攻撃なし
                attackMode = 0;
            }
        }
        else { attackCharge = 0; }

        // アビリティ
        Ability();

        // デバッグ
        Debugg();
    }

    // 移動
    void Move()
    {
        if (!isBacklash)
        {
            // 左右のベクトルを取得
            horizontalInput = Input.GetAxis("Horizontal");

            // ダッシュ
            if (canPushDash)
            {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) || Input.GetButton("Dash"))
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
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift) || Input.GetButtonDown("Dash"))
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
        if (Input.GetKeyUp(KeyCode.Z) || Input.GetButtonUp("Attack"))
        {
            if (!isAttack)
            {
                if (!onAbility)
                {
                    isAttack = true;
                    GameObject attackObj = Instantiate(playerAttackPrefab);
                }
            }
        }

        if (isAttack)
        {
            attackModeChange = false;            
        }
        else
        {
            attackModeChange = true;
        }
    }

    // 破壊攻撃メソッド
    void Break()
    {
        //playerBreakAttack.transform.position =
        //   new Vector3(transform.position.x + 0.95f, transform.position.y + 0.3f, transform.position.z);

        if (Input.GetKeyUp(KeyCode.Z) || Input.GetButtonUp("Attack"))
        {
            if (!isBreak)
            {
                if (!onAbility)
                {
                    isBreak = true;
                    GameObject breakObj = Instantiate(playerBreakAttackPrefab);
                }
            }
        }

        if (isBreak)
        {
            isBacklash = true;
            backlash = backLashStatus;

            attackModeChange = false;
        }
        else
        {
            attackModeChange = true;
        }
    }

    // 向き
    void Distance()
    {
        if (!isBacklash)
        {
            // キー入力で左右の向きか取得
            if (Input.GetKey(KeyCode.RightArrow) || horizontalInput > 0) distance = 0.8f;
            if (Input.GetKey(KeyCode.LeftArrow) || horizontalInput < 0) distance = -0.8f;

            if (distance != 0)
            {
                transform.localScale = new Vector3(distance, 1.5f, 1);
            }
        }
    }

    // アビリティ
    void Ability()
    {
        abilityChoose.transform.position = 
            new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);

        if (Input.GetKeyDown(KeyCode.X) || Input.GetButtonDown("Ability"))
        {
            if (!onAbility && abilityGage.GetComponent<Image>().fillAmount == 1)
            {
                abilityChooseTime = abilityChooseTimeStatus;
                onAbility = true;
            }
        }

        if (onAbility)
        {
            // 動きを止める
            for (int i = 0; i < abilityStopObj.Length; i++)
            {
                abilityStopObj[i].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                abilityStopObj[i].GetComponent<Rigidbody2D>().isKinematic = true;
            }

            // 効果選択
            abilityChoose.SetActive(true);
            Debug.Log("効果を選んでね！\nA:攻撃 D:回復");

            abilityChooseTime -= Time.deltaTime;
            if (abilityChooseTime <= 0)
            {
                choose = Input.GetAxis("AbilityChoose");
            }

            if (choose >= 0.1f)
            {
                abilityChoose.SetActive(false);
                isAbilityAttack = true;
                onAbility = false;
            }
            else if(choose <= -0.1f)
            {
                abilityChoose.SetActive(false);
                isAbilityHeal = true;
                onAbility = false;
            }
        }
        else
        {
            for (int i = 0; i < abilityStopObj.Length; i++)
            {
                abilityStopObj[i].GetComponent<Rigidbody2D>().velocity = 
                    abilityStopObj[i].GetComponent<Rigidbody2D>().velocity;
                abilityStopObj[i].GetComponent<Rigidbody2D>().isKinematic = isKinematicInitially;
            }
        }

        // 攻撃
        if (isAbilityAttack)
        {
            abilityGage.GetComponent<Image>().fillAmount = 0;
            GameObject attackAbilityObj = Instantiate(abilityAttackPrefab);
            isAbilityAttack = false;
        }
        else if (isAbilityHeal)
        {
            abilityGage.GetComponent<Image>().fillAmount = 0;
            GameObject attackHealObj = Instantiate(abilityHealPrefab);
            isAbilityHeal = false;
        }
    }

    // デバッグ
    void Debugg()
    {
        // デバッグ
        //Debug.Log(dashTimer);
        //Debug.Log("ダメージ" + damage);
        //Debug.Log("向き変数" +  distance);
       // Debug.Log("チャージ量" +  attackCharge);
        //Debug.Log("反動時間" +  backlash);
        //Debug.Log("反動フラグ" +  isBacklash);
        Debug.Log(choose);
    }
}