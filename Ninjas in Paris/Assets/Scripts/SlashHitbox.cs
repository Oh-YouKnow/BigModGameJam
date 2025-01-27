using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashHitbox : MonoBehaviour
{
    private int damage;
    public void Initialize(int damage)
    {
        this.damage = damage;
    }

    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Collided with {other.name}");
        if (other.CompareTag("enemy"))
        {
            Debug.Log("Enemy detected");
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}
