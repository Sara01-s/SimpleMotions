using UnityEngine.UI;
using UnityEngine;

public class ToggleSpriteSwap : MonoBehaviour {

	[SerializeField] private Toggle _targetToggle;
	[SerializeField] private Image _targetImage;
	[SerializeField] private Sprite _onSprite;
	[SerializeField] private Sprite _offSprite;

	private void Awake() {
		_targetToggle.toggleTransition = Toggle.ToggleTransition.None; 
		_targetToggle.onValueChanged.AddListener(SwapToggleSprite);
	}

	private void SwapToggleSprite(bool isOn) {
		_targetImage.sprite = isOn ? _onSprite : _offSprite;
	}
}