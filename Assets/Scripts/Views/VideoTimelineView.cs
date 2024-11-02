using UnityEngine.UI;
using UnityEngine;

namespace SimpleMotions {
    
    public sealed class VideoTimelineView : MonoBehaviour {

		[SerializeField] private Button _createTestEntity;
		[SerializeField] private Slider _cursor;
		[SerializeField] private RectTransform _contentArea;
		[SerializeField] private Sprite _whiteTexture;

        public void Configure(IVideoTimelineViewModel videoTimelineViewModel) {
			_createTestEntity.onClick.AddListener(() => videoTimelineViewModel.OnCreateTestEntity.Execute(null));
			_cursor.onValueChanged.AddListener(value => videoTimelineViewModel.OnSetCurrentFrame.Execute((int)value));

			videoTimelineViewModel.OnTimelineUpdate.Subscribe(SetCursorValue);

			GenerateTimeline();
        }

		private void GenerateTimeline() {
			
		}

		private void SetCursorValue(VideoDisplayInfo videoDisplayInfo) {
			_cursor.value = videoDisplayInfo.CurrentFrame;
		}

    }
}