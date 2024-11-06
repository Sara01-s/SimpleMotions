using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using SimpleMotions;

public sealed class VideoCanvasView : MonoBehaviour {

	[Header("Entity prefab")]
	[SerializeField] private GameObject _entityPrefab;

	[Header("Primitive Sprites")]
	[SerializeField] private Sprite _circleSprite;
	[SerializeField] private Sprite _rectSprite;
	[SerializeField] private Sprite _triangleSprite;

	private readonly Dictionary<int, GameObject> _displayedEntites = new();
	private IReadOnlyDictionary<string, Sprite> _spriteByPrimitiveShape;
	private IVideoCanvasViewModel _videoCanvasViewModel;

	public void Configure(IVideoCanvasViewModel videoCanvasViewModel) {
		_videoCanvasViewModel = videoCanvasViewModel;

		videoCanvasViewModel.OnCanvasUpdate.Subscribe(OnUpdateCanvas);
		PopulateSpriteDictionary();
	}

	private void PopulateSpriteDictionary() {
		_spriteByPrimitiveShape = new Dictionary<string, Sprite>() {
			{ "Circle", _circleSprite },
			{ "Triangle", _triangleSprite },
			{ "Rect", _rectSprite }
		};
	}

	private void OnUpdateCanvas(EntityDisplayInfo entityDisplayInfo) {
		UpdateEntityDisplay(entityDisplayInfo);
	}

	private void UpdateEntityDisplay(EntityDisplayInfo info) {
		// entity not registered, create it.
		if (!_displayedEntites.ContainsKey(info.EntityId)) {
			DisplayNewEntity(info.EntityId, info.EntityName);
		}

		// entity already registered, update it.
		UpdateEntity(info.EntityId);
	}

	private void DisplayNewEntity(int entityId, string entityName) {
		string entity = $"Entity {entityId}: \"{entityName}\"";
		var displayedEntity = Instantiate(_entityPrefab, parent: transform);

		displayedEntity.transform.name = entity;
		_displayedEntites.Add(entityId, displayedEntity);

		Debug.Log("Creada entidad: " + entity);
	}

	private void UpdateEntity(int entityId) {
		if (!_displayedEntites.TryGetValue(entityId, out var displayedEntity)) {
			return;
		}

		if (_videoCanvasViewModel.EntityHasTransform(entityId, out var transform)) {
			var entityRect = displayedEntity.GetComponent<RectTransform>();

			entityRect.anchoredPosition = new Vector2(transform.pos.x, transform.pos.y);
			entityRect.localScale = new Vector2(transform.scale.w, transform.scale.h);
			entityRect.rotation = Quaternion.AngleAxis(transform.rollAngleDegrees, Vector3.forward);
		}

		if (_videoCanvasViewModel.EntityHasShape(entityId, out var shape)) {
			if (!displayedEntity.TryGetComponent<Image>(out var image)) {
				image = displayedEntity.AddComponent<Image>();
			}

			image.color = new Color(shape.color.r, shape.color.g, shape.color.b, shape.color.a);
			image.sprite = _spriteByPrimitiveShape[shape.primitiveShape];
		}

	}

}