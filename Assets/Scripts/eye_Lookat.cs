using UnityEngine;
using System.Collections;

public class eye_Lookat : MonoBehaviour {
  public Transform target;
  void Update() {
    if(target != null)
    {
      Quaternion rotation = Quaternion.LookRotation
      (target.transform.position - transform.position, transform.TransformDirection(Vector3.up));
      transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
    }
  }
}
