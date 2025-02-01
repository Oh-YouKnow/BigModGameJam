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
}
