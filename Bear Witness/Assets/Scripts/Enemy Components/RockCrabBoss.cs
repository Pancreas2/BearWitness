using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Cinemachine.Editor;

public class RockCrabBoss : BossEnemy
{

    [SerializeField] private DoorOpener doorOpener;
    [SerializeField] private CinemachineVirtualCamera arenaCamera;
    [SerializeField] private Gate entranceGate;
    [SerializeField] private Gate exitGate;

    public override void BeginFight()
    {
        arenaCamera.GetComponent<CinemachineShake>().ShakeCamera(2f, 5f);
        StartCoroutine(WaitForFightStart());
    }

    public void ShakeOnAttack()
    {
        CinemachineShake.instance.ShakeCamera(0.25f, 1f);
    }

    IEnumerator WaitForFightStart()
    {
        arenaCamera.Priority = 12;
        entranceGate.SetForceOpen(false);
        FreezePlayer();

        yield return new WaitForSeconds(2f);

        animator.SetTrigger("WakeUp");
        UpdateHealth();
        base.BeginFight();
    }

    public override void Damage(int damageValue, float sourcePosX)
    {
        if (!fightActive) BeginFight();
        base.Damage(damageValue, sourcePosX);
    }

    public override void Perish()
    {
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
