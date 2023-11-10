using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityAttackRangeController : MonoBehaviour
{
    // ゲームオブジェクト
    /// <summary> アビリティ(攻撃)オブジェクト </summary>
    GameObject abilityAttack;

    /// <summary> 消えるまでの時間 </summary>
    public float deathTime = 2;

    /// <summary> 攻撃力 </summary>
    public int power = 5;

    // Start is called before the first frame update
    void Start()
    {
        // ゲームオブジェクト
        // アビリティ(回復)オブジェクト
        abilityAttack = GameObject.Find("abilityAttackPrefab(Clone)");

        // 本体の位置で出現
        transform.position = abilityAttack.transform.position;
        // 本体を解放
        Destroy(abilityAttack);
    }

    // Update is called once per frame
    void Update()
    {
        deathTime -= Time.deltaTime;
        if( deathTime < 0 )
        {
            Destroy(gameObject);
        }
    }
}
