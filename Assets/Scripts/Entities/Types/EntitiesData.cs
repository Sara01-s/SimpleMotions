using System.Collections.Generic;

namespace SimpleMotions {

	[System.Serializable]
	public sealed class EntitiesData {
		public List<int> AvailableIds = new();
		public Dictionary<int, Entity> Entities = new();
		public int NextAvailableId = 0;
		public HashSet<int> AliveEntities = new();
	}
}