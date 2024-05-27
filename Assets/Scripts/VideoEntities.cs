using System.Collections.Generic;
using UnityEngine;

public class VideoEntities : MonoBehaviour {

	private static readonly List<Interpolable> Interpolables = new();

	[SerializeField] private VideoData _videoData;

	private void OnEnable() => _videoData.OnCurrentTimeNormalizedChanged += UpdateInterpolables;
	private void OnDisable() => _videoData.OnCurrentTimeNormalizedChanged -= UpdateInterpolables;

	public static void AddInterpolable(Interpolable interpolable) {
		Interpolables.Add(interpolable);
	}

	public static void RemoveInterpolable(Interpolable interpolable) {
		Interpolables.Remove(interpolable);
	}

	private void UpdateInterpolables(float currentVideoTimeNormalized) {
		foreach (var interpolable in Interpolables) {
			interpolable.InterpolateAll(currentVideoTimeNormalized);
		}
	}

}