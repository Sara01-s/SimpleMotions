using System.Collections.Generic;
using SimpleMotions.Internal;

namespace SimpleMotions {

    public interface IEntitySelector {

		int SelectedEntity { get; }

		void SelectEntity(int entityId);
		void DeselectEntity(int entityId);
		
    }

	public class EntitySelector : IEntitySelector {
		public int SelectedEntity { get; private set; }

		public EntitySelector(EntitiesData entitiesData) {
			SelectedEntity = entitiesData.SelectedEntity;
		}

		public void SelectEntity(int entityId) {
			SelectedEntity = entityId;
		}

		public void DeselectEntity(int entityId) {
			SelectedEntity = Entity.INVALID_ID;
		}

	}
}