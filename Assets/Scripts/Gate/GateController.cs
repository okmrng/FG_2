using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : MonoBehaviour
{
    // コンポーネント
    Animator animator = default;

    /// <summary> 閉じるフラグ </summary>
    bool onClose = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y >= 5.67f)
        {
            onClose = true;
        }
        
        animator.SetBool("Close", onClose);

        //Debug.Log("onClose" + onClose);
    }
}
