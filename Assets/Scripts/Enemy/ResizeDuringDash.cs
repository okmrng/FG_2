using System.Collections;
using UnityEngine;

public class ResizeDuringDash : MonoBehaviour
{
   
    public float resizeDuration = 2.0f; // サイズ変更の期間

    private bool isResizing = false;
    private Vector3 originalSize;
    private Rigidbody2D rb;

     // 追加: 突進中かどうかを示す変数
    private bool isDashing = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalSize = transform.localScale;
    }

    void Update()
    {
        if ( !isResizing)
        {
            StartCoroutine(Resize());
        }
    }

    public void StartResize()
    {
        if (!isResizing)
        {
            StartCoroutine(Resize());
        }
    }

    public void SetDashing(bool isDashing)
    {
        this.isDashing = isDashing;
    }

    IEnumerator Resize()
    {
        isResizing = true;

        float elapsedTime = 0f;

        while (elapsedTime < resizeDuration)
        {
           // オブジェクトのサイズを突進中だけ半分にする
            transform.localScale = isDashing ? originalSize / 2 : originalSize;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // オブジェクトのサイズを元に戻す
        transform.localScale = originalSize;

        // 突進終了
        isResizing = false;
        rb.velocity = Vector2.zero;
    }
}
