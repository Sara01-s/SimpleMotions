namespace SimpleMotions {

	public sealed class DataSerializer<T> {

		private readonly ISerializer _serializer;

		public DataSerializer(ISerializer serializer) {
			_serializer = serializer;
		}

		public void SaveToFile(T data, string filepath) {
			_serializer.Serialize(data, filepath);
		}

		public T LoadFromFile(string filepath) {
			return _serializer.Deserialize<T>(filepath);
		}

	}
}