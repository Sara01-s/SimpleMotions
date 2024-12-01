using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class ConfirmationPanel : MonoBehaviour {

	[SerializeField] private TextMeshProUGUI _confirmationMessage;
	[SerializeField] private Button _accept;
	[SerializeField] private Button _cancel;

	private void Awake() {
		gameObject.SetActive(false);
	}

	public void OpenConfirmationPanel(string message, Action<bool> resultCallback) {
		_confirmationMessage.text = message;

		_accept.onClick.AddListener(() => {
			resultCallback(true);
			ResetConfirmationPanel();
		});

		_cancel.onClick.AddListener(() => {
			resultCallback(false);
			ResetConfirmationPanel();
		});

		gameObject.SetActive(true);
	}

	private void ResetConfirmationPanel() {
		_accept.onClick.RemoveAllListeners();
		_cancel.onClick.RemoveAllListeners();
		gameObject.SetActive(false);
	}

}