using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    enum states {idle, attack };


    [SerializeField] float attackDistance;
    [SerializeField] GameObject damageArea;
    [SerializeField] int currentHealth = 5;

    [SerializeField] GameObject attackMarker;
    GameObject player;
    states state = states.idle;

    float attackTimer = 0;
    float idleTimer = 0;
    

    
    void Start()
    {
        player = GameObject.FindWithTag("Player");

    }

    
    void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < attackDistance && state != states.attack && idleTimer < 0) {
            state = states.attack;

            attackMarker.SetActive(true);
            damageArea.SetActive(true);
            attackTimer = 3;
        }

        switch (state) {
            case states.attack:
                attackMarker.transform.localScale = new Vector3(1, 1, 1) * ((attackTimer / 3f) + .1f);
                attackTimer -= Time.deltaTime;

                if (attackTimer <= 0) {
                    player.GetComponent<Player>().takeDamage();
                    state = states.idle;
                }
                break;
            case states.idle:
                attackMarker.SetActive(false);
                damageArea.SetActive(false);
                idleTimer -= Time.deltaTime;
                break;
        }
    }

    public bool isAttacking() {
        return state == states.attack;
    }

    public void counter() {
        state = states.idle;
        idleTimer = 2;
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage! Remaining health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} has been destroyed!");
        Destroy(gameObject);
    }
}
