using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    GameObject abilityGage; // �A�r���e�B�Q�[�W

    // Start is called before the first frame update
    void Start()
    {
        abilityGage = GameObject.Find("abilityGage");
    }

    public void increaseAbility()
    {
        abilityGage.GetComponent<Image>().fillAmount += 0.1f;
    }
}
