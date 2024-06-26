using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockCrabBoss : MonoBehaviour
{
    private void Start()
    {
        GetComponent<BossEnemy>().BeginFight();
    }
}
