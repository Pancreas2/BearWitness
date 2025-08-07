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
    [SerializeField] private GameObject shockwave;
    [SerializeField] private Transform leftShockwavePoint;
    [SerializeField] private Transform rightShockwavePoint;

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

    public override void Damage(int damageValue, float sourcePosX, bool bypassInv = false)
    {
        if (!fightActive && uniqueEnemy.IsAlive()) BeginFight();
        base.Damage(damageValue, sourcePosX, bypassInv);
    }

    public override void Perish()
    {
        arenaCamera.Priority = 8;
        animator.SetTrigger("Perish");
        doorOpener.SetDoorState(true);
        entranceGate.SetForceClose(true);
        exitGate.SetForceClose(true);
        GameManager.instance.GrantAchievement("Shattered Stone");
        base.Perish();
    }

    public void AfterPerish()
    {
        entranceGate.SetForceClose(false);
        exitGate.SetForceClose(false);
    }

    private void CreateShockWave(Transform creationPoint)
    {
        Instantiate(shockwave, creationPoint.position + new Vector3(0.5f, 0f, 0f), creationPoint.rotation).GetComponent<Shockwave>().SetSpeed(2);
        Instantiate(shockwave, creationPoint.position - new Vector3(0.5f, 0f, 0f), creationPoint.rotation).GetComponent<Shockwave>().SetSpeed(-2);
    }

    public void ShockLeft()
    {
        CreateShockWave(leftShockwavePoint);
    }

    public void ShockRight()
    {
        CreateShockWave(rightShockwavePoint);
    }
}
