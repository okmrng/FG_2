using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHealRangeController : MonoBehaviour
{
    GameObject abilityHeal; // �񕜌��ʃI�u�W�F�N�g

    public float deathTime = 2; // ���ł܂ł̎���

    public int heal = 5; // �񕜗�

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
