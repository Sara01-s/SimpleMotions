using System.IO;
using System;

namespace SimpleMotions {

    public interface IInputValidator {
        string ValidateTransformComponentInput(string input);
        string ValidateOutputFilePathInput(string input);
        bool ContainsInvalidCharacters(string input);
        bool IsValidOutputFilePath(string input);
		bool ValidateDirectory(string filePath);
    }

    // TODO - VER CASO DE DOS PUNTOS EN DIFERENTES POSICIONES: 'O.123.123'

    public class InputValidator : IInputValidator {
		public bool ValidateDirectory(string filePath) {
			return Directory.Exists(filePath);
		} 

        public string ValidateTransformComponentInput(string input) {
            input = input.Replace('.', ',');

            int decimalIndex = input.IndexOf('.');

            if (decimalIndex != -1) {
                input = input.Substring(0, decimalIndex + 1) + input.Substring(decimalIndex + 1).Replace(",", "");
            }

            return input;
        }

        public bool ContainsInvalidCharacters(string newInput) {
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

        public string ValidateOutputFilePathInput(string input) {
            if (!IsValidOutputFilePath(input)) {
                throw new ArgumentException("La ruta proporcionada no es válida.");
            }

            if (input.Length > 1 && input.EndsWith("/")) {
                input = input.TrimEnd('/');
            }

            return input;
        }

        public bool IsValidOutputFilePath(string input) {
            if (string.IsNullOrWhiteSpace(input)) {
                return false;
            }

            if (!input.StartsWith("/")) {
                return false;
            }

            char[] invalidChars = Path.GetInvalidPathChars();
            if (input.IndexOfAny(invalidChars) >= 0) {
                return false;
            }

            return true;
        }

    }
}