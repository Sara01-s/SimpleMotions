using System.Collections.Generic;

namespace SimpleMotions {

    public sealed class VideoAnimator : IVideoAnimator {
        
        private readonly IKeyframeStorage _keyframeStorage;
        private readonly SortedDictionary<int, Keyframe<Transform>> _validTransformKeyframes = new();


        public VideoAnimator(IKeyframeStorage keyframeStorage) {
            _keyframeStorage = keyframeStorage;
        }

        public void GenerateVideoCache() {
            _validTransformKeyframes.Clear();

            int totalFrames = _keyframeStorage.GetTotalFrames();
            int validFrame = 1;

            for (int frame = 1; frame <= totalFrames; frame++) {
                UnityEngine.Debug.Log(frame);

                var keyframe = _keyframeStorage.GetKeyframeAt(frame);

                if (keyframe.EntityId == -1) {
                    continue;
                }  

                // PROBLEMA DE SARA DEL FUTURO WUAJAJAJA
                // keyframes con valores
                _validTransformKeyframes.Add(validFrame++, keyframe);
            }
        }

        public void InterpolateKeyframes(int currentFrame) {

            UnityEngine.Debug.Log(_validTransformKeyframes[currentFrame]);

        }
    }
}