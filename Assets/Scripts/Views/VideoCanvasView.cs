using System.Collections.Generic;
using UnityEngine.UI;
using SimpleMotions;
using UnityEngine;

public sealed class VideoCanvasView : MonoBehaviour {

	[Header("Primitive Sprites")]
	[SerializeField] private Sprite _circleSprite;
	[SerializeField] private Sprite _rectSprite;
	[SerializeField] private Sprite _arrowSprite;
	[SerializeField] private Sprite _triangleSprite;
	[SerializeField] private Sprite _starSprite;
	[SerializeField] private Sprite _heartSprite;
	[SerializeField] private Sprite _imageSprite;

	[Header("Render")]
	[SerializeField] private LayerMask _entityLayer;
	[SerializeField] private Transform _canvasOrigin;
	[SerializeField] private Image _background;

	[Header("Fullscreen")]
	[SerializeField] private string _midLayerName = "UI - Mid";

	private readonly Dictionary<int, GameObject> _displayedEntites = new();
	private IReadOnlyDictionary<string, Sprite> _spriteByPrimitiveShape;
	private IVideoCanvasViewModel _videoCanvasViewModel;
	private IEntitySelectorViewModel _entitySelectorViewModel;

	public void Configure(IVideoCanvasViewModel videoCanvasViewModel, IEntitySelectorViewModel entitySelectorViewModel) {
		videoCanvasViewModel.OnCanvasUpdate.Subscribe(OnUpdateCanvas);
		videoCanvasViewModel.OnEntityRemoved.Subscribe(RemoveEntity);
		videoCanvasViewModel.BackgroundColor.Subscribe(UpdateCanvasColor);
		videoCanvasViewModel.OnDisplayEntityImage.Subscribe(DisplayEntityImage);

		_videoCanvasViewModel = videoCanvasViewModel;
		_entitySelectorViewModel = entitySelectorViewModel;
		PopulateSpriteDictionary();
	}

	private void PopulateSpriteDictionary() {
		_spriteByPrimitiveShape = new Dictionary<string, Sprite>() {
			{ ShapeTypeUI.Circle.ToString()	  , _circleSprite 	},
			{ ShapeTypeUI.Rect.ToString()	  , _rectSprite 	},
			{ ShapeTypeUI.Triangle.ToString() , _triangleSprite },
			{ ShapeTypeUI.Arrow.ToString()	  , _arrowSprite 	},
			{ ShapeTypeUI.Star.ToString()	  , _starSprite 	},
			{ ShapeTypeUI.Heart.ToString()	  , _heartSprite 	},
			{ ShapeTypeUI.Image.ToString()	  , _imageSprite 	},
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
		UpdateEntityDisplay(entity.id);
	}

	private void DisplayNewEntity(int entityId, string entityName) {
		string entity = $"Entity {entityId}: \"{entityName}\"";
		var displayedEntity = new GameObject(entityName) {
			layer = LayerMask.NameToLayer("Entities")
		};

		displayedEntity.transform.SetParent(_canvasOrigin);
		displayedEntity.transform.localPosition = Vector2.zero;
		displayedEntity.transform.name = entity;
		displayedEntity.AddComponent<Selectable>().Configure(entityId, _entitySelectorViewModel);

		_displayedEntites.Add(entityId, displayedEntity);

		Debug.Log("Mostrando entidad: " + entity);
	}

	private void DisplayEntityImage(int entityId, string imageFilepath) {
		if (_videoCanvasViewModel.EntityHasShape(entityId, out var shape)) {
			var displayedEntity = _displayedEntites[entityId];

			if (!displayedEntity.TryGetComponent<SpriteRenderer>(out var spriteRenderer)) {
				spriteRenderer = displayedEntity.AddComponent<SpriteRenderer>();
				spriteRenderer.sortingLayerName = _midLayerName;
			}

			spriteRenderer.sprite = FilePathToSprite(imageFilepath);
		}
	}

	private void UpdateEntityDisplay(int entityId) {
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
				spriteRenderer.sortingLayerName = _midLayerName;
			}

			spriteRenderer.color = new Color(shape.color.r, shape.color.g, shape.color.b, shape.color.a);
			
			if (shape.primitiveShape.CompareTo(ShapeTypeUI.Image.ToString()) != 0) {
				spriteRenderer.sprite = _spriteByPrimitiveShape[shape.primitiveShape];
			}
		}
	}

	private void UpdateCanvasColor((float r, float g, float b, float a) color) {
		_background.color = new Color(color.r, color.g, color.b, color.a);
	}

	public static Sprite FilePathToSprite(string filePath) {
		if (!System.IO.File.Exists(filePath)) {
			Debug.LogError($"El archivo no existe: {filePath}");
			return null;
		}

		try {
			byte[] fileData = System.IO.File.ReadAllBytes(filePath);
			var texture = new Texture2D(2, 2);

			if (texture.LoadImage(fileData)) {
				return Sprite.Create(texture, 
									new Rect(0, 0, texture.width, texture.height), 
									new Vector2(0.5f, 0.5f));
			}
			else {
				Debug.LogError("No se pudo cargar la imagen en la textura.");
				return null;
			}
		}
		catch (System.Exception ex) {
			Debug.LogError($"Error al convertir la imagen a Sprite: {ex.Message}");
			return null;
		}
	}

}