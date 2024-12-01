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
	private float _startRollDegrees;

    public override void OnBeginDrag(PointerEventData eventData) {
		var transformDTO = _GetSelectedEntityTransformDTO();

		_startEntityScale = new Vector2(transformDTO.Scale.w, transformDTO.Scale.h);
		_startEntityPosition = new Vector2(transformDTO.Position.x, transformDTO.Position.y);
        _startPointerWorldPos = _GetPointerWorldPos(eventData.position);
		_startRollDegrees = transformDTO.RollDegrees;
    }
	
	public override void OnDrag(PointerEventData eventData) {
		var currentPointerWorldPos = _GetPointerWorldPos(eventData.position);
		var pointerDeltaWorld = currentPointerWorldPos - _startPointerWorldPos;
		var rotationRadians = _startRollDegrees * Mathf.Deg2Rad;

		var cos = Mathf.Cos(rotationRadians);
		var sin = Mathf.Sin(rotationRadians);
		var pointerDeltaLocal = new Vector2 (
			cos * pointerDeltaWorld.x + sin * pointerDeltaWorld.y,
			-sin * pointerDeltaWorld.x + cos * pointerDeltaWorld.y
		);

		var scaleDirection = _cornerToScaleDirection[_corner];
		var deltaScale = pointerDeltaLocal * scaleDirection;
		var newEntityScale = _startEntityScale + deltaScale;

		newEntityScale = new Vector2 (
			Mathf.Max(0.1f, newEntityScale.x),
			Mathf.Max(0.1f, newEntityScale.y)
		);

		var newCornerOffsetLocal = new Vector2 (
			newEntityScale.x * scaleDirection.x * 0.5f,
			newEntityScale.y * scaleDirection.y * 0.5f
		);

		var originalCornerOffsetLocal = new Vector2 (
			_startEntityScale.x * scaleDirection.x * 0.5f,
			_startEntityScale.y * scaleDirection.y * 0.5f
		);

		var cornerOffsetDeltaLocal = newCornerOffsetLocal - originalCornerOffsetLocal;

		var cornerOffsetDeltaWorld = new Vector2 (
			cos * cornerOffsetDeltaLocal.x - sin * cornerOffsetDeltaLocal.y,
			sin * cornerOffsetDeltaLocal.x + cos * cornerOffsetDeltaLocal.y
		);

		var newEntityPosition = _startEntityPosition + cornerOffsetDeltaWorld;

		_SetSelectedEntityPosition(newEntityPosition);
		_SetSelectedEntityScale(newEntityScale);
	}

}
