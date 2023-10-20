using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{ 
    // �A�r���e�B�Q�[�W
    GameObject director;

    // Start is called before the first frame update
    void Start()
    {
        director = GameObject.Find("gameDirector");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            director.GetComponent<GameDirector>().increaseAbility();
        }
    }
}
