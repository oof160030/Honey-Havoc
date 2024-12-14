using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CC_basic : MonoBehaviour
{
    private Rigidbody2D RB2;

    public float maxMoveSpeed, accelRate, decelRate;
    private int xIn, yIn;

    private int honeyCount;
    public TextMeshProUGUI scoreBox;
    
    // Start is called before the first frame update
    void Start()
    {
        RB2 = GetComponent<Rigidbody2D>();
        honeyCount = 0;

        scoreBox.text = honeyCount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        //Check Input
        xIn = 0 + (Input.GetKey(KeyCode.LeftArrow) ? -1 : 0) + (Input.GetKey(KeyCode.RightArrow) ? 1 : 0);
        yIn = 0 + (Input.GetKey(KeyCode.DownArrow) ? -1 : 0) + (Input.GetKey(KeyCode.UpArrow) ? 1 : 0);

        //Set Movement
        RB2.velocity += new Vector2(xIn, yIn).normalized * accelRate * Time.deltaTime;

        //Optional Additive Speed


        //Cap movespeed
        if (RB2.velocity.magnitude > maxMoveSpeed)
            RB2.velocity = RB2.velocity.normalized * maxMoveSpeed;

        //Reduce speed without input
        if (xIn == 0 && yIn == 0 && (decelRate * Time.deltaTime) >= RB2.velocity.magnitude)
            RB2.velocity = Vector2.zero;
        else if(xIn == 0 && yIn == 0)
        {
            RB2.velocity = RB2.velocity - (RB2.velocity.normalized * decelRate * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Item"))
        {
            honeyCount++;
            scoreBox.text = honeyCount.ToString();
            Destroy(collision.gameObject);
        }
    }
}
