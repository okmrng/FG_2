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
        transform.Translate(speed, 0, 0);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameObject abilityAttackRangeObj = Instantiate(abilityAttackRangePrefab);
            gameObject.SetActive(false);
        }
    }

    // ��ʊO�ɏo������
    void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
