using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHealRangeController : MonoBehaviour
{
    GameObject abilityHeal; // 回復効果オブジェクト

    public float deathTime = 2; // 消滅までの時間

    public int heal = 5; // 回復力

    // Start is called before the first frame update
    void Start()
    {
        abilityHeal = GameObject.Find("abilityHealPrefab(Clone)");
        transform.position = abilityHeal.transform.position;
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
