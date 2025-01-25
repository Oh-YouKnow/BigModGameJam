using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] float speed;
    [SerializeField] float counterSpeed;

    public int combo = 0;





    private float moveTimer = 0;
    private Vector3 oPlayerPosition;
    private Vector3 targetPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 input = Vector3.zero;

        if (Input.GetKey("left")) input += new Vector3(-1, 0, 0);
        if (Input.GetKey("right")) input += new Vector3(1, 0, 0);
        if (Input.GetKey("up")) input += new Vector3(0, 0, 1);
        if (Input.GetKey("down")) input += new Vector3(0, 0, -1);

        input = Vector3.Normalize(input);


        transform.position += input * Time.deltaTime * speed;

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
