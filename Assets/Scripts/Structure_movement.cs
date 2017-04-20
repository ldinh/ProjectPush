using UnityEngine;
using System.Collections;

public class Structure_movement : MonoBehaviour {

  // Update is called once per frame, constantly moves bricks downward
  void Update ()
  {
    if(this.gameObject.transform.position.y < -19f)
    {
      Destroy(this.gameObject.transform.parent.gameObject); //when bricks reach a heigh threshhold, they are destroyed
    }
  }
}
