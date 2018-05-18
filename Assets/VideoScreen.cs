using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoScreen : MonoBehaviour {

	public float TimeAfter = 0.5f;

	private RawImage Screen;
	private VideoPlayer Player;

	// Use this for initialization
	void Awake () {

		Screen = GetComponentInChildren<RawImage>();
		Player = GetComponent<VideoPlayer>();

		ApplicationManager.Instance.VideoScreen = this;
		Screen.gameObject.SetActive(false);
	}

	void OnDestroy() {

		ApplicationManager.Instance.VideoScreen = null;
	}

	public void SetVideo(VideoClip video) {

		Player.clip = video;
		StartCoroutine(PlayVideo());
	}

	private IEnumerator PlayVideo() {

		Player.Prepare();
		yield return new WaitUntil(() => Player.isPrepared);
		Screen.gameObject.SetActive(true);
		Player.Play();
		yield return new WaitForSeconds((float) Player.clip.length + TimeAfter);
		Screen.gameObject.SetActive(false);
		SceneController.UnloadScene("Video Player");
		yield break;
	}
}
