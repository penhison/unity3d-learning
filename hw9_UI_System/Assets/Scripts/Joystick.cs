using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joystick : MonoBehaviour {

	public float speedX = 10.0F;
	public float speedZ = 10.0F;

	// Update is called once per frame
	void Update () {
		float translationZ = Input.GetAxis("Vertical") * speedZ;
		float translationX = Input.GetAxis("Horizontal") * speedX;
		translationZ *= Time.deltaTime;
		translationX *= Time.deltaTime;
		//transform.Translate(0, translationY, 0);
		//transform.Translate(translationX, 0, 0);
		transform.Translate(translationX, 0, translationZ);

		// if (Input.GetButtonDown ("Fire1")) {
		// 	Debug.Log ("Fired Pressed");
		// }
	}
}