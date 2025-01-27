using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] float speed;
    [SerializeField] float counterSpeed;
    [SerializeField] private Transform sprite;
    [SerializeField] private GameObject slashPrefab;
    [SerializeField] private float slashRadius = 5f; 
    [SerializeField] private Vector3 slashOffset = Vector3.zero;
    [SerializeField] private float slashDistance = 1.5f;
    [SerializeField] private Vector3 slashRotation = Vector3.zero;
    [SerializeField] private GameObject slashHitboxPrefab;
    [SerializeField] private Vector3 slashHitboxOffset = Vector3.zero;
    [SerializeField] private Vector3 slashHitboxRotation = Vector3.zero;

    public int combo = 0;




    private PlayerAnimation playerAnimation; // Animation Controller

    private float moveTimer = 0;
    private Vector3 oPlayerPosition;
    private Vector3 targetPosition;
    
    void Start()
    {
        playerAnimation = GetComponentInChildren<PlayerAnimation>();

    }


    void Update()
    {
        Vector3 input = Vector3.zero;

        if (Input.GetKey("left")) input += new Vector3(-1, 0, 0);
        if (Input.GetKey("right")) input += new Vector3(1, 0, 0);
        if (Input.GetKey("up")) input += new Vector3(0, 0, 1);
        if (Input.GetKey("down")) input += new Vector3(0, 0, -1);
        if (Input.GetKey("a")) input += new Vector3(-1, 0, 0);
        if (Input.GetKey("d")) input += new Vector3(1, 0, 0);
        if (Input.GetKey("w")) input += new Vector3(0, 0, 1);
        if (Input.GetKey("s")) input += new Vector3(0, 0, -1);

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

        if (Input.GetKey("x") && moveTimer <= 0) {
            GameObject[] enemyList = GameObject.FindGameObjectsWithTag("enemy");
            Array.Sort(enemyList, DistanceComparison); //sort by distance to player
            foreach (GameObject Enemy in enemyList) {
                Vector3 enemyPos = Enemy.transform.position;
                Vector3 playerPos = transform.position;

                if (Vector3.Distance(enemyPos, playerPos) > 5) break;
                
                if (Enemy.GetComponent<Enemy>().isAttacking()) {
                    Enemy.GetComponent<Enemy>().counter();
                    combo++;


                    //zoom over to them

                    targetPosition = Vector3.Normalize(new Vector3(playerPos.x - enemyPos.x, 0, playerPos.z - enemyPos.z));
                    targetPosition *= 1.5f;
                    targetPosition += enemyPos;
                    moveTimer = counterSpeed;
                        oPlayerPosition = playerPos;
                    break;
                }
            }
            

        }
        if (Input.GetKey(KeyCode.Mouse1) && moveTimer <= 0)
        {
            GameObject[] enemyList = GameObject.FindGameObjectsWithTag("enemy");
            Array.Sort(enemyList, DistanceComparison); //sort by distance to player
            foreach (GameObject Enemy in enemyList)
            {
                Vector3 enemyPos = Enemy.transform.position;
                Vector3 playerPos = transform.position;

                if (Vector3.Distance(enemyPos, playerPos) > 5) break;

                if (Enemy.GetComponent<Enemy>().isAttacking())
                {
                    Enemy.GetComponent<Enemy>().counter();
                    combo++;


                    //zoom over to them

                    targetPosition = Vector3.Normalize(new Vector3(playerPos.x - enemyPos.x, 0, playerPos.z - enemyPos.z));
                    targetPosition *= 1.5f;
                    targetPosition += enemyPos;
                    moveTimer = counterSpeed;
                    oPlayerPosition = playerPos;
                    break;
                }
            }
            

        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            PerformSlash();
        }

    }
    private void PerformSlash()
    {
        if (combo <= 0) return;

        Vector3 facingDirection = sprite.localScale.x > 0 ? Vector3.right : Vector3.left;
        playerAnimation?.TriggerAttack();


        Vector3 slashPosition = transform.position + facingDirection * slashDistance;

        
        float adjustedYRotation = sprite.localScale.x > 0
            ? slashRotation.y
            : slashRotation.y - 180;

        Quaternion slashQuaternion = Quaternion.Euler(slashRotation.x, adjustedYRotation, slashRotation.z);

        GameObject slash = Instantiate(slashPrefab, slashPosition, slashQuaternion);
        Destroy(slash, 0.5f); 
        Vector3 slashHitboxPosition = slashPosition + slashHitboxOffset;
        float adjustedHitboxYRotation = sprite.localScale.x > 0
            ? slashHitboxRotation.y
            : slashHitboxRotation.y - 180;

        Quaternion slashHitboxQuaternion = Quaternion.Euler(
            slashHitboxRotation.x,
            adjustedHitboxYRotation,
            slashHitboxRotation.z
        );

       
        GameObject slashHitbox = Instantiate(slashHitboxPrefab, slashHitboxPosition, slashHitboxQuaternion);

        
        SlashHitbox hitboxScript = slashHitbox.GetComponent<SlashHitbox>();
        if (hitboxScript != null)
        {
            hitboxScript.Initialize(combo);
        }
        Destroy(slashHitbox, 0.5f);
        combo = 0;
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
