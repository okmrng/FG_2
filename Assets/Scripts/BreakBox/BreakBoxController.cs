using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBoxController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerBreakAttack")
        {
            Destroy(gameObject);
        }
    }
}
