
namespace SimpleMotions {

    public interface IInspectorViewModel : IEntityViewModel {

		string SelectedEntityName { get; set; }
        ReactiveCommand<(int id, string name)> OnEntitySelected { get; }

    }

    public class InspectorViewModel : EntityViewModel, IInspectorViewModel {

        public ReactiveCommand<(int id, string name)> OnEntitySelected { get; } = new();
		public string SelectedEntityName {
			get => _selectedEntityName.Value;
			set => _selectedEntityName.Value = value;
		}

		private readonly ReactiveValue<string> _selectedEntityName = new();

        public InspectorViewModel(IVideoCanvas videoCanvas, IVideoAnimator videoAnimator,
								  IEntitySelector entitySelector) : base(videoCanvas) 
		{
            videoCanvas.EntityDisplayInfo.Subscribe(UpdateEntityId);
            videoAnimator.EntityDisplayInfo.Subscribe(UpdateEntityId);

			_selectedEntityName.Subscribe(name => entitySelector.SelectedEntity.Name = name);
        }

        private void UpdateEntityId((int id, string name) entity) {
            OnEntitySelected.Execute(entity);
        }

    }
}