using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.Experimental;

public class Video : MonoBehaviour {

	public RawImage s;
	private VideoPlayer Player;

	private RenderTexture texture;

	// Use this for initialization
	void Awake () {

		s = GetComponentInChildren<RawImage>();
		Player = GetComponent<VideoPlayer>();

		//Player.targetCamera = ApplicationManager.Instance.MainCameraController.GetComponent<Camera>();
		Player.targetTexture = Resources.Load("Video") as RenderTexture;
	}

	public void Toggle() {

		if (Player.isPlaying) {

			print("Paused");
			Player.Pause();
		}

		else {

			if (s.texture.GetType() != typeof(RenderTexture)) s.texture = Resources.Load("Video") as RenderTexture;
			print("Playing");
			Player.Play();
		}
	}
}
