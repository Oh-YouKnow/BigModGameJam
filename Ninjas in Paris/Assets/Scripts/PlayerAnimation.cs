using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator YeAnimator;
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
    public void SetBlocking(bool isBlocking)
    {
        if (YeAnimator != null)
        {
            YeAnimator.SetBool("Blocking", isBlocking);
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
        Debug.Log("Finished waiting after attack animation.");
    }
}
