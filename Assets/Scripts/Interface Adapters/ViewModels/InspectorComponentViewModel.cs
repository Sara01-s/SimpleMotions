using SimpleMotions.Internal;

namespace SimpleMotions {

    public abstract class InspectorComponentViewModel : ComponentViewModel {

        private readonly IEntitySelectorViewModel _entitySelectorViewModel;
        private readonly IComponentStorage _componentStorage;
        private readonly IVideoPlayerData _videoPlayerData;

		protected int _SelectedEntityId => _entitySelectorViewModel.SelectedEntityId;
		protected int _CurrentFrame => _videoPlayerData.CurrentFrame.Value;

        public InspectorComponentViewModel(IEntitySelectorViewModel entitySelectorViewModel, IComponentStorage componentStorage, 
                                           IVideoPlayerData videoPlayerData, IVideoCanvas videoCanvas) : base(videoCanvas) {
            _entitySelectorViewModel = entitySelectorViewModel;
            _componentStorage = componentStorage;
            _videoPlayerData = videoPlayerData;
        }

        public T GetSelectedEntityComponent<T>() where T : Component {
			return _componentStorage.GetComponent<T>(_entitySelectorViewModel.SelectedEntityId);
		}

    }

}