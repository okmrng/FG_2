using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityAttackRangeController : MonoBehaviour
{
    GameObject abilityAttack; // アタック効果オブジェクト

    public float deathTime = 2; // 消滅までの時間

    public int power = 5; // 攻撃力

    // Start is called before the first frame update
    void Start()
    {
        abilityAttack = GameObject.Find("abilityAttackPrefab(Clone)");
        transform.position = abilityAttack.transform.position;
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
