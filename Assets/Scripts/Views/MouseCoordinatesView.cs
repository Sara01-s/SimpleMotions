using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MouseCoordinatesView : MonoBehaviour {

	[SerializeField] private Camera _editorCamera;
	[SerializeField] private Toggle _showMouseCoordiantes;
	[SerializeField] private TextMeshProUGUI _mouseCoordinates;
	[SerializeField] private Transform _canvasOrigin;

	private void Awake() {
		_showMouseCoordiantes.onValueChanged.AddListener(isOn => {
			if (isOn) {
				StartCoroutine(CO_ShowMouseCoordinates());
			}
			else {
				StopCoroutine(CO_ShowMouseCoordinates());
			}

			_mouseCoordinates.transform.parent.gameObject.SetActive(isOn);
		});
	}

	private IEnumerator CO_ShowMouseCoordinates() {
		while (true) {
			var coords = GetMouseCanvasOriginCoords(Input.mousePosition);
			string x = coords.x.ToString("#0.00");
			string y = coords.y.ToString("#0.00");

			_mouseCoordinates.text = $"({x}, {y})";
			yield return null;
		}
	}

	private Vector2 GetMouseCanvasOriginCoords(Vector2 mouseScreenPos) {
		var mouseWorldPos = _editorCamera.ScreenToWorldPoint(mouseScreenPos);
		return _canvasOrigin.InverseTransformPoint(mouseWorldPos);
	}
	
}