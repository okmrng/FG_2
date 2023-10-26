using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityAttckContoller : MonoBehaviour
{
    GameObject player; // 自機

    public float speed = 3; // 速度

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player");
        transform.position = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(speed, 0, 0);
    }

    // 画面外に出たら解放
    void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
