using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Enemy1 : MonoBehaviour
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
    PlayerController playerCon;
    /// <summary> 攻撃アビリティスクリプト </summary>
    AbilityAttackRangeController abilityAttackScript;
    /// <summary> 回復アビリティスクリプト </summary>
    AbilityHealRangeController abilityHealScript;

    public float speed = 3.0f;
    public bool isToRight = false;
    //public float revTime = 0;
    public LayerMask Ground;
    
    float time = 0;

    /// <summary> HP現在値 </summary>
    public int HP = 10;
    /// <summary> HP最大値 </summary>
    public int HPMax = 10;

    // Start is called before the first frame update
    void Start()
    {
        // ゲームオブジェクト
        player = GameObject.Find("player");

        // スクリプト
        playerCon = player.GetComponent<PlayerController>();                              // 自機スクリプト
        abilityAttackScript = abilityAttack.GetComponent<AbilityAttackRangeController>(); // 攻撃アビリティのスクリプト
        abilityHealScript = abilityHeal.GetComponent<AbilityHealRangeController>(); // 回復アビリティのスクリプト

        if (isToRight)
        {
            transform.localScale = new Vector2(-1, 1);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (!isToRight && player.transform.position.x > transform.position.x)
        {
            time++;
            if (time > 90)
            {
                isToRight = !isToRight;
                time = 0;
            }
        }
        if (isToRight && player.transform.position.x < transform.position.x)
        {
            time++;
            if (time > 90)
            {
                isToRight = !isToRight;
                time = 0;
            }
        }

        if (HP > HPMax)
        {
            HP = HPMax;
        }

        if (HP <= 0)
        {
            Destroy(gameObject);
        }
        // if(revTime > 0)
        // {
        //     time += Time.deltaTime;
        //     if(time>= revTime)
        //     {
        //       isToRight = !isToRight;
        //       time = 0;
        //       if(isToRight)
        //       {
        //         transform.localScale = new Vector2(-1,1);
        //       }
        //       else
        //       {
        //         transform.localScale = new Vector2(1,1);
        //       }
        //     }
        // }
    }

    void FixedUpdate()
    {
        bool onGround = Physics2D.CircleCast(transform.position,0.5f,Vector2.down,0.5f,Ground);
        if(onGround && !playerCon.onAbility)
        {
          Rigidbody2D rbody = GetComponent<Rigidbody2D>();
          if(isToRight)
          {
            rbody.velocity = new Vector2(speed,rbody.velocity.y);
          }
          else
          {
            rbody.velocity =  new Vector2(-speed,rbody.velocity.y);
          }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "PlayerAttack")
        {
            HP -= playerCon.attackPower;
        }
        if (other.gameObject.tag == "PlayerBreakAttack")
        {
            HP -= playerCon.breakPower;
        }
        if(other.gameObject.tag == "PlayerAbilityAttack")
        {
            HP -= abilityAttackScript.power;
        }
        if (other.gameObject.tag == "PlayerAbilityHeal")
        {
            if(HP < HPMax)
            {
                HP += abilityHealScript.heal;
            }
        }

        isToRight = !isToRight;
        time = 0;
        if (isToRight)
        {
            transform.localScale = new Vector2(-0.5f,0.5f);
        }
         else
        {
            transform.localScale = new Vector2(0.5f,0.5f);
        }
    }
}
