using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenuCamMove : MonoBehaviour
{
    public GameObject panelLogin;
    public InputField input;
    public Text hightScore;
    public Text displayName;
    public GameObject panelSettings;
    public Slider volumeSlider;
    public Slider volumeEffectsSlider;
    public Slider volumeMusikSlider;
    public Slider volumeVoiceSlider;
    private AudioSource lobbyMusik;
    public GameObject panelCodes;
    public InputField inputCode;
    public Text coins;
    public GameObject panelLevel;

    public RectTransform fader;

    Dictionary<string, int> myHashMap = new Dictionary<string, int>();

    private void Start() {
        hightScore.text = "High Score: " + PlayerPrefs.GetInt("livetimeHighScore", 0).ToString();

        displayName.text = PlayerPrefs.GetString("username", "");
        if (PlayerPrefs.GetString("username", "") != "")
        {
            panelLogin.SetActive(false);
        }
        else
        {
            panelLogin.SetActive(true);
        }

        volumeSlider.value = PlayerPrefs.GetFloat("volume", 0.5f);
        volumeMusikSlider.value = PlayerPrefs.GetFloat("volumeMusik", 0.5f);
        volumeEffectsSlider.value = PlayerPrefs.GetFloat("volumeEffects", 0.5f);
        volumeVoiceSlider.value = PlayerPrefs.GetFloat("volumeVoice", 0.5f);

        lobbyMusik = GetComponent<AudioSource>();
        lobbyMusik.volume = PlayerPrefs.GetFloat("volume", 0.5f) * PlayerPrefs.GetFloat("volumeMusik", 0.5f);

        myHashMap["FetziD1Sui"] = 50;
        myHashMap["Fetzi14"] = 140;

        GameObject.Find("Canvas").GetComponent<CanvasScaler>().matchWidthOrHeight = PlayerPrefs.GetFloat("GUIScale", 0.5f);

        fader.gameObject.SetActive(true);
        LeanTween.scale(fader, new Vector3(1, 1, 1), 0);
        Invoke("delayStartLoading", 0.5f);
    }

    private void delayStartLoading() {
        LeanTween.scale(fader, Vector3.zero, 0.5f).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() => {
            fader.gameObject.SetActive(false);
        });
    }

    public float speed = 1.0f;
    void Update() {
        // Erhöhe die x-Position des Objekts um speed pro Sekunde
        transform.position += new Vector3(speed * Time.deltaTime, 0.0f, 0.0f);

        // Überprüfe, ob die x-Position 100 überschritten hat
        if (transform.position.x > 100.0f)
        {
            // Setze die x-Position auf -100 zurück
            transform.position = new Vector3(-100.0f, transform.position.y, transform.position.z);
        }

        if (panelSettings.active)
        {
            PlayerPrefs.SetFloat("volume", volumeSlider.value);
            PlayerPrefs.SetFloat("volumeMusik", volumeMusikSlider.value);
            PlayerPrefs.SetFloat("volumeEffects", volumeEffectsSlider.value);
            PlayerPrefs.SetFloat("volumeVoice", volumeVoiceSlider.value);
            lobbyMusik.volume = PlayerPrefs.GetFloat("volume", 0.5f) * PlayerPrefs.GetFloat("volumeMusik", 0.5f);
        }

        coins.text = PlayerPrefs.GetInt("money", 0).ToString();


    }


    public void Login() {
        if(input.text.Length != 0)
        {
            PlayerPrefs.SetString("username", input.text);
            displayName.text = input.text;
            panelLogin.SetActive(false);
        }
    }

    public void OpenSettings() {
        panelSettings.SetActive(true);
    }
    
    public void CloseSettings() {
        panelSettings.SetActive(false);
    }
    public void OpenCodes() {
        panelCodes.SetActive(true);
    }

    public void CloseCodes() {
        panelCodes.SetActive(false);
    }

    public void CheckCode() {
        if (inputCode.text.Length != 0)
        {
            Debug.Log(inputCode.text);
            if (myHashMap.TryGetValue(inputCode.text, out int value) && PlayerPrefs.GetInt(inputCode.text, 0) == 0)
            {
                panelCodes.SetActive(false);

                PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") + value);
                PlayerPrefs.SetInt(inputCode.text, 1);

                inputCode.text = "";
            }
            else
            {
                inputCode.text = "";
            }
        }
    }

    public GameObject SoundScoll;
    public GameObject GrafikScoll;
    public GameObject SteuerungScoll;

    public void switchToSound() {
        SoundScoll.SetActive(true);
        GrafikScoll.SetActive(false);
        SteuerungScoll.SetActive(false);
    }
    public void switchToGrafik() {
        SoundScoll.SetActive(false);
        GrafikScoll.SetActive(true);
        SteuerungScoll.SetActive(false);
    }
    public void switchToSteuerung() {
        SoundScoll.SetActive(false);
        GrafikScoll.SetActive(false);
        SteuerungScoll.SetActive(true);
    }
    
    public void setGUIScale(float scale) {
        GameObject.Find("Canvas").GetComponent<CanvasScaler>().matchWidthOrHeight = scale;
        PlayerPrefs.SetFloat("GUIScale", scale);
    }

    public void openLevel() {
        panelLevel.SetActive(true);
    }

    public void closeLevel() {
        panelLevel.SetActive(true);
    }

    public void startGame() {
        fader.gameObject.SetActive(true);
        LeanTween.scale(fader, Vector3.zero, 0);
        LeanTween.scale(fader, new Vector3(1,1,1), 0.5f).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() => {
            Invoke("startGameD", 0.5f);
        });
        
    }

    public void startEndless() {
        fader.gameObject.SetActive(true);
        LeanTween.scale(fader, Vector3.zero, 0);
        LeanTween.scale(fader, new Vector3(1, 1, 1), 0.5f).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() => {
            Invoke("startEndlessD", 0.5f);
        });
    }

    private void startGameD() {
        SceneManager.LoadScene("Level1");
    }
    private void startEndlessD() {
        SceneManager.LoadScene("Endless");
    }

}
