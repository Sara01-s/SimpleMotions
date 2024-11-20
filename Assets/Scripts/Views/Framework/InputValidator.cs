using System.IO;
using System.Text.RegularExpressions;

namespace SimpleMotions {

    public interface IInputValidator {
        string ValidateComponentInput(string input);
        bool ComponentInputIsInvalid(string input);
		bool ValidateDirectory(string filePath);
        bool ValidateFileName(string fileName);
    }

    // TODO - VER CASO DE DOS PUNTOS EN DIFERENTES POSICIONES: 'O.123.123'

    public class InputValidator : IInputValidator {

		public bool ValidateDirectory(string filePath) {
			return Directory.Exists(filePath);
		} 

        public string ValidateComponentInput(string input) {
            input = input.Replace('.', ',');

            int decimalIndex = input.IndexOf('.');

            if (decimalIndex != -1) {
                input = input.Substring(0, decimalIndex + 1) + input.Substring(decimalIndex + 1).Replace(",", "");
            }

            return input;
        }

        public bool ComponentInputIsInvalid(string newInput) {
            bool lastWasSeparator = false;
            int separatorCount = 0;

            for (int i = 0; i < newInput.Length; i++) {
                char c = newInput[i];

                if (i == 0 && c == '-') {
                    continue;
                }
                else if (c == ',' || c == '.') {
                    separatorCount++;

                    if (lastWasSeparator) {
                        return true;
                    }

                    if (separatorCount > 1) {
                        return true;
                    }

                    lastWasSeparator = true;
                }
            }

            return false;
        }

        public bool ValidateFileName(string fileName) {
             if (string.IsNullOrWhiteSpace(fileName)) {
                return false;
             }

            string pattern = @"^[a-zA-Z0-9]+$";

            return Regex.IsMatch(fileName, pattern);
        }

    }
}