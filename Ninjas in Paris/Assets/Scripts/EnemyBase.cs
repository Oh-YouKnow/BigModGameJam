using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public abstract class EnemyBase : MonoBehaviour
{
    // Bare minimum stats for enemies
    
    [SerializeField] public float moveSpeed = 10f; // default values
    [SerializeField] public float attackDistance = 5f;
    [SerializeField] public GameObject damageArea;
    [SerializeField] public int currentHealth = 5;
    [SerializeField] public int armorClass = 1;
    [SerializeField] public GameObject attackMarker;
    [SerializeField] public TextMeshPro armorClassText;
    public float attackTimer;
    public float idleTimer;
    public float deathTimer;

    public Player player;
    public EnemySpawner spawner;
    public bool isParryable;
    public bool isCountered;
    public bool isDead;

    // Start is called before the first frame update
    public void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        armorClassText.text = armorClass.ToString();
        spawner = GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>();
    }

    // Update is called once per frame
    
    public void TakeDamage(int damage)
    {
        Debug.Log("TakeDamage Called");
        int lastCombo = GameObject.FindWithTag("Player").GetComponent<Player>().GetLastCombo(); // Get last combo

        // Check against Armor Class
        if (lastCombo < armorClass)
        {
            Debug.Log($"{gameObject.name}'s Armor Class blocked the attack! Required combo: {armorClass}, but got: {lastCombo}");
            return;
        }

        currentHealth -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage! Remaining health: {currentHealth}");

        if (currentHealth <= 0)
        {
            StartCoroutine(Die());
        }
    }
    
    private IEnumerator Die()
    {
        moveSpeed = 0;
        isParryable = false;
        attackMarker.SetActive(false);
        damageArea.SetActive(false);
        armorClassText.text = "";
        yield return new WaitForSeconds(deathTimer);
        Debug.Log($"{gameObject.name} has been destroyed!");
        Destroy(gameObject);
        
        // Find the Enemy Spawner and communicate an enemy has died.
        spawner.currentEnemyCount--;
    }

    // isCountered allows for any enemy to have individual countered states.
    public void Counter() {
        Debug.Log($"{gameObject.name} has been countered!");
        isCountered = true;
        isParryable = false;
        idleTimer = 2;
    }
}
