using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityAttckContoller : MonoBehaviour
{
    // ゲームオブジェクト
    /// <summary> 自機オブジェクト </summary>
    GameObject player;
    /// <summary> アビリティ(攻撃)の範囲オブジェクト </summary>
    public GameObject abilityAttackRangePrefab;

    // スクリプト
    /// <summary> 自機スクリプト </summary>
    PlayerController playerScript;

    /// <summary> 速度の設定 </summary>
    public float speedStatus = 3;
    /// <summary> 速度 </summary>
    float speed = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player");
        playerScript = player.GetComponent<PlayerController>();
        transform.position = player.transform.position;

        if(playerScript.distance == playerScript.rightDistance)
        {
            speed = speedStatus;
        }
        else if(playerScript.distance == playerScript.leftDistance)
        {
            speed = -speedStatus;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) || Input.GetButtonDown("Ability"))
        {
            GameObject abilityAttackRangeObj = Instantiate(abilityAttackRangePrefab);
        }
    }

    // 等速更新
    private void FixedUpdate()
    {
        transform.Translate(speed, 0, 0);
    }

    // 解放
    void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
