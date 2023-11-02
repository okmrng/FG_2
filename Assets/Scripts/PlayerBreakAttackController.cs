using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBreakAttackController : MonoBehaviour
{ 
    GameObject director;           // 監督オブジェクト
    GameObject player;             // 自機オブジェクト
    PlayerController playerScript; // 自機スクリプト

    float breakEndTime = 0;              // 攻撃時間
    public float breakEndTimeStatus = 1; // 攻撃時間

    // Start is called before the first frame update
    void Start()
    {
        director = GameObject.Find("gameDirector");             // 監督オブジェクト
        player = GameObject.Find("player");                     // 自機オブジェクト
        playerScript = player.GetComponent<PlayerController>(); // 自機スクリプト

        breakEndTime = breakEndTimeStatus;

        // 自機の向きによって出現位置を変える
        if (playerScript.distance == 0.8f)
        {
            transform.position = new Vector3(player.transform.position.x + 1,
                player.transform.position.y + 0.3f, player.transform.position.z);
        }
        else if (playerScript.distance == -0.8f)
        {
            transform.position = new Vector3(player.transform.position.x - 1,
               player.transform.position.y + 0.3f, player.transform.position.z);
        }
    }

    // Update is called once per frame
    public void Update()
    {
        breakEndTime -= Time.deltaTime;
        // 一定時間経過するか、キーを入力したら解放
        if (breakEndTime < 0)
        {
            playerScript.isBreak = false;
            Destroy(gameObject);
        }
    }
}
