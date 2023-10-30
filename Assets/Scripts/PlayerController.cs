using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D ridid2d;
    GameObject director;
    GameObject playerBreakAttack;
    public GameObject[] abilityStopObj;
    GameObject abilityChoose;
    public GameObject abilityAttackPrefab;
    public GameObject abilityHealPrefab;
    GameObject abilityGage;
    GameDirector gameDirectorScript;
    public GameObject playerAttackPrefab;

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
    int damage = 0;

    // �U��
    public bool isAttack = false;         // �U���t���O
    public int attackPower = 5;           // �p���[

    // �j��U��
    bool isBreak = false;                   // �j��U���t���O
    public int breakPower = 10;             // �j��p���[
    float breakEndTime = 0;                 // �j�󎞊�
    public float breakEndTimeStatus = 1.2f; // �j�󎞊�
    public float backLashStatus = 3;        // ����
    float backlash = 0;                     // ����
    bool isBacklash = false;                     // ����

    int attackMode = 0;           // 0 = �����A1 = �ʏ�U���A2 = �j��U��
    bool attackModeChange = true; // �U���ω��t���O

    public float attackChargeMax = 3; // �`���[�W����
    float attackCharge = 0;           // �`���[�W����

    // ����
    public float distance = 0;

    // �A�r���e�B
    bool onAbility = false;       // �A�r���e�B����
    bool isAbilityAttack = false; // �U������
    bool isAbilityHeal = false;   // �񕜌���

    void Start()
    {
        this.ridid2d = GetComponent<Rigidbody2D>();
        director = GameObject.Find("gameDirector");
        playerBreakAttack = GameObject.Find("playerBreakAttack");
        playerBreakAttack.SetActive(false);
        abilityChoose = GameObject.Find("abilityChoose");
        abilityChoose.SetActive(false);
        abilityGage = GameObject.Find("abilityGage");
        gameDirectorScript = GetComponent<GameDirector>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!onAbility)
        {
            // �ړ�
            Move();

            // ����
            Distance();
        }

        if(backlash <= 0)
        {
            isBacklash = false;
        }
        if (backlash > 0 && !isBreak)
        {
            backlash -= Time.deltaTime;
        }

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
        director.GetComponent<GameDirector>().HpIcons(damage);

        // �U��
        if (attackMode == 1)
        {
            Attack();
        }
        else if (attackMode == 2)
        {
            Break();
        }
        else if (attackMode == 0)
        {
            //playerBreakAttack.SetActive(false);
        }

        // �`���[�W
        if (attackModeChange)
        {
            if (Input.GetKey(KeyCode.Z))
            {
                attackCharge += Time.deltaTime;
            }
            else { attackCharge = 0; }

            if (attackCharge < attackChargeMax)
            {
                // �ʏ�U��
                attackMode = 1;
            }
            else if (attackCharge >= attackChargeMax)
            {
                // �j��U��
                attackMode = 2;
            }
            else
            {
                // �U���Ȃ�
                attackMode = 0;
            }
        }

        // �A�r���e�B
        Ability();

        // �f�o�b�O
        Debugg();
    }

    // �ړ�
    void Move()
    {
        if (!isBacklash)
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
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
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
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            HP -= 1;
            damage += 1;
        }
    }

    // �U�����\�b�h
    void Attack()
    {
        //playerAttack.transform.position = 
        //    new Vector3(transform.position.x + 0.8f, transform.position.y, transform.position.z);
        
        if (Input.GetKeyUp(KeyCode.Z))
        {
            if (!isAttack)
            {
                isAttack = true;
                GameObject attackObj = Instantiate(playerAttackPrefab);
            }
        }

        if (isAttack)
        {
            attackModeChange = false;            
        }
        else
        {
            attackModeChange = true;
        }
    }

    // �j��U�����\�b�h
    void Break()
    {
        playerBreakAttack.transform.position =
           new Vector3(transform.position.x + 0.95f, transform.position.y + 0.3f, transform.position.z);

        if (Input.GetKeyUp(KeyCode.Z))
        {
            isBreak = true;
        }

        if (isBreak)
        {
            isBacklash = true;
            backlash = backLashStatus;

            attackModeChange = false;

            playerBreakAttack.SetActive(true);

            breakEndTime -= Time.deltaTime;
            if (breakEndTime < 0)
            {
                isBreak = false;
            }
        }
        else
        {
            attackModeChange = true;
            playerBreakAttack.SetActive(false);
            breakEndTime = breakEndTimeStatus;
        }
    }

    // ����
    void Distance()
    {
        if (!isBacklash)
        {
            // �L�[���͂ō��E�̌������擾
            if (Input.GetKey(KeyCode.RightArrow)) distance = 0.8f;
            if (Input.GetKey(KeyCode.LeftArrow)) distance = -0.8f;

            if (distance != 0)
            {
                transform.localScale = new Vector3(distance, 1.5f, 1);
            }

            // �U���̌���
            //if (distance == 0.8f)
            //{
            //    playerAttack.transform.position = new Vector3(playerAttack.transform.position.x,
            //        playerAttack.transform.position.y, playerAttack.transform.position.z);
            //    playerBreakAttack.transform.position = new Vector3(playerBreakAttack.transform.position.x,
            //        playerBreakAttack.transform.position.y, playerBreakAttack.transform.position.z);
            //}
            //else if (distance == -0.8f)
            //{
            //    playerAttack.transform.position = new Vector3(playerAttack.transform.position.x - 1.58f,
            //    playerAttack.transform.position.y, playerAttack.transform.position.z);
            //    playerBreakAttack.transform.position =
            //        new Vector3(playerBreakAttack.transform.position.x - 1.9f,
            //        playerBreakAttack.transform.position.y, playerBreakAttack.transform.position.z);
            //}
        }
    }

    // �A�r���e�B
    void Ability()
    {
        abilityChoose.transform.position = 
            new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (!onAbility && abilityGage.GetComponent<Image>().fillAmount == 1)
            {
                onAbility = true;
            }
        }

        if (onAbility)
        {
            // �������~�߂�
            for (int i = 0; i < abilityStopObj.Length; i++)
            {
                abilityStopObj[i].GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                abilityStopObj[i].GetComponent<Rigidbody2D>().isKinematic = true;
            }

            // ���ʑI��
            abilityChoose.SetActive(true);
            Debug.Log("���ʂ�I��łˁI\nA:�U�� D:��");

            if (Input.GetKeyDown(KeyCode.A))
            {
                abilityChoose.SetActive(false);
                isAbilityAttack = true;
                onAbility = false;
            }
            else if(Input.GetKeyDown(KeyCode.D))
            {
                abilityChoose.SetActive(false);
                isAbilityHeal = true;
                onAbility = false;
            }
        }

        // �U��
        if (isAbilityAttack)
        {
            abilityGage.GetComponent<Image>().fillAmount = 0;
            GameObject attackAbilityObj = Instantiate(abilityAttackPrefab);
            isAbilityAttack = false;
        }
        else if (isAbilityHeal)
        {
            abilityGage.GetComponent<Image>().fillAmount = 0;
            GameObject attackHealObj = Instantiate(abilityHealPrefab);
            isAbilityHeal = false;
        }
    }

    // �f�o�b�O
    void Debugg()
    {
        // �f�o�b�O
        //Debug.Log(dashTimer);
        //Debug.Log("�_���[�W" + damage);
        //Debug.Log("�����ϐ�" +  distance);
        //Debug.Log("�`���[�W��" +  attackCharge);
        Debug.Log("��������" +  backlash);
        Debug.Log("�����t���O" +  isBacklash);
    }
}