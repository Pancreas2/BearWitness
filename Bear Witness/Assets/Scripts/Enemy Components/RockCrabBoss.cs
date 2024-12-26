using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class RockCrabBoss : BossEnemy
{

    [SerializeField] private DoorOpener doorOpener;
    [SerializeField] private CinemachineVirtualCamera arenaCamera;
    [SerializeField] private Gate entranceGate;
    [SerializeField] private Gate exitGate;

    public override void BeginFight()
    {
        arenaCamera.GetComponent<CinemachineShake>().ShakeCamera(2f, 5f);
        FreezePlayer();
        arenaCamera.Priority = 12;
        entranceGate.SetForceOpen(false);
        fightActive = true;
        animator.SetTrigger("WakeUp");
        StartCoroutine(WaitForFightStart());
    }

    public void ShakeOnAttack()
    {
        CinemachineShake.instance.ShakeCamera(0.25f, 1f);
    }

    IEnumerator WaitForFightStart()
    {
        yield return new WaitForSeconds(2f);

        FightReady();
    }

    public void FightReady()
    {
        UpdateHealth();
        base.BeginFight();
    }

    public override void Damage(int damageValue, float sourcePosX)
    {
        if (!fightActive && uniqueEnemy.IsAlive()) BeginFight();
        base.Damage(damageValue, sourcePosX);
    }

    public override void Perish()
    {
        arenaCamera.Priority = 8;
        animator.SetTrigger("Perish");
        doorOpener.SetDoorState(true);
        entranceGate.SetForceClose(true);
        exitGate.SetForceClose(true);
        base.Perish();
    }

    public void AfterPerish()
    {
        entranceGate.SetForceClose(false);
        exitGate.SetForceClose(false);
    }
}
