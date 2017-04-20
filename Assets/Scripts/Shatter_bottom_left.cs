using UnityEngine;
using System.Collections;

public class Shatter_bottom_left : MonoBehaviour {

  private Vector3 velocity;
  private float speed;
  
  // Use this for initialization
  void Awake ()
  {
    gameObject.GetComponent<Renderer>().material.color = Color.white;
    velocity = new Vector3 (-1, -1, 0);
    speed = 12f;
  }

  // Update is called once per frame
  void Update () {
    this.transform.position += velocity * Time.deltaTime * speed; //Update our position based on our new-found velocity
  }

  void OnCollisionEnter2D(Collision2D collision)
  {
    foreach (ContactPoint2D contact in collision.contacts)
    velocity = 2 * ( Vector3.Dot( velocity, Vector3.Normalize( contact.normal ) ) ) * Vector3.Normalize( contact.normal ) - velocity; //Following formula  v' = 2 * (v . n) * n - v
    velocity *= -1;
  }
}
