using UnityEngine;
using System.Collections;

public class GUI_controller_infinite : MonoBehaviour {

  public GUISkin customSkin;
  public bool isDead = false;
  public float currentScore;
  public Vector3 positionDied;
  private bool highScore = false;
  private bool firstDead = true;
  public AudioClip music;
  public AudioClip death;
  private float elapsedTime;
  private float prevTotalTime;

  void Start()
  {
    GetComponent<AudioSource>().PlayOneShot (music, 0.2f);
  }

  void Update()
  {
    elapsedTime = Time.timeSinceLevelLoad;
  }

  void OnGUI()
  {
    GUI.skin = customSkin; //dynamic screen size
    GUI.skin.box.fontSize = Screen.width / 20;
    GUI.skin.button.fontSize = Screen.width / 15;
    GUI.skin.label.fontSize = Screen.width / 20;
    if(!isDead)
    {
      GUI.Label (new Rect(Screen.width * (1f/2f) - ((Screen.width * (3f/5f))/2),Screen.height * (0.95f),Screen.width * (3f/5f),Screen.height * (1f/9f)), "Time: " + elapsedTime.ToString("F1") + " sec");
    }

    if(isDead)
    {
      GetComponent<AudioSource>().Pause();

      if(firstDead) // first occurance of a death related collision
      {
        GetComponent<AudioSource>().clip = death;
        GetComponent<AudioSource>().PlayOneShot(death, 1.0f);
        Instantiate(Resources.Load("Shatter"),positionDied,transform.rotation);
        firstDead = false;
        if(PlayerPrefs.GetFloat("TotalTime") == null)
        {
          PlayerPrefs.SetFloat("TotalTime", 0f);
        }
        else
        {
          prevTotalTime = PlayerPrefs.GetFloat("TotalTime");
          PlayerPrefs.SetFloat("TotalTime", PlayerPrefs.GetFloat("TotalTime") + currentScore);
        }
      }

      //UNLOCKABLES
      if(PlayerPrefs.GetInt("Level1") != 1)
      {
        if(currentScore > 10f) //55f
        {
          PlayerPrefs.SetInt("Level1", 1);
        }
      }
      if(PlayerPrefs.GetInt("Level2") != 1)
      {
        if(currentScore > 10f) //160f
        {
          PlayerPrefs.SetInt("Level2", 1);
        }
      }
      if(PlayerPrefs.GetInt("Level3") != 1)
      {
        if(PlayerPrefs.GetFloat("TotalTime") > 10f) //1200f
        {
          PlayerPrefs.SetInt("Level3", 1);
        }
      }
      if(PlayerPrefs.GetInt("Costume1") != 1)
      {
        if(PlayerPrefs.GetFloat("TotalTime") > 10f)  //300f
        {
          PlayerPrefs.SetInt("Costume1", 1);
        }
      }
      if(PlayerPrefs.GetInt("Costume2") != 1)
      {
        if(PlayerPrefs.GetFloat("TotalTime") > 10f)  //600f
        {
          PlayerPrefs.SetInt("Costume2", 1);
        }
      }
      if(PlayerPrefs.GetInt("Costume3") != 1)
      {
        if(PlayerPrefs.GetFloat("TotalTime") > 10f)  //900f
        {
          PlayerPrefs.SetInt("Costume3", 1);
        }
      }
      if(PlayerPrefs.GetInt("Special1") != 1)
      {
        if(currentScore > 10f)  //110f
        {
          PlayerPrefs.SetInt("Special1", 1);
        }
      }
      if(PlayerPrefs.GetInt("Special2") != 1)
      {
        if(currentScore > 10f)  //210f
        {
          PlayerPrefs.SetInt("Special2", 1);
        }
      }
      if(PlayerPrefs.GetInt("Special13") != 1)
      {
        if(PlayerPrefs.GetFloat("TotalTime") > 10f)  //1500f
        {
          PlayerPrefs.SetInt("Special3", 1);
        }
      }

      //IS HIGHSCORE?
      if(currentScore > PlayerPrefs.GetFloat("HighScore"))
      {
        PlayerPrefs.SetFloat("HighScore", currentScore);
        highScore = true;
      }
      if(highScore)
      {
        GUI.Box(new Rect(Screen.width * (0.10f),Screen.height * (0.10f),Screen.width * (0.8f), Screen.height * (0.8f)), "High Score!");
        GUI.Label (new Rect(Screen.width * (1f/2f) - ((Screen.width * (3f/5f))/2),Screen.height * (0.3f),Screen.width * (3f/5f),Screen.height * (1f/9f)), "Score: " + currentScore.ToString("F2") + " sec");
        if(GUI.Button(new Rect(Screen.width * (0.2f),Screen.height * (0.45f), Screen.width * (0.6f), Screen.height * (0.125f)), "Retry"))
        {
          Application.LoadLevel (Application.loadedLevelName);
        }

        if(GUI.Button(new Rect(Screen.width * (0.2f),Screen.height * (0.6f), Screen.width * (0.6f), Screen.height * (0.125f)), "Quit"))
        {
          Application.LoadLevel ("main_menu");
        }
      }
      else
      {
        GUI.Box(new Rect(Screen.width * (0.10f),Screen.height * (0.10f),Screen.width * (0.8f), Screen.height * (0.8f)), "Game Over");

        GUI.Label (new Rect(Screen.width * (1f/2f) - ((Screen.width * (3f/5f))/2),Screen.height * (0.2f),Screen.width * (3f/5f),Screen.height * (1f/9f)), "Score: " + currentScore.ToString("F2") + " sec");
        GUI.Label (new Rect(Screen.width * (1f/2f) - ((Screen.width * (3f/5f))/2),Screen.height * (0.3f),Screen.width * (3f/5f),Screen.height * (1f/9f)), "High Score: " + PlayerPrefs.GetFloat("HighScore").ToString("F2") + " sec");

        if(GUI.Button(new Rect(Screen.width * (0.2f),Screen.height * (0.5f), Screen.width * (0.6f), Screen.height * (0.125f)), "Retry"))
        {
          Application.LoadLevel (Application.loadedLevelName);
        }
        if(GUI.Button(new Rect(Screen.width * (0.2f),Screen.height * (0.65f), Screen.width * (0.6f), Screen.height * (0.125f)), "Quit"))
        {
          Application.LoadLevel ("main_menu");
        }
      }
    }
  }

  public void SetIsDead(bool dead)
  {
    isDead = dead;
  }
  public bool GetIsDead()
  {
    return isDead;
  }
  public void SetCurrentScore(float score)
  {
    currentScore = score;
  }
  public float GetCurrentScore()
  {
    return currentScore;
  }
  public void SetPositionDied(Vector3 died)
  {
    positionDied = died;
  }
  public Vector3 GetPositionDied()
  {
    return positionDied;
  }

}
