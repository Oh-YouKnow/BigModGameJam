using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class BasicEnemy : EnemyBase
{
    enum State {Idle, Attack}
    private State _state = State.Idle;
    private BasicEnemyAnim _animation;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        _animation = GetComponentInChildren<BasicEnemyAnim>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
            return;
        if (currentHealth <= 0)
        {
            isDead = true;
            Debug.Log("Enemy Dead");
            _animation.TriggerDeath();
            return;
        }
        
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
                attackMarker.transform.localScale = new Vector3(1, 1, 1) * ((attackTimer / 3f) + .1f);
                attackTimer -= Time.deltaTime;

                if (attackTimer <= 0) {
                    _animation.TriggerAttack();
                    isParryable = false;
                    if (Vector3.Distance(transform.position, player.transform.position) < attackDistance) player.GetComponent<Player>().takeDamage();
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
    
    private void FollowPlayer()
    {
        // Move towards the player
        _animation.SetRunning(true);
        Vector3 direction = (player.transform.position - transform.position).normalized;
        float spriteScaleX = (player.transform.position - transform.position).x > 0 ? 1 : -1;
        transform.localScale = new Vector3(spriteScaleX, transform.localScale.y, transform.localScale.z);
        armorClassText.transform.localScale = new Vector3(spriteScaleX, 1, 1);
        _lastDirection = spriteScaleX;
        // dummy code
        transform.position += direction * moveSpeed * Time.deltaTime;
    }
}
