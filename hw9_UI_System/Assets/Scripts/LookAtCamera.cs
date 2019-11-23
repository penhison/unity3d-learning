using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    void Update()
    {
        this.transform.LookAt(Camera.main.transform.position);
        // this.transform.Rotate(new Vector3(0, 180, 0));
        // this.transform.rotation += Quaternion.Euler(0,180,0);
    }
}
