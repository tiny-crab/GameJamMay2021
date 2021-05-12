using System;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Canvas canvas;
    public GameObject timerPanelPrefab;

    public Datastore datastore;

    public UniRx.IObservable<long> tick = Observable.Interval(TimeSpan.FromSeconds(1)).AsObservable();

    // Start is called before the first frame update
    void Start() {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        timerPanelPrefab = Resources.Load<GameObject>("Prefabs/UI/TimerPanel");

        var timerPanel = GameObject.Instantiate(timerPanelPrefab, canvas.transform);
        timerPanel.GetComponent<RectTransform>().anchorMin = new Vector2(1,0);
        timerPanel.GetComponent<RectTransform>().anchorMax = new Vector2(0,1);

        var advanceButton = (Button) timerPanel.transform.Find("AdvanceButton").GetComponent<Button>();
        advanceButton.onClick.AddListener(endTurn);

        tick.Subscribe(_ => {
            if (datastore.countdown.Value == 0) {
                datastore.countdown.SetValueAndForceNotify(datastore.turnLength.Value);
            } else {
                datastore.countdown.Value--;
            }
        });

        datastore.countdown.Subscribe(value => timerPanel.transform.Find("TimerText").GetComponent<Text>().text = value.ToString());

        datastore.countdown.Subscribe(value => {
            if (value == 0) {
                endTurn();
            }
            // doing this to prevent double skipping turns
            if (value < 1) {
                advanceButton.enabled = false;
            } else {
                advanceButton.enabled = true;
            }
        });
    }

    void endTurn() {
        Debug.Log("NewTurn");
        datastore.turnCount.Value++;
        datastore.countdown.Value = datastore.turnLength.Value;
    }
}
