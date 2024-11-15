using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class Mover : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler {

    [SerializeField] private RectTransform _mover;    
    [SerializeField] private Canvas _editorCanvas;

    private bool _isDragging;

    public void OnDrag(PointerEventData eventData) {
        if (!_isDragging) {
            return;
        }

        var canvasRect = _editorCanvas.GetComponent<RectTransform>();

        Vector2 localDelta = eventData.delta / _editorCanvas.scaleFactor;
        Vector2 newPosition = _mover.anchoredPosition + localDelta;

        float minX = -canvasRect.rect.width / 2 + _mover.rect.width / 2;
        float maxX = canvasRect.rect.width / 2 - _mover.rect.width / 2;
        float minY = -canvasRect.rect.height / 2 + _mover.rect.height / 2;
        float maxY = canvasRect.rect.height / 2 - _mover.rect.height / 2;

        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        _mover.anchoredPosition = newPosition;
    }

    public void OnPointerDown(PointerEventData eventData) {
        IsPointerOverSelf(eventData);
    }

    public void OnPointerUp(PointerEventData eventData) {
        _isDragging = false;
    }

    private void IsPointerOverSelf(PointerEventData eventData) {
        var raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);

        foreach (var result in raycastResults) {
            if (result.gameObject == gameObject) {
                _isDragging = true;
            }
        }
    }

}