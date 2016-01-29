using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CompanyScreen_FadeImageInAndOut : MonoBehaviour {

    public Image companyScreenImage;

    int state = 0;
    float alpha = 0.0f;
    float timer = 0.0f;

    public float fadeSpeed = 0.5f;
    public float waitTimeAfterFadeIn = 1.5f;
    public string levelToLoad;

	// Use this for initialization
	void Awake () {
        if (companyScreenImage == null)
        {
            Debug.Log("Image was not assigned in the inspector.");
            return;
        }
        // set the alpha to 0.
        alpha = 0.0f;
        companyScreenImage.color = new Color(companyScreenImage.color.r, companyScreenImage.color.g, companyScreenImage.color.b, alpha);
        state = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (companyScreenImage == null) return;

        if(Input.GetMouseButtonDown(0)) {
            state = 4;
        }

        if(state == 0) {
            // set the alpha to 0.
            alpha = 0.0f;
            companyScreenImage.color = new Color(companyScreenImage.color.r, companyScreenImage.color.g, companyScreenImage.color.b, alpha);
            // set the state to 1.
            state = 1;
        }

        else if(state == 1) {
            // fade in the alpha
            alpha += fadeSpeed * Time.deltaTime;
            companyScreenImage.color = new Color(companyScreenImage.color.r, companyScreenImage.color.g, companyScreenImage.color.b, Mathf.Clamp(alpha, 0.0f, 1.0f));

            // if it's fully faded in, set state to 2
            if(alpha >= 1.0f) {
                alpha = 1.0f;
                state = 2;
            }
        }

        else if(state == 2) {
            timer += 1.0f * Time.deltaTime;

            // if we've waited long enough, change state to 3
            if(timer >= waitTimeAfterFadeIn) {
                timer = 0.0f;
                state = 3;
            }
        }

        else if(state == 3) {
            // fade out the alpha
            alpha -= fadeSpeed * Time.deltaTime;
            companyScreenImage.color = new Color(companyScreenImage.color.r, companyScreenImage.color.g, companyScreenImage.color.b, Mathf.Clamp(alpha, 0.0f, 1.0f));

            // if it's fully faded in, set state to 4
            if (alpha <= 0.0f)
            {
                alpha = 0.0f;
                state = 4;
            }
        }

        else if(state == 4) {
            Application.LoadLevel(levelToLoad);
            state = 0;
            alpha = 0.0f;
            timer = 0.0f;
        }
	}
}
