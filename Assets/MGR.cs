using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGR : MonoBehaviour
{
    //Honey Spawn Timer
    public float honeySpawnTime;
    public float maxFall, minFall;
    private Timer honeyTimer;

    //Honey object
    public GameObject Honey;
    
    // Start is called before the first frame update
    void Start()
    {
        honeyTimer = new Timer(honeySpawnTime);
        honeyTimer.ResetTimer();
    }

    // Update is called once per frame
    void Update()
    {

        //Advance the timer
        honeyTimer.UpdateTimer();

        //If time is at zero, reset and spawn honey
        if(honeyTimer.HasTimerExpired())
        {
            honeyTimer.ResetTimer();

            //Spawn Honey
            GameObject Temp = Instantiate(Honey, new Vector3(Random.Range(-4, 4), 5.5f), Quaternion.identity);
            Temp.GetComponent<Rigidbody2D>().velocity = Vector2.down * Random.Range(minFall, maxFall);
        }

    }
}


class Timer
{
    private float maxTime, currentTime;
    private bool running, expired;

    public Timer()
    {
        maxTime = 0;
        currentTime = 0;
        running = false;
        expired = false;
    }

    public Timer(float startTime)
    {
        maxTime = startTime;
        currentTime = 0;
        running = false;
        expired = false;
    }

    public void UpdateTimer()
    {
        if(running)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                running = false;
                expired = true;
                currentTime = 0;
            }
        }
    }

    public void ResetTimer()
    {
        currentTime = maxTime;
        expired = false;
        running = true;
    }

    public bool HasTimerExpired()
    {
        return (expired);
    }

    public float CurrentTime()
    {
        if (running)
            return (currentTime);
        else
            return -1;
    }
}
