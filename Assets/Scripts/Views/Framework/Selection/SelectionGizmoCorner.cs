using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class SelectionGizmoCorner : SelectionGizmo {

    private enum Corner {
        DownLeft, DownRight, UpRight, UpLeft
    }

    [SerializeField] private Corner _corner;

    private readonly Dictionary<Corner, Vector2> _cornerToScaleDirection = new() {
        { Corner.DownLeft,  new Vector2(-1.0f, -1.0f) },
        { Corner.DownRight, new Vector2( 1.0f, -1.0f) },
        { Corner.UpRight,   new Vector2( 1.0f,  1.0f) },
        { Corner.UpLeft,    new Vector2(-1.0f,  1.0f) }
    };

    private Vector2 _startPointerWorldPos;
    private Vector2 _startBodySize;
    private Vector2 _startBodyPos;
    private Vector2 _startEntityScale;
    private Vector2 _startEntityPosition;
	private Vector2 _pointerDeltaWorld;

    public override void OnBeginDrag(PointerEventData eventData) {
		var (pos, scale, _) = _GetSelectedEntityTransformData();

		_startEntityScale = new Vector2(scale.w, scale.h);
		_startEntityPosition = new Vector2(pos.x, pos.y);
        _startPointerWorldPos = _GetPointerWorldPos(eventData.position);

        _startBodySize = _SelectionGizmoRect.sizeDelta;
        _startBodyPos = _SelectionGizmoRect.anchoredPosition;
    }

    public override void OnDrag(PointerEventData eventData) {
        var currentPointerWorldPos = _GetPointerWorldPos(eventData.position);
        var pointerDeltaWorld = currentPointerWorldPos - _startPointerWorldPos;
        var pointerDeltaScreen = _SelectionGizmoRect.InverseTransformVector(pointerDeltaWorld);
        var scaleDirection = _cornerToScaleDirection[_corner];

        ScaleEntity(pointerDeltaWorld, scaleDirection);
		_pointerDeltaWorld = pointerDeltaScreen;
    }

    private void ScaleEntity(Vector2 pointerDeltaWorld, Vector2 scaleDirection) {
        var deltaScale = new Vector2(pointerDeltaWorld.x, pointerDeltaWorld.y) * scaleDirection;
        var newEntityScale = _startEntityScale + deltaScale;
        var newEntityPosition = _startEntityPosition + pointerDeltaWorld * 0.5f;

		_SetSelectedEntityPosition(newEntityPosition);
		_SetSelectedEntityScale(newEntityScale);
    }

	protected override void SyncGizmoWithEntity() {
		var deltaSize = _pointerDeltaWorld * _cornerToScaleDirection[_corner];
        var newParentSize = _startBodySize + deltaSize;

        _SelectionGizmoRect.sizeDelta = newParentSize;
        _SelectionGizmoRect.anchoredPosition = _startBodyPos + _pointerDeltaWorld * 0.5f;
	}

}
