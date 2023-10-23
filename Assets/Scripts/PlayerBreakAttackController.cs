using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBreakAttackController : MonoBehaviour
{ 
    GameObject director; // 監督オブジェクト

    // Start is called before the first frame update
    void Start()
    {
        director = GameObject.Find("gameDirector"); // 監督オブジェクト
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
