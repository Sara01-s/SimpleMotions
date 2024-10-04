namespace SimpleMotions {

	public interface ISerializer {
		void Serialize<T>(T data, string filepath);
		T Deserialize<T>(string filepath);
	}

}