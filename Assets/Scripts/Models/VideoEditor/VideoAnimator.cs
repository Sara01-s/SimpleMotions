namespace SimpleMotions {

    public sealed class VideoAnimator : IVideoAnimator {
        
        private readonly IKeyframeStorage _keyframeStorage;

        public VideoAnimator(IKeyframeStorage keyframeStorage) {
            _keyframeStorage = keyframeStorage;
        }

        public void InterpolateKeyframes(int currentFrame) {

            UnityEngine.Debug.Log(_keyframeStorage.GetKeyframeAt(currentFrame));

        }
    }
}