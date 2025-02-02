using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BasicEnemyAnim : MonoBehaviour
{
    public Animator animator;
    [SerializeField] private float waitTime = 0.5f; 

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    
    public void SetRunning(bool isRunning)
    {
        if (animator != null)
        {
            animator.SetBool("Running", isRunning);
        }
    }

    
    public void TriggerAttack()
    {
        if (animator != null)
        {
            StartCoroutine(TriggerAttackWithWait());
        }
    }
    private IEnumerator TriggerAttackWithWait()
    {
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(waitTime);
        animator.SetTrigger("Attack");
        Debug.Log("Finished waiting after attack animation.");
    }
    public void TriggerDeath()
    {
        if (animator != null)
        {
            animator.SetTrigger("Dead");
        }
    }


    public void TriggerAttack2()
    {
        if (animator != null)
        {
            StartCoroutine(TriggerAttack2WithWait());
        }
    }
    private IEnumerator TriggerAttack2WithWait()
    {
        animator.SetTrigger("Attack2");
        yield return new WaitForSeconds(waitTime);
        animator.SetTrigger("Attack2");
        Debug.Log("Finished waiting after attack animation.");
    }
}
