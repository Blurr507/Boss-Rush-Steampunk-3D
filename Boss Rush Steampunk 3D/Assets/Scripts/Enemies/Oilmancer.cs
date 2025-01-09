using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oilmancer : Enemy
{
    private Health health;

    void Start()
    {
        health = GetComponent<Health>();
    }
}
