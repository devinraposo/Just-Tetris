using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
enum Label { PLAY, CONTROLS, SETTINGS, CREDITS, QUIT, PLAYMODE, SETTINGSMODE, QUITMODE };
enum Settings { BACKGROUND, MUSIC, STARTINGLEVEL, SOUND};
public class MainMenuScript : MonoBehaviour
{
    public GameObject labelBackground, menus, labels, background, player, gameUI;
    GameObject label, oldLabel;
    public TextMeshProUGUI backgroundChoice, musicChoice, levelChoice, soundChoice;
    int i, j, currTrack, currBackground;
    public int level;
    float labelOffset, time, a, b, settingsLabelOffset, initialMoveTime, fastMoveTime, currMoveTime, dpadVert, dpadHorz;
    public bool lerping, soundOn, initialMove, fastMove;
    public Sprite[] backgrounds;
    public AudioClip[] musicClips;
    public AudioSource musicSource, beepSource;
    public GameScript gameScript;
    public PlayerScript playerScript;
    private void Awake()
    {
        labelOffset = 71.0f;
        time = 0.0f;
        i = 0;
        j = 1;
        currTrack = 0;
        currBackground = 2;
        lerping = false;
        a = -310.0f;
        b = -290.0f;
        level = 1;
        soundOn = true;
        settingsLabelOffset = 94.0f;
        initialMoveTime = 0.3f;
        initialMove = false;
        fastMove = false;
        fastMoveTime = 0.05f;
        currMoveTime = initialMoveTime;
        dpadVert = 0.0f;
        dpadHorz = 0.0f;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonUp("KeyDown") || (Input.GetAxisRaw("ControllerVertical") == 0 && dpadVert < 0))
        {
            initialMove = false;
            fastMove = false;
            currMoveTime = initialMoveTime;
        }
        else if (Input.GetButton("KeyDown") || dpadVert < 0)
        {
            if (!initialMove)
            {
                if (i < (int)Label.PLAYMODE) MenuDown();
                else if (i == (int)Label.SETTINGSMODE) SettingsMenuDown();
                initialMove = true;
            }
            else if (initialMove && !fastMove && currMoveTime > 0.0f) currMoveTime -= Time.deltaTime;
            else if (initialMove && !fastMove && currMoveTime <= 0.0f)
            {
                fastMove = true;
                currMoveTime = fastMoveTime;
            }
            if (fastMove && currMoveTime > 0.0f) currMoveTime -= Time.deltaTime;
            else if (fastMove && currMoveTime <= 0.0f)
            {
                if (i < (int)Label.PLAYMODE) MenuDown();
                else if (i == (int)Label.SETTINGSMODE) SettingsMenuDown();
                currMoveTime = fastMoveTime;
            }
        }
        if (Input.GetButtonUp("KeyUp") || (Input.GetAxisRaw("ControllerVertical") == 0 && dpadVert > 0))
        {
            initialMove = false;
            fastMove = false;
            currMoveTime = initialMoveTime;
        }
        else if (Input.GetButton("KeyUp") || dpadVert > 0)
        {
            if (!initialMove)
            {
                if (i < (int)Label.PLAYMODE) MenuUp();
                else if (i == (int)Label.SETTINGSMODE) SettingsMenuUp();
                initialMove = true;
            }
            else if (initialMove && !fastMove && currMoveTime > 0.0f) currMoveTime -= Time.deltaTime;
            else if (initialMove && !fastMove && currMoveTime <= 0.0f)
            {
                fastMove = true;
                currMoveTime = fastMoveTime;
            }
            if (fastMove && currMoveTime > 0.0f) currMoveTime -= Time.deltaTime;
            else if (fastMove && currMoveTime <= 0.0f)
            {
                if (i < (int)Label.PLAYMODE) MenuUp();
                else if (i == (int)Label.SETTINGSMODE) SettingsMenuUp();
                currMoveTime = fastMoveTime;
            }
        }
        if(Input.GetButtonUp("KeyRight") || (Input.GetAxisRaw("ControllerHorizontal") == 0 && dpadHorz > 0))
        {
            initialMove = false;
            fastMove = false;
            currMoveTime = initialMoveTime;
        }
        else if(Input.GetButton("KeyRight") || dpadHorz > 0)
        {
            if (!initialMove)
            {
                if (i < (int)Label.PLAYMODE) MenuSubmit();
                else if(i == (int)Label.SETTINGSMODE) ChangeSettings(1);
                initialMove = true;
            }
            else if (initialMove && !fastMove && currMoveTime > 0.0f) currMoveTime -= Time.deltaTime;
            else if (initialMove && !fastMove && currMoveTime <= 0.0f)
            {
                fastMove = true;
                currMoveTime = fastMoveTime;
            }
            if (fastMove && currMoveTime > 0.0f) currMoveTime -= Time.deltaTime;
            else if (fastMove && currMoveTime <= 0.0f)
            {
                if (i < (int)Label.PLAYMODE) MenuSubmit();
                else if(i == (int)Label.SETTINGSMODE) ChangeSettings(1);
                currMoveTime = fastMoveTime;
            }
        }
        if (Input.GetButtonUp("KeyLeft") || (Input.GetAxisRaw("ControllerHorizontal") == 0 && dpadHorz < 0))
        {
            initialMove = false;
            fastMove = false;
            currMoveTime = initialMoveTime;
        }
        else if (Input.GetButton("KeyLeft") || dpadHorz < 0)
        {
            if (!initialMove)
            {
                if (i == (int)Label.PLAYMODE || i == (int)Label.QUITMODE) Back();
                else if (i == (int)Label.SETTINGSMODE) ChangeSettings(-1);
                initialMove = true;
            }
            else if (initialMove && !fastMove && currMoveTime > 0.0f) currMoveTime -= Time.deltaTime;
            else if (initialMove && !fastMove && currMoveTime <= 0.0f)
            {
                fastMove = true;
                currMoveTime = fastMoveTime;
            }
            if (fastMove && currMoveTime > 0.0f) currMoveTime -= Time.deltaTime;
            else if (fastMove && currMoveTime <= 0.0f)
            {
                if (i == (int)Label.PLAYMODE || i == (int)Label.QUITMODE) Back();
                else if (i == (int)Label.SETTINGSMODE) ChangeSettings(-1);
                currMoveTime = fastMoveTime;
            }
        }
        else if (Input.GetButtonDown("KeyReturn") || Input.GetButtonDown("ControllerSubmit") || Input.GetButtonDown("ControllerPause"))
        {
            if (i < (int)Label.PLAYMODE) MenuSubmit();
            else if (i == (int)Label.PLAYMODE)
            {
                player.SetActive(true);
                this.gameObject.SetActive(false);
                gameUI.SetActive(true);
                playerScript.Begin();
            }
            else if (i == (int)Label.QUITMODE)
            {
#if (!UNITY_EDITOR && !UNITY_WEBGL)
                Application.Quit();
#endif
            }
        }
        else if (Input.GetButtonDown("KeyBack") || Input.GetButtonDown("ControllerBack")) Back();
        if (lerping)
        {
            time += (Time.deltaTime * 5.0f);
            label.transform.localPosition = new Vector2(Mathf.Lerp(a, b, time), label.transform.localPosition.y);
            oldLabel.transform.localPosition = new Vector2(Mathf.Lerp(b, a, time), oldLabel.transform.localPosition.y);
            if (time >= 1.0f)
            {
                time = 0.0f;
                lerping = false;
            }
        }
        dpadVert = Input.GetAxisRaw("ControllerVertical");
        dpadHorz = Input.GetAxisRaw("ControllerHorizontal");
    }
    void MenuDown()
    {
        if (soundOn) beepSource.Play();
        menus.transform.GetChild(i).gameObject.SetActive(false);
        lerping = true;
        oldLabel = labels.transform.GetChild(i).gameObject;
        oldLabel.GetComponent<TextMeshProUGUI>().color = Color.white;
        if (i == 4)
        {
            i = 0;
            labelBackground.transform.localPosition = new Vector2(labelBackground.transform.localPosition.x, labelBackground.transform.localPosition.y + (labelOffset * 4.0f));
        }
        else
        {
            ++i;
            labelBackground.transform.localPosition = new Vector2(labelBackground.transform.localPosition.x, labelBackground.transform.localPosition.y - labelOffset);
        }
        label = labels.transform.GetChild(i).gameObject;
        menus.transform.GetChild(i).gameObject.SetActive(true);
        labels.transform.GetChild(i).gameObject.GetComponent<TextMeshProUGUI>().color = Color.black;
    }
    void MenuUp()
    {
        if (soundOn) beepSource.Play();
        menus.transform.GetChild(i).gameObject.SetActive(false);
        lerping = true;
        oldLabel = labels.transform.GetChild(i).gameObject;
        oldLabel.GetComponent<TextMeshProUGUI>().color = Color.white;
        if (i == 0)
        {
            i = 4;
            labelBackground.transform.localPosition = new Vector2(labelBackground.transform.localPosition.x, labelBackground.transform.localPosition.y - (labelOffset * 4.0f));
        }
        else
        {
            --i;
            labelBackground.transform.localPosition = new Vector2(labelBackground.transform.localPosition.x, labelBackground.transform.localPosition.y + labelOffset);
        }
        label = labels.transform.GetChild(i).gameObject;
        menus.transform.GetChild(i).gameObject.SetActive(true);
        labels.transform.GetChild(i).gameObject.GetComponent<TextMeshProUGUI>().color = Color.black;
    }
    void MenuSubmit()
    {
        if (soundOn) beepSource.Play();
        switch (i)
        {
            case (int)Label.PLAY:
                labels.transform.GetChild(i).gameObject.GetComponent<TextMeshProUGUI>().color = Color.white;
                i = (int)Label.PLAYMODE;
                menus.transform.GetChild(0).GetChild(3).GetComponent<TextMeshProUGUI>().color = Color.black;
                menus.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
                labelBackground.SetActive(false);
                break;
            case (int)Label.SETTINGS:
                labels.transform.GetChild(i).gameObject.GetComponent<TextMeshProUGUI>().color = Color.white;
                i = (int)Label.SETTINGSMODE;
                menus.transform.GetChild(2).GetChild(0).gameObject.SetActive(true);
                menus.transform.GetChild(2).GetChild(j).GetChild(3).GetComponent<TextMeshProUGUI>().color = Color.black;
                labelBackground.SetActive(false);
                break;
            case (int)Label.QUIT:
                labels.transform.GetChild(i).gameObject.GetComponent<TextMeshProUGUI>().color = Color.white;
                i = (int)Label.QUITMODE;
                menus.transform.GetChild(4).GetChild(0).gameObject.SetActive(true);
                menus.transform.GetChild(4).GetChild(2).GetComponent<TextMeshProUGUI>().color = Color.black;
                labelBackground.SetActive(false);
                break;
        }
    }
    void SettingsMenuDown()
    {
        if (soundOn) beepSource.Play();
        menus.transform.GetChild(2).GetChild(j).GetChild(3).GetComponent<TextMeshProUGUI>().color = Color.white;
        if (j == 4)
        {
            j = 1;
            menus.transform.GetChild(2).GetChild(0).localPosition = new Vector2(menus.transform.GetChild(2).GetChild(0).localPosition.x,
                menus.transform.GetChild(2).GetChild(0).localPosition.y + (settingsLabelOffset * 3.0f));
        }
        else
        {
            ++j;
            menus.transform.GetChild(2).GetChild(0).localPosition = new Vector2(menus.transform.GetChild(2).GetChild(0).localPosition.x,
                menus.transform.GetChild(2).GetChild(0).localPosition.y - settingsLabelOffset);
        }
        menus.transform.GetChild(2).GetChild(j).GetChild(3).GetComponent<TextMeshProUGUI>().color = Color.black;
    }
    void SettingsMenuUp()
    {
        if (soundOn) beepSource.Play();
        menus.transform.GetChild(2).GetChild(j).GetChild(3).GetComponent<TextMeshProUGUI>().color = Color.white;
        if (j == 1)
        {
            j = 4;
            menus.transform.GetChild(2).GetChild(0).localPosition = new Vector2(menus.transform.GetChild(2).GetChild(0).localPosition.x,
                menus.transform.GetChild(2).GetChild(0).localPosition.y - (settingsLabelOffset * 3.0f));
        }
        else
        {
            --j;
            menus.transform.GetChild(2).GetChild(0).localPosition = new Vector2(menus.transform.GetChild(2).GetChild(0).localPosition.x,
                menus.transform.GetChild(2).GetChild(0).localPosition.y + settingsLabelOffset);
        }
        menus.transform.GetChild(2).GetChild(j).GetChild(3).GetComponent<TextMeshProUGUI>().color = Color.black;
    }
    void SettingsSubmit()
    {
        i = (int)Label.SETTINGS;
        menus.transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
        menus.transform.GetChild(2).GetChild(j).GetChild(3).GetComponent<TextMeshProUGUI>().color = Color.white;
        labels.transform.GetChild(i).gameObject.GetComponent<TextMeshProUGUI>().color = Color.black;
        labelBackground.SetActive(true);
    }
    void Back()
    {
        if (soundOn) beepSource.Play();
        switch(i)
        {
            case (int)Label.PLAYMODE:
                i = (int)Label.PLAY;
                menus.transform.GetChild(0).GetChild(3).GetComponent<TextMeshProUGUI>().color = Color.white;
                menus.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
                break;
            case (int)Label.QUITMODE:
                i = (int)Label.QUIT;
                menus.transform.GetChild(4).GetChild(0).gameObject.SetActive(false);
                menus.transform.GetChild(4).GetChild(2).GetComponent<TextMeshProUGUI>().color = Color.white;
                break;
            case (int)Label.SETTINGSMODE:
                i = (int)Label.SETTINGS;
                menus.transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
                menus.transform.GetChild(2).GetChild(j).GetChild(3).GetComponent<TextMeshProUGUI>().color = Color.white;
                break;
        }
        labelBackground.SetActive(true);
        labels.transform.GetChild(i).gameObject.GetComponent<TextMeshProUGUI>().color = Color.black;
    }
    void ChangeSettings(int choice)
    {
        if (soundOn) beepSource.Play();
        switch (j - 1)
        {
            case (int)Settings.BACKGROUND:
                if (choice > 0)
                {
                    if (currBackground == 2) currBackground = 0;
                    else ++currBackground;
                }
                else
                {
                    if (currBackground == 0) currBackground = 2;
                    else --currBackground;
                }
                background.GetComponent<SpriteRenderer>().sprite = backgrounds[currBackground];
                switch (currBackground)
                {
                    case 0:
                        backgroundChoice.text = "Cyber";
                        break;
                    case 1:
                        backgroundChoice.text = "Mountains";
                        break;
                    case 2:
                        backgroundChoice.text = "Stage";
                        break;
                }
                break;
            case (int)Settings.MUSIC:
                if (choice > 0)
                {
                    if (currTrack == 2) currTrack = 0;
                    else ++currTrack;
                }
                else
                {
                    if (currTrack == 0) currTrack = 2;
                    else --currTrack;
                }
                musicSource.Stop();
                musicSource.clip = musicClips[currTrack];
                if(soundOn) musicSource.Play();
                switch (currTrack)
                {
                    case 0:
                        musicChoice.text = "Dance";
                        break;
                    case 1:
                        musicChoice.text = "Summer";
                        break;
                    case 2:
                        musicChoice.text = "Dark Techno";
                        break;
                }
                break;
            case (int)Settings.STARTINGLEVEL:
                if (choice > 0)
                {
                    if (level == 10) level = 1;
                    else ++level;
                }
                else
                {
                    if (level == 1) level = 10;
                    else --level;
                }
                levelChoice.text = level.ToString();
                break;
            case (int)Settings.SOUND:
                if (soundOn)
                {
                    soundOn = false;
                    musicSource.Stop();
                    soundChoice.text = "Off";
                }
                else
                {
                    soundOn = true;
                    musicSource.Play();
                    soundChoice.text = "On";
                }
                break;
        }
    }
}
