using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace FitnessAppAPI.Utilities
{
    /// <summary>
    /// Extension methods for string manipulation and validation
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Converts string to title case
        /// </summary>
        public static string ToTitleCase(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            var textInfo = CultureInfo.CurrentCulture.TextInfo;
            return textInfo.ToTitleCase(input.ToLower());
        }

        /// <summary>
        /// Converts string to camel case
        /// </summary>
        public static string ToCamelCase(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            var words = input.Split(new[] { ' ', '_', '-' }, StringSplitOptions.RemoveEmptyEntries);
            var result = new StringBuilder();

            for (int i = 0; i < words.Length; i++)
            {
                var word = words[i].ToLower();
                if (i == 0)
                {
                    result.Append(word);
                }
                else
                {
                    result.Append(char.ToUpper(word[0]) + word.Substring(1));
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Converts string to pascal case
        /// </summary>
        public static string ToPascalCase(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            var camelCase = input.ToCamelCase();
            return char.ToUpper(camelCase[0]) + camelCase.Substring(1);
        }

        /// <summary>
        /// Converts string to kebab case
        /// </summary>
        public static string ToKebabCase(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            return Regex.Replace(input, "([a-z])([A-Z])", "$1-$2")
                       .Replace(" ", "-")
                       .Replace("_", "-")
                       .ToLower();
        }

        /// <summary>
        /// Converts string to snake case
        /// </summary>
        public static string ToSnakeCase(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            return Regex.Replace(input, "([a-z])([A-Z])", "$1_$2")
                       .Replace(" ", "_")
                       .Replace("-", "_")
                       .ToLower();
        }

        /// <summary>
        /// Truncates string to specified length with ellipsis
        /// </summary>
        public static string Truncate(this string input, int maxLength, string suffix = "...")
        {
            if (string.IsNullOrEmpty(input) || input.Length <= maxLength)
                return input;

            return input.Substring(0, maxLength - suffix.Length) + suffix;
        }

        /// <summary>
        /// Removes all whitespace from string
        /// </summary>
        public static string RemoveWhitespace(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            return new string(input.Where(c => !char.IsWhiteSpace(c)).ToArray());
        }

        /// <summary>
        /// Reverses the string
        /// </summary>
        public static string Reverse(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            return new string(input.Reverse().ToArray());
        }

        /// <summary>
        /// Counts occurrences of substring
        /// </summary>
        public static int CountOccurrences(this string input, string substring, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(substring))
                return 0;

            int count = 0;
            int index = 0;

            while ((index = input.IndexOf(substring, index, comparison)) != -1)
            {
                count++;
                index += substring.Length;
            }

            return count;
        }

        /// <summary>
        /// Checks if string is a valid email address
        /// </summary>
        public static bool IsValidEmail(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            var emailRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
            return emailRegex.IsMatch(input);
        }

        /// <summary>
        /// Checks if string is a valid phone number
        /// </summary>
        public static bool IsValidPhoneNumber(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            var phoneRegex = new Regex(@"^\+?[\d\s\-\(\)]+$");
            var digitsOnly = input.RemoveWhitespace().Replace("-", "").Replace("(", "").Replace(")", "").Replace("+", "");
            
            return phoneRegex.IsMatch(input) && digitsOnly.Length >= 10 && digitsOnly.Length <= 15;
        }

        /// <summary>
        /// Checks if string contains only numeric characters
        /// </summary>
        public static bool IsNumeric(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            return input.All(char.IsDigit);
        }

        /// <summary>
        /// Checks if string contains only alphabetic characters
        /// </summary>
        public static bool IsAlphabetic(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            return input.All(char.IsLetter);
        }

        /// <summary>
        /// Checks if string contains only alphanumeric characters
        /// </summary>
        public static bool IsAlphanumeric(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            return input.All(char.IsLetterOrDigit);
        }

        /// <summary>
        /// Generates MD5 hash of the string
        /// </summary>
        public static string ToMD5Hash(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            using var md5 = MD5.Create();
            var inputBytes = Encoding.UTF8.GetBytes(input);
            var hashBytes = md5.ComputeHash(inputBytes);

            return Convert.ToHexString(hashBytes).ToLower();
        }

        /// <summary>
        /// Generates SHA256 hash of the string
        /// </summary>
        public static string ToSHA256Hash(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            using var sha256 = SHA256.Create();
            var inputBytes = Encoding.UTF8.GetBytes(input);
            var hashBytes = sha256.ComputeHash(inputBytes);

            return Convert.ToHexString(hashBytes).ToLower();
        }

        /// <summary>
        /// Encodes string to Base64
        /// </summary>
        public static string ToBase64(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            var bytes = Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Decodes Base64 string
        /// </summary>
        public static string FromBase64(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            try
            {
                var bytes = Convert.FromBase64String(input);
                return Encoding.UTF8.GetString(bytes);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Converts string to slug format (URL-friendly)
        /// </summary>
        public static string ToSlug(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            // Convert to lowercase
            var slug = input.ToLower();

            // Remove accents
            slug = Regex.Replace(slug, @"[àáâãäåąā]", "a");
            slug = Regex.Replace(slug, @"[èéêëęē]", "e");
            slug = Regex.Replace(slug, @"[ìíîïį]", "i");
            slug = Regex.Replace(slug, @"[òóôõöø]", "o");
            slug = Regex.Replace(slug, @"[ùúûüų]", "u");
            slug = Regex.Replace(slug, @"[ýÿ]", "y");
            slug = Regex.Replace(slug, @"[ñń]", "n");
            slug = Regex.Replace(slug, @"[çć]", "c");
            slug = Regex.Replace(slug, @"[ß]", "ss");
            slug = Regex.Replace(slug, @"[Ł]", "l");

            // Replace non-alphanumeric characters with hyphens
            slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");

            // Replace multiple spaces or hyphens with single hyphen
            slug = Regex.Replace(slug, @"[\s-]+", "-");

            // Trim hyphens from start and end
            slug = slug.Trim('-');

            return slug;
        }

        /// <summary>
        /// Masks sensitive information in string
        /// </summary>
        public static string MaskSensitiveData(this string input, int visibleChars = 4, char maskChar = '*')
        {
            if (string.IsNullOrEmpty(input) || input.Length <= visibleChars)
                return input;

            var visiblePart = input.Substring(0, visibleChars);
            var maskedPart = new string(maskChar, input.Length - visibleChars);

            return visiblePart + maskedPart;
        }

        /// <summary>
        /// Extracts numbers from string
        /// </summary>
        public static IEnumerable<int> ExtractNumbers(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return Enumerable.Empty<int>();

            var matches = Regex.Matches(input, @"-?\d+");
            return matches.Select(m => int.Parse(m.Value));
        }

        /// <summary>
        /// Removes HTML tags from string
        /// </summary>
        public static string StripHtmlTags(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            return Regex.Replace(input, @"<[^>]*>", "");
        }

        /// <summary>
        /// Converts string to proper JSON format
        /// </summary>
        public static T? FromJson<T>(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return default(T);

            try
            {
                return JsonSerializer.Deserialize<T>(input);
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// Converts object to JSON string
        /// </summary>
        public static string ToJson<T>(this T obj, bool indented = false)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = indented
            };

            return JsonSerializer.Serialize(obj, options);
        }

        /// <summary>
        /// Pluralizes a word based on count
        /// </summary>
        public static string Pluralize(this string word, int count)
        {
            if (count == 1)
                return word;

            // Simple pluralization rules
            if (word.EndsWith("y"))
                return word.Substring(0, word.Length - 1) + "ies";
            
            if (word.EndsWith("s") || word.EndsWith("sh") || word.EndsWith("ch") || word.EndsWith("x") || word.EndsWith("z"))
                return word + "es";
            
            return word + "s";
        }

        /// <summary>
        /// Generates a random string of specified length
        /// </summary>
        public static string GenerateRandomString(int length, bool includeNumbers = true, bool includeSpecialChars = false)
        {
            const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            const string numbers = "0123456789";
            const string specialChars = "!@#$%^&*()_+-=[]{}|;:,.<>?";

            var chars = letters;
            if (includeNumbers) chars += numbers;
            if (includeSpecialChars) chars += specialChars;

            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
