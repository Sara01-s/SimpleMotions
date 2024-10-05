using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace SimpleMotions {
    
    public sealed class VideoCanvasView : MonoBehaviour {

		[SerializeField] private GameObject _entityPrefab;

		private readonly Dictionary<Entity, GameObject> _activeEntities = new();

        public void Configure(IVideoCanvasViewModel videoCanvasViewModel) {
			videoCanvasViewModel.UpdateCanvas.Subscribe(OnUpdateCanvas);
        }

        private void OnUpdateCanvas(Entity entity) {
			UpdateEntity(entity);
        }

		private void UpdateEntity(Entity entity) {
			if (!_activeEntities.ContainsKey(entity)) {
				string entityName = $"Entity {entity.Id}: \"{entity.Name}\"";
				var entityGameObject = new GameObject(entityName);

				_activeEntities.Add(entity, entityGameObject);
				Debug.Log("Creada entidad: " + entity.Name);
				
				return;
			}

			if (_activeEntities.TryGetValue(entity, out var entityGO)) {
				entityGO.name = "YA ESTABA REGISTRADA XD";
			}
		}

    }
}