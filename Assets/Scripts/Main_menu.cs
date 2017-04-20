using UnityEngine;
using System.Collections;

public class Main_menu : MonoBehaviour {
  public GUISkin customSkin;
  bool plain = true;
  bool challenge_1 = false;
  bool challenge_2 = false;
  bool challenge_3 = false;
  bool unlockables = false;
  bool credits = false;
  public Texture2D lock_image;
  public Texture2D next_arrow;
  public Texture2D prev_arrow;
  public Vector2 scrollPosition;
  public AudioClip music;

  public int levelSelect = 0;
  public int costumeSelect = 0;
  public int specialSelect = 0;

  //public GUIStyle style;

  public string[] levelButtons = new string[] {"NONE", "???", "???", "???"};
  public string[] costumeButtons = new string[] {"NONE", "???", "???", "???"};
  public string[] specialButtons = new string[] {"NONE", "???", "???", "???"};

  void Start()
  {
    //style.normal.textColor = Color.white;
    GetComponent<AudioSource>().PlayOneShot (music, 0.2f);

    //are things unlocked?
    if(PlayerPrefs.GetInt("Level1") == 1)
    {
      levelButtons[1] = "DuskTower";
    }
    if(PlayerPrefs.GetInt("Level2") == 1)
    {
      levelButtons[2] = "Space Opera";
    }
    if(PlayerPrefs.GetInt("Level3") == 1)
    {
      levelButtons[3] = "Above All";
    }

    if(PlayerPrefs.GetInt("Costume1") == 1)
    {
      costumeButtons[1] = "Shocker";
    }
    if(PlayerPrefs.GetInt("Costume2") == 1)
    {
      costumeButtons[2] = "???";
    }
    if(PlayerPrefs.GetInt("Costume3") == 1)
    {
      costumeButtons[3] = "???";
    }

    if(PlayerPrefs.GetInt("Special1") == 1)
    {
      specialButtons[1] = "The Glow";
    }
    if(PlayerPrefs.GetInt("Special2") == 1)
    {
      specialButtons[2] = "Tele-jump";
    }
    if(PlayerPrefs.GetInt("Special3") == 1)
    {
      specialButtons[3] = "Rainbow";
    }

  }

  void Update ()
  {
    //COSTUME
    if(PlayerPrefs.GetInt("Costume1") == 1 && (costumeSelect == 1))
    {
      PlayerPrefs.SetInt("ChosenCostume1", 1);
    }
    else
    {
      PlayerPrefs.SetInt("ChosenCostume1", 0);
    }
    if(PlayerPrefs.GetInt("Costume2") == 1 && (costumeSelect == 2))
    {
      PlayerPrefs.SetInt("ChosenCostume2", 1);
    }
    else
    {
      PlayerPrefs.SetInt("ChosenCostume2", 0);
    }
    if(PlayerPrefs.GetInt("Costume3") == 1 && (costumeSelect == 3))
    {
      PlayerPrefs.SetInt("ChosenCostume3", 1);
    }
    else
    {
      PlayerPrefs.SetInt("ChosenCostume3", 0);
    }

    //SPECIAL
    if(PlayerPrefs.GetInt("Special1") == 1 && (specialSelect == 1))
    {
      PlayerPrefs.SetInt("ChosenSpecial1", 1);
    }
    else
    {
      PlayerPrefs.SetInt("ChosenSpecial1", 0);
    }
    if(PlayerPrefs.GetInt("Special2") == 1 && (specialSelect == 2))
    {
      PlayerPrefs.SetInt("ChosenSpecial2", 1);
    }
    else
    {
      PlayerPrefs.SetInt("ChosenSpecial2", 0);
    }
    if(PlayerPrefs.GetInt("Special3") == 1 && (specialSelect == 3))
    {
      PlayerPrefs.SetInt("ChosenSpecial3", 1);
    }
    else
    {
      PlayerPrefs.SetInt("ChosenSpecial3", 0);
    }
  }


  void OnGUI()
  {
    GUI.skin = customSkin;
    int w = Screen.width;
    int h = Screen.height;
    GUI.skin.box.fontSize = Screen.width / 20;
    GUI.skin.button.fontSize = Screen.width / 20;
    GUI.skin.label.fontSize = Screen.width / 20;

    if (plain)
    {
      GUI.Label (new Rect(Screen.width * (1f/2f) - ((Screen.width * (3f/5f))/2),Screen.height * (3f/7f),Screen.width * (3f/5f),Screen.height * (1f/9f)), "Boundless Tower");
      if(GUI.Button(new Rect(Screen.width * (1f/13f),Screen.height * (4f/7f),Screen.width * (2f/5f),Screen.height * (1f/9f)), "Play!")) {
        if(levelSelect == 1)
        {
          if(PlayerPrefs.GetInt("Level1") == 1)
          {
            Application.LoadLevel("survival1");
          }
          else
          {
            Application.LoadLevel("survival");
          }
        }
        else if(levelSelect == 2)
        {
          if(PlayerPrefs.GetInt("Level2") == 1)
          {
            Application.LoadLevel("survival2");
          }
          else
          {
            Application.LoadLevel("survival");
          }
        }
        else if(levelSelect == 3)
        {
          if(PlayerPrefs.GetInt("Level3") == 1)
          {
            Application.LoadLevel("survival3");
          }
          else
          {
            Application.LoadLevel("survival");
          }
        }
        else
        {
          Application.LoadLevel("survival");
        }
      }
      if(GUI.Button(new Rect(Screen.width * (6f/11f),Screen.height * (4f/7f),Screen.width * (2f/5f),Screen.height * (1f/9f)), "Unlockables")) {
        plain = false;
        unlockables = true;
      }
      if(GUI.Button(new Rect(Screen.width * (1f/13f),Screen.height * (5f/7f),Screen.width * (2f/5f),Screen.height * (1f/9f)), "Credits")) {
        plain = false;
        credits = true;
      }
    }

    if (unlockables)
    {
      //GUI.Box(new Rect(Screen.width * (1f/14f),Screen.height * (1f/35f),Screen.width * (1f/1.15f), Screen.height * 1/*(1f/1.10f)*/), "Unlockables");
      GUI.Box(new Rect(0,0,Screen.width , Screen.height ), "Unlockables");

      GUI.Label (new Rect(Screen.width * (1f/2f) - ((Screen.width * (3f/5f))/2),Screen.height * (1f/13f),Screen.width * (3f/5f),Screen.height * (1f/9f)), "Levels");
      levelSelect = GUI.SelectionGrid(new Rect(Screen.width * (1f/12f), Screen.height * (2f/13f), Screen.width * (1f/1.2f), Screen.height * (1f/6f)), levelSelect, levelButtons, 2);

      GUI.Label (new Rect(Screen.width * (1f/2f) - ((Screen.width * (3f/5f))/2),Screen.height * (4f/11f),Screen.width * (3f/5f),Screen.height * (1f/9f)), "Costumes");
      costumeSelect = GUI.SelectionGrid(new Rect(Screen.width * (1f/12f), Screen.height * (5f/12f), Screen.width * (1f/1.2f), Screen.height * (1f/6f)), costumeSelect, costumeButtons, 2);

      GUI.Label (new Rect(Screen.width * (1f/2f) - ((Screen.width * (3f/5f))/2),Screen.height * (6.3f/10f),Screen.width * (3f/5f),Screen.height * (1f/9f)), "Specials");
      specialSelect = GUI.SelectionGrid(new Rect(Screen.width * (1f/12f), Screen.height * (6.8f/10f), Screen.width * (1f/1.2f), Screen.height * (1f/6f)), specialSelect, specialButtons, 2);

      if(GUI.Button(new Rect(Screen.width * (1f/7.5f),Screen.height * (8f/9f),Screen.width * (2f/7f),Screen.height * (1f/11f)), prev_arrow))
      {
        plain = true;
        unlockables = false;
      }
    }

    if (credits)
    {
      //GUI.Box(new Rect(Screen.width * (1f/14f),Screen.height * (1f/35f),Screen.width * (1f/1.15f), Screen.height * 1/*(1f/1.10f)*/), "Unlockables");
      GUI.Box(new Rect(0,0,Screen.width , Screen.height ), "Credits");

      GUI.Label (new Rect(Screen.width * (1f/2f) - ((Screen.width * (3f/5f))/2),Screen.height * (1f/13f),Screen.width * (3f/5f),Screen.height * (1f/9f)), "Programming:");
      GUI.Label (new Rect(Screen.width * (1f/2f) - ((Screen.width * (3f/5f))/2),Screen.height * (2f/13f),Screen.width * (3f/5f),Screen.height * (1f/9f)), "TSG_Falsetto");

      GUI.Label (new Rect(Screen.width * (1f/2f) - ((Screen.width * (3f/5f))/2),Screen.height * (7f/11f),Screen.width * (3f/5f),Screen.height * (1f/9f)), "Music:");
      GUI.Label (new Rect(Screen.width * (1f/2f) - ((Screen.width * (3f/5f))/2),Screen.height * (8f/11f),Screen.width * (3f/5f),Screen.height * (1f/9f)), "Eric Skiff");

      if (GUI.Button(new Rect(Screen.width * (1f/7.5f),Screen.height * (8f/9f),Screen.width * (2f/7f),Screen.height * (1f/11f)), prev_arrow))
      {
        plain = true;
        credits = false;
      }
    }
  }
}
