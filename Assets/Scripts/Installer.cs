using UnityEngine;

namespace SimpleMotions {

    internal sealed class Installer : MonoBehaviour {

        [SerializeField] private VideoTimelineView _videoTimelineView;

        private void OnDisable() {
            Services.Instance.Clear();
        }

        private void Start() {
            var componentDatabase = new ComponentDatabase();
            var videoDatabase = new VideoDatabase();

            //var componentDatabase = Services.Instance.GetService<IComponentDatabase>();
			//var videoDatabase = Services.Instance.GetService<IVideoDatabase>();

            var videoTimeline = new VideoTimeline(videoDatabase);
            var videoPlayer = new VideoPlayer(componentDatabase);
            var videoCanvas = new VideoCanvas(componentDatabase);

            var videoEditor = new VideoEditor(videoTimeline, videoPlayer, videoCanvas);

            var videoTimelineViewModel = new VideoTimelineViewModel(videoEditor.GetVideoTimeline());

            _videoTimelineView.Configure(videoTimelineViewModel);
        }

    }
}