using System.Collections;
using UnityEngine;

public class SlashHitbox : MonoBehaviour
{
    
    private int damage=1;
    private CapsuleCollider hitbox; 
    private Renderer renderer; 

    public void Initialize(int damage, Vector3 rotation)
    {
        this.damage = damage;
        transform.eulerAngles = rotation;

        Debug.Log($"[SlashHitbox] Hitbox initialized with rotation: {rotation}");
    }

    private void Start()
    {
        
        hitbox = GetComponent<CapsuleCollider>();
        renderer = GetComponent<Renderer>();

        if (hitbox == null)
        {
            Debug.LogError("[SlashHitbox] CapsuleCollider is missing!");
        }

        if (renderer == null)
        {
            Debug.LogWarning("[SlashHitbox] Renderer is missing! Visualization won't work.");
        }

        
        if (hitbox != null) hitbox.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[SlashHitbox] Collided with: {other.name}");

        if (other.CompareTag("enemy"))
        {
            if (other != null)
            {
                other.GetComponent<EnemyBase>().TakeDamage(damage);
                Debug.Log($"[SlashHitbox] Hit {other.name} for {damage} damage.");
            }
            else
            {
                Debug.LogWarning($"[SlashHitbox] Collided with an enemy but couldn't find Enemy script on: {other.name}");
            }
        }
    }
}
