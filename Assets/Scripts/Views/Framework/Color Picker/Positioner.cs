using UnityEngine;

public class Positioner : MonoBehaviour {

    public RectTransform _colorPickerPosition;
    public Camera _editorCamera; 
    public Canvas _editorCanvas; 

    public void MoveToWorldPosition() {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _editorCanvas.GetComponent<RectTransform>(),
            Camera.main.WorldToScreenPoint(gameObject.transform.position),
            _editorCamera,
            out Vector2 anchoredPosition
        );

        _colorPickerPosition.anchoredPosition = anchoredPosition;
    }

}