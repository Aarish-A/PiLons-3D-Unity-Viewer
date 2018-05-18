using Assets.TouchSense;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour {

	public float[] FOVPositions;
	public float[] HeightPositions;

	public bool Offset = false;
    public int mouseButton = 2;
    public float offsetAmount = 20.0f;
    public float mouseSpeed = 5.0f;
    public float zoomSpeed = 1.0f;
    public Transform CenterPoint;
	public Transform Robot;
	public Slider FOV;
	public Slider Height;

	private Camera Cam;
    private Vector2 movement;
    private Vector2 movementLst;
	private Transform UI;

	// Use this for initialization
	void Awake () {

        ApplicationManager.Instance.MainCameraController = this;
		Cam = GetComponent<Camera>();
		UI = transform.GetChild(0);

		FOV.onValueChanged.AddListener(delegate { SetFOV((int)FOV.value); });
		Height.onValueChanged.AddListener(delegate { SetY((int)Height.value); });
	}
	
	// Fixed Update is called after all Updates()
	void Update () {

        if (CenterPoint && Robot && !ApplicationManager.Instance.OverlayOpen) {

			//// Rotate Around
			// Touch Input
			if (!EventSystem.current.IsPointerOverGameObject()) {

				StartCoroutine(TouchSense.FollowTouch.Test(1, (returnValue) => { movement = returnValue; }));
			}
            //
            // Mouse Input
            if (Input.GetMouseButton(2) && !ApplicationManager.Instance.OnTV) movement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
			//

            transform.RotateAround(Robot.position, Vector3.up, movement.x * mouseSpeed);
            //transform.RotateAround(CenterPoint.position, Vector3.right, -movement.y * mouseSpeed);

            //transform.LookAt(CenterPoint, CenterPoint.up);

            movementLst = movement;
            if (movement != Vector2.zero && movementLst == movement) movement = Vector2.zero;

			if (!UI.gameObject.activeInHierarchy) UI.gameObject.SetActive(true);
        }

		else {

			UI.gameObject.SetActive(true);
		}

		if (ApplicationManager.Instance.TimedOut) {

			transform.RotateAround(Robot.position, Vector3.up, mouseSpeed * Time.deltaTime);
		}
	}

    void OnDisable() {

        ApplicationManager.Instance.MainCameraController = null;
    }

	public void SetY(int pos) {

		transform.position = new Vector3(transform.position.x, HeightPositions[pos], transform.position.z);
	}

	public void SetFOV(int pos) {

		Cam.fieldOfView = FOVPositions[pos];
	}

	public void SetOffset(bool state, float offset) {

		Offset = state;
		transform.Rotate(new Vector3(0.0f, offset, 0.0f), Space.World);
	}
}
