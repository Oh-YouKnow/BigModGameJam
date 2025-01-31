using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{


    [SerializeField] float speed;
    [SerializeField] float counterSpeed;
    [SerializeField] float counterDistance;
    [SerializeField] private Transform sprite;
    [SerializeField] private GameObject cylinderHitbox;
    [SerializeField] private float hitboxWidth = 5f;
    [SerializeField] private float hitboxHeight = 0.5f;
    [SerializeField] private GameObject smokeSprite;

    [Header("Sliding Parameters")]
    [SerializeField] private float force = 2f;
    [SerializeField] private float friction = 3f;
    private Vector3 slidingDirection;
    private float slidingSpeed;
    private bool isSliding;
    private int lastCombo = 0; // Store the last combo value

    [SerializeField] private GameObject smokePrefab;
    [SerializeField] private Transform smokeParent; // Origin object for smoke
    [SerializeField] private float smokeSpacing = 1f; // Space between smoke
    [SerializeField] private float smokeSpawnDelay = 0.05f; // Delay for smoke
    [SerializeField] private float smokeSpawnHeight = 1f; // Height for smoke
    [SerializeField] private float smokeLifetime = 1f; // Lifetime for smoke
    [SerializeField] private float smokeScaleFactor = 1f; // Scale for smoke
    [SerializeField] private GameObject cylinderHitboxPrefab; // Prefab for cylinder hitbox (leave as empty game object)
    [SerializeField] private float hitboxLifetime = 0.5f; // Learn to read, dumbass
    [SerializeField] private float hitboxScaleFactor = 1f; // Refer to above comment.

    public int combo = 0;
    [SerializeField] private GameObject comboText;


    private int health = 5;
    private int maxHealth = 5;

    private GameObject healthUI;

    private PlayerAnimation playerAnimation; // Reference to the animation controller

    private float moveTimer = 0;
    private Vector3 oPlayerPosition;
    private Vector3 targetPosition;



    void Start()
    {
        playerAnimation = GetComponentInChildren<PlayerAnimation>();
        
        cylinderHitbox.SetActive(false); // Hide initially

        healthUI = GameObject.Find("HeartUI");
    }


    void Update()
    {
        Vector3 input = Vector3.zero;

        if (Input.GetButton("Left")) input += new Vector3(-1, 0, 0);
        if (Input.GetButton("Right")) input += new Vector3(1, 0, 0);
        if (Input.GetButton("Up")) input += new Vector3(0, 0, 1);
        if (Input.GetButton("Down")) input += new Vector3(0, 0, -1);

        if (isSliding)
        {
            HandleSliding();
            
        }



        input = Vector3.Normalize(input);


        transform.position += input * Time.deltaTime * speed;
        if (input.magnitude > 0)
        {
            playerAnimation?.SetRunning(true);
        }
        else
        {
            playerAnimation?.SetRunning(false);

        }

        if (input.x != 0)
        {
            float spriteScaleX = input.x > 0 ? 1 : -1;

            sprite.localScale = new Vector3(spriteScaleX, sprite.localScale.y, sprite.localScale.z);
        }
        moveTimer -= Time.deltaTime;
        if(moveTimer > 0) {
            transform.position = targetPosition + (oPlayerPosition - targetPosition) * (moveTimer / counterSpeed);
        }

        if (Input.GetButtonDown("Counter") && moveTimer <= 0) {
            GameObject[] enemyList = GameObject.FindGameObjectsWithTag("enemy");
            Array.Sort(enemyList, DistanceComparison); //sort by distance to player
            foreach (GameObject Enemy in enemyList) {
                Vector3 enemyPos = Enemy.transform.position;
                Vector3 playerPos = transform.position;

                if (Vector3.Distance(enemyPos, playerPos) > counterDistance) break;
                
                if (Enemy.GetComponent<Enemy>().isAttacking()) {
                    Enemy.GetComponent<Enemy>().counter();

                    increaseCombo();

                    Vector3 pushDirection = (transform.position - Enemy.transform.position).normalized;
                    Debug.Log($"[Parry] Calculated Push Direction: {pushDirection}");

                    StartSliding(pushDirection);
                    //zoom over to them

                    targetPosition = Vector3.Normalize(new Vector3(playerPos.x - enemyPos.x, 0, playerPos.z - enemyPos.z));
                    targetPosition *= 1.5f;
                    targetPosition += enemyPos;
                    moveTimer = counterSpeed;
                    oPlayerPosition = playerPos;

                    //change sprite direction
                    float spriteScaleX = (enemyPos - playerPos).x > 0 ? 1 : -1;
                    sprite.localScale = new Vector3(spriteScaleX, sprite.localScale.y, sprite.localScale.z);


                    GameObject player = GameObject.FindWithTag("Player"); // Find the player GameObject (ensure it's tagged "Player")
                    if (player != null)
                    {
                        pushDirection = (player.transform.position - transform.position).normalized; // Direction from enemy to player
                        player.GetComponent<Player>().StartSliding(pushDirection); // Call StartSliding on the Player instance
                        break;

                        Debug.Log($"[Parry] Push direction: {pushDirection}");

                    }
                    break;
                }
            }
            

        }
        
        if (Input.GetButtonDown("Attack"))
        {
            PerformSlash();
        }

    }
    private void PerformSlash()
    {
        if (combo <= 0) return;
        playerAnimation?.TriggerAttack();

        float facingDirection = sprite.localScale.x > 0 ? 1 : -1;

        // Scale hitbox
        float baseHitboxSize = cylinderHitboxPrefab.transform.localScale.x;
        float scaledHitboxSize = baseHitboxSize * hitboxScaleFactor;

        float hitboxOffset = (scaledHitboxSize / 2) + 0.5f; // Offset hitbox in front of player

        // Determine spawn position
        Vector3 spawnPosition = new Vector3(
            transform.position.x + (hitboxOffset * facingDirection),
            transform.position.y,
            transform.position.z
        );

        // Rotate that shi
        Quaternion hitboxRotation = Quaternion.Euler(0, facingDirection > 0 ? 0 : 180, 0);

        if (cylinderHitboxPrefab == null)
        {
            Debug.LogError("[PerformSlash] Error: cylinderHitboxPrefab is NOT assigned!");
            return;
        }

        // Store the last combo value before resetting
        lastCombo = combo;

        GameObject spawnedHitbox = Instantiate(cylinderHitboxPrefab, spawnPosition, hitboxRotation);

        if (spawnedHitbox == null)
        {
            Debug.LogError("[PerformSlash] Error: Hitbox failed to spawn!");
        }
        else
        {
            Debug.Log($"[PerformSlash] Spawned hitbox at {spawnPosition} with scale {hitboxScaleFactor}, Last Combo Level: {lastCombo}");
        }

        spawnedHitbox.transform.localScale *= hitboxScaleFactor;

        Destroy(spawnedHitbox, hitboxLifetime);

        StartCoroutine(SpawnSmokeEffects(spawnedHitbox.transform));

        combo = 0;
        comboText.GetComponent<ComboText>().killCombo();
    }

    // Duh
    public int GetLastCombo()
    {
        return lastCombo;
    }


    private void increaseCombo() {
        combo++;
        comboText.GetComponent<ComboText>().increaseCombo(combo);
    }

    public void takeDamage() {
        health--;
        combo = 0;
        comboText.GetComponent<ComboText>().killCombo();

        healthUI.GetComponent<HealthUI>().takeDamage(health);
    }
    private IEnumerator ActivateHitbox()
    {
        cylinderHitbox.SetActive(true); // Enable hitbox
        yield return new WaitForSeconds(0.5f); // Keep it active for 0.5 seconds
        cylinderHitbox.SetActive(false); // Disable hitbox
    }

    private IEnumerator SpawnSmokeEffects(Transform hitboxTransform)
    {
        // Clear old smoke effects
        foreach (Transform child in smokeParent)
        {
            Destroy(child.gameObject);
        }

        float hitboxSize = Mathf.Max(hitboxTransform.localScale.x, hitboxTransform.localScale.z);
        float finalSmokeSize = hitboxSize * smokeScaleFactor;

        Vector3 hitboxCenter = hitboxTransform.position;
        float facingDirection = sprite.localScale.x > 0 ? 1 : -1;

        // Spawn smoke, staggered from player
        int smokeCount = 3;
        for (int i = 0; i < smokeCount; i++)
        {
            Vector3 spawnPosition = new Vector3(
                hitboxCenter.x + (i - smokeCount / 2f) * hitboxSize / smokeCount * facingDirection,
                hitboxCenter.y + smokeSpawnHeight,
                hitboxCenter.z
            );

            // Spawn smoke
            GameObject smoke = Instantiate(smokePrefab, spawnPosition, Quaternion.Euler(0, hitboxTransform.eulerAngles.y, 0));

            // Scale smoke
            smoke.transform.localScale = new Vector3(finalSmokeSize, finalSmokeSize, finalSmokeSize);

            // Destroy smoke
            Destroy(smoke, smokeLifetime);

            yield return new WaitForSeconds(smokeSpawnDelay);
        }
    }



    // Currently not being used, may be useful for scaling the hitbox.
    private float CalculateComboScale(int combo)
    {
        if (combo >= 1 && combo <= 5)
            return 1f * combo;
        else if (combo >= 6 && combo <= 8)
            return 2f * combo;
        else if (combo >= 9 && combo <= 10)
            return 4f * combo;
        else if (combo >= 11)
            return 5f + combo;

        return 1f;
    }

    public void StartSliding(Vector3 direction)
    {
        if (direction == Vector3.zero)
        {
            Debug.LogWarning("[StartSliding] Direction is zero. Sliding will not start.");
            return;
        }
        isSliding = true;
        slidingDirection = direction.normalized;
        slidingSpeed = force;
        Debug.Log($"[StartSliding] Sliding started. Direction: {slidingDirection}, Speed: {slidingSpeed}, Force: {force}");
    }

    private void HandleSliding()
    {
        if (slidingSpeed > 0)
        {
            // Apply sliding movement
            transform.position += slidingDirection * slidingSpeed * Time.deltaTime;

            // Debug log for sliding progress
            Debug.Log($"[HandleSliding] Sliding. Position: {transform.position}, Speed: {slidingSpeed}, Direction: {slidingDirection}");

            // Reduce speed using friction
            slidingSpeed -= friction * Time.deltaTime;
        }
        else
        {
            // Stop sliding
            isSliding = false;
            Debug.Log("[HandleSliding] Sliding stopped.");
        }
    }





    private int DistanceComparison(GameObject a, GameObject b) {
        //null check, I consider nulls to be less than non-null
        if (a == null) return (b == null) ? 0 : -1;
        if (b == null) return 1;

        var ya = Vector3.Distance(a.transform.position, transform.position);
        var yb = Vector3.Distance(b.transform.position, transform.position);
        return ya.CompareTo(yb); //here I use the default comparison of floats
    }
}
