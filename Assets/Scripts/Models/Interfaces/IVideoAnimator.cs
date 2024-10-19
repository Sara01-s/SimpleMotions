namespace SimpleMotions {

    public interface IVideoAnimator {
        
        void GenerateVideoCache();
        void InterpolateAllEntities(int currentFrame);

    }
}