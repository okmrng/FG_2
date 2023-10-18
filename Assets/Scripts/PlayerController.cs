using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D ridid2d;

    public float moveSpeed = 5.0f;  // �ړ����x
    public float jumpForce = 10.0f; // �W�����v�̗�
    bool canJanp = false;

    public int HP = 5;
    public GameObject[] hpIcons; // HP�̃A�C�R��
    int damage = 0;

    void Start()
    {
        this.ridid2d = GetComponent<Rigidbody2D>(); 
    }

    // Update is called once per frame
    void Update()
    {
        // ���E�̈ړ�
        float horizontalInput = Input.GetAxis("Horizontal");
        ridid2d.velocity = new Vector2(horizontalInput * moveSpeed, ridid2d.velocity.y);

        // �W�����v
        if (Input.GetKeyDown(KeyCode.Space) && canJanp == true)
        {
            ridid2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            canJanp = false;
        }

        UpdateHpIcons();

        // �f�o�b�O
        Debug.Log("HP:" + HP);
        Debug.Log("�W�����v�t���O" + canJanp);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            HP -= 1;
            damage += 1;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.tag == "Ground")
        {
            canJanp = true;
        }
    }

    // ���@�̎c�@����\�����郁�\�b�h
    void UpdateHpIcons()
    {
        for (int i = 0; i < hpIcons.Length; i++)
        {
            if (damage <= i)
            {
                hpIcons[i].SetActive(true);
            }
            else
            {
                hpIcons[i].SetActive(false);
            }
        }
    }
}