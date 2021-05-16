using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Linq;
using UnityEngine.UI;
using UniRx;

public class PauseMenu : MonoBehaviour
{
    public Datastore datastore;

    public ReactiveProperty<bool> paused = new ReactiveProperty<bool>(false);

    public GameObject topLevelContainer;
    public List<GameObject> backgrounds;
    public GameObject mainMenu;
    public GameObject settingsMenu;

    public Slider volumeSlider;

    
    void Awake() {
        datastore = GameObject.Find("Datastore").GetComponent<Datastore>();
    }

    void Start()
    {
        topLevelContainer = transform.Find("TopLevelContainer").gameObject;
        backgrounds.Add(topLevelContainer.transform.Find("MainMenu/ResumeBackground").gameObject);
        backgrounds.Add(topLevelContainer.transform.Find("MainMenu/SettingsBackground").gameObject);
        backgrounds.Add(topLevelContainer.transform.Find("MainMenu/ExitBackground").gameObject);
        mainMenu = topLevelContainer.transform.Find("MainMenu").gameObject;
        settingsMenu = topLevelContainer.transform.Find("SettingsMenu").gameObject;
        volumeSlider = settingsMenu.transform.Find("VolumeSlider").gameObject.GetComponent<Slider>();
        paused.Subscribe(value => {
            if (value) {
                enable();
            } else {
                disable();
            }
        });
        volumeSlider.onValueChanged.AddListener(delegate {
            datastore.volumeSliderValue.Value = volumeSlider.value;
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void togglePause() {
        paused.Value = !paused.Value;
    }

    private void enable() {
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
        topLevelContainer.SetActive(true);
    }

    private void disable() {
        backgrounds.ForEach(background => {
            background.GetComponent<Image>().enabled = false;
        });
        topLevelContainer.SetActive(false);
    }

    public void quit() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}
