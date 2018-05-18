using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoBlurbSpawner : MonoBehaviour {

	private Animator Animator;

	// Use this for initialization
	void Awake () {

		Animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown(KeyCode.Space)) DestoryBlurb();
	}

	public void DestoryBlurb() {

		Animator.SetFloat("Direction", -1.0f);
		Animator.Play("Blurb Out");
		DestroyTimer();
	}

	private IEnumerator DestroyTimer() {

		yield return new WaitForSeconds(Mathf.Abs(Animator.GetCurrentAnimatorStateInfo(0).length));

		Destroy(transform.GetChild(0).gameObject);
	}
}
