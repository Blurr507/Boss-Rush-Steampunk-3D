using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulley : MonoBehaviour
{
    [SerializeField]
    private GameObject chandelierObject;
    private GameObject chandelier;

    void Update()
    {
        if (BattleStateManager.me.GetState() == 0 && chandelier == null)
        {
            CreateChandelier();
        }
    }

    private void CreateChandelier()
    {
        chandelier = Instantiate(chandelierObject, Vector3.zero, Quaternion.identity);
    }
}
