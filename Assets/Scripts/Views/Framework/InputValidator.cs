using System.Text.RegularExpressions;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace SimpleMotions {

    public interface IInputValidator {
        string ValidateComponentInput(string input);
        bool IsComponentNewInputInvalid(string input);
		bool ValidateDirectory(string filePath);
        bool ValidateFileName(string fileName);
    }

    // TODO - VER CASO DE DOS PUNTOS EN DIFERENTES POSICIONES: 'O.123.123'

    public class InputValidator : IInputValidator {

		public bool ValidateDirectory(string filePath) {
			return Directory.Exists(filePath);
		} 

        public string ValidateComponentInput(string input) {
            input = input.Replace('.', '.');
            input = input.Substring(0).Replace("+", "");

            int decimalIndex = input.IndexOf(',');

            if (decimalIndex != -1) {
                input = input.Substring(0, decimalIndex + 1) + input.Substring(decimalIndex + 1).Replace(".", "");
            }

            if (input.StartsWith("-")) {
                input = "-" + input.Substring(1).Replace("-", "");
            }

            if (input.Length > 2) {
                if (input.StartsWith("-") && input[1] == '.' && Regex.IsMatch(input, @"[1-9]$")) {
                    var stringBuilder = new StringBuilder(input);
                    stringBuilder.Insert(1, "0");
                    input = stringBuilder.ToString();
                }
                else if (input.StartsWith(".") && Regex.IsMatch(input, @"[1-9]$")) {
                    var stringBuilder = new StringBuilder(input);
                    stringBuilder.Insert(0, "0");
                    input = stringBuilder.ToString();
                }
            }

            if (input.StartsWith('.')) {
                var stringBuilder = new StringBuilder(input);
                stringBuilder.Insert(0, "0");
                input = stringBuilder.ToString();
            }

            return input;
        }

        public bool IsComponentNewInputInvalid(string newInput) {
            var numberRegex = new Regex(@"^-?\d+([.,]\d+)?$");

            if (!numberRegex.IsMatch(newInput)) {
                return true;
            }

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

            string pattern = @"^[a-zA-Z0-9 _]+$";

            return Regex.IsMatch(fileName, pattern);
        }

    }
}