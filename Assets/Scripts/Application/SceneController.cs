using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;


class SceneController : MonoBehaviour {

    public static List<string> LoadedScenes = new List<string>();
    public static string MainScene;

    public static void LoadScene(string name, bool main = false) {

        if (!LoadedScenes.Contains(name)) {

            SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
            LoadedScenes.Add(name);

            if (main) {

                if (MainScene != null) UnloadScene(MainScene);
                MainScene = name;
            }
        }
    }

	public static void UnloadScene(string name) {

        if (LoadedScenes.Contains(name)) {

            SceneManager.UnloadSceneAsync(name);
            LoadedScenes.Remove(name);
        }
    }

    public static bool IsSceneLoaded(string name) {

        return LoadedScenes.Contains(name);
    }

    public static void LoadScene_A(string name) {

        SceneManager.LoadScene(name, LoadSceneMode.Additive);
    }

	public static IEnumerator WhenSceneLoaded(string name, System.Action<bool> done) {

		LoadScene(name);
		yield return new WaitUntil(() => IsSceneLoaded(name));
		done(true);
		yield break;
	}
}