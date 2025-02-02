using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingEnemy : EnemyBase
{
    [SerializeField] private float explosionRadius = 2f;
    [SerializeField] private int explosionDamage = 10;
    [SerializeField] private float speed = 5f;
    private bool isExploding = false;
    [SerializeField] private float explosionDelay = 0.5f;
    [SerializeField] private float delayMultValue = 10f;
    private Animator animator;
    [SerializeField] private GameObject explosionSprite; // Assign this in the Inspector
    [SerializeField] private GameObject clownSprite;
    void Start()
    {
        if (player == null)
        {
            GameObject playerObject = GameObject.Find("Player");
            if (playerObject != null)
                player = playerObject.GetComponent<Player>();
        }

        animator = GetComponentInChildren<Animator>();

        if (animator == null)
        {
            Debug.LogError("ExplodingEnemy: No Animator found!");
        }

        if (explosionSprite != null)
        {
            explosionSprite.SetActive(false); // Make sure it's hidden initially
        }
        else
        {
            Debug.LogWarning("ExplodingEnemy: No explosion sprite assigned!");
        }

        if (attackMarker == null || damageArea == null)
        {
            Debug.LogError("ExplodingEnemy: Missing attackMarker or damageArea!");
        }
    }

    void Update()
    {
        if (isExploding || player == null) return;

        FollowPlayer();

        if (Vector3.Distance(transform.position, player.transform.position) <= explosionRadius)
        {
            StartExplosion();
        }
    }

    private void FollowPlayer()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        float spriteScaleX = (direction.x > 0) ? 1 : -1;
        transform.localScale = new Vector3(spriteScaleX, transform.localScale.y, transform.localScale.z);
        transform.position += direction * speed * Time.deltaTime;
    }

    private void StartExplosion()
    {
        if (isExploding) return; // Prevent multiple explosions

        isExploding = true;
        isParryable = true;
        speed = 0;

        if (attackMarker != null) attackMarker.SetActive(true);
        if (damageArea != null) damageArea.SetActive(true);

        Debug.Log("ExplodingEnemy started explosion!");

        if (animator != null)
        {
            animator.ResetTrigger("Idle");
            animator.SetTrigger("Explode");
        }
        else
        {
            Debug.LogWarning("ExplodingEnemy has no Animator, using sprite explosion instead.");
        }

        StartCoroutine(TriggerExplosionEffect());
    }

    private IEnumerator TriggerExplosionEffect()
    {
        yield return new WaitForSeconds(explosionDelay);

        // If animation isn't playing, enable explosion sprite
        if (animator == null || !animator.GetCurrentAnimatorStateInfo(0).IsName("Explode"))
        {
            Debug.LogWarning("Explosion animation did not play, using sprite instead.");
            if (explosionSprite != null)
            {
                explosionSprite.SetActive(true);
                clownSprite.SetActive(false);
                damageArea.SetActive(false);
            }
        }

        yield return new WaitForSeconds(explosionDelay); // Wait for explosion effect

        StartCoroutine(ExplodeAfterDelay(0f));
    }

    private IEnumerator ExplodeAfterDelay(float delay)
    {
        

        Debug.Log($"Checking parry state: isParryable = {isParryable}");

        if (isParryable && player != null && player.IsParrying())
        {
            Debug.Log("Explosion was parried!");
            isExploding = false;
            isParryable = false;

            if (attackMarker != null) attackMarker.SetActive(false);
            if (damageArea != null) damageArea.SetActive(false);

            

            player.Block();
            yield return new WaitForSeconds(delayMultValue * explosionDelay);
            if (explosionSprite != null) explosionSprite.SetActive(false); // Hide explosion sprite after parry
            yield break;
        }

        Debug.Log("ExplodingEnemy exploded!");

        if (player != null && Vector3.Distance(transform.position, player.transform.position) <= explosionRadius)
        {
            player.GetComponent<Player>().takeDamage();
        }

        if (explosionSprite != null)
        {
            yield return new WaitForSeconds(delayMultValue * explosionDelay); // Keep explosion visible
            explosionSprite.SetActive(false); // Hide explosion
        }

        Destroy(gameObject);
    }
}
