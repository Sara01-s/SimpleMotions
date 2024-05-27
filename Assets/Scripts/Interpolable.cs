using static Unity.Mathematics.math;
using UnityEngine;

public class Interpolable : MonoBehaviour {

	[SerializeField] private Vector2 _moveTo;
	[SerializeField] private Vector2 _scaleTo;
	[SerializeField] private float _rollTo;
	[SerializeField] private Color _colorTo;

	private Vector2 _initPos;
	private Vector2 _initScale;
	private float _initRoll;
	private Color _initColor;
	private SpriteRenderer _renderer;

	private void OnEnable() => VideoEntities.AddInterpolable(this);
	private void OnDisable() => VideoEntities.RemoveInterpolable(this);

	private void Start() {
		_renderer = GetComponent<SpriteRenderer>();
		_initPos = transform.position;
		_initScale = transform.localScale;
		_initRoll = transform.rotation.eulerAngles.z;
		_initColor = _renderer.color;
	}

	public void InterpolateAll(float currentVideoTimeNormalized) {
		Move(_moveTo, currentVideoTimeNormalized);
		Scale(_scaleTo, currentVideoTimeNormalized);
		RollTo(_rollTo, currentVideoTimeNormalized);
		ColorTo(_colorTo, currentVideoTimeNormalized);
	}

	private void Move(Vector2 to, float t) {
		Vector2 tempPos = lerp(_initPos, to, t);
		transform.position = tempPos;
	}

	private void Scale(Vector2 to, float t) {
		Vector2 tempScale = lerp(_initScale, to, t);
		transform.localScale = tempScale;
	}

	private void RollTo(float to, float t) {
		float tempRoll = lerp(_initRoll, to, t);
		transform.rotation = Quaternion.Euler(0.0f, 0.0f, tempRoll);
	}

	private void ColorTo(Color to, float t) {
		var tempColor = Color.Lerp(_initColor, to, t);
		_renderer.color = tempColor;
	}

}
