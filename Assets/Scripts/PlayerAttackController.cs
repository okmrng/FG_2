using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{ 
    GameObject director; // 監督オブジェクト

    GameObject player; // 自機

    // 攻撃
    public bool isAttack = false;         // 攻撃フラグ
    public int attackPower = 5;           // パワー
    float attackEndTime = 0;              // 攻撃時間
    public float attackEndTimeStatus = 1; // 攻撃時間

    // Start is called before the first frame update
    void Start()
    {
        director = GameObject.Find("gameDirector"); // 監督オブジェクト
    }

    // Update is called once per frame
    public void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            director.GetComponent<GameDirector>().increaseAbility();
        }
    }
}
