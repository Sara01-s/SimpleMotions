using Newtonsoft.Json;
using System.IO;

namespace SimpleMotions {

	public sealed class NewtonJsonSerializer : ISerializer {

		public T Deserialize<T>(string filepath) {
			if (!Directory.Exists(filepath)) {
				Directory.CreateDirectory(Path.GetDirectoryName(filepath));
				return default;
			}

			string dataToLoad = string.Empty;

			using (var stream = new FileStream(filepath, FileMode.Open)) {
				using (var streamReader = new StreamReader(stream)) {
					dataToLoad = streamReader.ReadToEnd();
				}
			}

			T loadedData;

			try {
				loadedData = JsonConvert.DeserializeObject<T>(dataToLoad);
			}
			catch (System.Exception e) {
				throw new System.Exception("Error while trying to load data from JSON", e);
			}
			
			return loadedData;
		}

		public void Serialize<T>(T data, string filepath) {
			var dataToStore = JsonConvert.SerializeObject(data, Formatting.Indented);

			Directory.CreateDirectory(Path.GetDirectoryName(filepath));

			using (var stream = new FileStream(filepath, FileMode.Create)) {
				using (var streamWriter = new StreamWriter(stream)) {
					streamWriter.Write(dataToStore);
				}
			}
		}
		
	}
}