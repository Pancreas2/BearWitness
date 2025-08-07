using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeFrame : MonoBehaviour
{
    bool _isFrozen = false;
    bool _isSlowed = false;
    float _pendingFreezeDuration;
    float _pendingSlowDuration;
    float _pendingSlowIntensity;

    void Update()
    {
        if (_pendingFreezeDuration > 0f && !_isFrozen)
        {
            StartCoroutine(DoFreeze());
        }
        
        if (_pendingSlowDuration > 0f && !_isSlowed)
        {
            StartCoroutine(DoSlow());
        }
    }

    public void Freeze(float duration)
    {
        _pendingFreezeDuration = duration;
    }

    IEnumerator DoFreeze()
    {
        _isFrozen = true;
        float originalScale = Time.timeScale;
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(_pendingFreezeDuration);

        Time.timeScale = originalScale;
        _pendingFreezeDuration = 0f;
        _isFrozen = false;
    }

    IEnumerator DoSlow()
    {
        _isSlowed = true;
        float originalScale = Time.timeScale;
        Time.timeScale = _pendingSlowIntensity;

        yield return new WaitForSecondsRealtime(_pendingSlowDuration);

        Time.timeScale = originalScale;
        _pendingSlowDuration = 0f;
        _isSlowed = false;
    }

    public void Slow(float duration, float speed)
    {
        _pendingSlowDuration = duration;
        _pendingSlowIntensity = speed;
    }
}
