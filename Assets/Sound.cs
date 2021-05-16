using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UniRx;
using System;
using System.Linq;

public class Sound : MonoBehaviour
{
    public List<AudioClip> ambienceClips;
    public AudioSource ambienceSource;

    public List<AudioClip> pianoClips = new List<AudioClip>();
    public AudioSource pianoSource;

    public List<AudioClip> robinClips = new List<AudioClip>();
    public AudioSource robinSource;
    public int secondsTilNextRobin = 0;
    public int robinMinInterval = 15;
    public int robinMaxInterval = 225;

    public List<AudioClip> finchClips = new List<AudioClip>();
    public AudioSource finchSource;
    public int secondsTilNextFinch = 0;
    public int finchMinInterval = 7;
    public int finchMaxInterval = 230;

    public List<AudioClip> sparrowClips = new List<AudioClip>();
    public AudioSource sparrowSource;
    public int secondsTilNextSparrow = 0;
    public int sparrowMinInterval = 32;
    public int sparrowMaxInterval = 424;

    public List<AudioClip> gooseClips = new List<AudioClip>();
    public AudioSource gooseSource;
    public int secondsTilNextGoose = 0;
    public int gooseMinInterval = 240;
    public int gooseMaxInterval = 667;

    public List<AudioClip> whipClips = new List<AudioClip>();
    public AudioSource whipSource;
    public int secondsTilNextWhip = 0;
    public int whipMinInterval = 180;
    public int whipMaxInterval = 1000;

    public List<AudioClip> clickClips = new List<AudioClip>();
    public AudioSource clickSource;

    public List<AudioClip> dropClips = new List<AudioClip>();
    public AudioSource dropSource;

    public List<AudioClip> happyClips = new List<AudioClip>();
    public AudioSource happySource;

    public List<AudioClip> digClips = new List<AudioClip>();
    public AudioSource digSource;

    public List<AudioClip> switchClips = new List<AudioClip>();
    public AudioSource switchSource;

    public List<AudioClip> mouseClips = new List<AudioClip>();
    public AudioSource mouseSource;

    public List<AudioClip> errorClips = new List<AudioClip>();
    public AudioSource errorSource;

    System.Random random = new System.Random();
    UniRx.IObservable<long> tick = Observable.Interval(TimeSpan.FromSeconds(1)).AsObservable();

    public Datastore datastore;

    public AudioMixer audioMixer;
    public AudioMixerGroup masterAudioMixerGroup;

    void Awake() {
        datastore = GameObject.Find("Datastore").GetComponent<Datastore>();
        audioMixer = Resources.Load<AudioMixer>("MainMixer");
        masterAudioMixerGroup = audioMixer.FindMatchingGroups("Master")[0];

        (pianoSource, pianoClips) = loadClips(20, "SFX/piano/piano_", "D3");
        pianoSource.volume = 0.7f;

        (robinSource, robinClips) = loadClips(4, "SFX/birds/robin_");
        robinSource.volume = 0.28f;
        secondsTilNextRobin = random.Next(robinMinInterval, robinMaxInterval);

        (finchSource, finchClips) = loadClips(5, "SFX/birds/finch_");
        finchSource.volume = 0.24f;
        secondsTilNextFinch = random.Next(finchMinInterval, finchMaxInterval);

        (gooseSource, gooseClips) = loadClips(1, "SFX/birds/goose_");
        gooseSource.volume = 0.3f;
        secondsTilNextGoose = random.Next(gooseMinInterval, gooseMaxInterval);

        (sparrowSource, sparrowClips) = loadClips(3, "SFX/birds/sparrow_");
        sparrowSource.volume = 0.3f;
        secondsTilNextSparrow = random.Next(sparrowMinInterval, sparrowMaxInterval);

        (whipSource, whipClips) = loadClips(1, "SFX/birds/whippoorwill_");
        whipSource.volume = 0.23f;
        secondsTilNextWhip = random.Next(whipMinInterval, whipMaxInterval);

        (ambienceSource, ambienceClips) = loadClips(1, "SFX/ambience_fixloop_");

        (clickSource, clickClips) = loadClips(2, "SFX/UI/click", "D1", 1);
        (digSource, digClips) = loadClips(5, "SFX/impact/footstep_snow_", "D3", 0);
        (happySource, happyClips) = loadClips(1, "SFX/UI/confirmation_", "D3", 3);
        (dropSource, dropClips) = loadClips(3, "SFX/UI/drop_", "D3", 2);
        (switchSource, switchClips) = loadClips(1, "SFX/UI/switch", "D1", 1);
        (mouseSource, mouseClips) = loadClips(2, "SFX/UI/mouseclick", "D1", 1);
        (errorSource, errorClips) = loadClips(1, "SFX/UI/error_", "D3", 2);

        ambienceSource.gameObject.name = "Ambience source";
        ambienceSource.volume = 0.2f;
        ambienceSource.clip = ambienceClips.Single();
        ambienceSource.loop = true;
        ambienceSource.Play();

        datastore.volumeSliderValue.Subscribe(newValue => {
            audioMixer.SetFloat("SoundVolume", Mathf.Log10(newValue) * 20);
        });

        tick.Subscribe(_ => {
            playClipAtRandomInterval(robinSource, robinClips, ref secondsTilNextRobin, robinMinInterval, robinMaxInterval);
            playClipAtRandomInterval(finchSource, finchClips, ref secondsTilNextFinch, finchMinInterval, finchMaxInterval);
            playClipAtRandomInterval(sparrowSource, sparrowClips, ref secondsTilNextSparrow, sparrowMinInterval, sparrowMaxInterval);
            playClipAtRandomInterval(gooseSource, gooseClips, ref secondsTilNextGoose, gooseMinInterval, gooseMaxInterval);
            playClipAtRandomInterval(whipSource, whipClips, ref secondsTilNextWhip, whipMinInterval, whipMaxInterval);
        });

        datastore.turnCount.Subscribe(value => {
            if (value % 12 == 0 && value != 0) {
                pianoSource.clip = pianoClips.getRandomElement();
                pianoSource.Play();
            }
        });

        datastore.turnCount.Subscribe(_ => {
            switchSource.clip = switchClips.getRandomElement();
            switchSource.Play();
        });

        datastore.ordersFulfilled.Subscribe(_ => {
            clickSource.clip = clickClips.getRandomElement();
            clickSource.Play();
        });

        datastore.customersSatisfied.Subscribe(_ => {
            happySource.clip = happyClips.getRandomElement();
            happySource.Play();
        });

        datastore.mouseController.plantedSeeds.Subscribe(_ => {
            clickSource.clip = clickClips.getRandomElement();
            clickSource.Play();
        });

        datastore.mouseController.harvestedCrops.Subscribe(_ => {
            dropSource.clip = dropClips.getRandomElement();
            dropSource.Play();
        });

        datastore.mouseController.tilledSoil.Subscribe(_ => {
            digSource.clip = digClips.getRandomElement();
            digSource.Play();
        });

        datastore.mouseController.errorClick.Subscribe(_ => {
            errorSource.clip = errorClips.getRandomElement();
            errorSource.Play();
        });

        // datastore.inventoryShop.cardClicked.Subscribe(value => {
        //     mouseSource.clip = mouseClips.getRandomElement();
        //     mouseSource.Play();
        // });
    }

    void playClipAtRandomInterval(AudioSource source, List<AudioClip> clips, ref int secondsRemaining, int minInterval, int maxInterval) {
        if (secondsRemaining == 0) {
            source.clip = clips.getRandomElement();
            // swap to PlayWithEcho once that's working
            source.Play();
            secondsRemaining = random.Next(minInterval, maxInterval);
        } else {
            secondsRemaining--;
        }
    }

    (AudioSource, List<AudioClip>) loadClips(int numClips, string pathPrefix, string numSuffixLength="D2", int startIndex=1) {
        var clips = new List<AudioClip>();
        var source = new GameObject();
        source.transform.parent = this.transform;
        source.AddComponent<AudioSource>();
        for (var i = startIndex; i < startIndex + numClips; i++) {
            clips.Add(Resources.Load<AudioClip>($"{pathPrefix}{i.ToString(numSuffixLength)}"));
        }
        AudioSource audioSource = source.GetComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = masterAudioMixerGroup;
        return (audioSource, clips);
    }
}
