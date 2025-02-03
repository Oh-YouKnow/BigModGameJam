using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class BossEnemy : EnemyBase
{
    enum State {Idle, Attack}
    private State _state = State.Idle;
    private BasicEnemyAnim _animation;

    [SerializeField] private float lungeForce = 5f;
    [SerializeField] private float lungeFriction = 3f;
    private Vector3 lungeDirection;
    private float lungeSpeed;

    public int minArmor;
    public int maxArmor;
    // Start is called before the first frame update
    new void Start()
    {
        armorClass = Random.Range(minArmor, maxArmor);
        _animation = GetComponentInChildren<BasicEnemyAnim>();
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return;
        if (armorClassText != null)
        {
            armorClassText.transform.rotation = Camera.main.transform.rotation; // Always face the camera
        }
        // If not attacking, follow player.
        if (_state == State.Idle && player != null)
        {
            FollowPlayer();
        }

        if (Vector3.Distance(transform.position, player.transform.position) < attackDistance && 
            _state != State.Attack && idleTimer < 0) {
            _state = State.Attack;

            attackMarker.SetActive(true);
            damageArea.SetActive(true);
            attackTimer = 1;
        }

        // Upon being countered, ends attack and moves to idle
        
        if (isCountered)
        {
            _animation.SetRunning(false);
            isCountered = false;
            _state = State.Idle;
        }

        switch (_state) {
            case State.Attack:
                _animation.SetRunning(false);
                _animation.TriggerAttack();
                isParryable = true;
                damageArea.SetActive(true);
                attackMarker.transform.localScale = new Vector3(1, 1, 1) * ((attackTimer / 3f) + .1f);
                attackTimer -= Time.deltaTime;
                if (Vector3.Distance(transform.position, player.transform.position) > attackDistance)
                {
                    Debug.Log("[BasicEnemy] Player moved out of range, attack canceled.");
                    _state = State.Idle;
                    isParryable = false;
                    return;
                }
                if (attackTimer <= 0) {
                    _animation.TriggerAttack();
                    isParryable = false;
                    player.GetComponent<Player>().takeDamage();
                    idleTimer = 2;
                    _state = State.Idle;
                }
                break;
            case State.Idle:
                attackMarker.SetActive(false);
                damageArea.SetActive(false);
                idleTimer -= Time.deltaTime;
                break;
        }
    }
    private IEnumerator DisableDamageAreaAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        damageArea.SetActive(false);
    }

    private void FollowPlayer()
    {
        // Move towards the player
        _animation.SetRunning(true);
        Vector3 direction = (player.transform.position - transform.position).normalized;
        float spriteScaleX = (player.transform.position - transform.position).x > 0 ? 2 : -2;
        transform.localScale = new Vector3(spriteScaleX, transform.localScale.y, transform.localScale.z);
        transform.position += direction * moveSpeed * Time.deltaTime;
    }
    private bool isDead = false;

    public void Die()
    {
        if (isDead) return; // Prevent multiple calls
        moveSpeed = 0; // Stop movement
        isDead = true;
        //Make enemy non interactable
        isParryable = false;
        _state = State.Idle;
        attackMarker.SetActive(false);
        damageArea.SetActive(false);
        _animation.TriggerDeath();
        StartCoroutine(DestroyAfterAnimation());
    }

    private IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitForSeconds(1.5f); // Adjust based on animation length
        Destroy(gameObject);
    }

}
