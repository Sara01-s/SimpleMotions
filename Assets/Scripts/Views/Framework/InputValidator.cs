

namespace SimpleMotions {

    public interface IInputValidator {
        string ValidateInput(string newInput, string previousInput);
    }

    public class InputValidator : IInputValidator {

        public string ValidateInput(string newInput, string previousInput) {

            if (ContainsInvalidCharacters(newInput)) {
                return previousInput;
            }

            newInput = newInput.Replace('.', ',');

            int decimalIndex = newInput.IndexOf('.');

            if (decimalIndex != -1) {
                newInput = newInput.Substring(0, decimalIndex + 1) + newInput.Substring(decimalIndex + 1).Replace(",", "");
            }

            return newInput;
        }

        private bool ContainsInvalidCharacters(string newInput) {
            bool lastWasSeparator = false;

            for (int i = 0; i < newInput.Length; i++) {
                char c = newInput[i];

                if (i == 0 && c == '-') {
                    continue;
                }
                else if (char.IsDigit(c)) {
                    lastWasSeparator = false;
                }
                else if (c == '.' || c == ',') {
                    if (lastWasSeparator) {
                        return true;
                    }
                    lastWasSeparator = true;
                }
                else {
                    return true;
                }
            }

            return false;
        }

    }

}
