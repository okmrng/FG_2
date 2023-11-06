using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    GameObject abilityGage; // �A�r���e�B�Q�[�W

    public GameObject[] hpIcons; // ���@HP�A�C�R��

    public float addWidth; // �Q�[�W�̑���

    // Start is called before the first frame update
    void Start()
    {
        abilityGage = GameObject.Find("abilityGage");
    }

    private void Update()
    {
        abilityGage.GetComponent<Image>().fillAmount += addWidth;
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
