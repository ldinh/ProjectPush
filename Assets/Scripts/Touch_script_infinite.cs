using UnityEngine;
using System.Collections;

public class Touch_script_infinite : MonoBehaviour {
  private bool startedTouch = false;
  private bool touchingFloor = false;
  private Vector2 sideGrav = new Vector2(12.0f, 0);
  private bool isDead = false;
  private bool highScore = false;
  private float currentScore;
  private bool firstDead = true;
  private float speed = 8.0f;
  private int colCounter = 0;
  private float timeSinceSpeedIncrease = 0.0f;

  private bool onLeftWall = false;
  private bool onRightWall = false;
  private bool weirdFloor = false;
  private Vector3 positionDied;
  GameObject main_Cam;
  GUI_controller_infinite other;
  public Animator anim;
  private bool touchingLeftAnim;
  private bool touchingRightAnim;
  //TODO: WEIRD FLOOR IS CAUSING THE CLIPPING???

  public AudioClip jumpAudio;

  Vector3 scaler;
  void Start()
  {
    PlayerPrefs.SetFloat ("zSet", 10.0f);
    main_Cam = GameObject.Find ("Main Camera");
    other = (GUI_controller_infinite) main_Cam.GetComponent (typeof(GUI_controller_infinite));
    if (PlayerPrefs.GetInt ("ChosenSpecial3") == 1)
    {

    }
    else
    {
      this.GetComponent<TrailRenderer>().enabled=false;
    }
    if (PlayerPrefs.GetInt ("ChosenSpecial1") == 1)
    {

    }
    else
    {
      (gameObject.GetComponent("Halo") as Behaviour).enabled = false;
    }
  }

  void Awake()
  {
    Application.targetFrameRate = 30;
    if(!PlayerPrefs.HasKey("HighScore")) //set highscore if current score is greater
    {
      PlayerPrefs.SetFloat("HighScore",0.0f);
    }

  }
  void Update()
  {
    if(Time.time - 3 > timeSinceSpeedIncrease && Time.timeSinceLevelLoad < 80.0f)
    {
      timeSinceSpeedIncrease = Time.time;
      //TODO: CHARACTER SPEED!!!!
      speed += 0.80f;
      if(sideGrav.x < 0) // if sideGrav is negative
      {
        sideGrav = new Vector2( -speed, 0);
      }
      else
      {
        sideGrav = new Vector2( speed, 0);
      }
    }
    if (colCounter == 0)
    {
      touchingFloor = false;
    }
    else
    {
      touchingFloor = true;
    }
    if (Input.touchCount > 0) {
      Touch touch = Input.touches[0];
      switch (touch.phase) {
        case TouchPhase.Began:
        startedTouch = true;
        break;
        case TouchPhase.Ended:
        startedTouch = false;
        break;
      }
    }
    if (touchingFloor == false)
    {
      if(PlayerPrefs.GetInt("ChosenSpecial2") == 1)
      {
        this.GetComponent<Renderer>().enabled = false;
      }
      this.gameObject.transform.Translate (sideGrav * Time.deltaTime);
      if (sideGrav.x < 0)
      {
        if(PlayerPrefs.GetInt("ChosenCostume1") == 1)
        {
          anim.SetBool("jumpLeft2", true);
          anim.SetBool("jumpLeft", false);
          anim.SetBool("jumpRight", false);
          anim.SetBool("touchingRightAnim", false);
          anim.SetBool("touchingLeftAnim", false);

          anim.SetBool("jumpRight2", false);
          anim.SetBool("touchingRightAnim2", false);
          anim.SetBool("touchingLeftAnim2", false);
        }
        else
        {
          anim.SetBool("jumpLeft", true);
          anim.SetBool("jumpRight", false);
          anim.SetBool("touchingRightAnim", false);
          anim.SetBool("touchingLeftAnim", false);
          anim.SetBool("jumpLeft2", false);
          anim.SetBool("jumpRight2", false);
          anim.SetBool("touchingRightAnim2", false);
          anim.SetBool("touchingLeftAnim2", false);
        }
      }
      else
      {
        if(PlayerPrefs.GetInt("ChosenCostume1") == 1)
        {
          anim.SetBool("jumpRight2", true);
          anim.SetBool("jumpLeft", false);
          anim.SetBool("jumpRight", false);
          anim.SetBool("touchingRightAnim", false);
          anim.SetBool("touchingLeftAnim", false);
          anim.SetBool("jumpLeft2", false);

          anim.SetBool("touchingRightAnim2", false);
          anim.SetBool("touchingLeftAnim2", false);
        }
        else
        {
          anim.SetBool("jumpRight", true);
          anim.SetBool("jumpLeft", false);

          anim.SetBool("touchingRightAnim", false);
          anim.SetBool("touchingLeftAnim", false);
          anim.SetBool("jumpLeft2", false);
          anim.SetBool("jumpRight2", false);
          anim.SetBool("touchingRightAnim2", false);
          anim.SetBool("touchingLeftAnim2", false);
        }
      }
    }
    else
    {
      if(PlayerPrefs.GetInt("ChosenSpecial2") == 1)
      {
        this.GetComponent<Renderer>().enabled = true;
      }
    }

    if ((startedTouch && touchingFloor) || (Input.GetKeyDown (KeyCode.Space)&& touchingFloor))
    {
      GetComponent<AudioSource>().PlayOneShot(jumpAudio, 1.0f);
      this.gameObject.transform.Translate (-sideGrav * Time.deltaTime);
      sideGrav = -sideGrav;
      touchingFloor = false;

      startedTouch = false;
    }
    if (((this.gameObject.transform.position.x > 3.1f ) && firstDead) || ((this.gameObject.transform.position.x < -5.1f) && firstDead))
    {
      positionDied = this.gameObject.transform.position;
      firstDead = false;
      isDead = true;
      currentScore = Time.timeSinceLevelLoad;
      other.SetPositionDied(positionDied);
      other.SetCurrentScore(currentScore);
      other.SetIsDead(true);
      Destroy(this.gameObject);
    }
    if (((this.gameObject.transform.position.y > 3.0f ) && firstDead) || ((this.gameObject.transform.position.y < -6.2f) && firstDead))
    {
      positionDied = this.gameObject.transform.position;
      Instantiate(Resources.Load("Shatter"),positionDied,transform.rotation);
      firstDead = false;
      isDead = true;
      currentScore = Time.timeSinceLevelLoad;
      other.SetPositionDied(positionDied);
      other.SetCurrentScore(currentScore);
      other.SetIsDead(true);
      Destroy(this.gameObject);
    }
  }

  void OnCollisionEnter2D(Collision2D collision)
  {
    colCounter++;
    if (onLeftWall)
    {
      //TODO: IGNORE LEFT COLLISION FROM HERE
      if (collision.contacts[0].point.x > this.gameObject.transform.position.x) //touching right wall
      {
        //flip ();
        onRightWall = true;
        onLeftWall = false;
        if(PlayerPrefs.GetInt("ChosenCostume1") == 1)
        {
          anim.SetBool("touchingRightAnim2", true);
          anim.SetBool("jumpLeft", false);
          anim.SetBool("jumpRight", false);
          anim.SetBool("touchingRightAnim", false);
          anim.SetBool("touchingLeftAnim", false);
          anim.SetBool("jumpLeft2", false);
          anim.SetBool("jumpRight2", false);
          anim.SetBool("touchingLeftAnim2", false);
        }
        else
        {
          anim.SetBool("touchingRightAnim", true);
          anim.SetBool("jumpLeft", false);
          anim.SetBool("jumpRight", false);
          anim.SetBool("touchingLeftAnim", false);
          anim.SetBool("jumpLeft2", false);
          anim.SetBool("jumpRight2", false);
          anim.SetBool("touchingRightAnim2", false);
          anim.SetBool("touchingLeftAnim2", false);
        }
      }
      else
      {
        if(PlayerPrefs.GetInt("ChosenCostume1") == 1)
        {
          anim.SetBool("touchingLeftAnim2", true);
          anim.SetBool("jumpLeft", false);
          anim.SetBool("jumpRight", false);
          anim.SetBool("touchingRightAnim", false);
          anim.SetBool("touchingLeftAnim", false);
          anim.SetBool("jumpLeft2", false);
          anim.SetBool("jumpRight2", false);
          anim.SetBool("touchingRightAnim2", false);
        }
        else
        {
          anim.SetBool("touchingLeftAnim", true);
          anim.SetBool("jumpLeft", false);
          anim.SetBool("jumpRight", false);
          anim.SetBool("touchingRightAnim", false);
          anim.SetBool("jumpLeft2", false);
          anim.SetBool("jumpRight2", false);
          anim.SetBool("touchingRightAnim2", false);
          anim.SetBool("touchingLeftAnim2", false);
        }
      }
    }
    else if (onRightWall)
    {

      //TODO: IGNORE RIGHT COLLISION FROM HERE
      if (collision.contacts[0].point.x < this.gameObject.transform.position.x) //touching left wall
      {
        onRightWall = false;
        onLeftWall = true;
        if(PlayerPrefs.GetInt("ChosenCostume1") == 1)
        {
          anim.SetBool("touchingLeftAnim2", true);

          anim.SetBool("jumpLeft", false);
          anim.SetBool("jumpRight", false);
          anim.SetBool("touchingRightAnim", false);
          anim.SetBool("touchingLeftAnim", false);
          anim.SetBool("jumpLeft2", false);
          anim.SetBool("jumpRight2", false);
          anim.SetBool("touchingRightAnim2", false);
        }
        else
        {
          anim.SetBool("touchingLeftAnim", true);

          anim.SetBool("jumpLeft", false);
          anim.SetBool("jumpRight", false);
          anim.SetBool("touchingRightAnim", false);
          anim.SetBool("jumpLeft2", false);
          anim.SetBool("jumpRight2", false);
          anim.SetBool("touchingRightAnim2", false);
          anim.SetBool("touchingLeftAnim2", false);
        }
      }
      else
      {
        if(PlayerPrefs.GetInt("ChosenCostume1") == 1)
        {
          anim.SetBool("touchingRightAnim2", true);
          anim.SetBool("jumpLeft", false);
          anim.SetBool("jumpRight", false);
          anim.SetBool("touchingRightAnim", false);
          anim.SetBool("touchingLeftAnim", false);
          anim.SetBool("jumpLeft2", false);
          anim.SetBool("jumpRight2", false);
          anim.SetBool("touchingLeftAnim2", false);
        }
        else
        {
          anim.SetBool("touchingRightAnim", true);
          anim.SetBool("jumpLeft", false);
          anim.SetBool("jumpRight", false);
          anim.SetBool("touchingLeftAnim", false);
          anim.SetBool("jumpLeft2", false);
          anim.SetBool("jumpRight2", false);
          anim.SetBool("touchingRightAnim2", false);
          anim.SetBool("touchingLeftAnim2", false);
        }
      }
    }
    else if (collision.contacts[0].point.x < this.gameObject.transform.position.x)
    {
      onRightWall = false;
      onLeftWall = true;
      if(PlayerPrefs.GetInt("ChosenCostume1") == 1)
      {
        anim.SetBool("touchingLeftAnim2", true);
        anim.SetBool("jumpLeft", false);
        anim.SetBool("jumpRight", false);
        anim.SetBool("touchingRightAnim", false);
        anim.SetBool("touchingLeftAnim", false);
        anim.SetBool("jumpLeft2", false);
        anim.SetBool("jumpRight2", false);
        anim.SetBool("touchingRightAnim2", false);
      }
      else
      {
        anim.SetBool("touchingLeftAnim", true);
        anim.SetBool("jumpLeft", false);
        anim.SetBool("jumpRight", false);
        anim.SetBool("touchingRightAnim", false);
        anim.SetBool("jumpLeft2", false);
        anim.SetBool("jumpRight2", false);
        anim.SetBool("touchingRightAnim2", false);
        anim.SetBool("touchingLeftAnim2", false);
      }
    }
    else if (collision.contacts[0].point.x > this.gameObject.transform.position.x)
    {
      onRightWall = true;
      onLeftWall = false;
      if(PlayerPrefs.GetInt("ChosenCostume1") == 1)
      {
        anim.SetBool("touchingRightAnim2", true);
        anim.SetBool("jumpLeft", false);
        anim.SetBool("jumpRight", false);
        anim.SetBool("touchingRightAnim", false);
        anim.SetBool("touchingLeftAnim", false);
        anim.SetBool("jumpLeft2", false);
        anim.SetBool("jumpRight2", false);
        anim.SetBool("touchingLeftAnim2", false);
      }
      else
      {
        anim.SetBool("touchingRightAnim", true);
        anim.SetBool("jumpLeft", false);
        anim.SetBool("jumpRight", false);
        anim.SetBool("touchingLeftAnim", false);
        anim.SetBool("jumpLeft2", false);
        anim.SetBool("jumpRight2", false);
        anim.SetBool("touchingRightAnim2", false);
        anim.SetBool("touchingLeftAnim2", false);
      }
    }
  }

  void OnCollisionExit2D(Collision2D collision)
  {
    colCounter--;
    weirdFloor = false;
  }
}
