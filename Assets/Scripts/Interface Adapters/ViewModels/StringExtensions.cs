using System;
using System.Globalization;

public static class StringExtensions {
	
    public static float ParseFloat(string value) {
		UnityEngine.Debug.Log("Desde el inspector ingresé: " + value);
        if (string.IsNullOrWhiteSpace(value)) {
            throw new ArgumentException("Tried to parse an empty string.");
        }

        try {
            return float.Parse(value.Replace(',', '.'), CultureInfo.InvariantCulture);
        }
        catch (FormatException ex) {
            throw new FormatException($"Value '{value}' does not have correct format.", ex);
        }
        catch (OverflowException ex) {
            throw new OverflowException($"Value '{value}' overflow float capacity.", ex);
        }
    }

}