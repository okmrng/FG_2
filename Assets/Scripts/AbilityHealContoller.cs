using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHealContoller : MonoBehaviour
{
    GameObject player; // ���@
    public GameObject abilityHealRangePrefab; // �U���͈�

    public float speed = 0.2f; // ���x

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
