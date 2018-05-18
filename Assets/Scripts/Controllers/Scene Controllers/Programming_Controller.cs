using Assets.Scripts.Application.Layout;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Programming_Controller : MonoBehaviour {

	private Layout[] Layouts;
	private Transform Holder;
	private Transform TabSelect;
	private Transform ContentWindow;

	// Use this for initialization
	void Awake () {

		Layouts = GetComponents<Layout>();

		TabSelect = transform.GetChild(0).GetChild(0);
		Holder = transform.GetChild(0).GetChild(1);
		ContentWindow = Holder.GetChild(0).GetChild(0);

		for (int i = 0; i < Layouts.Length; i++) {

			Layout layout = Layouts[i];

			GameObject tab = Instantiate(Resources.Load("Prefabs/Layouts/Programming/Tab"), TabSelect) as GameObject;
			GameObject entry = Instantiate(Resources.Load("Prefabs/Layouts/Programming/Entry"), ContentWindow) as GameObject;

			Button tabBtn = tab.GetComponent<Button>();
			Text tabText = tab.GetComponentInChildren<Text>();

			tabBtn.onClick.AddListener(delegate { ChangeTab(i); });
			tabText.text = layout.Title;

			foreach (PictureBlock block in layout.Content) {

				RawImage entryImage = entry.GetComponentInChildren<RawImage>();
				VideoPlayer entryVideo = entry.GetComponentInChildren<VideoPlayer>();
				Text entryDescription = entry.GetComponentInChildren<Text>();

				if (block.Image) entryImage.texture = block.Image;
				else entryImage.gameObject.SetActive(false);

				if (block.Video) entryVideo.clip = block.Video;
				else entryVideo.gameObject.SetActive(false);

				if (block.Text.Length > 0) entryDescription.text = block.Text;
				else entryDescription.gameObject.SetActive(false);
			}

			ChangeTab(0);
		}
	}

	void ChangeTab(int slide) {
		
		for (int i = 0; i < ContentWindow.GetComponentsInChildren<Transform>().Length; i++) {
				
			Transform entry = ContentWindow.GetChild(i);

			entry.gameObject.SetActive(false);
			if (slide == i) entry.gameObject.SetActive(true);
		}
	}

	public void OpenSideMenu(Layout layout) {

	}
}
