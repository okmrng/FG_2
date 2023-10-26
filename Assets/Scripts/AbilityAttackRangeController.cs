using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityAttackRangeController : MonoBehaviour
{
    GameObject abilityAttack;

    // Start is called before the first frame update
    void Start()
    {
        abilityAttack = GameObject.Find("abilityAttackPrefab(Clone)");
        transform.position = abilityAttack.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
