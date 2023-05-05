using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowOnMobile : MonoBehaviour
{

    public GameObject[] mobileElements;
    private bool mobile;
    public RectTransform fader;


    // Start is called before the first frame update
    void Start() {
        mobile = Application.isMobilePlatform;

        for (int i = 0; i < mobileElements.Length; i++)
        {
            mobileElements[i].SetActive(mobile);
        }

        fader.gameObject.SetActive(true);

        LeanTween.scale(fader, new Vector3(1, 1, 1), 0);
        LeanTween.scale(fader, Vector3.zero, 0.35f).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() => {
            fader.gameObject.SetActive(false);
        });

    }

}
