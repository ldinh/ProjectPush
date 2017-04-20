using UnityEngine;
using System.Collections;

public class Brick_Z_Setter : MonoBehaviour {
  Vector3 trans;
  private float replacer;

  // Use this for initialization
  void Start ()
  {
    // z position cannot be set until the object is instantiated.
    trans = this.transform.position;
    if(PlayerPrefs.GetFloat("zSet") == null)
    {
      PlayerPrefs.SetFloat("zSet", 10.0f);
    }
    transform.position = new Vector3 (transform.position.x, transform.position.y, PlayerPrefs.GetFloat ("zSet"));
    replacer = PlayerPrefs.GetFloat ("zSet") - 0.01f;
    PlayerPrefs.SetFloat ("zSet", replacer);
  }

  // Update is called once per frame
  void Update () {

  }
}
