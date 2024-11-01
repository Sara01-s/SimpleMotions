using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace SimpleMotions {
    
    public sealed class VideoTimelineView : MonoBehaviour {

		[SerializeField] private Button _createTestEntity;
		[SerializeField] private Slider _cursor;

        public void Configure(IVideoTimelineViewModel videoTimelineViewModel) {
			_createTestEntity.onClick.AddListener(() => videoTimelineViewModel.OnCreateTestEntity.Execute(null));
			_cursor.onValueChanged.AddListener(value => videoTimelineViewModel.OnSetCurrentFrame.Execute((int)value));
        }

    }
}