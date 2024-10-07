namespace SimpleMotions {

    public interface IVideoAnimator {

        void InterpolateKeyframes<T>(Keyframe<T>[] keyframes) where T : Component;

    }
}