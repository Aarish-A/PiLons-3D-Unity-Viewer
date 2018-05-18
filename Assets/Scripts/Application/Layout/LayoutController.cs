using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Application.Layout {

    public interface IScreen {

        Layout[] Layouts { get; set; }
        int CurrentLayout { get; set; }
        Transform Canvas { get; set; }

        void Awake();
        void BuildLayout(Layout layout, bool ShowImmediate);
        void Show();
        void Destroy();
    }

    public enum IScreenTypes{
        Fullscreen,
        SideWindow
    }

    public class IScreenControllor {

        public static void LoadIScreen(IScreenTypes type) {

            switch (type) {

                case IScreenTypes.Fullscreen:

                    SceneController.LoadScene("Fullscreen");
                    break;
                case IScreenTypes.SideWindow:
                    SceneController.LoadScene("SideWindow");
                    break;
            }
        }

        public static void DestoryIScreen() {

            if (ApplicationManager.Instance.IScreenController != null) ApplicationManager.Instance.IScreenController.Destroy();
        }

        public static IEnumerator BuildIScreen(System.Type type, Layout[] layouts, bool ShowImmediate = false) {

            if (type == typeof(SideWindowController)) SceneController.LoadScene("SideWindow");

            yield return new WaitUntil(() => ApplicationManager.Instance.IScreenController != null);
            yield return new WaitUntil(() => ApplicationManager.Instance.IScreenController.GetType() == type);

            ApplicationManager.Instance.IScreenController.Layouts = layouts;
            ApplicationManager.Instance.IScreenController.BuildLayout(layouts[0], ShowImmediate);

            yield break;
        }
    }
}
