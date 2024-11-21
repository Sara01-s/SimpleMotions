using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using SimpleMotions;

public class SelectionGizmoCorner : MonoBehaviour, IDragHandler, IBeginDragHandler {

    private enum Corner {
        DownLeft, DownRight, UpRight, UpLeft
    }

    [SerializeField] private Canvas _editorCanvas;
    [SerializeField] private Corner _corner;

    private readonly Dictionary<Corner, Vector2> _cornerToScaleDirection = new() {
        { Corner.DownLeft,  new Vector2(-1.0f, -1.0f) },
        { Corner.DownRight, new Vector2( 1.0f, -1.0f) },
        { Corner.UpRight,   new Vector2( 1.0f,  1.0f) },
        { Corner.UpLeft,    new Vector2(-1.0f,  1.0f) }
    };

    private RectTransform _rectTransform;
    private RectTransform _parentRectTransform;
    private IComponentViewModel _entityViewModel;
    private IEntitySelector _entitySelector;

    private Vector2 _startPointerWorldPos;
    private Vector2 _startParentSize;
    private Vector2 _startParentPosition;
    private Vector2 _startEntityScale;
    private Vector2 _startEntityPosition;


    public void Configure(IComponentViewModel entityViewModel, IEntitySelector entitySelector) {
        _rectTransform = GetComponent<RectTransform>();
        _parentRectTransform = (RectTransform)transform.parent;
        _entityViewModel = entityViewModel;
        _entitySelector = entitySelector;
    }

    private Vector2 GetPointerWorldPos(Vector2 pointerScreenPos) {
        var camera = _editorCanvas.worldCamera;
        return camera.ScreenToWorldPoint(pointerScreenPos);
    }

    public void OnBeginDrag(PointerEventData eventData) {
        int selectedEntityId = _entitySelector.SelectedEntity.Id;

        if (_entityViewModel.EntityHasTransform(selectedEntityId, out var transform)) {
            _startEntityScale = new Vector2(transform.scale.w, transform.scale.h);
            _startEntityPosition = new Vector2(transform.pos.x, transform.pos.y);
        }

        _startPointerWorldPos = GetPointerWorldPos(eventData.position);
        _startParentSize = _parentRectTransform.sizeDelta;
        _startParentPosition = _parentRectTransform.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData) {
        var currentPointerWorldPos = GetPointerWorldPos(eventData.position);
        var pointerDeltaWorld = currentPointerWorldPos - _startPointerWorldPos;
        var pointerDeltaScreen = _parentRectTransform.InverseTransformVector(pointerDeltaWorld);
        var scaleDirection = _cornerToScaleDirection[_corner];

        ScaleEntity(pointerDeltaWorld, scaleDirection);
        ScaleSelectionGizmoBody(pointerDeltaScreen, scaleDirection);
    }

    private void ScaleEntity(Vector2 pointerDeltaWorld, Vector2 scaleDirection) {
        var deltaScale = new Vector2(pointerDeltaWorld.x, pointerDeltaWorld.y) * scaleDirection;
        var newEntityScale = _startEntityScale + deltaScale;

        var newEntityPosition = _startEntityPosition + pointerDeltaWorld * 0.5f;
        int selectedEntityId = _entitySelector.SelectedEntity.Id;

        _entityViewModel.SetEntityScale(selectedEntityId, (newEntityScale.x, newEntityScale.y));
        _entityViewModel.SetEntityPosition(selectedEntityId, (newEntityPosition.x, newEntityPosition.y));
    }

    private void ScaleSelectionGizmoBody(Vector2 pointerDeltaScreen, Vector2 scaleDirection) {
        var deltaSize = pointerDeltaScreen * scaleDirection;
        var newParentSize = _startParentSize + deltaSize;

        _parentRectTransform.sizeDelta = newParentSize;
        _parentRectTransform.anchoredPosition = _startParentPosition + pointerDeltaScreen * 0.5f;
    }

}
