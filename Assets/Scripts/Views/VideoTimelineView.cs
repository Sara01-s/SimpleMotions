using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace SimpleMotions {
    
    public sealed class VideoTimelineView : MonoBehaviour {

		[SerializeField] private Button _createTestEntity;

        public void Configure(IVideoTimelineViewModel videoTimelineViewModel) {
			_createTestEntity.onClick.AddListener(() => videoTimelineViewModel.CreateTestEntity.Execute(null));
        }

    }
}