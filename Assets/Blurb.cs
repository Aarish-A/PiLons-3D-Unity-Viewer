using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blurb : MonoBehaviour {

	private Animation Anim;

	// Use this for initialization
	void Awake () {

		Anim = GetComponent<Animation>();

		Anim.Play("Blurb In");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
