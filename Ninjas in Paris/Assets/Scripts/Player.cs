using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] float speed;

    public int combo = 0;
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

        if (Input.GetKey("x")) {
            GameObject[] enemyList = GameObject.FindGameObjectsWithTag("enemy");
            Array.Sort(enemyList, DistanceComparison); //sort by distance to player
            foreach (GameObject Enemy in enemyList) {
                if (Vector3.Distance(Enemy.transform.position, transform.position) > 5) break;

                if (Enemy.GetComponent<Enemy>().isAttacking()) {
                    Enemy.GetComponent<Enemy>().counter();
                    combo++;
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
