using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour {

    private Transform Container;
    private bool _State;

    public bool State {

        get {

            return _State;
        }

        set {

            _State = value;
            Container.gameObject.SetActive(value);
            ApplicationManager.Instance.OverlayOpen = value;
        }
    }

	// Use this for initialization
	void Awake () {

		Container = transform.GetChild(1);

        foreach(string level in ApplicationManager.Instance.Loadable) {
                
            GameObject item = Instantiate(Resources.Load("Prefabs/MainMenuItem"), Container) as GameObject;
            Button itemButton = item.GetComponent<Button>();
            Text itemText = item.GetComponentInChildren<Text>();

            itemButton.onClick.AddListener(delegate { SceneController.LoadScene(level, true); });
            itemText.text = level;
        }

        State = false;
    }
	
	// Update is called once per frame
	void Update () {

    }

	public void ToggleMenu() {

		State = !State;
	}

	public void Quit() {

		Application.Quit();
	}
}
