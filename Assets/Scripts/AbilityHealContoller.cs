using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHealContoller : MonoBehaviour
{
    GameObject player; // 自機
    public GameObject abilityHealRangePrefab; // 攻撃範囲

    public float speed = 0.2f; // 速度

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player");
        transform.position = player.transform.position;
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
