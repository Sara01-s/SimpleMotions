using UnityEngine;

namespace SimpleMotions {

    internal sealed class Installer : MonoBehaviour {

        [SerializeField] private VideoTimelineView _videoTimelineView;
		[SerializeField] private VideoCanvasView _videoCanvasView;

		private IEventService _eventService;

		private IComponentStorage _componentStorage;
		private IEntityStorage _entityStorage;
		private IVideoDatabase _videoDatabase;

		private IVideoTimeline _videoTimeline;
		private VideoPlayer _videoPlayer; // TODO - Hacer interfaz.
		private VideoCanvas _videoCanvas;
		private IVideoEntities _videoEntities;

        private void Start() {
			_eventService = new EventDispatcher();
			
			BuildStorage();
			BuildVideoEditor();
			BuildGUI();
        }

		private void BuildStorage() {
            _componentStorage = new ComponentStorage();
			_entityStorage = new EntityStorage();
            _videoDatabase = new VideoDatabase();
		}

		private void BuildVideoEditor() {
			_videoTimeline = new VideoTimeline(_videoDatabase);
            _videoPlayer = new VideoPlayer(_componentStorage);
            _videoCanvas = new VideoCanvas(_entityStorage, _eventService);
			_videoEntities = new VideoEntities(_componentStorage, _entityStorage, _videoCanvas);
		}

		private void BuildGUI() {
			var videoTimelineViewModel = new VideoTimelineViewModel(_videoTimeline, _videoEntities);
			var videoCanvasViewModel = new VideoCanvasViewModel(_eventService);

            _videoTimelineView.Configure(videoTimelineViewModel);
			_videoCanvasView.Configure(videoCanvasViewModel);
		}

		private void OnDisable() {
            Services.Instance.Clear();
        }

    }
}