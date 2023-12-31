using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // コンポーネント
    Rigidbody2D ridid2d;
    Renderer rendere;
    SpriteRenderer spriteRenderer;
    Animator anim;

    // ゲームオブジェクト
    /// <summary> UIオブジェクト </summary>
    GameObject director;
    /// <summary> 自機通常攻撃のプレファブオブジェクト </summary>
    public GameObject playerAttackPrefab;
    /// <summary> 自機破壊攻撃のプレファブオブジェクト </summary>
    public GameObject playerBreakAttackPrefab;
    /// <summary> アビリティ選択中に止めるオブジェクト </summary>
    public GameObject[] abilityStopObj;
    /// <summary> アビリティ選択(攻撃)オブジェクト </summary>
    GameObject abilityAttackImage;
    /// <summary> アビリティ選択(回復)オブジェクト </summary>
    GameObject abilityHealImage;
    /// <summary> アビリティ(攻撃)のプレファブオブジェクト </summary>
    public GameObject abilityAttackPrefab;
    /// <summary> アビリティ(回復)のプレファブオブジェクト </summary>
    public GameObject abilityHealPrefab;
    /// <summary> アビリティゲージオブジェクト </summary>
    GameObject abilityGage;
    /// <summary> ボスへの遷移オブジェクト </summary>
    GameObject load;
    /// <summary> ボス </summary>
    GameObject boss1;

    // スクリプト
    /// <summary> UIスクリプト </summary>
    GameDirector gameDirectorScript;
    /// <summary> ボス </summary>
    AttackPattern1 bossScript;

    // スプライト
    /// <summary> 通常モーション </summary>
    public Sprite idleSprite;
    /// <summary> 攻撃モーション </summary>
    public Sprite attackSprite;
    /// <summary> 破壊攻撃モーション </summary>
    public Sprite breakSprite;

    // 移動速度
    /// <summary> 移動速度 </summary>
    public float moveSpeed = 5.0f;

    // ダッシュ
    /// <summary> ダッシュの速度 </summary>
    float dashSpeed = 0.0f;
    /// <summary> ダッシュの速度の設定 </summary>
    public float dashPower = 20.0f;
    /// <summary> ダッシュフラグ </summary>
    bool canDash = false;
    /// <summary> 何秒間ダッシュするか </summary>
    float dashTimer = 0;
    /// <summary> 何秒間ダッシュするかの設定 </summary>
    public float dashTimerStatus = 0.5f;
    /// <summary> 次ダッシュできるようになるまでのクールタイム </summary>
    float dashCoolTime = 0;
    /// <summary> 次ダッシュできるようになるまでのクールタイムの設定 </summary>
    public float dashCoolTimeStatus = 0.5f;
    /// <summary> ダッシュボタンが押せるかのフラグ </summary>
    bool canPushDash = true;
    /// <summary> 右へのダッシュフラグ </summary>
    bool rightDash = true; 
    /// <summary> 左へのダッシュフラグ </summary>
    bool leftDash = true;

    // ジャンプ
    /// <summary> ジャンプ力 </summary>
    public float jumpForce = 10.0f;
    /// <summary> ジャンプボタンを押せるかのフラグ </summary>
    bool canPushJanp = true;
    /// <summary> 地面と接触しているか確認するためのフラグ </summary>
    public bool isGround = false;

    // HP
    /// <summary> HPの最大値 </summary>
    public int HPMAX = 5;
    /// <summary> HPの現在値 </summary>
    public static int HP = 5;
    /// <summary> ダメージを受けた時にHPアイコンを減らすための変数 </summary>
    static int damage = 0;

    // 通常攻撃
    /// <summary> 通常攻撃フラグ </summary>
    public bool isAttack = false;
    /// <summary> 通常攻撃の威力 </summary>
    public int attackPower = 5;

    // 破壊攻撃
    /// <summary> 破壊攻撃フラグ </summary>
    public bool isBreak = false;
    /// <summary> 破壊攻撃の威力 </summary>
    public int breakPower = 10; 
    /// <summary> 反動の時間の設定 </summary>
    public float backLashStatus = 3;
    /// <summary> 反動の時間 </summary>
    float backlash = 0;
    /// <summary> 反動フラグ </summary>
    bool isBacklash = false;

    // 攻撃切り替え
    /// <summary> 攻撃のモードの列挙体 </summary>
    enum AttackMode 
    {
        /// <summary> 無し </summary>
        NON,
        /// <summary> 通常攻撃  </summary>
        NORMAL,
        /// <summary> 破壊攻撃  </summary>
        BREAK
    }
    /// <summary> 攻撃の切り替え用変数  </summary>
    AttackMode attackMode = AttackMode.NON;
    /// <summary> 攻撃が切り替え可能か管理するフラグ  </summary>
    bool attackModeChange = true;
    /// <summary> チャージの最大値  </summary>
    public float attackChargeMax = 3;
    /// <summary> チャージの現在値 </summary>
    float attackCharge = 0;

    // 向き
    /// <summary> 現在どっち向いているか </summary>
    public float distance = 0.8f;
    /// <summary> 右向き </summary>
    public float rightDistance = 0.8f;
    /// <summary> 左向き </summary>
    public float leftDistance = -0.8f;
    /// <summary> 現在どっち向いているかを判断する変数 //true = 右 false = 左// </summary>
    bool right = true;

    // アビリティ
    /// <summary> アビリティ発動フラグ </summary>
    public bool onAbility = false;
    /// <summary> アビリティ(攻撃)フラグ </summary>
    public bool isAbilityAttack = false;
    /// <summary> アビリティ(回復)フラグ </summary>
    public bool isAbilityHeal = false;
    /// <summary> アビリティ効果用の列挙体 </summary>
    enum ability
    {
        /// <summary> 攻撃 </summary>
        ATTACK,
        /// <summary> 回復 </summary>
        HEAL
    }
    /// <summary> 何のアビリティを選択しているかを管理する変数 </summary>
    ability abilityMode = ability.ATTACK;
    /// <summary> アビリティキャンセルする時のクールタイム </summary>
    float abilityChooseTime = 0;
    /// <summary> アビリティキャンセルする時のクールタイムの設定 </summary>
    public float abilityChooseTimeStatus = 0.1f;
    /// <summary> 攻撃から選択を遷移するためのクールタイム </summary>
    float abilityChangeAttackTime = 0.5f;
    /// <summary> 回復から選択を遷移するためのクールタイム </summary>
    float abilityChangeHealTime = 0.5f;

    bool isKinematicInitially; // kinematicを保存しておくフラグ

    float horizontalInput; // キーの値を受け取る変数

    // ノックバック
    /// <summary> ノックバックする速さの設定 </summary>
    public float KnockbackSpeedStatus = 20;
    /// <summary> ノックバックする速さ </summary>
    float KnockbackSpeed = 0;
    /// <summary> ノックバックする速さに対しての加速度 </summary>
    public float knockbackAcceleration = 1;
    /// <summary> ノックバックフラグ </summary>
    bool onKnockback = false;
    /// <summary> ノックバック後、再び操作可能になるまでの時間の設定 </summary>
    public float canMoveTimeStatus = 0.5f;
    /// <summary> ノックバック後、再び操作可能になるまでの時間 </summary>
    float canMoveTime = 0.5f;
    /// <summary> 当たった方向を受け取る用の変数 </summary>
    Vector3 colPos = Vector3.zero;
    /// <summary> 無敵時間フラグ </summary>
    bool noDamage = false;
    /// <summary> 無敵時間の設定 </summary>
    public float noDamageTimeStatus = 1;
    /// <summary> 無敵時間 </summary>
    float noDamageTime = 1;
    /// <summary> 点滅の速さ </summary>
    public float blinkSpeed = 0.1f;

    /// <summary> 操作できるか確認するフラグ </summary>
    public bool canPlay = true;
    /// <summary> 操作できるか確認するフラグ </summary>
    public float cantPlayTime = 3;

    /// <summary> 死亡フラグ </summary>
    public bool death = false;

    void Start()
    {
        // コンポーネント
        this.ridid2d = GetComponent<Rigidbody2D>();
        rendere = GetComponent<Renderer>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        // ゲームオブジェクト
        director = GameObject.Find("gameDirector");             // UI
        abilityAttackImage = GameObject.Find("ability-attack"); // アビリティ選択(攻撃)
        abilityAttackImage.SetActive(false);
        abilityHealImage = GameObject.Find("ability-heal");     // アビリティ選択(回復)
        abilityHealImage.SetActive(false);
        abilityGage = GameObject.Find("abilityGage");           // アビリティゲージ
        load = GameObject.Find("Load");                         // ボスへの遷移
        if (load)
        {
            load.SetActive(false);
        }
        boss1 = GameObject.Find("Boss1");

        // スクリプト
        gameDirectorScript = GetComponent<GameDirector>(); // UI
        if (boss1)
        {
            bossScript = boss1.GetComponent<AttackPattern1>();
        }

        isKinematicInitially = ridid2d.isKinematic; // kinematicフラグの保存
    }

    // Update is called once per frame
    void Update()
    {
        if (canPlay)
        {
            if (!onAbility && !isBacklash)
            {
                if (!onKnockback)
                {
                    // 移動
                    Move();

                    // 向き
                    Distance();

                    // ジャンプ
                    if (canPushJanp)
                    {
                        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump")) && isGround)
                        {
                            ridid2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                        }
                    }
                }

                ridid2d.GetComponent<Rigidbody2D>().velocity = ridid2d.GetComponent<Rigidbody2D>().velocity;
                ridid2d.GetComponent<Rigidbody2D>().isKinematic = false;
            }
            else
            {
                if (onAbility)
                {
                    // アビリティ選択中は自機を静止
                    ridid2d.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    ridid2d.GetComponent<Rigidbody2D>().isKinematic = true;
                }
                else
                {
                    ridid2d.GetComponent<Rigidbody2D>().velocity = new Vector2(0, ridid2d.velocity.y);
                    ridid2d.GetComponent<Rigidbody2D>().isKinematic = false;
                }
            }

            // 時間で反動フラグをfalseに
            if (backlash <= 0)
            {
                isBacklash = false;
            }
            if (backlash > 0 && !isBreak)
            {
                backlash -= Time.deltaTime;
            }

            // 自機が地面と触れているかチェック
            isGround = false;
            isGround = Physics2D.Raycast(transform.position, Vector2.down, 2.0f, LayerMask.GetMask("Ground"));

            // HPアイコン
            director.GetComponent<GameDirector>().HpIcons(damage);

            // 攻撃呼び出し
            if (attackMode == AttackMode.NORMAL)
            {
                if (!isBacklash)
                {
                    // 通常攻撃
                    Attack();
                }
            }
            else if (attackMode == AttackMode.BREAK)
            {
                // 破壊攻撃
                Break();
            }

            // 攻撃モード確認
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
                    attackMode = AttackMode.NORMAL;
                }
                else if (attackCharge >= attackChargeMax)
                {
                    // 破壊攻撃
                    attackMode = AttackMode.BREAK;
                }
                else
                {
                    // 無し
                    attackMode = AttackMode.NON;
                }
            }
            else { attackCharge = 0; }

            // アビリティ
            Ability();

            // ゲームオーバー
            if (HP <= 0)
            {
                death = true;
            }

            if (death)
            {
                GameDirector.width = 0;
                HP = HPMAX;
                damage = 0;
                SceneManager.LoadScene("GameOver");
            }
            if (boss1)
            {
                if (bossScript.isDeath)
                {
                    HP = HPMAX;
                    damage = 0;
                }
            }
        }
        else
        {
            cantPlayTime -= Time.deltaTime;
            if(cantPlayTime < 0)
            {
                canPlay = true;
            }
        }

        anim.SetFloat("charge", attackCharge);

        // デバッグ
        Debugg();
    }

    private void FixedUpdate()
    {
        if (canPlay)
        {
            // ノックバック
            Knockback();
            // 無敵時間
            NoDamage();
        }
    }

    /// <summary>
    /// 移動、ダッシュ
    /// </summary>
    void Move()
    {
        if (!isBacklash)
        {
            // 左右キー入力
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
                if (right)
                {
                    rightDash = true;
                }
                if (!right)
                {
                    leftDash = true;
                }
            }

            if (canDash)
            {
                canPushJanp = false;
                dashSpeed = dashPower;
                if (horizontalInput != 0)
                {
                    ridid2d.velocity = new Vector2(horizontalInput * dashSpeed, 0);
                }

                // ダッシュ終わり処理
                dashTimer -= Time.deltaTime;
                if (dashTimer < 0)
                {
                    dashCoolTime = dashCoolTimeStatus;
                    canDash = false;
                }

                // ダッシュ中に反対方向向くとダッシュを強制解除
                if (rightDash)
                {
                    if (!right)
                    {
                        rightDash = false;
                        canDash = false;
                    }
                }
                else if (leftDash)
                {
                    if (right)
                    {
                        leftDash = false;
                        canDash = false;
                    }
                }
            }
            else
            {
                canPushJanp = true;
                dashTimer = dashTimerStatus;
                dashCoolTime -= Time.deltaTime;
                // 移動
                ridid2d.velocity = new Vector2(horizontalInput * moveSpeed, ridid2d.velocity.y);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //if (other.gameObject.tag == "Enemy")
        //{
        //    HP -= 1;
        //    damage += 1;
        //}
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "PlayerAbilityAttack")
        {
            HP -= 1;
            damage += 1;
        }
        if(other.gameObject.tag == "PlayerAbilityHeal")
        {
            if(HP < HPMAX)
            {
                HP += 1;
                damage -= 1;
            }
        }
        if (other.gameObject.tag == "Enemy")
        {
            if (!noDamage)
            {
                HP -= 1;
                damage += 1;
                if (!onKnockback)
                {
                    colPos = other.transform.position;
                    onKnockback = true;
                }
                noDamage = true;
            }
        }
        if (other.gameObject.tag == "Killzone")
        {
            death = true;
            SceneManager.LoadScene("GameOver");
        }
        if (other.gameObject.tag == "ToBoss1")
        {
            OPStageDirector.retryPointer = true;

            if (load)
            {
                load.SetActive(true);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        //if (other.gameObject.tag == "Enemy")
        //{
        //    if (!noDamage)
        //    {
        //        HP -= 1;
        //        damage += 1;
        //        if (!onKnockback)
        //        {
        //            colPos = other.transform.position;
        //            onKnockback = true;
        //        }
        //        noDamage = true;
        //    }
        //}
    }

    /// <summary>
    /// ノックバック
    /// </summary>
    void Knockback()
    {
        if (onKnockback)
        {
            Vector2 knockbackDirection = (transform.position - colPos).normalized;
            ridid2d.velocity = new Vector2(knockbackDirection.x * KnockbackSpeed, ridid2d.velocity.y);
            if (KnockbackSpeed > 0)
            {
                KnockbackSpeed -= knockbackAcceleration;
            }
            else
            {
                KnockbackSpeed = 0;
                canMoveTime -= Time.deltaTime;
                if (canMoveTime < 0)
                {
                    onKnockback = false;
                }
            }
        }
        else
        {
            canMoveTime = canMoveTimeStatus;
            KnockbackSpeed = KnockbackSpeedStatus;
        }
    }

    /// <summary>
    /// 無敵時間
    /// </summary>
    void NoDamage()
    {
        if (noDamage)
        {
            noDamageTime -= Time.deltaTime;
            if (noDamageTime < 0)
            {
                noDamage = false;
            }

            // 点滅
            float alpha = Mathf.PingPong(Time.time * blinkSpeed, 1.0f);
            Color newColor = rendere.material.color;
            newColor.a = alpha;
            rendere.material.color = newColor;
        }
        else
        {
            noDamageTime = noDamageTimeStatus;
            Color newColor = rendere.material.color;
            newColor.a = 1.0f;
            rendere.material.color = newColor;
        }
    }

    /// <summary>
    /// 通常攻撃
    /// </summary>
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
            // スプライトを攻撃モーションに切り替え
            spriteRenderer.sprite = attackSprite;

            attackModeChange = false;            
        }
        else
        {
            // スプライトを元に戻す
            spriteRenderer.sprite = idleSprite;

            attackModeChange = true;
        }
    }

    /// <summary>
    /// 破壊攻撃
    /// </summary>
    void Break()
    {
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
            // スプライトを破壊モーションに切り替え
            spriteRenderer.sprite = breakSprite;

            isBacklash = true;
            backlash = backLashStatus;

            attackModeChange = false;
        }
        else
        {
            // スプライトを元に戻す
            spriteRenderer.sprite = idleSprite;

            attackModeChange = true;
        }
    }

    /// <summary>
    /// 向き
    /// </summary>
    void Distance()
    {
        if (!isBacklash)
        {
            // キーで向きを確認
            if (Input.GetKey(KeyCode.RightArrow) || horizontalInput > 0)
            {
                distance = rightDistance;
                right = true;
            }
            if (Input.GetKey(KeyCode.LeftArrow) || horizontalInput < 0)
            {
                distance = leftDistance;
                right = false;
            }

            if (distance != 0)
            {
                transform.localScale = new Vector3(distance, 0.4f, 1);
            }
        }
    }

    /// <summary>
    /// アビリティ
    /// </summary>
    void Ability()
    {
        abilityAttackImage.transform.position =
            new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
        abilityHealImage.transform.position =
            new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);

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
            horizontalInput = Input.GetAxis("Horizontal");

            // 他オブジェクトの動きを止める
            for (int i = 0; i < abilityStopObj.Length; i++)
            {
                if (abilityStopObj[i] != null)
                {
                    abilityStopObj[i].GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                    abilityStopObj[i].GetComponent<Rigidbody2D>().isKinematic = true;
                }
            }

            // アビリティ効果選択
            switch (abilityMode)
            {
                case ability.ATTACK:
                    abilityAttackImage.SetActive(true);
                    abilityHealImage.SetActive(false);

                    abilityChangeHealTime = 0.2f;
                    abilityChangeAttackTime -= Time.deltaTime;

                    if (abilityChangeAttackTime < 0 && 
                        ((Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow)) ||
                        (horizontalInput > 0.1f || horizontalInput < -0.1f)))
                    {
                        abilityMode = ability.HEAL;
                    }

                    if (abilityChooseTime <= 0 && (Input.GetKeyDown(KeyCode.X) || Input.GetButtonDown("Ability")))
                    {
                        abilityAttackImage.SetActive(false);
                        isAbilityAttack = true;
                        onAbility = false;
                    }

                    if (Input.GetKeyDown(KeyCode.Z) || Input.GetButtonDown("Attack"))
                    {
                        abilityAttackImage.SetActive(false);
                        abilityHealImage.SetActive(false);
                        onAbility = false;
                    }

                    break;

                case ability.HEAL:
                    abilityHealImage.SetActive(true);
                    abilityAttackImage.SetActive(false);

                    abilityChangeAttackTime = 0.2f;
                    abilityChangeHealTime -= Time.deltaTime;

                    if (abilityChangeHealTime < 0 && (Input.GetKeyDown(KeyCode.LeftArrow) ||
                        (horizontalInput < -0.1f || horizontalInput > 0.1f)))
                    {
                        abilityMode = ability.ATTACK;
                    }

                    if (abilityChooseTime <= 0 && (Input.GetKeyDown(KeyCode.X) || Input.GetButtonDown("Ability")))
                    {
                        abilityHealImage.SetActive(false);
                        isAbilityHeal = true;
                        onAbility = false;
                    }

                    if (Input.GetKeyDown(KeyCode.Z) || Input.GetButtonDown("Attack"))
                    {
                        abilityAttackImage.SetActive(false);
                        abilityHealImage.SetActive(false);
                        onAbility = false;
                    }

                    break;
            }

            abilityChooseTime -= Time.deltaTime;
        }
        else
        {
            for (int i = 0; i < abilityStopObj.Length; i++)
            {
                if (abilityStopObj[i] != null)
                {
                    abilityStopObj[i].GetComponent<Rigidbody2D>().velocity =
                        abilityStopObj[i].GetComponent<Rigidbody2D>().velocity;
                    abilityStopObj[i].GetComponent<Rigidbody2D>().isKinematic = isKinematicInitially;
                }
            }
        }

        // アビリティ発動後
        if (isAbilityAttack)
        {
            GameDirector.width = 0;
            GameObject attackAbilityObj = Instantiate(abilityAttackPrefab);
            isAbilityAttack = false;
        }
        else if (isAbilityHeal)
        {
            GameDirector.width = 0;
            GameObject attackHealObj = Instantiate(abilityHealPrefab);
            isAbilityHeal = false;
        }
    }

    /// <summary>
    /// デバッグ
    /// </summary>
    void Debugg()
    {
        //Debug.Log("自機HP: " + HP);
        //Debug.Log("ダメージ: " + damage);
        //if (!attackSprite)
        //{
        //    Debug.Log("スプライトがありません");
        //}
        //
        Debug.Log("アビリティ攻撃:" + isAbilityAttack);
        Debug.Log("アビリティ回復:" + isAbilityHeal);
    }
}