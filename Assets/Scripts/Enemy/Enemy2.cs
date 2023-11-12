using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
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

    public float speed = 3.0f;  // 移動速度
    public float amplitude = 1.0f;  // 波の振幅
    public float frequency = 1.0f;  // 波の周波数

    /// <summary> HPの最大値 </summary>
    public int HPMax = 1;
    /// <summary> HPの現在値 </summary>
    public int HP = 1;

    private Vector3 initialPosition;
    // Start is called before the first frame update
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

    // Update is called once per frame
    void Update()
    {
        // 横方向の移動（左から右への波状移動）
        //float horizontalMovement = Mathf.Sin(Time.time * frequency) * amplitude;
        //Vector3 newPosition = initialPosition + Vector3.right * horizontalMovement;

        //// 上下方向への移動（任意の高さを保つ）
        //newPosition.y = initialPosition.y;

        //// 移動
        //transform.position = Vector3.MoveTowards(transform.position, newPosition, speed * Time.deltaTime);

        // 縦方向の移動（上から下への波状移動）
        float verticalMovement = Mathf.Sin(Time.time * frequency) * amplitude;
        Vector3 newPosition = initialPosition + Vector3.up * verticalMovement;

        // 横方向への移動
        newPosition.x = initialPosition.x;

        // 移動
        transform.position = Vector3.MoveTowards(transform.position, newPosition, speed * Time.deltaTime);

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
