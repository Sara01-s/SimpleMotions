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
			UpdateEntity(entityDisplayInfo);
        }

		private void UpdateEntity(EntityDisplayInfo entityDisplayInfo) {
			var entity = entityDisplayInfo.Entity;
			var components = entityDisplayInfo.Components;

			// entity not registered, create it.
			if (!_activeEntities.ContainsKey(entity.Id)) {
				CreateNewEntity(entity, components);
				return;
			}

			// entity already registered, update it.
			UpdateEntity(entity, components);
		}

		private void UpdateEntity(Entity entity, Component[] components) {
			if (_activeEntities.TryGetValue(entity.Id, out var entityUnity)) {
				var entityUnityTransform = entityUnity.GetComponent<RectTransform>();

				foreach (var component in components) {
					if (component is Transform transform) {
						var entitySimplePosition = new Vector2(transform.Position.X, transform.Position.Y);
						entityUnityTransform.anchoredPosition = entitySimplePosition;
					}
					else if (component is Shape shape) {
						var entityUnityRenderer = entityUnity.GetComponent<Image>();
						var entityUnityColor = new UnityEngine.Color(shape.Color.R, shape.Color.G, shape.Color.B, shape.Color.A);
						entityUnityRenderer.color = entityUnityColor;
					}
				}
			}

		}

		private void CreateNewEntity(Entity entity, Component[] components) {
			string entityName = $"Entity {entity.Id}: \"{entity.Name}\"";
			var entityUnity = Instantiate(_entityPrefab, parent: transform);

			entityUnity.transform.name = entityName;

			// Add components
			foreach (var component in components) {
				if (component is SimpleMotions.Transform transform) {
					var entitySimplePosition = new Vector2(transform.Position.X, transform.Position.Y);
					var entitySimpleScale	 = new Vector2(transform.Scale.Width, transform.Scale.Height);
					var entitySimpleRoll 	 = new Vector3(0.0f, 0.0f, transform.Roll.Angle);

					var entityUnityTransform 				= entityUnity.GetComponent<RectTransform>();
					entityUnityTransform.anchoredPosition 	= entitySimplePosition;
					entityUnityTransform.localScale 		= entitySimpleScale;
					entityUnityTransform.eulerAngles 		= entitySimpleRoll;
				}
				else if (component is SimpleMotions.Shape shape) {
					var entitySimpleColor = new UnityEngine.Color(shape.Color.R, shape.Color.G, shape.Color.B, shape.Color.A);
					var entitySimpleRenderer = entityUnity.AddComponent<Image>();

					entitySimpleRenderer.color = entitySimpleColor;

					switch (shape.PrimitiveShape) {
						case Shape.Primitive.Triangle:
							entitySimpleRenderer.sprite = _triangleSprite;
							break;
						case Shape.Primitive.Circle:
							entitySimpleRenderer.sprite = _circleSprite;
							break;
						case Shape.Primitive.Rect:
							entitySimpleRenderer.sprite = _rectSprite;
							break;
						default:
							Debug.LogError("Something horrendous happened.");
							break;
					}
				}
			}

			_activeEntities.Add(entity.Id, entityUnity);
			Debug.Log("Creada entidad: " + entity.Name);
		}

	}
}