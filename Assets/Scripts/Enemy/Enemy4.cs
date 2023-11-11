using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy4 : MonoBehaviour
{
    // ゲームオブジェクト
    /// <summary> 敵オブジェクト </summary>
    public GameObject EnemyPrefab;
    /// <summary> 自機オブジェクト </summary>
    GameObject player;
    /// <summary> 攻撃アビリティオブジェクト </summary>
    public GameObject abilityAttack;
    /// <summary> 回復アビリティオブジェクト </summary>
    public GameObject abilityHeal;

    // スクリプト
    /// <summary> 自機スクリプト </summary>
    PlayerController playerScript;
    /// <summary> 攻撃アビリティスクリプト </summary>
    AbilityAttackRangeController attackRangeController;
    /// <summary> 回復アビリティスクリプト </summary>
    AbilityHealRangeController healRangeController;

    /// <summary> 敵発生までのスパン </summary>
    public float span = 1.0f;
    /// <summary> 敵発生までのカウント </summary>
    float delta = 0;

    /// <summary> 敵発生を開始させる自機の位置 </summary>
    public float startInstant = 153;
    /// <summary> 敵発生を終わらせる自機の位置 </summary>
    public float endInstant = 173;

    /// <summary> HPの最大値 </summary>
    public int HPMax = 10;
    /// <summary> HPの現在値 </summary>
    public int HP = 10;

    /// <summary> 破壊攻撃が当たったか管理するフラグ </summary>
    bool isBreak = false;

    // Start is called before the first frame update
    void Start()
    {
        // ゲームオブジェクト
        // 自機オブジェクト
        player = GameObject.Find("player");

        // スクリプト
        playerScript = player.GetComponent<PlayerController>();                             // 自機スクリプト
        if (abilityAttack)
        {
            attackRangeController = abilityAttack.GetComponent<AbilityAttackRangeController>(); // 攻撃アビリティのスクリプト
        }
        if (abilityHeal)
        {
            healRangeController = abilityHeal.GetComponent<AbilityHealRangeController>();       // 回復アビリティのスクリプト
        }

        delta = span;
    }

    // Update is called once per frame
    void Update()
    {
        if (HP > HPMax)
        {
            HP = HPMax;
        }

        if (HP <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        // 一定スパンで敵生成
        if (player.transform.position.x >= startInstant && player.transform.position.x <= endInstant)
        {
            delta += Time.deltaTime;
            if (delta > span)
            {
                delta = 0;
                if (EnemyPrefab)
                {
                    GameObject Enemy = Instantiate(EnemyPrefab, this.transform.position, Quaternion.identity);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerAttack")
        {
            if (!isBreak)
            {
                HP -= playerScript.attackPower;
            }
            else { HP -= playerScript.attackPower * 3; }
        }
        if (collision.gameObject.tag == "PlayerBreakAttack")
        {
            HP -= playerScript.breakPower;
            if (!isBreak)
            {
                isBreak = true;
            }
        }
        if (collision.gameObject.tag == "PlayerAbilityAttack")
        {
            HP -= attackRangeController.power;
        }
        if (collision.gameObject.tag == "PlayerAbilityHeal")
        {
            if (HP < HPMax)
            {
                HP -= healRangeController.heal;
            }
        }
    }
}
