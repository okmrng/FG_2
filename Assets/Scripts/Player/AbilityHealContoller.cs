using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHealContoller : MonoBehaviour
{
    GameObject player;                          // ���@
    PlayerController playerScript;              // ���@�X�N���v�g
    public GameObject abilityHealRangePrefab;   // �U���͈�

    public float speedStatus = 3;
    float speed = 0; // ���x

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player");
        playerScript = player.GetComponent<PlayerController>();
        transform.position = player.transform.position;

        if (playerScript.distance == 0.8f)
        {
            speed = speedStatus;
        }
        else if (playerScript.distance == -0.8f)
        {
            speed = -speedStatus;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) || Input.GetButtonDown("Ability"))
        {
            GameObject abilityHealRangeObj = Instantiate(abilityHealRangePrefab);
        }
    }

    // �����Œ�
    private void FixedUpdate()
    {
        transform.Translate(speed, 0, 0);
    }

    // ��ʊO�ɏo������
    void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
