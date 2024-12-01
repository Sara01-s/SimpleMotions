using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class SelectionGizmoSide : SelectionGizmo {

    private enum Side {
        Left, Down, Up, Right
    }

    [SerializeField] private Side _side;

    private readonly Dictionary<Side, Vector2> _sideToScaleDirection = new() {
        { Side.Left,  new Vector2(-1.0f,  0.0f) },
        { Side.Down,  new Vector2( 0.0f, -1.0f) },
        { Side.Up,    new Vector2( 0.0f,  1.0f) },
        { Side.Right, new Vector2( 1.0f,  0.0f) }
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

        var scaleDirection = _sideToScaleDirection[_side];
        var deltaScale = new Vector2 (
            scaleDirection.x * pointerDeltaLocal.x,
            scaleDirection.y * pointerDeltaLocal.y
        );

        var newEntityScale = _startEntityScale + deltaScale;

        newEntityScale = new Vector2 (
            Mathf.Max(0.1f, newEntityScale.x),
            Mathf.Max(0.1f, newEntityScale.y)
        );

        var scaleCenterOffset = new Vector2 (
            scaleDirection.x * (newEntityScale.x - _startEntityScale.x) * 0.5f,
            scaleDirection.y * (newEntityScale.y - _startEntityScale.y) * 0.5f
        );

        var scaleCenterOffsetWorld = new Vector2 (
            cos * scaleCenterOffset.x - sin * scaleCenterOffset.y,
            sin * scaleCenterOffset.x + cos * scaleCenterOffset.y
        );

        var newEntityPosition = _startEntityPosition + scaleCenterOffsetWorld;

        _SetSelectedEntityPosition(newEntityPosition);
        _SetSelectedEntityScale(newEntityScale);
    }
}
