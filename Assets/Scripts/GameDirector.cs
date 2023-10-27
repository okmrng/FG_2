using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    GameObject abilityGage; // アビリティゲージ

    public GameObject[] hpIcons; // 自機HPアイコン

    // Start is called before the first frame update
    void Start()
    {
        abilityGage = GameObject.Find("abilityGage");
    }

    // ゲージを溜める
    public void increaseAbility()
    {
        abilityGage.GetComponent<Image>().fillAmount += 0.1f;
    }

    // 自機の残機数を表示するメソッド
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
