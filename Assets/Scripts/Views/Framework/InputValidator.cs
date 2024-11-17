
namespace SimpleMotions {

    public interface IInputValidator {
        (string, bool) ValidateInput(string input);
        bool ContainsInvalidCharacters(string input);
    }

    // TODO - VER CASO DE DOS PUNTOS EN DIFERENTES POSICIONES: 'O.123.123'

    public class InputValidator : IInputValidator {

        private bool _hasInvalidCharacters;

        public (string, bool) ValidateInput(string input) {
            input = input.Replace('.', ',');

            _hasInvalidCharacters = ContainsInvalidCharacters(input);
            
            if (_hasInvalidCharacters) {
                return (input, _hasInvalidCharacters);
            }

            int decimalIndex = input.IndexOf('.');

            if (decimalIndex != -1) {
                input = input.Substring(0, decimalIndex + 1) + input.Substring(decimalIndex + 1).Replace(",", "");
            }

            return (input, _hasInvalidCharacters);
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

    }
}