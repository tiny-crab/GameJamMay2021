using System;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Datastore datastore;

    // public UniRx.IObservable<long> tick = Observable.Interval(TimeSpan.FromSeconds(1)).AsObservable();

    private IDisposable tickSubscription;

    // Start is called before the first frame update
    void Start() {
        datastore = GameObject.Find("Datastore").GetComponent<Datastore>();
        var timerPanel = GameObject.Find("TimerPanel");
        timerPanel.GetComponent<RectTransform>().anchorMin = new Vector2(1,1);
        timerPanel.GetComponent<RectTransform>().anchorMax = new Vector2(1,1);

        var advanceButton = (Button) timerPanel.transform.Find("AdvanceButton").GetComponent<Button>();
        advanceButton.onClick.AddListener(endTurn);

        // tickSubscription = tick.Subscribe(_ => {
        //     if (datastore.countdown.Value == 0) {
        //         datastore.countdown.SetValueAndForceNotify(datastore.turnLength.Value);
        //     } else {
        //         datastore.countdown.Value--;
        //     }
        // });

        // datastore.countdown.Subscribe(value => timerPanel.transform.Find("TimerText").GetComponent<Text>().text = value.ToString());

        // datastore.countdown.Subscribe(value => {
        //     if (value == 0) {
        //         endTurn();
        //     }
        //     // doing this to prevent double skipping turns
        //     if (value < 1) {
        //         advanceButton.enabled = false;
        //     } else {
        //         advanceButton.enabled = true;
        //     }
        // });
    }

    void endTurn() {
        datastore.turnCount.Value++;
        // datastore.countdown.Value = datastore.turnLength.Value;
    }

    
}
