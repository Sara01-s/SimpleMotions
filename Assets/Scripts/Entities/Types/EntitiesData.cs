using System.Collections.Generic;

namespace SimpleMotions.Internal {

	[System.Serializable]
	public sealed class EntitiesData {
		public List<int> AvailableIds = new();
		public Dictionary<int, Entity> Entities = new();
		public int NextAvailableId = 0;
		public HashSet<int> AliveEntities = new();
		public HashSet<int> ActiveEntities = new();

		public Entity SelectedEntity = Entity.Invalid;
	}
}