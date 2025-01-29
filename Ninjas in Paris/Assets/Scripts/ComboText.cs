using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ComboText : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI comboText;
    [SerializeField] float shakeIntensity;
    [SerializeField] float jumpIntensity;
    private int baseFontSize;

    private float shake = 0;
    private float jump = 0;


    private void Start() {
        baseFontSize = (int)comboText.fontSize;
    }

    // Update is called once per frame
    void Update()
    {
        if(shake > 0) {
            shake -= Time.deltaTime;
            transform.eulerAngles = new Vector3(0, 0, Mathf.Sin(shake * Mathf.PI * 6.66f) * shakeIntensity);


        }

        if(jump > 0) {
            jump -= Time.deltaTime;

            Vector3 pos = transform.position;
            pos.y -= Mathf.Sin(jump * Mathf.PI * 10) * jumpIntensity * Time.deltaTime;
            transform.position = pos;
        }
    }

    public void increaseCombo(int combo) {
        comboText.text = (combo + "x");

        shake = .3f;
        jump = .2f;
        if(comboText.fontSize < 130) comboText.fontSize += 1f / combo * 15;
    }

    public void killCombo() {
        comboText.fontSize = baseFontSize;
        comboText.text = ("0x");
        jump = .2f;
    }
}
