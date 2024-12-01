using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour {

	[SerializeField] private AudioClip _errorSound;
	
	private AudioSource _audioSource;

	private void Awake() {
		_audioSource = GetComponent<AudioSource>();
	}

	public void PlayErrorSound() {
		_audioSource.PlayOneShot(_errorSound);
	}

}