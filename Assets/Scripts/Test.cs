using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {
    private Vector2 LAUNCH_VELOCITY = new Vector2(5f, 20f);
    private Vector2 INITIAL_POSITION;
    private readonly Vector2 GRAVITY = new Vector2(0f, -29.4f);
    private const float DELAY_UNTIL_LAUNCH = 1f;
    private bool launched = false;
    private float timeUntilLaunch = DELAY_UNTIL_LAUNCH;
    private Rigidbody2D rigidBody;
    public int NUM_DOTS_TO_SHOW;
    public GameObject trajectoryDotPrefab;
    public float DOT_TIME_STEP;
    List<GameObject> trajectoryDot;
    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        trajectoryDot = new List<GameObject>();
        for (int i = 0; i < NUM_DOTS_TO_SHOW; i++)
        {
            GameObject Dot = Instantiate(trajectoryDotPrefab);
            trajectoryDot.Add(Dot);
            trajectoryDot[i].SetActive(false);
        }
        INITIAL_POSITION = transform.position;
    }
    private void Update()
    {
        
        if (Input.GetMouseButton(0))
        {
            LAUNCH_VELOCITY = new Vector2((transform.position.x - Camera.main.ScreenToWorldPoint(Input.mousePosition).x)*5, (transform.position.y - Camera.main.ScreenToWorldPoint(Input.mousePosition).y)*5);
            if (LAUNCH_VELOCITY.x < -10) LAUNCH_VELOCITY = new Vector2(-10, LAUNCH_VELOCITY.y);
            else if(LAUNCH_VELOCITY.x > 10) LAUNCH_VELOCITY = new Vector2(10, LAUNCH_VELOCITY.y);
            if(LAUNCH_VELOCITY.y < -10) LAUNCH_VELOCITY = new Vector2(LAUNCH_VELOCITY.x, -10);
            else if(LAUNCH_VELOCITY.y > 10) LAUNCH_VELOCITY = new Vector2(LAUNCH_VELOCITY.x, 10);
            print(LAUNCH_VELOCITY);
            for (int i = 0; i < NUM_DOTS_TO_SHOW; i++)
            {
                trajectoryDot[i].SetActive(true);
                trajectoryDot[i].transform.position = CalculatePosition(DOT_TIME_STEP * i);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            for (int i = 0; i < NUM_DOTS_TO_SHOW; i++)
            {
                trajectoryDot[i].SetActive(false);
                Launch();
                rigidBody.bodyType = RigidbodyType2D.Dynamic;
            }
        }


    }
    private void Launch()
    {
        rigidBody.velocity = LAUNCH_VELOCITY;
        launched = true;
    }
     
    private Vector2 CalculatePosition(float elapsedTime)
    {
        return GRAVITY * elapsedTime * elapsedTime * 0.5f +
                   LAUNCH_VELOCITY * elapsedTime + INITIAL_POSITION;
    }
}

