using System;
using System.Text.RegularExpressions;

namespace FitnessAppAPI.Utilities
{
    /// <summary>
    /// Helper class for data validation
    /// </summary>
    public static class ValidationHelper
    {
        /// <summary>
        /// Validates if a string is not null or empty
        /// </summary>
        public static bool IsNotNullOrEmpty(string value)
        {
            return !string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// Validates if a string is not null, empty, or whitespace
        /// </summary>
        public static bool IsNotNullOrWhiteSpace(string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// Validates email format
        /// </summary>
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            var emailRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
            return emailRegex.IsMatch(email);
        }

        /// <summary>
        /// Validates phone number format
        /// </summary>
        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return false;

            var phoneRegex = new Regex(@"^\+?[\d\s\-\(\)]{10,15}$");
            return phoneRegex.IsMatch(phoneNumber);
        }

        /// <summary>
        /// Validates if a number is within a specified range
        /// </summary>
        public static bool IsInRange(int value, int min, int max)
        {
            return value >= min && value <= max;
        }

        /// <summary>
        /// Validates if a double is within a specified range
        /// </summary>
        public static bool IsInRange(double value, double min, double max)
        {
            return value >= min && value <= max;
        }

        /// <summary>
        /// Validates if a date is within a specified range
        /// </summary>
        public static bool IsDateInRange(DateTime date, DateTime minDate, DateTime maxDate)
        {
            return date >= minDate && date <= maxDate;
        }

        /// <summary>
        /// Validates if a string has a minimum length
        /// </summary>
        public static bool HasMinimumLength(string value, int minLength)
        {
            return !string.IsNullOrEmpty(value) && value.Length >= minLength;
        }

        /// <summary>
        /// Validates if a string has a maximum length
        /// </summary>
        public static bool HasMaximumLength(string value, int maxLength)
        {
            return string.IsNullOrEmpty(value) || value.Length <= maxLength;
        }

        /// <summary>
        /// Validates if a string contains only alphabetic characters
        /// </summary>
        public static bool IsAlphabetic(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            foreach (char c in value)
            {
                if (!char.IsLetter(c))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Validates if a string contains only numeric characters
        /// </summary>
        public static bool IsNumeric(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            foreach (char c in value)
            {
                if (!char.IsDigit(c))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Validates if a string contains only alphanumeric characters
        /// </summary>
        public static bool IsAlphanumeric(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            foreach (char c in value)
            {
                if (!char.IsLetterOrDigit(c))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Validates if a URL is in correct format
        /// </summary>
        public static bool IsValidUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return false;

            return Uri.TryCreate(url, UriKind.Absolute, out Uri result) &&
                   (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps);
        }

        /// <summary>
        /// Validates if an age is valid (between 0 and 150)
        /// </summary>
        public static bool IsValidAge(int age)
        {
            return age >= 0 && age <= 150;
        }

        /// <summary>
        /// Validates if a postal code is in correct format
        /// </summary>
        public static bool IsValidPostalCode(string postalCode, string countryCode = "US")
        {
            if (string.IsNullOrWhiteSpace(postalCode))
                return false;

            return countryCode.ToUpper() switch
            {
                "US" => Regex.IsMatch(postalCode, @"^\d{5}(-\d{4})?$"),
                "CA" => Regex.IsMatch(postalCode, @"^[A-Za-z]\d[A-Za-z] \d[A-Za-z]\d$"),
                "UK" => Regex.IsMatch(postalCode, @"^[A-Za-z]{1,2}\d[A-Za-z\d]? \d[A-Za-z]{2}$"),
                _ => postalCode.Length >= 3 && postalCode.Length <= 10
            };
        }

        /// <summary>
        /// Validates if a credit card number passes Luhn algorithm
        /// </summary>
        public static bool IsValidCreditCardNumber(string cardNumber)
        {
            if (string.IsNullOrWhiteSpace(cardNumber))
                return false;

            cardNumber = cardNumber.Replace(" ", "").Replace("-", "");

            if (!IsNumeric(cardNumber) || cardNumber.Length < 13 || cardNumber.Length > 19)
                return false;

            int sum = 0;
            bool alternate = false;

            for (int i = cardNumber.Length - 1; i >= 0; i--)
            {
                int n = int.Parse(cardNumber[i].ToString());

                if (alternate)
                {
                    n *= 2;
                    if (n > 9)
                        n = (n % 10) + 1;
                }

                sum += n;
                alternate = !alternate;
            }

            return sum % 10 == 0;
        }

        /// <summary>
        /// Validates if a password meets complexity requirements
        /// </summary>
        public static bool IsValidPassword(string password, int minLength = 8)
        {
            if (string.IsNullOrEmpty(password) || password.Length < minLength)
                return false;

            bool hasUpper = false;
            bool hasLower = false;
            bool hasDigit = false;
            bool hasSpecial = false;

            foreach (char c in password)
            {
                if (char.IsUpper(c)) hasUpper = true;
                else if (char.IsLower(c)) hasLower = true;
                else if (char.IsDigit(c)) hasDigit = true;
                else if (!char.IsLetterOrDigit(c)) hasSpecial = true;
            }

            return hasUpper && hasLower && hasDigit && hasSpecial;
        }
    }
}
