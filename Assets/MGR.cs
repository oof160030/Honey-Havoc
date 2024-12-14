using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum gameState { START, GAME, END }
public class MGR : MonoBehaviour
{
    //Game State Management
    private gameState currentState;

    //UI Assets
    public CanvasGroup startCG, endCG;
    public TextMeshProUGUI scoreBox, highScoreBox;

    //Game Variables
    public int honeyCount;
    private int difficulty, highscore, misses;
    
    //Honey Spawn Timer
    public float honeySpawnTime;
    public float maxFall, minFall;
    private Timer honeyTimer, phaseTimer;

    //GameObjects
    public GameObject honey;
    public CC_basic playerSCR;
    
    // Start is called before the first frame update
    void Start()
    {
        currentState = gameState.START;
        highscore = 0;
        startCG.alpha = 1;
        endCG.alpha = 0;

        //Start Playing Music

        honeyTimer = new Timer(honeySpawnTime);
        phaseTimer = new Timer(12);
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentState)
        {

            case gameState.START:
                
                if(Input.GetKeyDown(KeyCode.Space))
                    SwitchState(gameState.GAME);

                break;

            case gameState.GAME:

                RunGame();

                break;

            case gameState.END:

                if (Input.GetKeyDown(KeyCode.Space))
                    SwitchState(gameState.GAME);

                break;

        }
    }

    private void InitializeGame()
    {
        //Set Player Position
        playerSCR.transform.position = Vector2.zero;
        playerSCR.ControlOn(true);

        //Set Game Variables
        honeyCount = 0;
        scoreBox.text = honeyCount.ToString();
        difficulty = 1;
        misses = 0;

        //Start Gameplay
        honeyTimer.ResetTimer(1);
        phaseTimer.ResetTimer();
    }

    private void RunGame()
    {
        //Advance the timer
        honeyTimer.UpdateTimer();
        phaseTimer.UpdateTimer();

        //If Honey Time is at zero, reset and spawn honey
        if(honeyTimer.HasTimerExpired())
        {
            GameObject Temp = Instantiate(honey, new Vector3(Random.Range(-4, 4), 5.5f), Quaternion.identity);

            switch (difficulty)
            {
                case 1:
                    honeyTimer.ResetTimer(2);
                    Temp.GetComponent<Rigidbody2D>().velocity = Vector2.down * Random.Range(1f, 2f);
                    break;

                case 2:
                    honeyTimer.ResetTimer(2);
                    Temp.GetComponent<Rigidbody2D>().velocity = Vector2.down * (Random.Range(1f, 3f));
                    break;

                case 3:
                    honeyTimer.ResetTimer(1.5f);
                    Temp.GetComponent<Rigidbody2D>().velocity = Vector2.down * (Random.Range(1f, 3f));
                    break;

                case 4:
                    honeyTimer.ResetTimer(1.2f);
                    Temp.GetComponent<Rigidbody2D>().velocity = Vector2.down * (Random.Range(2f, 3f));
                    break;

                case 5:
                    honeyTimer.ResetTimer(0.8f);
                    Temp.GetComponent<Rigidbody2D>().velocity = Vector2.down * (Random.Range(2f, 3f));
                    break;

            }
        }

        if(phaseTimer.HasTimerExpired())
        {
            if(difficulty <= 4)
            {
                phaseTimer.ResetTimer();
                difficulty++;
                Debug.Log("Difficulty now " + difficulty.ToString());
            }
            else if (difficulty == 5)
            {
                phaseTimer.StopAlarm();
            }
        }

        if(misses >= 3)
        {
            SwitchState(gameState.END);
        }
    }

    public void UpdateHoney(int change)
    {
        honeyCount = honeyCount + change;
        if (honeyCount < 0)
            honeyCount = 0;
        scoreBox.text = honeyCount.ToString();
    }

    public void UpdateMisses(int change)
    {
        misses += change;

        //Update misses visual
        if(currentState == gameState.GAME)
        { /* Update Visual */ }
    }

    private void SwitchState(gameState newState)
    {
        switch (currentState)
        {

            case gameState.START:
                if(newState == gameState.GAME)
                {
                    //Hide UI
                    startCG.alpha = 0;

                    //Initialize Game Content
                    InitializeGame();

                    currentState = gameState.GAME;
                }
                break;

            case gameState.GAME:
                if (newState == gameState.END)
                {
                    //Hide UI
                    endCG.alpha = 1;

                    //Stop the Game
                    playerSCR.ControlOn(true);
                    honeyTimer.PauseTimer();

                    highScoreBox.text = "Score: " + honeyCount.ToString() + "\nHigh Score: " + highscore.ToString();

                    currentState = gameState.END;
                }
                break;

            case gameState.END:
                //Hide UI
                endCG.alpha = 0;

                //Initialize Game Content
                InitializeGame();

                currentState = gameState.GAME;
                break;
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

    public void ResetTimer(float X)
    {
        currentTime = X;
        expired = false;
        running = true;
    }

    public void StartTimer()
    {
        running = true;
        expired = false;
    }

    public void PauseTimer()
    {
        running = false;
    }

    public bool HasTimerExpired()
    {
        return (expired);
    }

    public void StopAlarm()
    {
        if(expired)
        {
            expired = false;
            running = false;
        }
    }

    public float CurrentTime()
    {
        if (running)
            return (currentTime);
        else
            return -1;
    }
}
