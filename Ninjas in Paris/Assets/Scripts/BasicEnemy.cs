using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class BasicEnemy : EnemyBase
{
    enum State {Idle, Attack}
    private State _state = State.Idle;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (armorClassText != null)
        {
            armorClassText.transform.rotation = Camera.main.transform.rotation; // Always face the camera
        }
        // If not attacking, follow player.
        if (_state == State.Idle && player != null)
        {
            FollowPlayer();
        }

        if (Vector3.Distance(transform.position, player.transform.position) < attackDistance && _state != State.Attack && idleTimer < 0) {
            _state = State.Attack;

            attackMarker.SetActive(true);
            damageArea.SetActive(true);
            attackTimer = 3;
        }

        if (isCountered)
        {
            isCountered = false;
            _state = State.Idle;
        }

        switch (_state) {
            case State.Attack:
                attackMarker.transform.localScale = new Vector3(1, 1, 1) * ((attackTimer / 3f) + .1f);
                attackTimer -= Time.deltaTime;

                if (attackTimer <= 0) {
                    player.GetComponent<Player>().takeDamage();
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
        Vector3 direction = (player.transform.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    public new void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }
}
