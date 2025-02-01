using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HealthUI : MonoBehaviour
{

    [SerializeField] Sprite healthIcon;
    [SerializeField] float heartSpacing;

    private int currentHealth = 5;

    private List<GameObject> hearts = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        drawHearts();
    }

    public void takeDamage(int health) {
        currentHealth = health;
        drawHearts();
    }


    private void drawHearts() {
        foreach(GameObject heart in hearts) { Destroy(heart); }
        hearts.Clear();

        for(int i = 0; i < currentHealth; i++) {
            GameObject heart = new GameObject("heart", typeof(SpriteRenderer));
            heart.GetComponent<SpriteRenderer>().sprite = healthIcon;
            heart.transform.parent = this.transform;
            heart.transform.position = new Vector3(transform.position.x + heartSpacing * i, transform.position.y, transform.position.z);
            heart.transform.localScale *= 1.8f;
            hearts.Add(heart);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
