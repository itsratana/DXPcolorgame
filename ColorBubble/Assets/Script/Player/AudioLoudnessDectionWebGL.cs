using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLoudnessDectionWebGL : MonoBehaviour
{
    public float totalLoudness = 0;

    public float MicrophoneLoudness()
    {
        return totalLoudness;
    }

    public void OnData(float[] waveData)
    {
        int n = waveData.Length;
        for (int i = 0; i < n; i++)
        {
            totalLoudness += Mathf.Abs(waveData[i]);
        }
        totalLoudness /= n;
    }
    /*public float GetLoudnessFromAudio(int clipPosition, AudioClip clip)
    {
        int startPosition = clipPosition - sampleWindow;

        if (startPosition < 0)
        {
            return 0;
        }
        float[] waveData = new float[sampleWindow];
        clip.GetData(waveData, startPosition);

        float totalLoudness = 0;

        for (int i = 0; i < sampleWindow; i++)
        {
            totalLoudness += Mathf.Abs(waveData[i]);
        }

        return totalLoudness / sampleWindow;
    }*/
}

