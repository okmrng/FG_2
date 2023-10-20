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
    bool canPushJanp = true;
    bool isGround = false;

    // HP
    public int HP = 5;
    public GameObject[] hpIcons; // HP�̃A�C�R��
    int damage = 0;

    // �U��
    bool isAttack = false;                // �U���t���O
    public int attackPower = 5;           // �p���[
    GameObject playerAttack;
    float attackEndTime = 0;              // �U������
    public float attackEndTimeStatus = 1; // �U������

    // ����
    float distance = 0;

    void Start()
    {
        this.ridid2d = GetComponent<Rigidbody2D>();

        playerAttack = GameObject.Find("playerAttack");
    }

    // Update is called once per frame
    void Update()
    {
        // �ړ�
        Move();

        // �W�����v
        isGround = false;
        isGround = Physics2D.Raycast(transform.position, Vector2.down, 1.0f, LayerMask.GetMask("Ground"));

        if (canPushJanp)
        {
            if (Input.GetKeyDown(KeyCode.Space) && isGround)
            {
                ridid2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }

        // HP�A�C�R��
        UpdateHpIcons();

        // �U��
        Attack();

        // ����
        Distance();

        // �f�o�b�O
        Debug.Log(dashTimer);
        Debug.Log(isGround);
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

    // �U�����\�b�h
    void Attack()
    {
        playerAttack.transform.position = 
            new Vector3(transform.position.x + 0.8f, transform.position.y, transform.position.z);
        
        if (Input.GetKeyDown(KeyCode.Z))
        {
            isAttack = true;
        }

        if (isAttack)
        {
            playerAttack.SetActive(true);

            attackEndTime -= Time.deltaTime;
            if (attackEndTime < 0)
            {
                isAttack = false;
            }
        }
        else
        {
            playerAttack.SetActive(false);
            attackEndTime = attackEndTimeStatus;
        }
    }

    // ����
    void Distance()
    {
        // �L�[���͂ō��E�̌������擾
        if (Input.GetKey(KeyCode.RightArrow)) distance = 0.8f;
        if (Input.GetKey(KeyCode.LeftArrow)) distance = -0.8f;

        if (distance != 0)
        {
            transform.localScale = new Vector3(distance, 1.5f, 1);

            // �U���̌���
            if (distance == 0.8f)
            {
                playerAttack.transform.position = new Vector3(playerAttack.transform.position.x, 
                    playerAttack.transform.position.y, playerAttack.transform.position.z);
            }
            else if (distance == -0.8f)
            {
                playerAttack.transform.position = new Vector3(playerAttack.transform.position.x - 1.58f,
                    playerAttack.transform.position.y, playerAttack.transform.position.z);
            }
        }
    }
}