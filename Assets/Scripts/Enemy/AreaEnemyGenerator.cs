using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEnemyGenerator : MonoBehaviour
{
    // ゲームオブジェクト
    /// <summary> 敵オブジェクト </summary>
    public GameObject EnemyPrefab;
    /// <summary> 自機オブジェクト </summary>
    GameObject player;

    /// <summary> 敵発生までのスパン </summary>
    public float span = 1.0f;
    /// <summary> 敵発生までのカウント </summary>
    float delta = 0;

    // Start is called before the first frame update
    void Start()
    {
        // ゲームオブジェクト
        // 自機オブジェクト
        player = GameObject.Find("player");

        // 出現位置
        EnemyPrefab.transform.position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // 一定スパンで敵生成
        if (player.transform.position.x >= 70 && player.transform.position.x <= 145)
        {
            delta += Time.deltaTime;
            if (delta > span)
            {
                delta = 0;
                GameObject Enemy = Instantiate(EnemyPrefab);
            }
        }
    }
}
