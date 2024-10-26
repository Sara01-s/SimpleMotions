using UnityEngine;
using System.IO;

namespace SimpleMotions {

    public sealed class Installer : MonoBehaviour {

		[Header("UI")]
		[SerializeField] private VideoPlaybackView _videoPlaybackView;
        [SerializeField] private VideoTimelineView _videoTimelineView;
		[SerializeField] private VideoCanvasView _videoCanvasView;

		[Header("Data")]
		[SerializeField] private string _projectName;
		[SerializeField] private string _projectDataFileName;
		[SerializeField] private string _editorDataFileName;

		[Header("Video Settings")]
		[SerializeField, Range(12, 1024)] private int _targetFrameRate = 60;

		private IEventService _eventService;

		private IVideoPlayback _videoPlayback;
		private IVideoTimeline _videoTimeline;
		private VideoCanvas _videoCanvas;
		private IVideoEntities _videoEntities;

		private IVideoAnimator _videoAnimator;
		private IVideoPlayer _videoPlayer;

		// APP DATA //
		private IKeyframeStorage _keyframeStorage;
		private IComponentStorage _componentStorage;
		private IEntityStorage _entityStorage;

		private ProjectDataHandler _projectDataHandler;
		private EditorDataHandler _editorDataHandler;
		private ProjectData _projectData;
		private EditorData _editorData;
		private VideoData _videoData;

        private void Start() {
			Application.targetFrameRate = _targetFrameRate;
			_eventService = new EventDispatcher();
			
			// DO NOT CHANGE ORDER OF EXECUTION.
			BuildStorage();
			BuildVideoEditor();
			BuildGUI();
        }

		private void BuildStorage() {
			ISerializer serializer = new NewtonJsonSerializer(); // Intercambiable por otros serializadores
			var projectDataSerializer = new DataSerializer<ProjectData>(serializer);
			var editorDataSerializer = new DataSerializer<EditorData>(serializer);

			string projectDataFilepath = Path.Combine(Application.persistentDataPath, _projectDataFileName);
			string editorDataFilepath = Path.Combine(Application.persistentDataPath, _editorDataFileName);

			Debug.Log("Data path: " + projectDataFilepath);

			_projectDataHandler = new ProjectDataHandler(projectDataSerializer, projectDataFilepath);
			_editorDataHandler = new EditorDataHandler(editorDataSerializer, editorDataFilepath);

			_projectData = _projectDataHandler.LoadData();
			_editorData = _editorDataHandler.LoadData();

			_projectData.ProjectName = _projectName;

			var componentsData = _projectData.Timeline.Components;
			var entitiesData   = _projectData.Timeline.Entities;
			var keyframeData   = _projectData.Timeline.Keyframes;
			
			_videoData = _projectData.Video;
			_videoData.TargetFrameRate = _targetFrameRate;
			
			_keyframeStorage  = new KeyframeStorage(keyframeData, _videoData);
            _componentStorage = new ComponentStorage(componentsData);
			_entityStorage 	  = new EntityStorage(entitiesData);
		}

		private void BuildVideoEditor() {
			_videoAnimator	= new VideoAnimator(_keyframeStorage, _componentStorage, _eventService, _entityStorage);
			_videoPlayer 	= new VideoPlayer(_videoData, _videoAnimator);
			_videoPlayback 	= new VideoPlayback(_videoPlayer);
			_videoTimeline 	= new VideoTimeline(_videoPlayer);
            _videoCanvas 	= new VideoCanvas(_componentStorage, _eventService);
			_videoEntities 	= new VideoEntities(_keyframeStorage, _componentStorage, _entityStorage, _videoCanvas);
		}

		private void BuildGUI() {
			var videoPlaybackViewModel = new VideoPlaybackViewModel(_videoPlayback);
			var videoTimelineViewModel = new VideoTimelineViewModel(_videoEntities);
			var videoCanvasViewModel = new VideoCanvasViewModel(_eventService);

			_videoPlaybackView.Configure(videoPlaybackViewModel);
            _videoTimelineView.Configure(videoTimelineViewModel);
			_videoCanvasView.Configure(videoCanvasViewModel);
		}

		private void Save() {
			_projectData.Timeline.Entities   = _entityStorage.GetEntitiesData();
			_projectData.Timeline.Components = _componentStorage.GetComponentsData();
			_projectData.Timeline.Keyframes  = _keyframeStorage.GetKeyframesData();
			_projectData.Video = _videoPlayer.GetVideoData();
			
			_projectData.Video.CurrentTime = 0;
			_projectData.Video.CurrentFrame = 0;
			_projectData.Video.IsPlaying = false;

			_projectDataHandler.SaveData(_projectData);
			_editorDataHandler.SaveData(_editorData);
		}

		private void OnDisable() {
			Save();
        }

    }
}