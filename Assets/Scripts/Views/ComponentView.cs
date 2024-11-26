using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public abstract class ComponentView : MonoBehaviour {

    [SerializeField] protected Button _AddOrRemoveKeyframe;
    [SerializeField] protected Button _UpdateKeyframe;

    [SerializeField] protected Image _KeyframeImage;

    [SerializeField] protected GameObject _AddOrRemoveBlocker;
    [SerializeField] protected GameObject _Updateblocker;

    [SerializeField] protected EditorPainter _EditorPainter;

    [SerializeField] protected Sprite _Add;
    [SerializeField] protected Sprite _Remove;
    [SerializeField] protected Sprite _Unchangeable;
    [SerializeField] protected Image _Update;

    protected bool _FrameHasKeyframe;

	protected void _FlashInputField(TMPro.TMP_InputField inputField) {
		StopCoroutine(CO_FlashInputField(inputField));
		StartCoroutine(CO_FlashInputField(inputField));
	}

	private IEnumerator CO_FlashInputField(TMPro.TMP_InputField inputField) {
		const float startIntensity = 1.0f;
		const float flashIntensity = 2.0f;
		const float decreaseMultiplier = 0.1f;

		var colorBlock = inputField.colors;
		colorBlock.colorMultiplier = flashIntensity;
		inputField.colors = colorBlock;

		while (colorBlock.colorMultiplier >= startIntensity) {
			colorBlock.colorMultiplier -= decreaseMultiplier;
			inputField.colors = colorBlock;
			yield return null;
		}

		colorBlock.colorMultiplier = startIntensity;
		inputField.colors = colorBlock;
	}

}