using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 input = Vector3.zero;

        if (Input.GetButton("Left")) Debug.Log("a");
        if (Input.GetButton("Left")) input += new Vector3(-1, 0, 0);
        if (Input.GetButton("Right")) input += new Vector3(1, 0, 0);
        if (Input.GetButton("Up")) input += new Vector3(0, 0, 1);
        if (Input.GetButton("Down")) input += new Vector3(0, 0, -1);

        input = Vector3.Normalize(input);


        transform.position += input * Time.deltaTime * speed;

    }
}
