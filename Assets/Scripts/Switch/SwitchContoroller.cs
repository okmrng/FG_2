using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Events;

public class SwitchContoroller : MonoBehaviour
{
    [SerializeField] UnityEvent _onEnter = default;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
                _onEnter.Invoke();
        }
    }
}
