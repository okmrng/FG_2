using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{ 
    GameObject director;           // �ēI�u�W�F�N�g
    GameObject player;             // ���@�I�u�W�F�N�g
    PlayerController playerScript; // ���@�X�N���v�g

    float attackEndTime = 0;              // �U������
    public float attackEndTimeStatus = 1; // �U������

    // Start is called before the first frame update
    void Start()
    {
        director = GameObject.Find("gameDirector");             // �ēI�u�W�F�N�g
        player = GameObject.Find("player");                     // ���@�I�u�W�F�N�g
        playerScript = player.GetComponent<PlayerController>(); // ���@�X�N���v�g

        attackEndTime = attackEndTimeStatus;

        if (playerScript.distance == 0.8f)
        {
            transform.position = new Vector3(player.transform.position.x + 1, 
                player.transform.position.y, player.transform.position.z);
        }
        else if(playerScript.distance == -0.8f)
        {
            transform.position = new Vector3(player.transform.position.x + -1.58f,
               player.transform.position.y, player.transform.position.z);
        }
    }

    // Update is called once per frame
    public void Update()
    {
        attackEndTime -= Time.deltaTime;
        if (attackEndTime < 0)
        {
            playerScript.isAttack = false;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            director.GetComponent<GameDirector>().increaseAbility();
        }
    }
}
