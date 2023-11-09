using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBreakAttackController : MonoBehaviour
{ 
    GameObject director;           // �ēI�u�W�F�N�g
    GameObject player;             // ���@�I�u�W�F�N�g
    PlayerController playerScript; // ���@�X�N���v�g

    float breakEndTime = 0;              // �U������
    public float breakEndTimeStatus = 1; // �U������

    // Start is called before the first frame update
    void Start()
    {
        director = GameObject.Find("gameDirector");             // �ēI�u�W�F�N�g
        player = GameObject.Find("player");                     // ���@�I�u�W�F�N�g
        playerScript = player.GetComponent<PlayerController>(); // ���@�X�N���v�g

        breakEndTime = breakEndTimeStatus;

        // ���@�̌����ɂ���ďo���ʒu��ς���
        if (playerScript.distance == 0.8f)
        {
            transform.position = new Vector3(player.transform.position.x + 1,
                player.transform.position.y + 0.3f, player.transform.position.z);
        }
        else if (playerScript.distance == -0.8f)
        {
            transform.position = new Vector3(player.transform.position.x - 1,
               player.transform.position.y + 0.3f, player.transform.position.z);
        }
    }

    // Update is called once per frame
    public void Update()
    {
        if (!playerScript.onAbility)
        {
            breakEndTime -= Time.deltaTime;
        }
        // ��莞�Ԍo�߂��邩�A�L�[����͂�������
        if (breakEndTime < 0)
        {
            playerScript.isBreak = false;
            Destroy(gameObject);
        }
    }
}
