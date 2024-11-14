

namespace SimpleMotions {

    public interface IInputValidator {
        string ValidateInput(string input);
    }

    public class InputValidator : IInputValidator {

        public string ValidateInput(string input) {
            input = input.Replace('.', ',');

            int decimalIndex = input.IndexOf('.');

            if (decimalIndex != -1) {
                input = input.Substring(0, decimalIndex + 1) + input.Substring(decimalIndex + 1).Replace(",", "");
            }

            return input;
        }

    }

}
