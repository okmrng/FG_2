using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityAttackRangeController : MonoBehaviour
{
    GameObject abilityAttack; // �A�^�b�N���ʃI�u�W�F�N�g

    public float deathTime = 2; // ���ł܂ł̎���

    public int power = 5; // �U����

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
