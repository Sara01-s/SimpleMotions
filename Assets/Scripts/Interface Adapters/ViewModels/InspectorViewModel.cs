
namespace SimpleMotions {

    public interface IInspectorViewModel : IComponentViewModel {

		string SelectedEntityName { get; set; }
        ReactiveCommand<(int id, string name)> OnEntitySelected { get; }

    }

    public class InspectorViewModel : ComponentViewModel, IInspectorViewModel {

        public ReactiveCommand<(int id, string name)> OnEntitySelected { get; } = new();
		public string SelectedEntityName {
			get => _selectedEntityName.Value;
			set => _selectedEntityName.Value = value;
		}

		private readonly ReactiveValue<string> _selectedEntityName = new();
		private readonly IEntitySelector _entitySelector;
		private readonly IEntityViewModel _entityViewModel;

        public InspectorViewModel(IVideoCanvas videoCanvas, IVideoAnimator videoAnimator,
								  IEntitySelector entitySelector, IEntityViewModel entityViewModel) : base(videoCanvas) 
		{
            videoCanvas.EntityDisplayInfo.Subscribe(UpdateEntityInfo);
            videoAnimator.EntityDisplayInfo.Subscribe(UpdateEntityInfo);
			entityViewModel.OnEntityNameChanged.Subscribe((id, name) => UpdateEntityInfo((id, name)));

			_entitySelector = entitySelector;
			_entityViewModel = entityViewModel;
			_selectedEntityName.Subscribe(ChangeSelectedEntityName);
        }

		private void ChangeSelectedEntityName(string newName) {
			_entityViewModel.ChangeEntityName(_entitySelector.SelectedEntity.Id, newName);
		}

        private void UpdateEntityInfo((int id, string name) entity) {
            OnEntitySelected.Execute(entity);
        }

    }
}