using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    // ゲームオブジェクト
    /// <summary> 自機 </summary>
    GameObject player;
    /// <summary> アビリティゲージ </summary>
    GameObject abilityGage;
    /// <summary> HPアイコン </summary>
    public GameObject[] hpIcons;
    /// <summary> ボス </summary>
    GameObject boss1;

    // スクリプト
    /// <summary> 自機 </summary>
    PlayerController playerController;
    /// <summary> ボス </summary>
    AttackPattern1 bossScript;

    /// <summary> ゲージを増やしていく量 </summary>
    public float addWidth;
    public static float width = 0;

    // Start is called before the first frame update
    void Start()
    {
        // オブジェクト
        player = GameObject.Find("player");
        abilityGage = GameObject.Find("abilityGage");
        boss1 = GameObject.Find("Boss1");

        // スクリプト
        playerController = player.GetComponent<PlayerController>();
        if (boss1)
        {
            bossScript = boss1.GetComponent<AttackPattern1>();
        }
    }

    private void Update()
    {
        if (playerController.death)
        {
            width = 0;
        }
        if (boss1)
        {
            if (bossScript.isDeath)
            {
                width = 0;
                Destroy(boss1);
                SceneManager.LoadScene("ClearScene");
            }
        }
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
