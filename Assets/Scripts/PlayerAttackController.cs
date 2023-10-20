using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{ 
    GameObject director; // �ēI�u�W�F�N�g

    GameObject player; // ���@

    // �U��
    public bool isAttack = false;         // �U���t���O
    public int attackPower = 5;           // �p���[
    float attackEndTime = 0;              // �U������
    public float attackEndTimeStatus = 1; // �U������

    // Start is called before the first frame update
    void Start()
    {
        director = GameObject.Find("gameDirector"); // �ēI�u�W�F�N�g
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
