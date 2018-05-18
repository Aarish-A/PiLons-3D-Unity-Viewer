using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.TouchSense {

    class TouchSense : MonoBehaviour {

        public static TouchSense Instance { get; private set; }
        public float SwipeActivationLength = 15.0f;
        public float TapActivation = 1.0f;
		public float MovementThresehold = 15.0f;
		public int Tuning = 2;
        public float Tolerance = 35.0f;

        void Awake() {

            // Enforce Singleton Pattern
            if (Instance == null) Instance = this;
            else if (Instance != this) DestroyObject(gameObject);
        }

        /*
        * EXAMPLES
        * 
        * StartCoroutine(Tap.Run(2, (returnVal) => {
        *      if (returnVal) print(returnVal);
        * }));
        * 
        * StartCoroutine(Swipe.Run(3, "down", (returnVal) => {
        *      if (returnVal) print(returnVal);
        * }));
        * 
        * StartCoroutine(Pinch.Run((returnVal) => {
        *      if (returnVal != 0)
        * }));
        */

        public class Tap {

            private static Vector3 startPos;
			private static float timeDown = 0.0f;

            public static IEnumerator Test(int fingers, System.Action<bool, Vector3> callback) {

                if (Input.touchCount == fingers && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) {

                    Touch touch = Input.GetTouch(0);
                    timeDown += Time.deltaTime;

					if (touch.phase == TouchPhase.Began) {
						startPos = touch.position;
					}

					else if (touch.phase == TouchPhase.Ended) {
						if (Vector3.Distance(startPos, touch.position) < 10 && timeDown <= Instance.TapActivation) {
							callback(true, touch.position);
							Debug.Log("tapped " + timeDown);
						}
						timeDown = 0.0f;
					}
				}

                // Make sure it runs once a frame
                yield return 0;
            }
        }

        public class Swipe {

            private static Vector3 fp;
            private static Vector3 lp;
            private static Vector2 Distance;
            private static bool returnValue;
			private static float timeDown = 0.0f;

            public static IEnumerator Run(int fingers, string direction, System.Action<bool> callback) {

                yield return null;
                if (Input.touchCount == fingers) {

					print("S_TC: " + Input.touchCount);

                    Touch touch = Input.GetTouch(0);
                    timeDown += Time.deltaTime;
                    if (touch.phase == TouchPhase.Began) {

                        fp = touch.position;
                        lp = touch.position;
                    }

                    else if (touch.phase == TouchPhase.Moved) {

                        lp = touch.position;
                    }

                    else if (touch.phase == TouchPhase.Ended) {

                        lp = touch.position;
                        Distance = new Vector2(Mathf.Abs(lp.x - fp.x), Mathf.Abs(lp.y - fp.y));

                        switch (direction) {

                            case "up":
                                
                                if (Distance.y > Instance.SwipeActivationLength && fp.y < lp.y)
                                    if (Distance.x < Instance.Tolerance)
                                        returnValue = true;
                                fp = lp = Vector3.zero;
                                break;

                            case "down":

                                if (Distance.y > Instance.SwipeActivationLength && fp.y > lp.y)
                                    if (Distance.x < Instance.Tolerance)
                                        returnValue = true;
                                fp = lp = Vector3.zero;
                                break;

                            case "right":

                                if (Distance.x > Instance.SwipeActivationLength && fp.x < lp.x)
                                    if (Distance.y < Instance.Tolerance)
                                        returnValue = true;
                                fp = lp = Vector3.zero;
                                break;

                            case "left":

                                if (Distance.x > Instance.SwipeActivationLength && fp.x > lp.x)
                                    if (Distance.y < Instance.Tolerance)
                                        returnValue = true;
                                break;

                            default:

                                Debug.Log("Unknown Direction");
                                break;
                        }

						if (returnValue) {
							callback(true);
							Debug.Log("Swiped");
						}
						else callback(false);

                        returnValue = false;
                        fp = lp = Vector3.zero;
						timeDown = 0.0f;
                    }
                }
            }
        }

        public class Pinch : MonoBehaviour {

			private static Vector3[] touches = new Vector3[2];
			private static Vector3 distance;
			private static Vector3 oldDistance;
            private static int direction;
			private static float timeDown;

            public static IEnumerator Test(System.Action<float> callback) {

                if (Input.touchCount == 2) {

					timeDown += Time.deltaTime;

                    for (int i = 0; i < 2; i++) {

                        Touch touch = Input.GetTouch(i);
						touches[i] = touch.position;
                    }

                    distance = touches[0] + touches[1];

					if (distance != oldDistance && distance.magnitude > 100.0f ) {

                        if (distance.magnitude >= oldDistance.magnitude) direction = 1;
                        else direction = -1;
                        callback((distance.magnitude/ Instance.Tuning) * direction);
						Debug.Log("Pinched: " + direction * distance);

                        oldDistance = distance;
                    }

					if (Input.touches.All(element => { return element.phase == TouchPhase.Ended; })) {

						distance = Vector3.zero;
						direction = 0;
					}
                }

				yield return 0;
            }
        }

        public class FollowTouch : MonoBehaviour {

			private static float timeDown = 0.0f;
			private static Vector3 startPos;

			public static IEnumerator Test(int fingers, System.Action<Vector3> returnValue) {

				if (Input.touchCount == fingers && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) {

					timeDown += Time.deltaTime;
					Touch touch = Input.GetTouch(0);

					if (touch.phase == TouchPhase.Began) {

						startPos = touch.position;
					}

					else if (touch.phase == TouchPhase.Moved && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) {

						if (Vector3.Distance(startPos, touch.position) > Instance.MovementThresehold) {

							returnValue(touch.deltaPosition/Instance.Tuning);
							Debug.Log("Following " + timeDown);
						}
					}

					else if (touch.phase == TouchPhase.Ended) {

						timeDown = 0.0f;
						startPos = Vector3.zero;
					}
                }

                yield return 0;
            }
        }
    }
}
