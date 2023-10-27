using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    GameObject player;                              // ���@�I�u�W�F�N�g
    PlayerController playerScript;                  // ���@�̃X�N���v�g
    public GameObject abilityAttackPrefab;          // �A�r���e�B�̍U���I�u�W�F�N�g�v���t�@�u
    AbilityAttackRangeController attackRangeScript; // �A�r���e�B�̍U���͈͂̃X�N���v�g

    public int HP = 5;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player");                                                   // �I�u�W�F�N�g
        playerScript = player.GetComponent<PlayerController>();                               // �X�N���v�g
        attackRangeScript = abilityAttackPrefab.GetComponent<AbilityAttackRangeController>(); // �A�r���e�B�̍U���͈͂̃X�N���v�g
    }

    // Update is called once per frame
    void Update()
    {
        if (HP <= 0)
        {
            Destroy(gameObject);
        }

        Debug.Log("�G��HP�F" + HP);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "PlayerAttack")
        {
            HP -= playerScript.attackPower;
        }
        if (other.gameObject.tag == "PlayerBreakAttack")
        {
            HP -= playerScript.breakPower;
        }
        if(other.gameObject.tag == "PlayerAbilityAttack")
        {
            HP -= attackRangeScript.power;
        }
    }
}
