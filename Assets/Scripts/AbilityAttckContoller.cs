using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityAttckContoller : MonoBehaviour
{
    GameObject player; // ���@
    public GameObject abilityAttackRangePrefab; // �U���͈�

    public float speed = 3; // ���x

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player");
        transform.position = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) || Input.GetButtonDown("Ability"))
        {
            GameObject abilityAttackRangeObj = Instantiate(abilityAttackRangePrefab);
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
