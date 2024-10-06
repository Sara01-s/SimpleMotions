using System.Collections.Generic;
using UnityEngine;

namespace SimpleMotions {
    
    public sealed class VideoCanvasView : MonoBehaviour {

		[Header("Primitive Sprites")]
		[SerializeField] private Sprite _circleSprite;
		[SerializeField] private Sprite _rectSprite;
		[SerializeField] private Sprite _triangleSprite;

		private readonly Dictionary<Entity, GameObject> _activeEntities = new();

        public void Configure(IVideoCanvasViewModel videoCanvasViewModel) {
			videoCanvasViewModel.UpdateCanvas.Subscribe(OnUpdateCanvas);
        }

        private void OnUpdateCanvas(EntityDisplayInfo entityDisplayInfo) {
			UpdateEntity(entityDisplayInfo);
        }

		private void UpdateEntity(EntityDisplayInfo entityDisplayInfo) {
			var entity = entityDisplayInfo.Entity;
			var components = entityDisplayInfo.Components;

			if (!_activeEntities.ContainsKey(entity)) {
				CreateNewEntity(entity, components);
				return;
			}

			if (_activeEntities.TryGetValue(entity, out var _)) {
				print("YA ESTABA REGISTRADA XD");
			}
		}

		private void CreateNewEntity(Entity entity, Component[] components) {
			string entityName = $"Entity {entity.Id}: \"{entity.Name}\"";
			var entityGameObject = new GameObject(entityName);

			entityGameObject.transform.SetParent(transform);

			foreach (var component in components) {
				if (component is SimpleMotions.Transform) {
					var entityTransform = component as SimpleMotions.Transform;
					var entityPosition = new Vector2(entityTransform.Position.X, entityTransform.Position.Y);
					var entityScale = new Vector2(entityTransform.Scale.Width, entityTransform.Scale.Height);
					var entityRoll = new Vector3(0.0f, 0.0f, entityTransform.Roll.Angle);

					entityGameObject.transform.position = entityPosition;
					entityGameObject.transform.localScale = entityScale;
					entityGameObject.transform.eulerAngles = entityRoll;
				}
				else if (component is SimpleMotions.Shape) {
					var entityShape = component as Shape;
					var entityColor = new UnityEngine.Color(entityShape.Color.R, entityShape.Color.G, entityShape.Color.B, entityShape.Color.A);
					var entityRenderer = entityGameObject.AddComponent<SpriteRenderer>();

					entityRenderer.color = entityColor;

					switch (entityShape.PrimitiveShape) {
						case Shape.Primitive.Triangle:
							entityRenderer.sprite = _triangleSprite;
							break;
						case Shape.Primitive.Circle:
							entityRenderer.sprite = _circleSprite;
							break;
						case Shape.Primitive.Rect:
							entityRenderer.sprite = _rectSprite;
							break;
						default:
							Debug.LogError("Something horrendous happened.");
							break;
					}
				}
			}

			_activeEntities.Add(entity, entityGameObject);
			Debug.Log("Creada entidad: " + entity.Name);
		}

	}
}