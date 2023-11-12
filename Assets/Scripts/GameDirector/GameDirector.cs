using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    // ゲームオブジェクト
    /// <summary> アビリティゲージ </summary>
    GameObject abilityGage;
    /// <summary> HPアイコン </summary>
    public GameObject[] hpIcons;

    /// <summary> ゲージを増やしていく量 </summary>
    public float addWidth;
    public float width = 0;

    // Start is called before the first frame update
    void Start()
    {
        abilityGage = GameObject.Find("abilityGage");
    }

    private void Update()
    {
        //abilityGage.GetComponent<Image>().fillAmount += addWidth;
    }

    private void FixedUpdate()
    {
        width += addWidth;
        abilityGage.GetComponent<Image>().fillAmount = width;
    }

    // HPアイコン管理
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
