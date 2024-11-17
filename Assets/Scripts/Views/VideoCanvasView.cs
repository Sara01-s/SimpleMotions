using System.Collections.Generic;
using UnityEngine.UI;
using SimpleMotions;
using UnityEngine;

public sealed class VideoCanvasView : MonoBehaviour {

	[Header("Primitive Sprites")]
	[SerializeField] private Sprite _circleSprite;
	[SerializeField] private Sprite _rectSprite;
	[SerializeField] private Sprite _triangleSprite;

	[Header("Render")]
	[SerializeField] private Transform _canvasOrigin;
	[SerializeField] private Camera _editorCamera;

	private readonly Dictionary<int, GameObject> _displayedEntites = new();
	private IReadOnlyDictionary<string, Sprite> _spriteByPrimitiveShape;
	private IVideoCanvasViewModel _videoCanvasViewModel;

	public void Configure(IVideoCanvasViewModel videoCanvasViewModel) {
		videoCanvasViewModel.OnCanvasUpdate.Subscribe(OnUpdateCanvas);
		videoCanvasViewModel.OnEntityRemoved.Subscribe(RemoveEntity);
		videoCanvasViewModel.BackgroundColorUpdated.Subscribe(UpdateCanvasColor);

		_videoCanvasViewModel = videoCanvasViewModel;

		PopulateSpriteDictionary();
	}

	private void PopulateSpriteDictionary() {
		_spriteByPrimitiveShape = new Dictionary<string, Sprite>() {
			{ "Circle", _circleSprite },
			{ "Triangle", _triangleSprite },
			{ "Rect", _rectSprite }
		};
	}

	private void OnUpdateCanvas((int id, string name) entity) {
		UpdateEntityDisplay(entity);
	}

	private void RemoveEntity(int entityId) {
		if (_displayedEntites.TryGetValue(entityId, out var entityDisplay)) {
			_displayedEntites.Remove(entityId);
			Destroy(entityDisplay);
			print($"Entidad {entityId} removida del canvas.");
		}
	}

	private void UpdateEntityDisplay((int id, string name) entity) {
		// entity not registered, create it.
		if (!_displayedEntites.ContainsKey(entity.id)) {
			DisplayNewEntity(entity.id, entity.name);
		}

		// entity already registered, update it.
		UpdateEntity(entity.id);
	}

	private void DisplayNewEntity(int entityId, string entityName) {
		string entity = $"Entity {entityId}: \"{entityName}\"";
		var displayedEntity = new GameObject(entityName);

		displayedEntity.transform.SetParent(_canvasOrigin);
		displayedEntity.transform.localPosition = Vector2.zero;
		displayedEntity.transform.name = entity;

		_displayedEntites.Add(entityId, displayedEntity);

		Debug.Log("Mostrando entidad: " + entity);
	}

	private void UpdateEntity(int entityId) {
		if (!_displayedEntites.TryGetValue(entityId, out var displayedEntity)) {
			return;
		}

		if (_videoCanvasViewModel.EntityHasTransform(entityId, out var transform)) {
			var entityTransform = displayedEntity.GetComponent<Transform>();

			entityTransform.localPosition = new Vector3(transform.pos.x, transform.pos.y, 0.0f);
			entityTransform.localScale = new Vector2(transform.scale.w, transform.scale.h);
			entityTransform.rotation = Quaternion.AngleAxis(transform.rollAngleDegrees, Vector3.forward);
		}

		if (_videoCanvasViewModel.EntityHasShape(entityId, out var shape)) {
			if (!displayedEntity.TryGetComponent<SpriteRenderer>(out var spriteRenderer)) {
				spriteRenderer = displayedEntity.AddComponent<SpriteRenderer>();
			}

			spriteRenderer.color = new Color(shape.color.r, shape.color.g, shape.color.b, shape.color.a);
			spriteRenderer.sprite = _spriteByPrimitiveShape[shape.primitiveShape];
		}
	}

	private void UpdateCanvasColor((float r, float g, float b, float a) color) {
		_editorCamera.backgroundColor = new Color(color.r, color.g, color.b, color.a);
	}

}