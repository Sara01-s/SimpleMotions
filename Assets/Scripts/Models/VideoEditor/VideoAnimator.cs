using System.Collections.Generic;

namespace SimpleMotions {

    public sealed class VideoAnimator : IVideoAnimator {
        
        private readonly IKeyframeStorage _keyframeStorage;
        private readonly IComponentStorage _componentStorage;
        private readonly SortedDictionary<int, Keyframe<Transform>> _validTransformKeyframes = new();


        public VideoAnimator(IKeyframeStorage keyframeStorage, IComponentStorage componentStorage) {
            _keyframeStorage = keyframeStorage;
            _componentStorage = componentStorage;
        }

        public void GenerateVideoCache() {
            _validTransformKeyframes.Clear();

            int totalFrames = _keyframeStorage.GetTotalFrames();
            int validFrame = 0;

            for (int frame = TimelineData.FIRST_KEYFRAME; frame <= totalFrames; frame++) {
                var keyframe = _keyframeStorage.GetKeyframeAt(frame);

                if (keyframe.EntityId == -1) {
                    continue;
                }

                _validTransformKeyframes.Add(validFrame, keyframe);
                validFrame++;
            }
        }

        public void InterpolateKeyframes(int currentFrame) {
            if (currentFrame >= _validTransformKeyframes.Count) {
                return;
            }

            int entityId = _keyframeStorage.GetKeyframeAt(_validTransformKeyframes[currentFrame].Frame).EntityId;
            var transform = _componentStorage.GetComponent<Transform>(entityId);

            if (currentFrame == TimelineData.FIRST_KEYFRAME) {
                UnityEngine.Debug.Log(_validTransformKeyframes[TimelineData.FIRST_KEYFRAME].Value.Position);
                transform.Position = _validTransformKeyframes[TimelineData.FIRST_KEYFRAME].Value.Position;

                var newtransform = _componentStorage.GetComponent<Transform>(entityId);
                UnityEngine.Debug.Log(newtransform.Position);
            }
        }

    }
}