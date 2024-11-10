using UnityEngine;
using System.IO;
using SimpleMotions.Internal;

namespace SimpleMotions {

    public sealed class Installer : MonoBehaviour {

		[Header("UI")]
		[SerializeField] private EditorPainter _editorPainter;
		[SerializeField] private VideoPlaybackView _videoPlaybackView;
        [SerializeField] private VideoTimelineView _videoTimelineView;
		[SerializeField] private VideoCanvasView _videoCanvasView;
		[SerializeField] private InspectorView _inspectorView;

		[Header("Data")]
		[SerializeField] private string _projectName;
		[SerializeField] private string _projectDataFileName;
		[SerializeField] private string _editorDataFileName;

		[Header("Video Settings")]
		[SerializeField, Range(12, 1024)] private int _targetFrameRate = 60;

		private IEditorPainterParser _editorPainterParser;

		private IVideoPlayback _videoPlayback;
		private IVideoTimeline _videoTimeline;
		private IVideoCanvas _videoCanvas;
		private IVideoEntities _videoEntities;
		private IVideoAnimator _videoAnimator;
		private VideoPlayer _videoPlayer;

		private IEntitySelector _entitySelector;

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
			_editorPainterParser = new EditorPainterParser();
			
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
			_projectData.Video = new VideoData(_targetFrameRate, 300); // TODO - XDDDDD

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
			_videoAnimator	= new VideoAnimator(_keyframeStorage, _componentStorage, _entityStorage);
			_videoPlayer 	= new VideoPlayer(_videoData, _videoAnimator);
			_videoTimeline 	= new VideoTimeline(_projectData.Video, _videoPlayer);
			_videoPlayback 	= new VideoPlayback(_videoPlayer);
            _videoCanvas 	= new VideoCanvas(_componentStorage);
			_videoEntities 	= new VideoEntities(_keyframeStorage, _componentStorage, _entityStorage, _videoCanvas);

			_entitySelector = new EntitySelector(_entityStorage.GetEntitiesData());
		}

		private void BuildGUI() {
			var videoPlaybackViewModel = new VideoPlaybackViewModel(_videoPlayback, _videoPlayer);
			var videoTimelineViewModel = new VideoTimelineViewModel(_videoTimeline, _videoEntities, _videoPlayer);
			var videoCanvasViewModel = new VideoCanvasViewModel(_videoCanvas, _videoAnimator);
			var inspectorViewModel = new InspectorViewModel(_videoCanvas, _videoAnimator);

			_videoPlaybackView.Configure(videoPlaybackViewModel);
            _videoTimelineView.Configure(videoTimelineViewModel);
			_videoCanvasView.Configure(videoCanvasViewModel);
			_inspectorView.Configure(inspectorViewModel, _editorPainter);

			_editorPainter.ApplyThemeIfNotEmpty(_editorPainterParser.SmEditorThemeToUnity(_editorData.Theme));
		}

		private void Save() {
			_projectData.Timeline.Entities   = _entityStorage.GetEntitiesData();
			_projectData.Timeline.Components = _componentStorage.GetComponentsData();
			_projectData.Timeline.Keyframes  = _keyframeStorage.GetKeyframesData();
			_projectData.Video = _videoPlayer.GetVideoData();
			
			_projectData.Video.CurrentTime = 0;
			_projectData.Video.CurrentFrame = 0;
			_projectData.Video.IsPlaying = false;

			_editorData.Theme = _editorPainterParser.EditorThemeUnityToSm(_editorPainter.Theme);

			_projectDataHandler.SaveData(_projectData);
			_editorDataHandler.SaveData(_editorData);
		}

		private void OnDisable() {
			Save();
        }

    }
}