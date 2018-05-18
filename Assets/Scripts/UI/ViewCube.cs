using Assets.TouchSense;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewCube : MonoBehaviour {

    Camera Cam;
    Vector3 Pos;

	// Use this for initialization
	void Start () {

        Cam = GetComponentInParent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {

        StartCoroutine(TouchSense.FollowTouch.Test(1, (pos) => { Pos = pos;  }));

        transform.rotation = Quaternion.Euler(-Pos.x, -Pos.y, -Pos.z);
	}
}
