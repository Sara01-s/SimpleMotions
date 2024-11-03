using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System;

namespace SimpleMotions {

	public sealed class CsvSerializer : ISerializer {

		public CsvSerializer() {}

		public T Deserialize<T>(string filepath) {
			var loadedData = new List<T>();

			using (var stream = new FileStream(filepath, FileMode.Open)) {
				using (var streamReader = new StreamReader(stream)) {
					string headerLine = streamReader.ReadLine();

					if (string.IsNullOrEmpty(headerLine)) {
						throw new Exception("CSV file is empty.");
					}

					string[] headers = headerLine.Split(',');

					while (!streamReader.EndOfStream) {
						string dataLine = streamReader.ReadLine();
						if (string.IsNullOrEmpty(dataLine)) {
							continue;
						}

						string[] values = dataLine.Split(',');
						var obj = (T)Activator.CreateInstance(typeof(T)); // Activar constructor sin 'new()'
						var type = typeof(T);

						for (int i = 0; i < headers.Length; i++) {
							var prop = type.GetProperty(headers[i], BindingFlags.Public | BindingFlags.Instance);

							if (prop != null && i < values.Length) {
								object convertedValue = Convert.ChangeType(values[i], prop.PropertyType);
								prop.SetValue(obj, convertedValue);
							}
						}
						
						loadedData.Add(obj);
					}
				}
			}

			return loadedData.Count > 0 ? loadedData[0] : default;
		}

		public void Serialize<T>(T data, string filepath) {
			var type = typeof(T);
			var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

			Directory.CreateDirectory(Path.GetDirectoryName(filepath));

			using (var stream = new FileStream(filepath, FileMode.Create)) {
				using (var streamWriter = new StreamWriter(stream)) {
					string header = string.Join(",", Array.ConvertAll(properties, p => p.Name));
					streamWriter.WriteLine(header);

					string[] values = new string[properties.Length];

					for (int i = 0; i < properties.Length; i++) {
						object value = properties[i].GetValue(data);
						values[i] = value != null ? value.ToString() : string.Empty;
					}

					string dataLine = string.Join(",", values);
					streamWriter.WriteLine(dataLine);
				}
			}
		}

	}
}