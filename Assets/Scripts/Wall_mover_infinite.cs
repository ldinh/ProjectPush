using UnityEngine;
using System.Collections;

public class Wall_mover_infinite : MonoBehaviour {
  public Vector2 verticalMovement;
  private float timeSinceSpeedIncrease = 0.0f;
  public int verticalSpeed = -10;

  // Use this for initialization
  void Start ()
  {
    verticalMovement = new Vector2 (0, verticalSpeed);
  }

  // Update is called once per frame
  void Update () {
    if(Time.time - 3 > timeSinceSpeedIncrease && Time.timeSinceLevelLoad < 80.0f) //when to increase level speed
    {
      timeSinceSpeedIncrease = Time.time;
      verticalSpeed--;
      verticalMovement = new Vector2 (0, verticalSpeed);
    }
    foreach(Transform child in this.gameObject.transform)
    {
      child.Translate (verticalMovement * Time.deltaTime);
    }
  }

  public int SpeedGetter()
  {
    return verticalSpeed;
  }
}
