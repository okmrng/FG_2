using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBreakAttackController : MonoBehaviour
{ 
    GameObject director; // �ēI�u�W�F�N�g

    // Start is called before the first frame update
    void Start()
    {
        director = GameObject.Find("gameDirector"); // �ēI�u�W�F�N�g
    }

    // Update is called once per frame
    public void Update()
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
