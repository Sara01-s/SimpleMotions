using UnityEngine;
using System.IO;
using SimpleMotions.Internal;

namespace SimpleMotions {

    public sealed class Installer : MonoBehaviour {

		[Header("UI")]
		[SerializeField] private EditorPainter _editorPainter;
		[SerializeField] private VideoPlaybackView _videoPlaybackView;
        [SerializeField] private VideoTimelineView _videoTimelineView;
		[SerializeField] private TimelinePanelView _timelinePanelView;
		[SerializeField] private VideoCanvasView _videoCanvasView;
		[SerializeField] private InspectorView _inspectorView;
		[SerializeField] private EntitySelectorView _entitySelectorView;

		[SerializeField] private TransformComponentView _transformComponentView;
		[SerializeField] private ShapeComponentView _shapeComponentView;
		[SerializeField] private TextComponentView _textComponentView;

		[Header("Gizmos")]
		[SerializeField] private SelectionGizmoBody _selectionGizmoBody;

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
		private IVideoPlayer _videoPlayer;

		private IEntitySelector _entitySelector;

		// APP DATA //
		private IKeyframeStorage _keyframeStorage;
		private IComponentStorage _componentStorage;
		private IEntityStorage _entityStorage;

		private ProjectDataHandler _projectDataHandler;
		private EditorDataHandler _editorDataHandler;
		private ProjectData _projectData;
		private EditorData _editorData;

		private IServices _services;


        private void Start() {
			Application.targetFrameRate = _targetFrameRate;
			_editorPainterParser = new EditorPainterParser();

			_services = new Services();
			_services.RegisterService<ISerializer, NewtonJsonSerializer>();

			// Data
			_services.RegisterService<VideoData, VideoData>();
			_services.RegisterService<ComponentsData, ComponentsData>();
			_services.RegisterService<EntitiesData, EntitiesData>();
			_services.RegisterService<IKeyframeStorage, KeyframeStorage>();
			_services.RegisterService<IComponentStorage, ComponentStorage>();
			_services.RegisterService<IEntityStorage, EntityStorage>();
			_services.RegisterService<IVideoPlayerData, VideoPlayer>();

			// Models
			_services.RegisterService<IEntitySelector, EntitySelector>();
			_services.RegisterService<IVideoAnimator, VideoAnimator>();
			_services.RegisterService<IVideoPlayer, VideoPlayer>();
			_services.RegisterService<IVideoTimeline, VideoTimeline>();
			_services.RegisterService<IVideoPlayback, VideoPlayback>();
			_services.RegisterService<IVideoCanvas, VideoCanvas>();
			_services.RegisterService<IVideoEntities, VideoEntities>();

			// ViewModels
			_services.RegisterService<IVideoPlaybackViewModel, VideoPlaybackViewModel>();
			_services.RegisterService<IVideoTimelineViewModel, VideoTimelineViewModel>();
			_services.RegisterService<ITimelinePanelViewModel, TimelinePanelViewModel>();
			_services.RegisterService<IVideoCanvasViewModel, VideoCanvasViewModel>();
			_services.RegisterService<IInspectorViewModel, InspectorViewModel>();
			_services.RegisterService<IEntitySelectorViewModel, EntitySelectorViewModel>();
			_services.RegisterService<ITransformComponentViewModel, TransformComponentViewModel>();
			_services.RegisterService<IShapeComponentViewModel, ShapeComponentViewModel>();
			_services.RegisterService<ITextComponentViewModel, TextComponentViewModel>();
			
			// DO NOT CHANGE ORDER OF EXECUTION.
			BuildStorage();
			BuildVideoEditor();
			BuildGUI();
        }

		private void BuildStorage() {
			var serializer = _services.GetService<ISerializer>();
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
			_projectData.Video = new VideoData() {
				TargetFrameRate = _targetFrameRate,
				TotalFrames = 300, // TODO - El 300 debería llegar de otro lado.
			};

			_services.RegisterInstance(_projectData.Timeline.Components);
			_services.RegisterInstance(_projectData.Timeline.Entities);
			_services.RegisterInstance(_projectData.Timeline.Keyframes);
			_services.RegisterInstance(_projectData.Video);
			
			_keyframeStorage  = _services.GetService<IKeyframeStorage>();
            _componentStorage = _services.GetService<IComponentStorage>();
			_entityStorage 	  = _services.GetService<IEntityStorage>();
		}

		private void BuildVideoEditor() {
			_entitySelector = _services.GetService<IEntitySelector>();
			_videoAnimator  = _services.GetService<IVideoAnimator>();
			_videoPlayer 	= _services.GetService<IVideoPlayer>();
			_videoTimeline 	= _services.GetService<IVideoTimeline>();
			_videoPlayback 	= _services.GetService<IVideoPlayback>();
            _videoCanvas 	= _services.GetService<IVideoCanvas>();
			_videoEntities 	= _services.GetService<IVideoEntities>();
		}

		private void BuildGUI() {
			var videoPlaybackViewModel  = _services.GetService<IVideoPlaybackViewModel>();
			var videoTimelineViewModel  = _services.GetService<IVideoTimelineViewModel>();
			var timelinePanelViewModel  = _services.GetService<ITimelinePanelViewModel>();
			var videoCanvasViewModel    = _services.GetService<IVideoCanvasViewModel>();
			var inspectorViewModel      = _services.GetService<IInspectorViewModel>();
			var entitySelectorViewModel = _services.GetService<IEntitySelectorViewModel>();

			_videoPlaybackView.Configure(videoPlaybackViewModel);
            _videoTimelineView.Configure(videoTimelineViewModel);
			_timelinePanelView.Configure(timelinePanelViewModel);
			_videoCanvasView.Configure(videoCanvasViewModel);
			_inspectorView.Configure(inspectorViewModel, _editorPainter);
			_entitySelectorView.Configure(entitySelectorViewModel);

			var transformComponentViewModel = _services.GetService<ITransformComponentViewModel>();
			var shapeComponentViewModel = _services.GetService<IShapeComponentViewModel>();
			var textComponentViewModel = _services.GetService<ITextComponentViewModel>();

			_transformComponentView.Configure(transformComponentViewModel);
			_shapeComponentView.Configure(shapeComponentViewModel);
			_textComponentView.Configure(textComponentViewModel);

			_selectionGizmoBody.Configure(videoCanvasViewModel, _entitySelector);

			var editorThemeUnity = _editorPainterParser.SmEditorThemeToUnity(_editorData.Theme);
			_editorPainter.ApplyThemeIfNotEmpty(editorThemeUnity, checkForNewUI: true);
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