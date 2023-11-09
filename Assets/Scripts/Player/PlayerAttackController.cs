using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{ 
　　// オブジェクト
    /// <summary> UIオブジェクト </summary>
    GameObject director;
    /// <summary> 自機オブジェクト </summary>
    GameObject player;

    // スクリプト
    /// <summary> 自機スクリプト </summary>
    PlayerController playerScript;

    /// <summary> 消えるまでの時間 </summary>
    float attackEndTime = 0;
    /// <summary> 消えるまでの時間の設定 </summary>
    public float attackEndTimeStatus = 1;

    // Start is called before the first frame update
    void Start()
    {
        // オブジェクト
        director = GameObject.Find("gameDirector");             // UIオブジェクト
        player = GameObject.Find("player");                     // 自機オブジェクト

        // スクリプト
        playerScript = player.GetComponent<PlayerController>(); // 自機スクリプト

        attackEndTime = attackEndTimeStatus;

        // 向きを確認して出現する位置を決める
        if (playerScript.distance == 0.8f)
        {
            transform.position = new Vector3(player.transform.position.x + 0.8f, 
                player.transform.position.y, player.transform.position.z);
        }
        else if(playerScript.distance == -0.8f)
        {
            transform.position = new Vector3(player.transform.position.x - 0.8f,
               player.transform.position.y, player.transform.position.z);
        }
    }

    // Update is called once per frame
    public void Update()
    {
        if (!playerScript.onAbility)
        {
            attackEndTime -= Time.deltaTime;
        }
        // 時間で解放
        if (attackEndTime < 0 || Input.GetKeyDown(KeyCode.Z) || Input.GetButtonDown("Attack"))
        {
            playerScript.isAttack = false;
            Destroy(gameObject);
        }
    }
}
