using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    GameObject player;                              // 自機オブジェクト
    PlayerController playerScript;                  // 自機のスクリプト
    AbilityAttackRangeController attackRangeScript; // アビリティの攻撃範囲のスクリプト

    public int HP = 5;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player");                     // オブジェクト
        playerScript = player.GetComponent<PlayerController>(); // スクリプト
    }

    // Update is called once per frame
    void Update()
    {
        if (HP <= 0)
        {
            Destroy(gameObject);
        }

        Debug.Log("敵のHP：" + HP);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "PlayerAttack")
        {
            HP -= playerScript.attackPower;
        }
        if (other.gameObject.tag == "PlayerBreakAttack")
        {
            HP -= playerScript.breakPower;
        }
    }
}
