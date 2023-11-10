using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    public float speed = 3.0f;  // 移動速度
    public float amplitude = 1.0f;  // 波の振幅
    public float frequency = 1.0f;  // 波の周波数

    private Vector3 initialPosition;
    // Start is called before the first frame update
    void Start()
    {
      initialPosition = transform.position;   
    }

    // Update is called once per frame
    void Update()
    {
        // 横方向の移動（左から右への波状移動）
        float horizontalMovement = Mathf.Sin(Time.time * frequency) * amplitude;
        Vector3 newPosition = initialPosition + Vector3.right * horizontalMovement;

        // 上下方向への移動（任意の高さを保つ）
        newPosition.y = initialPosition.y;

        // 移動
        transform.position = Vector3.MoveTowards(transform.position, newPosition, speed * Time.deltaTime);
    }
}
