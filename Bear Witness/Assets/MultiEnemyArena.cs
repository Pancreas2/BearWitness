using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MultiEnemyArena : MonoBehaviour
{
    public int enemyCount;
    private bool active = true;
    public UnityEvent OnArenaComplete;

    public void ArenaEnemyDead()
    {
        if (active)
        {
            enemyCount--;
            if (enemyCount <= 0)
            {
                OnArenaComplete.Invoke();
                active = false;
            }
        }
    }
}
