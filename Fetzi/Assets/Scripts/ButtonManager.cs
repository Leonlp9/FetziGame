using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    private GameObject player;
    public RectTransform fader;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (player == null && SceneManager.GetActiveScene().name != "StartScene")
            {
                if (fader != null)
                CheckPointNew(fader);
            }
        }
    }
    
    public void startScrene() {
        SceneManager.LoadScene("StartScene");
    }

    public void startScreneNew(RectTransform fader) {
        fader.gameObject.SetActive(true);
        LeanTween.scale(fader, Vector3.zero, 0);
        Time.timeScale = 1f;
        LeanTween.scale(fader, new Vector3(1, 1, 1), 0.5f).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() => {
            Invoke("startScrene", 0.2f);
        });
    }

    public void CheckPoint()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("CheckPoint");
    }

    public void CheckPointNew(RectTransform fader) {
        fader.gameObject.SetActive(true);
        LeanTween.scale(fader, Vector3.zero, 0);
        LeanTween.scale(fader, new Vector3(1, 1, 1), 0.25f).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() => {
            Invoke("CheckPoint", 0.2f);
        });
    }

    public void Restart()
    {
        SceneManager.LoadScene("Level1");
        Debug.Log("Start");
    }

}
