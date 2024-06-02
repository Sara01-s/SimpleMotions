using UnityEngine;
using System;

[CreateAssetMenu]
public class VideoData : ScriptableObject {

	public Action<float> OnCurrentTimeChanged;
	public Action<float> OnCurrentTimeNormalizedChanged;

	public float DurationSec;
	public float CurrentTime;
	public float CurrentTimeNormalized;

	public bool IsPlaying;

}