using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator YeAnimator;
    //Animation length
    [SerializeField] private float waitTime = 0.5f;

    void Start()
    {
        YeAnimator = GetComponent<Animator>();
    }
    public void SetRunning(bool isRunning)
    {
        if (YeAnimator != null)
        {
            YeAnimator.SetBool("Running", isRunning);
        }
    }
    public void TriggerAttack()
    {
        if (YeAnimator != null)
        {
            StartCoroutine(TriggerAttackWithWait());
        }
    }
    private IEnumerator TriggerAttackWithWait()
    {
        YeAnimator.SetTrigger("Attack");
        yield return new WaitForSeconds(waitTime);
        YeAnimator.SetTrigger("Attack");
        Debug.Log("Attacked.");
    }
}
