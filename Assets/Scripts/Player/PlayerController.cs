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
    public GameObject playerAttackPrefab;
    public GameObject playerBreakAttackPrefab;
    public GameObject[] abilityStopObj;
    GameObject abilityAttackImage;
    GameObject abilityHealImage;
    public GameObject abilityAttackPrefab;
    public GameObject abilityHealPrefab;
    GameObject abilityGage;
    GameDirector gameDirectorScript;

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
    bool rightDash = true;
    bool leftDash = true;

    // �W�����v
    public float jumpForce = 10.0f; // �W�����v
    bool canPushJanp = true;
    bool isGround = false;

    // HP
    public int HPMAX = 5;
    public int HP = 5;
    int damage = 0;

    // �U��
    public bool isAttack = false;         // �U���t���O
    public int attackPower = 5;           // �p���[

    // �j��U��
    public bool isBreak = false;     // �j��U���t���O
    public int breakPower = 10;      // �j��p���[
    public float backLashStatus = 3; // ����
    float backlash = 0;              // ����
    bool isBacklash = false;         // ����

    int attackMode = 0;           // 0 = �����A1 = �ʏ�U���A2 = �j��U��
    bool attackModeChange = true; // �U���ω��t���O

    public float attackChargeMax = 3; // �`���[�W����
    float attackCharge = 0;           // �`���[�W����

    // ����
    public float distance = 0.8f;
    bool right = true;         // �����t���O true = �E�����Afalse = ������

    // �A�r���e�B
    public bool onAbility = false;       // �A�r���e�B����
    bool isAbilityAttack = false; // �U������
    bool isAbilityHeal = false;   // �񕜌���
    float canceltimer = 0.1f;     // �L�����Z���^�C�}�[
    enum ability
    {
        ATTACK,
        HEAL
    }
    ability abilityMode = ability.ATTACK;

    bool isKinematicInitially; // ������isKinematic�̏�Ԃ�ۑ�

    float horizontalInput;

    float abilityChooseTime = 0;
    public float abilityChooseTimeStatus = 0.1f;
    float choose = 0;

    // ノックバック
    public float KnockbackSpeedStatus = 20;
    float KnockbackSpeed = 0;
    public float knockbackAcceleration = 1;
    bool onKnockback = false;
    public float canMoveTimeStatus = 0.5f;
    float canMoveTime = 0.5f;

    void Start()
    {
        this.ridid2d = GetComponent<Rigidbody2D>();
        director = GameObject.Find("gameDirector");
        abilityAttackImage = GameObject.Find("ability-attack");
        abilityAttackImage.SetActive(false);
        abilityHealImage = GameObject.Find("ability-heal");
        abilityHealImage.SetActive(false);
        abilityGage = GameObject.Find("abilityGage");
        gameDirectorScript = GetComponent<GameDirector>();

        isKinematicInitially = ridid2d.isKinematic;
    }

    // Update is called once per frame
    void Update()
    {
        if (!onAbility && !isBacklash)
        {
            // �ړ�
            if (!onKnockback)
            {
                Move();

                // ����
                Distance();
            }

            // ノックバック
            Knockback();

            if (canPushJanp)
            {
                if ((Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump")) && isGround)
                {
                    ridid2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                }
            }

            ridid2d.GetComponent<Rigidbody2D>().velocity = ridid2d.GetComponent<Rigidbody2D>().velocity;
            ridid2d.GetComponent<Rigidbody2D>().isKinematic = false;
        }
        else
        {
            if (onAbility)
            {
                ridid2d.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                ridid2d.GetComponent<Rigidbody2D>().isKinematic = true;
            }
            else
            {
                ridid2d.GetComponent<Rigidbody2D>().velocity = new Vector2(0, ridid2d.velocity.y);
                ridid2d.GetComponent<Rigidbody2D>().isKinematic = false;
            }
        }

        // ����
        if (backlash <= 0)
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

        // HP�A�C�R��
        director.GetComponent<GameDirector>().HpIcons(damage);

        // �U��
        if (attackMode == 1)
        {
            if (!isBacklash)
            {
                Attack();
            }
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
            if (Input.GetKey(KeyCode.Z) || Input.GetButton("Attack"))
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
        else { attackCharge = 0; }

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
            // ���E�̃x�N�g����擾
            horizontalInput = Input.GetAxis("Horizontal");

            // �_�b�V��
            if (canPushDash)
            {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) || Input.GetButton("Dash"))
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
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift) || Input.GetButtonDown("Dash"))
            {
                canPushDash = true;
                if (right)
                {
                    rightDash = true;
                }
                if (!right)
                {
                    leftDash = true;
                }
            }

            if (canDash)
            {
                canPushJanp = false;
                dashSpeed = dashPower;
                if (horizontalInput != 0)
                {
                    ridid2d.velocity = new Vector2(horizontalInput * dashSpeed, 0);
                }

                // �_�b�V���I���
                dashTimer -= Time.deltaTime;
                if (dashTimer < 0)
                {
                    dashCoolTime = dashCoolTimeStatus;
                    canDash = false;
                }

                // �_�b�V�����ɐU������Ń_�b�V���L�����Z��
                if (rightDash)
                {
                    if (!right)
                    {
                        rightDash = false;
                        canDash = false;
                    }
                }
                else if (leftDash)
                {
                    if (right)
                    {
                        leftDash = false;
                        canDash = false;
                    }
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "PlayerAbilityAttack")
        {
            HP -= 1;
            damage += 1;
        }
        if(other.gameObject.tag == "PlayerAbilityHeal")
        {
            if(HP < HPMAX)
            {
                HP += 1;
                damage -= 1;
            }
        }
        if (other.gameObject.tag == "Enemy")
        {
            HP -= 1;
            damage += 1;
            if (!onKnockback)
            {
                onKnockback = true;
            }
        }
    }

    // ノックバック
    void Knockback()
    {
        if (onKnockback)
        {
            ridid2d.velocity = new Vector2(-horizontalInput * KnockbackSpeed, ridid2d.velocity.y);
            if (KnockbackSpeed > 0)
            {
                KnockbackSpeed -= knockbackAcceleration;
            }
            else
            {
                KnockbackSpeed = 0;
                canMoveTime -= Time.deltaTime;
                if (canMoveTime < 0)
                {
                    onKnockback = false;
                }
            }
        }
        else
        {
            canMoveTime = canMoveTimeStatus;
            KnockbackSpeed = KnockbackSpeedStatus;
        }
    }

    // �U�����\�b�h
    void Attack()
    {
        if (Input.GetKeyUp(KeyCode.Z) || Input.GetButtonUp("Attack"))
        {
            if (!isAttack)
            {
                if (!onAbility)
                {
                    isAttack = true;
                    GameObject attackObj = Instantiate(playerAttackPrefab);
                }
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
        //playerBreakAttack.transform.position =
        //   new Vector3(transform.position.x + 0.95f, transform.position.y + 0.3f, transform.position.z);

        if (Input.GetKeyUp(KeyCode.Z) || Input.GetButtonUp("Attack"))
        {
            if (!isBreak)
            {
                if (!onAbility)
                {
                    isBreak = true;
                    GameObject breakObj = Instantiate(playerBreakAttackPrefab);
                }
            }
        }

        if (isBreak)
        {
            isBacklash = true;
            backlash = backLashStatus;

            attackModeChange = false;
        }
        else
        {
            attackModeChange = true;
        }
    }

    // ����
    void Distance()
    {
        if (!isBacklash)
        {
            // �L�[���͂ō��E�̌������擾
            if (Input.GetKey(KeyCode.RightArrow) || horizontalInput > 0)
            {
                distance = 0.8f;
                right = true;
            }
            if (Input.GetKey(KeyCode.LeftArrow) || horizontalInput < 0)
            {
                distance = -0.8f;
                right = false;
            }

            if (distance != 0)
            {
                transform.localScale = new Vector3(distance, 1.5f, 1);
            }
        }
    }

    // �A�r���e�B
    void Ability()
    {
        abilityAttackImage.transform.position =
            new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
        abilityHealImage.transform.position =
            new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);

        if (Input.GetKeyDown(KeyCode.X) || Input.GetButtonDown("Ability"))
        {
            if (!onAbility && abilityGage.GetComponent<Image>().fillAmount == 1)
            {
                abilityChooseTime = abilityChooseTimeStatus;
                onAbility = true;
            }
        }

        if (onAbility)
        {
            // ������~�߂�
            for (int i = 0; i < abilityStopObj.Length; i++)
            {
                if (abilityStopObj[i])
                {
                    abilityStopObj[i].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    abilityStopObj[i].GetComponent<Rigidbody2D>().isKinematic = true;
                }
            }

            // ���ʑI��
            switch (abilityMode)
            {
                case ability.ATTACK:
                    abilityAttackImage.SetActive(true);
                    abilityHealImage.SetActive(false);

                    if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        abilityMode = ability.HEAL;
                    }

                    if (abilityChooseTime <= 0 && Input.GetKeyDown(KeyCode.X))
                    {
                        abilityAttackImage.SetActive(false);
                        isAbilityAttack = true;
                        onAbility = false;
                    }

                    if (Input.GetKeyDown(KeyCode.Z) || Input.GetButtonDown("Attack"))
                    {
                        abilityAttackImage.SetActive(false);
                        abilityHealImage.SetActive(false);
                        onAbility = false;
                    }

                    break;

                case ability.HEAL:
                    abilityHealImage.SetActive(true);
                    abilityAttackImage.SetActive(false);

                    if (abilityChooseTime <= 0 && Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        abilityMode = ability.ATTACK;
                    }

                    if (choose <= -0.1f || Input.GetKeyDown(KeyCode.X))
                    {
                        abilityHealImage.SetActive(false);
                        isAbilityHeal = true;
                        onAbility = false;
                    }

                    if (Input.GetKeyDown(KeyCode.Z) || Input.GetButtonDown("Attack"))
                    {
                        abilityAttackImage.SetActive(false);
                        abilityHealImage.SetActive(false);
                        onAbility = false;
                    }

                    break;
            }

            abilityChooseTime -= Time.deltaTime;
            canceltimer -= Time.deltaTime;

            if (abilityChooseTime <= 0)
            {
                choose = Input.GetAxis("AbilityChoose");
            }
        }
        else
        {
            for (int i = 0; i < abilityStopObj.Length; i++)
            {
                if (abilityStopObj[i])
                {
                    abilityStopObj[i].GetComponent<Rigidbody2D>().velocity =
                        abilityStopObj[i].GetComponent<Rigidbody2D>().velocity;
                    abilityStopObj[i].GetComponent<Rigidbody2D>().isKinematic = isKinematicInitially;
                }
            }
            canceltimer = 0.1f;
            choose = 0;
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
       // Debug.Log("�`���[�W��" +  attackCharge);
        //Debug.Log("��������" +  backlash);
        //Debug.Log("�����t���O" +  isBacklash);
        Debug.Log(isGround);
    }
}