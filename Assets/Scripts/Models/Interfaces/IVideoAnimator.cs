namespace SimpleMotions {

    public interface IVideoAnimator {
        
        void GenerateVideoCache();
        void InterpolateKeyframes(int currentFrame);

    }
}