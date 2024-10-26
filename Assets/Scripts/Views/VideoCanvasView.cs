using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace SimpleMotions {
    
    public sealed class VideoCanvasView : MonoBehaviour {

		[Header("Entity prefab")]
		[SerializeField] private GameObject _entityPrefab;

		[Header("Primitive Sprites")]
		[SerializeField] private Sprite _circleSprite;
		[SerializeField] private Sprite _rectSprite;
		[SerializeField] private Sprite _triangleSprite;

		private readonly Dictionary<int, GameObject> _activeEntities = new();

        public void Configure(IVideoCanvasViewModel videoCanvasViewModel) {
			videoCanvasViewModel.UpdateCanvas.Subscribe(OnUpdateCanvas);
        }

        private void OnUpdateCanvas(EntityDisplayInfo entityDisplayInfo) {
			UpdateEntityDisplay(entityDisplayInfo);
        }

		private void UpdateEntityDisplay(EntityDisplayInfo entityDisplayInfo) {
			var entity = entityDisplayInfo.Entity;
			var components = entityDisplayInfo.Components;

			// entity not registered, create it.
			if (!_activeEntities.ContainsKey(entity.Id)) {
				CreateNewEntity(entity);
			}

			// entity already registered, update it.
			UpdateEntity(entity, components);
		}

		private void CreateNewEntity(Entity entity) {
			string entityName = $"Entity {entity.Id}: \"{entity.Name}\"";
			var unityObject = Instantiate(_entityPrefab, parent: transform);

			unityObject.transform.name = entityName;
			_activeEntities.Add(entity.Id, unityObject);

			Debug.Log("Creada entidad: " + entity.Name);
		}

		private void UpdateEntity(Entity entity, Component[] components) {
			if (!_activeEntities.TryGetValue(entity.Id, out var unityObject)) {
				return;
			}

			var unityRectTransform = unityObject.GetComponent<RectTransform>();

			if (!unityObject.TryGetComponent<Image>(out var unityImage)) {
				unityImage = unityObject.AddComponent<Image>();
			}

			// TODO - cachear esto de alguna forma?
			foreach (var component in components) {
				switch (component) {
					case Transform transform:
						unityRectTransform.FromSM(transform);
						break;
					case Shape shape:
						unityImage.sprite = GetSpriteFromPrimitive(shape.PrimitiveShape);
						unityImage.color = SmToUnity.GetColorFrom(shape.Color);
						break;
				}
			}
		}

		private Sprite GetSpriteFromPrimitive(Shape.Primitive primitiveType) {
			return primitiveType switch {
				Shape.Primitive.Rect => _rectSprite,
				Shape.Primitive.Circle => _circleSprite,
				Shape.Primitive.Triangle => _triangleSprite,
				_ => throw new System.ArgumentException(primitiveType.ToString())
			};
		}

	}
}