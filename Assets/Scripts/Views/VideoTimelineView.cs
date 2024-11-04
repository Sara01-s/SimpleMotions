using UnityEngine.UI;
using UnityEngine;

namespace SimpleMotions {
    
    public sealed class VideoTimelineView : MonoBehaviour {

		[Header("References")]
		[SerializeField] private Button _createTestEntity;
		[SerializeField] private Slider _cursor;
		[SerializeField] private Sprite _whiteTexture;

		[Header("Draw")]
		[SerializeField] private GameObject _framesHolder;
		[SerializeField] private GameObject _framePrefab;

		private EditorTheme _editorTheme;

        public void Configure(IVideoTimelineViewModel videoTimelineViewModel, EditorTheme editorTheme) {
			_createTestEntity.onClick.AddListener(() => videoTimelineViewModel.OnCreateTestEntity.Execute(null));
			_cursor.onValueChanged.AddListener(value => videoTimelineViewModel.OnSetCurrentFrame.Execute((int)value));

			videoTimelineViewModel.OnTimelineUpdate.Subscribe(SetCursorValue);

			_editorTheme = editorTheme;

			DrawTimeline(videoTimelineViewModel.DefaultKeyframes);
        }

		private void DrawTimeline(int framesCount) {
			for (int i = 0; i < framesCount; i++) {
				Instantiate(_framePrefab, parent: _framesHolder.transform).name = $"Frame_{i}";
			}
		}

		private void SetCursorValue(VideoDisplayInfo videoDisplayInfo) {
			_cursor.value = videoDisplayInfo.CurrentFrame;
		}

    }
}