using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Assets.Scripts.Application.Layout {

    [Serializable]
    public class SideWindowController : MonoBehaviour, IScreen {

        public Layout[] Layouts { get; set; }
        public int CurrentLayout { get; set; }
        public Transform Canvas { get; set; }

		public Sprite[] Icons;
		public VideoScreen VideoScreen;

        public void Awake() {

            if (ApplicationManager.Instance.IScreenController != null) {

                SceneController.UnloadScene("Fullscreen");
            }

            ApplicationManager.Instance.IScreenController = this;
            Canvas = transform.GetChild(0);
            CurrentLayout = 0;
        }

        public void BuildLayout(Layout layout, bool ShowImmediate) {

         
            for (int i = 1; i < Canvas.childCount; i++) 
                DestroyObject(Canvas.GetChild(i).gameObject);

            switch (layout.LayoutType) {

                case LayoutType.Viewer3D: {

					Debug.Log("Loading: " + layout.Title);

					GameObject header = Instantiate(Resources.Load("Prefabs/Layouts/Side Window/Header"), Canvas) as GameObject;
					GameObject holder = Instantiate(Resources.Load("Prefabs/Layouts/Side Window/Holder"), Canvas) as GameObject;

					Text title = header.GetComponentInChildren<Text>();
					title.text = layout.Title;

					Button headerButton = header.GetComponentInChildren<Button>();
					headerButton.onClick.AddListener(Increment);

					Image headerButtonImage = headerButton.GetComponent<Image>();
					headerButtonImage.sprite = Icons[CurrentLayout];

					Transform ContentWindow = holder.GetComponentInChildren<VerticalLayoutGroup>().transform;

					foreach (PanelBlock panel in layout.Panels) {

						GameObject entry = Instantiate(Resources.Load("Prefabs/Layouts/Side Window/Entry"), ContentWindow) as GameObject;

						RawImage panelScreen = entry.GetComponentInChildren<RawImage>();
						Button startVideo = entry.GetComponentInChildren<Button>();

						panelScreen.texture = panel.Panel;

						if (panel.Video) startVideo.onClick.AddListener(delegate { StartCoroutine(WaitForVideoPlayer((done) => {

							ApplicationManager.Instance.VideoScreen.SetVideo(panel.Video);
						})); });
						else startVideo.gameObject.SetActive(false);
						
					}

					break;
                }
                case LayoutType.Programming: {

                    break;
                }
            }

            if (ShowImmediate) Show();
        }

        public void Show() {

            gameObject.SetActive(true);
        }

        public void Increment() {

            CurrentLayout += 1;
            if (CurrentLayout >= Layouts.Length) CurrentLayout = 0;
            BuildLayout(Layouts[CurrentLayout], true);
        }

        public void Destroy() {

            SceneController.UnloadScene("SideWindow");
            ApplicationManager.Instance.IScreenController = null;
        }
		
		private IEnumerator WaitForVideoPlayer(System.Action<bool> done) {

			SceneController.LoadScene("Video Player");
			yield return new WaitUntil(() => ApplicationManager.Instance.VideoScreen != null);
			done(true);
			yield break;
		}
    }
}
