using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CC_basic : MonoBehaviour
{
    public MGR manager;
    private Rigidbody2D RB2;

    public float maxMoveSpeed, accelRate, decelRate;
    private int xIn, yIn;
    private bool controlOn; public void ControlOn(bool X) { controlOn = X; }
    
    // Start is called before the first frame update
    void Start()
    {
        RB2 = GetComponent<Rigidbody2D>();
        controlOn = false;
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

        //Allow dash, if (1) space pressed, and (2) directional input and (3) enough honey on hand 
        if(Input.GetKeyDown(KeyCode.Space) && (xIn != 0 || yIn != 0) && manager.honeyCount >= 3)
        {
            RB2.velocity = new Vector2(xIn, yIn).normalized * maxMoveSpeed;
            manager.UpdateHoney(-3);
        }

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
            manager.UpdateHoney(1);
            Destroy(collision.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Wall"))
        {
            manager.UpdateHoney(-1);            
        }
    }
}
