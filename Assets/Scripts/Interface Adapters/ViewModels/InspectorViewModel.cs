
namespace SimpleMotions {

    public interface IInspectorViewModel : IComponentViewModel {

		string SelectedEntityName { get; set; }
        ReactiveCommand<EntityDTO> OnEntitySelected { get; }
		ReactiveCommand OnEntityDeselected { get; }
		ReactiveCommand OnClearInspector { get; }

    }

    public class InspectorViewModel : ComponentViewModel, IInspectorViewModel {

        public ReactiveCommand<EntityDTO> OnEntitySelected { get; } = new();
        public ReactiveCommand OnEntityDeselected { get; } = new();
		public ReactiveCommand OnClearInspector { get; } = new();
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
            videoCanvas.EntityDisplayInfo.Subscribe(entity => UpdateEntityInfo(new EntityDTO(entity.Id, entity.Name)));
			videoCanvas.OnEntityRemoved.Subscribe(_ => OnClearInspector.Execute());

            videoAnimator.EntityDisplayInfo.Subscribe(entity => UpdateEntityInfo(new EntityDTO(entity.Id, entity.Name)));

			entityViewModel.OnEntityNameChanged.Subscribe(UpdateEntityInfo);
			
			entitySelector.OnEntitySelected.Subscribe(entity => UpdateEntityInfo(new EntityDTO(entity.Id, entity.Name)));
			entitySelector.OnEntityDeselected.Subscribe(OnEntityDeselected.Execute);

			_entitySelector = entitySelector;
			_entityViewModel = entityViewModel;
			_selectedEntityName.Subscribe(ChangeSelectedEntityName);
        }

		private void ChangeSelectedEntityName(string newName) {
			_entityViewModel.ChangeEntityName(_entitySelector.SelectedEntity.Id, newName);
		}

        private void UpdateEntityInfo(EntityDTO entityDTO) {
            OnEntitySelected.Execute(entityDTO);
        }

    }
}