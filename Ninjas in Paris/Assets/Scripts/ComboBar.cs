using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboBar : MonoBehaviour
{


    GameObject player;
    private static Image ComboBarImage;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        ComboBarImage = this.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        ComboBarImage.fillAmount = player.GetComponent<Player>().combo / 10f;
    }
}
