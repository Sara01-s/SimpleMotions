using SimpleMotions.Internal;

namespace SimpleMotions {

    public abstract class InspectorComponentViewModel : ComponentViewModel {

        private readonly IEntitySelectorViewModel _entitySelectorViewModel;
        private readonly IComponentStorage _componentStorage;
        private readonly IVideoPlayerData _videoPlayerData;

        public InspectorComponentViewModel(IEntitySelectorViewModel entitySelectorViewModel, IComponentStorage componentStorage, 
                                           IVideoPlayerData videoPlayerData, IVideoCanvas videoCanvas) : base(videoCanvas) {
            _entitySelectorViewModel = entitySelectorViewModel;
            _componentStorage = componentStorage;
            _videoPlayerData = videoPlayerData;
        }

        public T GetSelectedSEntityComponent<T>() where T : Component {
			return _componentStorage.GetComponent<T>(GetSelectedEntityId());
		}

		public int GetSelectedEntityId() {
			return _entitySelectorViewModel.SelectedEntityId;
		}

		public int GetCurrentFrame() {
			return _videoPlayerData.CurrentFrame.Value;
		}

    }

}