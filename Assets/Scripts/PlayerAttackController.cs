using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{ 
    GameObject director;           // 監督オブジェクト
    GameObject player;             // 自機オブジェクト
    PlayerController playerScript; // 自機スクリプト

    float attackEndTime = 0;              // 攻撃時間
    public float attackEndTimeStatus = 1; // 攻撃時間

    // Start is called before the first frame update
    void Start()
    {
        director = GameObject.Find("gameDirector");             // 監督オブジェクト
        player = GameObject.Find("player");                     // 自機オブジェクト
        playerScript = player.GetComponent<PlayerController>(); // 自機スクリプト

        attackEndTime = attackEndTimeStatus;

        if (playerScript.distance == 0.8f)
        {
            transform.position = new Vector3(player.transform.position.x + 1, 
                player.transform.position.y, player.transform.position.z);
        }
        else if(playerScript.distance == -0.8f)
        {
            transform.position = new Vector3(player.transform.position.x + -1.58f,
               player.transform.position.y, player.transform.position.z);
        }
    }

    // Update is called once per frame
    public void Update()
    {
        attackEndTime -= Time.deltaTime;
        if (attackEndTime < 0)
        {
            playerScript.isAttack = false;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            director.GetComponent<GameDirector>().increaseAbility();
        }
    }
}
