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

    /// <summary> 敵発生を開始させる自機の位置 </summary>
    public float startInstant = 70;
    /// <summary> 敵発生を終わらせる自機の位置 </summary>
    public float endInstant = 145;

    // Start is called before the first frame update
    void Start()
    {
        // ゲームオブジェクト
        // 自機オブジェクト
        player = GameObject.Find("player");

        delta = span;
    }

    // Update is called once per frame
    void Update()
    {
        // 一定スパンで敵生成
        if (player.transform.position.x >= startInstant && player.transform.position.x <= endInstant)
        {
            delta += Time.deltaTime;
            if (delta > span)
            {
                delta = 0;
                if (EnemyPrefab)
                {
                    GameObject Enemy = Instantiate(EnemyPrefab, this.transform.position, Quaternion.identity);
                }
            }
        }
    }
}
