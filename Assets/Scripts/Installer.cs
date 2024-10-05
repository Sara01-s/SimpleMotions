using UnityEngine;
using System.IO;

namespace SimpleMotions {

    public sealed class Installer : MonoBehaviour {

		[Header("UI")]
        [SerializeField] private VideoTimelineView _videoTimelineView;
		[SerializeField] private VideoCanvasView _videoCanvasView;

		[Header("Data")]
		[SerializeField] private string _projectName;
		[SerializeField] private string _projectDataFileName;
		[SerializeField] private string _editorDataFileName;

		private IEventService _eventService;

		private IComponentStorage _componentStorage;
		private IEntityStorage _entityStorage;
		private IVideoDatabase _videoDatabase;

		private IVideoTimeline _videoTimeline;
		private VideoPlayer _videoPlayer; // TODO - Hacer interfaz.
		private VideoCanvas _videoCanvas;
		private IVideoEntities _videoEntities;

		// APP DATA //
		private ProjectDataHandler _projectDataHandler;
		private EditorDataHandler _editorDataHandler;
		private ProjectData _projectData;
		private EditorData _editorData;

        private void Start() {
			_eventService = new EventDispatcher();
			
			BuildStorage();
			BuildVideoEditor();
			BuildGUI();
        }

		private void BuildStorage() {
			ISerializer serializer = new JsonSerializer(prettyPrint: true); // Intercambiable por otros serializadores
			var projectDataSerializer = new DataSerializer<ProjectData>(serializer);
			var editorDataSerializer = new DataSerializer<EditorData>(serializer);

			string projectDataFilepath = Path.Combine(Application.persistentDataPath, _projectDataFileName);
			string editorDataFilepath = Path.Combine(Application.persistentDataPath, _editorDataFileName);

			Debug.Log("original: " + projectDataFilepath);

			_projectDataHandler = new ProjectDataHandler(projectDataSerializer, projectDataFilepath);
			_editorDataHandler = new EditorDataHandler(editorDataSerializer, editorDataFilepath);

			_projectData = _projectDataHandler.LoadData();
			_editorData = _editorDataHandler.LoadData();

			print(_projectData.ProjectName);
			_projectData.ProjectName = _projectName;

			var componentsData = _projectData.Timeline.Components;
			var entitiesData = _projectData.Timeline.Entities;
			var videoData = new VideoData();

            _componentStorage = new ComponentStorage(componentsData);
			_entityStorage = new EntityStorage(entitiesData);
            //_videoDatabase = new VideoData();
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

		private void Save() {
			_projectData.Timeline.Entities = _entityStorage.GetEntitiesData();
			_projectData.Timeline.Components = _componentStorage.GetComponentsData();

			_projectDataHandler.SaveData(_projectData);
			_editorDataHandler.SaveData(_editorData);
		}

		private void OnDisable() {
			Save();
        }

    }
}