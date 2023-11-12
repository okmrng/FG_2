using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    // ゲームオブジェクト
    /// <summary> 自機オブジェクト </summary>
    GameObject player;
    /// <summary> 説明 </summary>
    public GameObject info;

    // スクリプト
    /// <summary> 自機スクリプト </summary>
    PlayerController playerScript;

    // Start is called before the first frame update
    void Start()
    {
        // ゲームオブジェクト
        player = GameObject.Find("player");

        // スクリプト
        playerScript = player.GetComponent<PlayerController>();

        info.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            info.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            info.SetActive(false);
        }
    }
}
