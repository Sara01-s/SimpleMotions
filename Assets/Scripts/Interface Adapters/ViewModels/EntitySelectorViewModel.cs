using SimpleMotions.Internal;

namespace SimpleMotions {


	public interface IEntitySelectorViewModel : IComponentViewModel {
		
		ReactiveCommand<int> SelectEntity { get; }
		ReactiveCommand<(int entityId, string entityName)> OnEntitySelected { get; }
		ReactiveCommand OnEntityDeselected { get; }

	}

	public class EntitySelectorViewModel : ComponentViewModel, IEntitySelectorViewModel {

		public ReactiveCommand<(int, string)> OnEntitySelected { get; } = new();
		public ReactiveCommand OnEntityDeselected { get; } = new();
		public ReactiveCommand<int> SelectEntity { get; } = new();

		public EntitySelectorViewModel(IEntitySelector entitySelector, IVideoCanvas videoCanvas) : base(videoCanvas) {
			entitySelector.OnEntitySelected.Subscribe(entity => OnEntitySelected.Execute((entity.Id, entity.Name)));
			entitySelector.OnEntityDeselected.Subscribe(OnEntityDeselected.Execute);

			SelectEntity.Subscribe(entitySelector.SelectEntity);
        }

	}
}