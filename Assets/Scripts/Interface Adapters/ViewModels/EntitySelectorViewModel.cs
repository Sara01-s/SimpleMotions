namespace SimpleMotions {

	public interface IEntitySelectorViewModel : IComponentViewModel {
		
		public int SelectedEntityId { get; }
		ReactiveCommand<int> SelectEntity { get; }
		ReactiveCommand DeselectEntity { get; }
		ReactiveCommand<EntityDTO> OnEntitySelected { get; }
		ReactiveCommand OnEntityDeselected { get; }
		bool HasSelectedEntity { get; }

	}

	// FIXME - Está sus que EntitySelectorViewModel herede de ComponentViewModel (wtf?), 
	// aunque igualmente sirvió para simplificar las dependencias de EntitySelectorView.
	public class EntitySelectorViewModel : ComponentViewModel, IEntitySelectorViewModel {

		public ReactiveCommand<EntityDTO> OnEntitySelected { get; } = new();
		public ReactiveCommand OnEntityDeselected { get; } = new();
		public ReactiveCommand<int> SelectEntity { get; } = new();
		public ReactiveCommand DeselectEntity { get; } = new();

        public int SelectedEntityId => _entitySelector.SelectedEntity.Id;
		public bool HasSelectedEntity => _entitySelector.HasSelectedEntity;

		private readonly IEntitySelector _entitySelector;

        public EntitySelectorViewModel(IEntitySelector entitySelector, IVideoCanvas videoCanvas) : base(videoCanvas) {
			entitySelector.OnEntityDeselected.Subscribe(OnEntityDeselected.Execute);
			entitySelector.OnEntitySelected.Subscribe(entity => {
				OnEntitySelected.Execute(new EntityDTO(entity.Id, entity.Name));
			});

			DeselectEntity.Subscribe(entitySelector.DeselectEntity);
			SelectEntity.Subscribe(entitySelector.SelectEntity);
			_entitySelector = entitySelector;
        }

	}
}