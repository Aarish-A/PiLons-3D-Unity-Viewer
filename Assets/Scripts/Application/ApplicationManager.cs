using Assets.Scripts.Application.Layout;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

class ApplicationManager : MonoBehaviour {

    public static ApplicationManager Instance;
	public float TimeOutValue = 120;

    public bool OverlayOpen;
	public bool OnTV;
	public bool TimedOut;
	public string[] Loadable;
    public CameraController MainCameraController;
    public IScreen IScreenController;
	public VideoScreen VideoScreen;

	private float TimeSinceLastInput = 0.0f;

	void Awake() {

		// Enforce Singleton
		if (Instance == null) Instance = this;
		else if (Instance != this) DestroyImmediate(gameObject);

		DontDestroyOnLoad(gameObject);

		InitApp();
	}

    void InitApp() {

		Application.targetFrameRate = 30;

        SceneController.LoadScene("MainMenu");
        SceneController.LoadScene("Build & Programming", true);

        SceneController.UnloadScene("Preload");
    }

	public IEnumerator Wait(float time) {

		yield return new WaitForSecondsRealtime(time);
	}

	void Update() {
		
		if (Input.touchCount == 0) {

			TimeSinceLastInput += Time.deltaTime;

			if (!TimedOut && TimeSinceLastInput >= TimeOutValue) TimedOut = true;
		}

		else {

			if (TimedOut) {

				TimedOut = false;
				TimeSinceLastInput = 0.0f;
			}
		}
	}
}
