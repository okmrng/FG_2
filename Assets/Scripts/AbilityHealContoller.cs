using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHealContoller : MonoBehaviour
{
    GameObject player;                          // 自機
    PlayerController playerScript;              // 自機スクリプト
    public GameObject abilityHealRangePrefab;   // 攻撃範囲

    public float speedStatus = 3;
    float speed = 0; // 速度

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player");
        playerScript = player.GetComponent<PlayerController>();
        transform.position = player.transform.position;

        if (playerScript.distance == 0.8f)
        {
            speed = speedStatus;
        }
        else if (playerScript.distance == -0.8f)
        {
            speed = -speedStatus;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) || Input.GetButtonDown("Ability"))
        {
            GameObject abilityHealRangeObj = Instantiate(abilityHealRangePrefab);
        }
    }

    // 速さ固定
    private void FixedUpdate()
    {
        transform.Translate(speed, 0, 0);
    }

    // 画面外に出たら解放
    void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
