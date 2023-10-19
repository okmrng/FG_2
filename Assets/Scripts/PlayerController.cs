using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D ridid2d;

    public float moveSpeed = 5.0f; // �ړ����x
    
    // �_�b�V��
    float dashSpeed = 0.0f;         // �_�b�V���̑��x
    public float dashPower = 20.0f; // �_�b�V���̑��x
    bool canDash = false;
    float dashTimer = 0;
    public float dashTimerStatus = 0.5f;
    float dashCoolTime = 0;
    public float dashCoolTimeStatus = 0.5f;
    bool canPushDash = true;

    // �W�����v
    public float jumpForce = 10.0f; // �W�����v
    bool canJanp = false;
    bool canPushJanp = true;

    // HP
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
        // �ړ�
        Move();

        // �W�����v
        if (canPushJanp)
        {
            if (Input.GetKeyDown(KeyCode.Space) && canJanp == true)
            {
                ridid2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                canJanp = false;
            }
        }

        UpdateHpIcons();

        // �f�o�b�O
        Debug.Log(dashTimer);
        Debug.Log("�_�b�V���t���O" + canPushDash);
    }


    void Move()
    {
        // ���E�̃x�N�g�����擾
        float horizontalInput = Input.GetAxis("Horizontal");

        // �_�b�V��
        if (canPushDash)
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                canPushDash = false;

                if (dashCoolTime < 0)
                {
                    canDash = true;
                }
            }
            else
            {
                canDash = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            canPushDash = true;
        }

        if (canDash)
        {
            canPushJanp = false;
            dashSpeed = dashPower;
            ridid2d.velocity = new Vector2(horizontalInput * dashSpeed, 0);

            // �_�b�V���I���
            dashTimer -= Time.deltaTime;
            if (dashTimer < 0)
            {
                dashCoolTime = dashCoolTimeStatus;
                canDash = false;
            }
        }
        else
        {
            canPushJanp = true;
            dashTimer = dashTimerStatus;
            dashCoolTime -= Time.deltaTime;
            // ���E�ړ�
            ridid2d.velocity = new Vector2(horizontalInput * moveSpeed, ridid2d.velocity.y);
        }
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