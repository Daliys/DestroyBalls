using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour {
    public GameObject CometParticles;
    GameObject DotGroup;
    public static int inverse; // only 1 or -1
    public GameObject DestroingParticles;
    Vector3 startPosition;
    public static int hillPoint
    {
        get { return HillPoint; }
        set { if (value < 0) HillPoint = 0; else HillPoint = value; }
    }
    static int HillPoint;
    Text HillPointText;
    bool ToutchToScreen;
    float objectGravity;
    public static bool fly;
    /////////////////////////////////////
    private Vector2 LAUNCH_VELOCITY = new Vector2(5f, 20f);
    private Vector2 INITIAL_POSITION;
    private readonly Vector2 GRAVITY = new Vector2(0f, -9.8f);
    private const float DELAY_UNTIL_LAUNCH = 1f;
    private bool launched = false;
    private float timeUntilLaunch = DELAY_UNTIL_LAUNCH;
    public Rigidbody2D rigidBody;
    public int NUM_DOTS_TO_SHOW;
    public GameObject trajectoryDotPrefab;
    public float DOT_TIME_STEP;
    List<GameObject> trajectoryDot;
    ////////////////////////////////////
    public GameObject BallDouble;
    public bool balldouble;
    public static bool ballComet;
    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }
    void Start () {
        objectGravity = rigidBody.gravityScale;
        DotGroup = Instantiate(new GameObject());
        DotGroup.name = "DotGroup";
        //Physics2D.IgnoreLayerCollision(8, 10);
        balldouble = true;
        fly = false;
        inverse = PlayerPrefs.GetInt("Inverse");
        fly = false;
        trajectoryDot = new List<GameObject>();
        for (int i = 0; i < NUM_DOTS_TO_SHOW; i++)
        {
            GameObject Dot = Instantiate(trajectoryDotPrefab, DotGroup.transform);
            trajectoryDot.Add(Dot);
            trajectoryDot[i].SetActive(false);
        }
        INITIAL_POSITION = transform.position;
        ///////////////////////////////////////////////////////////////
        ToutchToScreen = false;
        startPosition = transform.position;
        HillPoint = Game.MainBallHillPoint;
        HillPointText = transform.GetChild(0).GetChild(0).GetComponent<Text>();
    }
	
	void Update () {

        if (ballComet && !fly) SetCometBall();
        Controls();
    }

    void Controls()
    {
        if (Input.GetMouseButton(0) && Interface.start && !Game.isGenerate && !fly && GameObject.FindGameObjectsWithTag("BallDouble").Length == 0)
        {
            ToutchToScreen = true;
            LAUNCH_VELOCITY = new Vector2((transform.position.x - Camera.main.ScreenToWorldPoint(Input.mousePosition).x) * 2 * inverse, ((transform.position.y - 2) - Camera.main.ScreenToWorldPoint(Input.mousePosition).y - 2) * 2 * inverse);
            if (LAUNCH_VELOCITY.x < -5) LAUNCH_VELOCITY = new Vector2(-5, LAUNCH_VELOCITY.y);
            else if (LAUNCH_VELOCITY.x > 5) LAUNCH_VELOCITY = new Vector2(5, LAUNCH_VELOCITY.y);
            if (LAUNCH_VELOCITY.y < -5) LAUNCH_VELOCITY = new Vector2(LAUNCH_VELOCITY.x, -5);
            else if (LAUNCH_VELOCITY.y > 5) LAUNCH_VELOCITY = new Vector2(LAUNCH_VELOCITY.x, 5);
            //print(LAUNCH_VELOCITY);
            for (int i = 0; i < NUM_DOTS_TO_SHOW; i++)
            {
                trajectoryDot[i].SetActive(true);
                trajectoryDot[i].transform.position = CalculatePosition(DOT_TIME_STEP * i);
            }
        }
        else if (Input.GetMouseButtonUp(0) && !fly && ToutchToScreen && Interface.start)
        {
            fly = true;
            for (int i = 0; i < NUM_DOTS_TO_SHOW; i++)
            {
                trajectoryDot[i].SetActive(false);
                rigidBody.bodyType = RigidbodyType2D.Dynamic;
                Launch();
            }
        }
        else if (!Interface.start)
        {
            for (int i = 0; i < NUM_DOTS_TO_SHOW; i++)
            {
                trajectoryDot[i].SetActive(false);
            }
        }

        if (!ToutchToScreen)
        {
            gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            transform.position = startPosition;
            hillPoint = Game.MainBallHillPoint;
            HillPointText.text = Game.MainBallHillPoint.ToString();
            transform.rotation = new Quaternion(0, 0, 0, 0);
        }
        HillPointText.text = hillPoint.ToString();
        if (hillPoint <= 0)
        {
            GameObject i = Instantiate(DestroingParticles, transform.position, transform.rotation, transform.parent);
            i.GetComponent<DestroingGroupParticles>().RigidVelocity = rigidBody.velocity;
            HillPointText.text = null;
            ResetBall();
        }
        if (transform.position.y <= -20 || transform.position.y >= 10)
        {
            ResetBall();
        }
    }

    void SetCometBall()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        GetComponent<CircleCollider2D>().isTrigger = true;
        CometParticles.SetActive(true);
        ballComet = false;
    }

    void ResetBall()
    {
        hillPoint = Game.MainBallHillPoint;
        fly = false;
        Physics2D.IgnoreLayerCollision(8, 9, false);
        transform.position = startPosition;
        ToutchToScreen = false;
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        Game.isChekingClearField = true;
        GetComponent<SpriteRenderer>().color = Color.white;
        GetComponent<CircleCollider2D>().isTrigger = false;
        CometParticles.SetActive(false);
    }

    private void Launch()
    {
        rigidBody.velocity = LAUNCH_VELOCITY;
        launched = true;
    }

    private Vector2 CalculatePosition(float elapsedTime)
    {
        return GRAVITY * objectGravity * elapsedTime * elapsedTime * 0.5f +
                   LAUNCH_VELOCITY  * elapsedTime + INITIAL_POSITION;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "BallDestroy" && balldouble && hillPoint > 0 && fly)
        {
            GameObject d = Instantiate(BallDouble, transform.position, transform.rotation, null);
            d.GetComponent<BallDouble>().hillPoint = hillPoint;
            d.GetComponent<Rigidbody2D>().velocity = new Vector2(-rigidBody.velocity.x, rigidBody.velocity.y);
            balldouble = false;
        }
        
    }
}
