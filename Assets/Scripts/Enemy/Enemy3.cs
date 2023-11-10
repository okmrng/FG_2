using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3 : MonoBehaviour
{
    // ゲームオブジェクト
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

    public float speed = 10.0f;  // 移動速度
    public float amplitude = 2.0f;  // 揺れの振幅
    public float frequency = 4.0f;  // 揺れの周波数

    private Vector3 initialPosition;

    /// <summary> HPの最大値 </summary>
    public int HPMax = 1;
    /// <summary> HPの現在値 </summary>
    public int HP = 1;

    void Start()
    {
        // ゲームオブジェクト
        player = GameObject.Find("player"); // 自機オブジェクト

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

        initialPosition = transform.position;
    }

    void Update()
    {
        // 横方向の移動
        float horizontalMovement = speed * Time.deltaTime;
        Vector3 newPosition = transform.position + Vector3.left * horizontalMovement;

        // 上下方向への揺れ
        float verticalMovement = Mathf.Sin(Time.time * frequency) * amplitude;
        newPosition.y = initialPosition.y + verticalMovement;

        // 移動
        transform.position = Vector3.MoveTowards(transform.position, newPosition, speed * Time.deltaTime);

        if (playerScript.onAbility)
        {
            speed = 0;
            amplitude = 0;
        }
        else
        {
            speed = 10;
            amplitude = 2;
        }

        if (HP > HPMax)
        {
            HP = HPMax;
        }

        if (HP <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerAttack")
        {
            HP -= playerScript.attackPower;
        }
        if (collision.gameObject.tag == "PlayerBreakAttack")
        {
            HP -= playerScript.breakPower;
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