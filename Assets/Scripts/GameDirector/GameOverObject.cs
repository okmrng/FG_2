using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Attack"))
        {
            if (OPStageDirector.retryPointer)
            {
                SceneManager.LoadScene("Boss1Scene");
            }
            SceneManager.LoadScene("OPStage");
        }
    }
}
