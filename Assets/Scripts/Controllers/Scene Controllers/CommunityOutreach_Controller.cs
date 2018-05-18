using Assets.Scripts.Application.Layout;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommunityOutreach_Controller : MonoBehaviour {

    private Transform Canvas;
    private Layout Layout;

	// Use this for initialization
	void Awake () {

        Canvas = transform.GetChild(0);
        Layout = GetComponent<Layout>();

        Transform ContentWindow = Canvas.GetChild(0).GetChild(0).GetChild(0);

        foreach (OutreachBlock block in Layout.OutreachContent) {

            GameObject Entry = Instantiate(Resources.Load("Prefabs/Layouts/Community Outreach/Entry"), ContentWindow) as GameObject;
            Slideshow slideshow = Entry.GetComponentInChildren<Slideshow>();

            if (block.Slides.Length > 0) {

                
                slideshow.Slides = block.Slides;
            }

            else {

                slideshow.gameObject.SetActive(false);
            }
            Text[] fields = Entry.GetComponentsInChildren<Text>();

            Text header = fields[0];
            header.text = block.Header;

            Text description = fields[1];
            description.text = block.Description;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
