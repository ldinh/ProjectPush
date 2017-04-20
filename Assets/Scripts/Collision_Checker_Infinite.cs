using UnityEngine;
using System.Collections;

public class Collision_Checker_Infinite : MonoBehaviour {
  public static bool colliding = false;
  private GameObject newWall = null;
  int randomizer = 0;
  int colCounter = 0;
  private float wall_offset_y = 0.059f; //0.049f has a few rare cracks when speed is near 50
  private int speed = 0;
  GameObject main_Cam;
  Wall_mover_infinite other;
  private float timeTilSafe = 7.0f;

  void Start()
  {
    main_Cam = GameObject.Find ("Main Camera");
    other = (Wall_mover_infinite) main_Cam.GetComponent (typeof(Wall_mover_infinite));
  }

  void OnTriggerEnter2D (Collider2D collider)
  {
    if (collider.tag == "dd")
    {
      colCounter++;
    }
  }

  void OnTriggerExit2D(Collider2D collider)
  {
    if (collider.tag == "dd")
    {
      colCounter--;
    }
  }

  void Update()
  {
    speed = other.SpeedGetter ();
    if (colCounter == 0)
    {
      colliding = false;
    }
    else
    {
      colliding = true;
    }
    if(colliding == false)
    {
      //colliding = true;
      if(GameObject.FindGameObjectsWithTag("Wall").Length < 4)
      {
        randomizer = Random.Range(1, 5);
        switch (randomizer) //Randomize a brick type to instantiate
        {
          case 1:
            newWall = (GameObject)Instantiate(Resources.Load("Series1"),new Vector3(0, 19f - (speed * -1)* wall_offset_y, 0),transform.rotation);
            newWall.transform.parent = GameObject.Find("Main Camera").transform;
            break;
          case 2:
            newWall = (GameObject)Instantiate(Resources.Load("Series2"),new Vector3(0, 19f - (speed * -1)* wall_offset_y, 0),transform.rotation);
            newWall.transform.parent = GameObject.Find("Main Camera").transform;
            break;
          case 3:
            newWall = (GameObject)Instantiate(Resources.Load("Series3"),new Vector3(0, 19f - (speed * -1)* wall_offset_y, 0),transform.rotation);
            newWall.transform.parent = GameObject.Find("Main Camera").transform;
            break;
          case 4:
            newWall = (GameObject)Instantiate(Resources.Load("Series4"),new Vector3(0, 19f - (speed * -1)* wall_offset_y, 0),transform.rotation);
            newWall.transform.parent = GameObject.Find("Main Camera").transform;
            break;
          default:
            break;
        }
      }
    }
  }

}
