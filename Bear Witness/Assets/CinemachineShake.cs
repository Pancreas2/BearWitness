using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineShake : MonoBehaviour
{
    public static CinemachineShake instance;
    private CinemachineVirtualCamera virtualCamera;
    private float shakeTimer;
    private float startIntesity;
    private float totalShakeTime;

    private CinemachineBrain camera;

    private void Awake()
    {
        camera = FindObjectOfType<CinemachineBrain>();
        if (!instance) instance = this;
    }


    public void ShakeCamera(float time, float intensity)
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        CinemachineBasicMultiChannelPerlin noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        noise.m_AmplitudeGain = intensity;
        startIntesity = intensity;
        shakeTimer = time;
        totalShakeTime = time;
    }

    void Update ()
    {
        if (camera.ActiveVirtualCamera == virtualCamera)
        {
            instance = this;
        }

        if (shakeTimer > 0f)
        {
            shakeTimer -= Time.deltaTime;
            CinemachineBasicMultiChannelPerlin noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            noise.m_AmplitudeGain = Mathf.Lerp(startIntesity, 0f, 1f - (shakeTimer / totalShakeTime));
        }
    }
}
