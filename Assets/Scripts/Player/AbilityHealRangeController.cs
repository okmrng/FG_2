using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHealRangeController : MonoBehaviour
{
    // ゲームオブジェクト
    /// <summary> アビリティ(回復)オブジェクト </summary>
    GameObject abilityHeal;

    /// <summary> 消えるまでの時間 </summary>
    public float deathTime = 2;

    /// <summary> 回復量 </summary>
    public int heal = 5; 

    // Start is called before the first frame update
    void Start()
    {
        // ゲームオブジェクト
        // アビリティ(回復)オブジェクト
        abilityHeal = GameObject.Find("abilityHealPrefab(Clone)");

        // 本体の位置で出現
        transform.position = abilityHeal.transform.position;
        // 本体を解放
        Destroy(abilityHeal);
    }

    // Update is called once per frame
    void Update()
    {
        deathTime -= Time.deltaTime;
        if (deathTime < 0)
        {
            Destroy(gameObject);
        }
    }
}
