using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3 : MonoBehaviour
{
    public float speed = 10.0f;  // 移動速度
    public float amplitude = 2.0f;  // 揺れの振幅
    public float frequency = 4.0f;  // 揺れの周波数

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        // 横方向の移動
        float horizontalMovement = speed * Time.deltaTime;
        Vector3 newPosition = transform.position + Vector3.left * horizontalMovement;

        // 上下方向への揺れ
        float verticalMovement = Mathf.Sin(Time.time * frequency) * amplitude;
        newPosition.y = initialPosition.y + verticalMovement;

        // 移動
        transform.position = Vector3.MoveTowards(transform.position, newPosition, speed * Time.deltaTime);
    }
}