using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy4Bullet : MonoBehaviour
{
    // ゲームオブジェクト
    /// <summary> 自機オブジェクト </summary>
    GameObject player;

    // スクリプト
    /// <summary> 自機スクリプト </summary>
    PlayerController playerController;

    /// <summary> 速度の設定 </summary>
    public float speedStatus = -0.3f;
    /// <summary> 速度 </summary>
    float speed = 0;

    // Start is called before the first frame update
    void Start()
    {
        // ゲームオブジェクト
        player = GameObject.Find("player"); // 自機オブジェクト

        // スクリプト
        playerController = player.GetComponent<PlayerController>(); // 自機スクリプト
    }

    // Update is called once per frame
    void Update()
    {
        // アビリティ選択中は止める
        if (playerController.onAbility)
        {
            speed = 0;
        }
        else { speed = speedStatus; }
    }

    private void FixedUpdate()
    {
        // 移動
        transform.position = new Vector3(transform.position.x + speed, transform.position.y, transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}