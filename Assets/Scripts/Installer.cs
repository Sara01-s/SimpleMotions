using UnityEngine;
using UnityEditor;
using System.IO;
using SimpleMotions.Internal;
using TMPro;

namespace SimpleMotions {

    public sealed class Installer : MonoBehaviour {

		[Header("UI")]
		[SerializeField] private EditorPainter _editorPainter;
		[SerializeField] private VideoPlaybackView _videoPlaybackView;
        [SerializeField] private TimelineView _videoTimelineView;
		[SerializeField] private TimelinePanelView _timelinePanelView;
		[SerializeField] private TimelineHeaderView _timelineHeaderView;
		[SerializeField] private TimelineCursorView _timelineCursorView;
		[SerializeField] private VideoCanvasView _videoCanvasView;
		[SerializeField] private InspectorView _inspectorView;
		[SerializeField] private EntitySelectorView _entitySelectorView;
		[SerializeField] private ConfirmationPanel _confirmationPanel;

		[Header("Video Editor")]
		[SerializeField] private TransformComponentView _transformComponentView;
		[SerializeField] private TextComponentView _textComponentView;
		[SerializeField] private ShapeComponentView _shapeComponentView;
		[SerializeField] private VideoSettingsView _videoSettingsView;
		[SerializeField] private EditorSettingsView _editorSettingsView;
		[SerializeField] private ExportSettingsView _exportSettingsView;
		[SerializeField] private ColorPickerView _colorPickerView;
		[SerializeField] private FullscreenView _fullscreenView;
		[SerializeField] private FullscreenPlaybackView _fullscreenPlaybackView;
		[SerializeField] private ExportView _exportView;
		[SerializeField] private EntityDeselector _entityDeselector;
		[SerializeField] private CameraSelector _cameraSelector;
		[SerializeField] private EditKeyframeView _editKeyframeView;

		[Header("Data")]
		[SerializeField] private string _projectName;
		[SerializeField] private string _projectDataFileName;
		[SerializeField] private string _editorDataFileName;

		[Header("Video Settings")]
		[SerializeField, Range(12, 1024)] private int _targetFrameRate = 60;

		[Header("Versions")]
		[SerializeField] private TextMeshProUGUI[] _textVersions;

		private IEditorPainterParser _editorPainterParser;

		private IVideoPlayback _videoPlayback;
		private IVideoCanvas _videoCanvas;
		private IVideoEntities _videoEntities;
		private IVideoAnimator _videoAnimator;
		private IVideoPlayer _videoPlayer;

		private IEntitySelectorViewModel _entitySelector;

		// APP DATA //
		private IKeyframeStorage _keyframeStorage;
		private IComponentStorage _componentStorage;
		private IEntityStorage _entityStorage;

		private ProjectDataHandler _projectDataHandler;
		private EditorDataHandler _editorDataHandler;
		private ProjectData _projectData;
		private EditorData _editorData;

		private IServices _services;
		private IInputValidator _inputValidator;

        private void Start() {
			Application.targetFrameRate = _targetFrameRate;

			_services = new Services();
			_inputValidator = new InputValidator();
			_services.RegisterService<ISerializer, NewtonJsonSerializer>();
			_services.RegisterService<IEditorPainterParser, EditorPainterParser>();

			// Data
			_services.RegisterService<IInterpolator,	 Interpolator>();
			_services.RegisterService<VideoData, 		 VideoData>();
			_services.RegisterService<ComponentsData, 	 ComponentsData>();
			_services.RegisterService<EntitiesData, 	 EntitiesData>();
			_services.RegisterService<IKeyframeStorage,  KeyframeStorage>();
			_services.RegisterService<IComponentStorage, ComponentStorage>();
			_services.RegisterService<IEntityStorage, 	 EntityStorage>();
			_services.RegisterService<IVideoPlayerData,  VideoPlayer>();

			// Models
			_services.RegisterService<IEntitySelector, 	 EntitySelector>();
			_services.RegisterService<IVideoAnimator, 	 VideoAnimator>();
			_services.RegisterService<IVideoPlayer, 	 VideoPlayer>();
			_services.RegisterService<IVideoPlayback, 	 VideoPlayback>();
			_services.RegisterService<IVideoCanvas, 	 VideoCanvas>();
			_services.RegisterService<IVideoEntities, 	 VideoEntities>();
			_services.RegisterService<IExportModel, 	 ExportModel>();

			// ViewModels
			_services.RegisterService<IFullscreenViewModel, 		FullscreenViewModel>();
			_services.RegisterService<IVideoPlaybackViewModel, 		VideoPlaybackViewModel>();
			_services.RegisterService<IVideoTimelineViewModel, 		VideoTimelineViewModel>();
			_services.RegisterService<ITimelinePanelViewModel, 		TimelinePanelViewModel>();
			_services.RegisterService<IVideoCanvasViewModel, 		VideoCanvasViewModel>();
			_services.RegisterService<IInspectorViewModel, 			InspectorViewModel>();
			_services.RegisterService<IEntitySelectorViewModel, 	EntitySelectorViewModel>();
			_services.RegisterService<ITransformComponentViewModel, TransformComponentViewModel>();
			_services.RegisterService<IShapeComponentViewModel, 	ShapeComponentViewModel>();
			_services.RegisterService<ITextComponentViewModel, 		TextComponentViewModel>();
			_services.RegisterService<IInputValidator, 				InputValidator>();
			_services.RegisterService<IEntityViewModel,				EntityViewModel>();
			_services.RegisterService<IVideoSettingsViewModel, 		VideoSettingsViewModel>();
			_services.RegisterService<IExportSettingsViewModel, 	ExportSettingsViewModel>();
			_services.RegisterService<IExportViewModel, 			ExportViewModel>();
			_services.RegisterService<ICameraSelectorViewModel, 	CameraSelectorViewModel>();
			_services.RegisterService<IEditKeyframeViewModel,		EditKeyframeViewModel>();

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
			_entitySelector = _services.GetService<IEntitySelectorViewModel>();
			_videoAnimator  = _services.GetService<IVideoAnimator>();
			_videoPlayer 	= _services.GetService<IVideoPlayer>();
			_videoPlayback 	= _services.GetService<IVideoPlayback>();
            _videoCanvas 	= _services.GetService<IVideoCanvas>();
			_videoEntities 	= _services.GetService<IVideoEntities>();
		}

		private void BuildGUI() {
			var videoTimelineViewModel = _services.GetService<IVideoTimelineViewModel>();
			var videoCanvasViewModel = _services.GetService<IVideoCanvasViewModel>();
			var entitySelectorViewModel = _services.GetService<IEntitySelectorViewModel>();
			var videoPlaybackViewModel = _services.GetService<IVideoPlaybackViewModel>();
			var editorPainterParser = _services.GetService<IEditorPainterParser>();
			
			_editorPainterParser = editorPainterParser;
			_fullscreenView.Configure(_services.GetService<IFullscreenViewModel>());
			_timelinePanelView.Configure(videoCanvasViewModel, _services.GetService<ITimelinePanelViewModel>(), entitySelectorViewModel);
			_videoTimelineView.Configure(videoTimelineViewModel, entitySelectorViewModel, _services.GetService<IEditKeyframeViewModel>());
			_timelineCursorView.Configure(videoTimelineViewModel);
			_timelineHeaderView.Configure(videoTimelineViewModel);
			_videoPlaybackView.Configure(videoPlaybackViewModel, _inputValidator);
			_videoCanvasView.Configure(videoCanvasViewModel, entitySelectorViewModel);
			_inspectorView.Configure(_services.GetService<IInspectorViewModel>());
			_entitySelectorView.Configure(entitySelectorViewModel, videoCanvasViewModel, _services.GetService<IFullscreenViewModel>(), videoPlaybackViewModel, _services.GetService<IExportViewModel>());
			_transformComponentView.Configure(_services.GetService<ITransformComponentViewModel>(), _inputValidator);
			_shapeComponentView.Configure(_services.GetService<IShapeComponentViewModel>(), editorPainterParser);
			_textComponentView.Configure(_services.GetService<ITextComponentViewModel>());
			_videoSettingsView.Configure(_services.GetService<IVideoSettingsViewModel>(), videoCanvasViewModel);
			_editorSettingsView.Configure();
			_exportSettingsView.Configure(_services.GetService<IExportSettingsViewModel>(), _inputValidator);
			_fullscreenPlaybackView.Configure();
			_exportView.Configure(_services.GetService<IExportViewModel>());
			_entityDeselector.Configure(entitySelectorViewModel);
			_colorPickerView.Configure();
			_cameraSelector.Configure(_services.GetService<ICameraSelectorViewModel>());
			_editKeyframeView.Configure(_services.GetService<IEditKeyframeViewModel>(), videoTimelineViewModel);

			var editorThemeUnity = editorPainterParser.SmEditorThemeToUnity(_editorData.Theme);
			_editorPainter.Configure();
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

		public void ResetApplication() {
			const string softYellow = "#E3D233";
			const string softRed = "#FF1000";
			// This should be const, but const interpolated strings are only available in C# 9.0 or above (we're currently using C# 8.0).
			string resetMessage = $"<b><color={softYellow}>Warning!</color></b> this will reset the application\n and <b><color={softRed}>DELETE</color></b> all your progress."; 

			_confirmationPanel.OpenConfirmationPanel(resetMessage, Reset);

			static void Reset(bool accepted) {
				if (accepted) {
					var currentSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
					UnityEngine.SceneManagement.SceneManager.LoadScene(currentSceneIndex);
				}
			}
		}

		public void CloseApplication() {
			const string softYellow = "#E3D233";
			string quitMessage = $"Are you sure you want to quit?\n <color={softYellow}>project saving not supported yet.</color>";
			_confirmationPanel.OpenConfirmationPanel(quitMessage, Quit);

			static void Quit(bool accepted) {
				if (accepted) {
					Debug.Log("Application Closed. Bye Bye.");
					Application.Quit();
				}
			}
		}

		[ContextMenu("Incremente Version Patch")]
		public void IncrementeVersionPatch() {
			foreach (var textVersion in _textVersions) {
				string currentVersion = textVersion.text;
        
				string[] versionParts = currentVersion.Split('.');

				if (versionParts.Length == 3) {
					int patchVersion = int.Parse(versionParts[2]);
					patchVersion++;

					currentVersion = $"{versionParts[0]}.{versionParts[1]}.{patchVersion}";

					textVersion.text = currentVersion;
					PlayerSettings.bundleVersion = currentVersion;
				}
			}
		}

    }
}