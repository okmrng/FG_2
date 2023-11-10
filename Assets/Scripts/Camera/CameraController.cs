using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // ゲームオブジェクト
    /// <summary> 自機オブジェクト </summary>
    GameObject player;

    /// <summary> カメラ動かすフラグ </summary>
    bool moveCameraX = true;
    bool moveCameraY = false;

    float targetYPosition; // カメラが移動する目標のY座標
    float YPosition; // カメラY座標の初期値
    public float cameraSpeed = 5.0f; // カメラの移動速度

    public float scrollXMinNon = -8.37f;
    public float scrollXStart = -8.38f;

    // Start is called before the first frame update
    void Start()
    {
        // ゲームオブジェクト
        player = GameObject.Find("player");

        targetYPosition = transform.position.y;
        YPosition = transform.position.y;
    }


    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.x < scrollXMinNon)
        {
            moveCameraX = false;
        }
        if (player.transform.position.x >= scrollXStart)
        {
            moveCameraX = true;
        }
        if (player.transform.position.y < 2)
        {
            moveCameraY = false;
            targetYPosition = transform.position.y; // カメラが固定されるときに目標Y座標を現在のY座標に設定
        }
        if (player.transform.position.y >= 2)
        {
            moveCameraY = true;
            targetYPosition = player.transform.position.y; // カメラが移動するときに目標Y座標をプレイヤーのY座標に設定
        }

        if (moveCameraX)
        {
            transform.position = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
        }
        if (moveCameraY)
        {
            float newY = Mathf.Lerp(transform.position.y, targetYPosition, cameraSpeed * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
        else
        {
            float newY = Mathf.Lerp(transform.position.y, YPosition, cameraSpeed * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }

        Debug.Log(moveCameraX);
    }
}