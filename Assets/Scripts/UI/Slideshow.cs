using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slideshow : MonoBehaviour {

    public Texture[] Slides;
    public float TimeChange = 3.0f;

    private float currentTime = 0.0f;
    private int currentSlide = 0;

    private RawImage Screen;

	// Use this for initialization
	void Awake () {

        Screen = GetComponent<RawImage>();
	}
	
	// Update is called once per frame
	void Update () {

        currentTime += Time.deltaTime;

        if (currentTime > TimeChange) {

            NextSlide();
            currentTime = 0.0f;
        }
	}

    private void UpdateScreen() {

        Screen.texture = Slides[currentSlide];
    }

    public void NextSlide() {

        currentSlide += 1;
        if (currentSlide > Slides.Length - 1)
            currentSlide = 0;
        UpdateScreen();
    }

    public void PrevSlide() {

        currentSlide -= 1;
        if (currentSlide < 0)
            currentSlide = Slides.Length - 1;
        UpdateScreen();
    }
}
