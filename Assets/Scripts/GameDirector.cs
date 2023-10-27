using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    GameObject abilityGage; // �A�r���e�B�Q�[�W

    public GameObject[] hpIcons; // ���@HP�A�C�R��

    // Start is called before the first frame update
    void Start()
    {
        abilityGage = GameObject.Find("abilityGage");
    }

    // �Q�[�W�𗭂߂�
    public void increaseAbility()
    {
        abilityGage.GetComponent<Image>().fillAmount += 0.1f;
    }

    // ���@�̎c�@����\�����郁�\�b�h
    public void HpIcons(int damage)
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
