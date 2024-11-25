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
    private Vector2 _startEntityScale;
    private Vector2 _startEntityPosition;

    public override void OnBeginDrag(PointerEventData eventData) {
		var transformDTO = _GetSelectedEntityTransformData();

		_startEntityScale = new Vector2(transformDTO.Scale.w, transformDTO.Scale.h);
		_startEntityPosition = new Vector2(transformDTO.Position.x, transformDTO.Position.y);
        _startPointerWorldPos = _GetPointerWorldPos(eventData.position);
    }

    public override void OnDrag(PointerEventData eventData) {
        var currentPointerWorldPos = _GetPointerWorldPos(eventData.position);
        var pointerDeltaWorld = currentPointerWorldPos - _startPointerWorldPos;
        var scaleDirection = _cornerToScaleDirection[_corner];

        ScaleEntity(pointerDeltaWorld, scaleDirection);
    }

    private void ScaleEntity(Vector2 pointerDeltaWorld, Vector2 scaleDirection) {
        var deltaScale = new Vector2(pointerDeltaWorld.x, pointerDeltaWorld.y) * scaleDirection;
        var newEntityScale = _startEntityScale + deltaScale;
        var newEntityPosition = _startEntityPosition + pointerDeltaWorld * 0.5f;

		_SetSelectedEntityPosition(newEntityPosition);
		_SetSelectedEntityScale(newEntityScale);
    }

}
