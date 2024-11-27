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
	[SerializeField] private int _sortingOrder = 1;

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

	private void OnUpdateCanvas(EntityDTO entityDTO) {
		UpdateEntityDisplay(entityDTO);
	}

	private void RemoveEntity(int entityId) {
		if (_displayedEntites.TryGetValue(entityId, out var entityDisplay)) {
			_displayedEntites.Remove(entityId);

			Destroy(entityDisplay);
			print($"Entidad {entityId} removida del canvas.");
		}
	}

	private void UpdateEntityDisplay(EntityDTO entityDTO) {
		// entity not registered, create it.
		if (!_displayedEntites.ContainsKey(entityDTO.Id)) {
			DisplayNewEntity(entityDTO.Id, entityDTO.Name);
		}

		// entity already registered, update it.
		UpdateEntityDisplay(entityDTO.Id);
	}

	private void DisplayNewEntity(int entityId, string entityName) {
		string entity = $"Entity {entityId}: \"{entityName}\"";
		var displayedEntity = new GameObject(entityName) {
			layer = LayerMask.NameToLayer("Entities")
		};

		displayedEntity.transform.SetParent(_canvasOrigin);
		displayedEntity.transform.localPosition = Vector2.zero;
		displayedEntity.transform.name = entity;
		displayedEntity.AddComponent<EntitySelector>().Configure(entityId, _entitySelectorViewModel);

		_displayedEntites.Add(entityId, displayedEntity);

		Debug.Log("Mostrando entidad: " + entity);
	}

	private void DisplayEntityImage(int entityId, string imageFilepath) {
		if (_videoCanvasViewModel.TryGetEntityShape(entityId, out var _)) {
			var displayedEntity = _displayedEntites[entityId];
			displayedEntity.GetComponent<SpriteRenderer>().enabled = false;

			if (displayedEntity.transform.childCount >= 2) {
				Debug.LogError("2 or more children inside entity, this is unwanted behaviour.");
				return;
			}

			if (displayedEntity.transform.childCount == 0) {
				var imageSlot = new GameObject (
					name: $"{displayedEntity.name} Image Slot",
					components: typeof(SpriteRenderer)
				);

				imageSlot.transform.SetParent(displayedEntity.transform);
				imageSlot.transform.localPosition = Vector2.zero;
			}

			displayedEntity.transform.GetChild(index: 0).gameObject.SetActive(true); // Make sure is active.

			var imageSlotRenderer = displayedEntity.GetComponentsInChildren<SpriteRenderer>()[1];
			imageSlotRenderer.sprite = FilePathToSprite(imageFilepath);

			var size = imageSlotRenderer.bounds.size;
			imageSlotRenderer.transform.SetParent(null);

			_videoCanvasViewModel.SetEntityScale(entityId, (size.x, size.y));
			imageSlotRenderer.transform.SetParent(displayedEntity.transform);
		}
	}

	private void UpdateEntityDisplay(int entityId) {
		if (!_displayedEntites.TryGetValue(entityId, out var displayedEntity)) {
			return;
		}

		if (_videoCanvasViewModel.TryGetEntityTransform(entityId, out var transformDTO)) {
			var entityTransform = displayedEntity.GetComponent<Transform>();

			entityTransform.localPosition = new Vector3(transformDTO.Position.x, transformDTO.Position.y, 0.0f);
			entityTransform.localScale = new Vector2(transformDTO.Scale.w, transformDTO.Scale.h);
			entityTransform.rotation = Quaternion.AngleAxis(transformDTO.RollDegrees, Vector3.forward);
		}

		if (_videoCanvasViewModel.TryGetEntityShape(entityId, out var shape)) {
			if (!displayedEntity.TryGetComponent<SpriteRenderer>(out var spriteRenderer)) {
				spriteRenderer = displayedEntity.AddComponent<SpriteRenderer>();
				spriteRenderer.sortingOrder = _sortingOrder;
			}

			spriteRenderer.color = new Color(shape.Color.r, shape.Color.g, shape.Color.b, shape.Color.a);
			
			// Primitive shape is NOT an image.
			if (shape.PrimitiveShape.CompareTo(ShapeTypeUI.Image.ToString()) != 0) {
				spriteRenderer.enabled = true;
				spriteRenderer.sprite = _spriteByPrimitiveShape[shape.PrimitiveShape];
				
				if (displayedEntity.transform.childCount >= 1) { // Entity had image slot.
					displayedEntity.transform.GetChild(index: 0).gameObject.SetActive(false);
				}
			}
		}
	}

	// TODO - Use color DTO?
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