using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OPStageDirector : MonoBehaviour
{
    /// <summary> 中間 </summary>
    public static bool retryPointer = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("中間ポイント：" +  retryPointer);
    }
}
