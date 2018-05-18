using Assets.TouchSense;
using Assets.Scripts.Application.Layout;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Viewer3D_Controller : MonoBehaviour {

	public float offset = 35.0f;
    public Transform Robot;

    private ApplicationManager ApplicationManager = ApplicationManager.Instance;
    private bool Click;
    private bool SideWindowOpen;
    private Vector3 ClickPosition;

	// Use this for initialization
	void Awake () {

	}

    // Update is called once per frame
    void Update() {

		if (!ApplicationManager.OverlayOpen) {

			if (!ApplicationManager.OnTV && Input.GetMouseButtonUp(0)) {
				Click = true;
				ClickPosition = Input.mousePosition;
			}
			StartCoroutine(TouchSense.Tap.Test(1, (wasClicked, pos) => {
				if (wasClicked) {
					Click = true;
					ClickPosition = pos;
				}
			}));

			if (Input.GetMouseButtonDown(0) && !ApplicationManager.OnTV) {
				Click = true;
				ClickPosition = Input.mousePosition;
			}

			if (Click) {

				Camera cam = ApplicationManager.MainCameraController.GetComponent<Camera>();

				RaycastHit hit;
				Ray ray = cam.ScreenPointToRay(ClickPosition);

				if (Physics.Raycast(ray, out hit)) {

					Debug.DrawLine(cam.transform.position, hit.point, Color.red);
					Debug.Log(hit.transform.name);

					Layout[] layouts = hit.transform.GetComponents<Layout>();

					if (layouts.Length > 0 && !EventSystem.current.IsPointerOverGameObject()) {

						if (!ApplicationManager.MainCameraController.Offset) ApplicationManager.MainCameraController.SetOffset(true, offset);
						StartCoroutine(IScreenControllor.BuildIScreen(typeof(SideWindowController), layouts, true));
						SideWindowOpen = true;
					}
				}

				else {

					if (!EventSystem.current.IsPointerOverGameObject()) {

						IScreenControllor.DestoryIScreen();
						SideWindowOpen = false;
						if (ApplicationManager.MainCameraController.Offset) ApplicationManager.MainCameraController.SetOffset(false, -offset);
					}
				}

				Click = false;
			}
        }
    }
}
