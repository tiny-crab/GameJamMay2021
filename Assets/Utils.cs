using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Utils {

    public static T getRandomElement<T>(this IEnumerable<T> list) {
        var rnd = new System.Random();
        return list.OrderBy(i => rnd.Next()).First();
    }

    public static List<T> getManyRandomElements<T>(this IEnumerable<T> list, int number) {
        var rnd = new System.Random();
        return list.OrderBy(i => rnd.Next()).Take(number).ToList();
    }

    public static void assignSpriteFromPath(this GameObject gameObj, string path) {
        gameObj.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(path);
    }

    public static void PlayWithEcho(this AudioSource source, AudioClip clipToPlay, float chanceOfEcho=1f, float echoMaxVol=.5f, bool pan=true) {
        var random = new System.Random();

        source.clip = clipToPlay;

        var initialPan = source.panStereo;
        var initialVol = source.volume;

        if (random.Next(100) <= chanceOfEcho * 100) {
            var left = random.Next(2) == 0;
            var panLevel = random.NextDouble();
            source.panStereo = left ? (float) panLevel * -1 : (float) panLevel;
            source.Play();

            var echoLevel = echoMaxVol * initialVol;
            source.volume = (float) echoLevel;
            source.panStereo = left ? (float) panLevel : (float) panLevel * -1;

            // this doesn't work for some reason. Need to come back to it.
            var echoStart = random.Next(Mathf.FloorToInt(clipToPlay.length - 1), Mathf.FloorToInt(clipToPlay.length + 3));
            source.PlayDelayed(echoStart);
        } else {
            source.Play();
        }

        // reset source params
        source.panStereo = initialPan;
        source.volume = initialVol;
    }
}