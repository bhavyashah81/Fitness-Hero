using System;

namespace FitnessAppAPI.Utilities
{
    /// <summary>
    /// Helper class for mathematical calculations
    /// </summary>
    public static class MathHelper
    {
        /// <summary>
        /// Calculates the average of an array of numbers
        /// </summary>
        public static double CalculateAverage(params double[] numbers)
        {
            if (numbers == null || numbers.Length == 0)
                return 0;
                
            double sum = 0;
            foreach (var number in numbers)
            {
                sum += number;
            }
            
            return sum / numbers.Length;
        }

        /// <summary>
        /// Calculates the median of an array of numbers
        /// </summary>
        public static double CalculateMedian(params double[] numbers)
        {
            if (numbers == null || numbers.Length == 0)
                return 0;
                
            Array.Sort(numbers);
            int middle = numbers.Length / 2;
            
            if (numbers.Length % 2 == 0)
            {
                return (numbers[middle - 1] + numbers[middle]) / 2.0;
            }
            else
            {
                return numbers[middle];
            }
        }

        /// <summary>
        /// Calculates the standard deviation of an array of numbers
        /// </summary>
        public static double CalculateStandardDeviation(params double[] numbers)
        {
            if (numbers == null || numbers.Length == 0)
                return 0;
                
            double average = CalculateAverage(numbers);
            double sumOfSquaredDifferences = 0;
            
            foreach (var number in numbers)
            {
                double difference = number - average;
                sumOfSquaredDifferences += difference * difference;
            }
            
            double variance = sumOfSquaredDifferences / numbers.Length;
            return Math.Sqrt(variance);
        }

        /// <summary>
        /// Calculates the percentage change between two values
        /// </summary>
        public static double CalculatePercentageChange(double oldValue, double newValue)
        {
            if (oldValue == 0)
                return newValue == 0 ? 0 : 100;
                
            return ((newValue - oldValue) / oldValue) * 100;
        }

        /// <summary>
        /// Rounds a number to the specified number of decimal places
        /// </summary>
        public static double RoundToDecimalPlaces(double value, int decimalPlaces)
        {
            return Math.Round(value, decimalPlaces);
        }

        /// <summary>
        /// Calculates the distance between two points
        /// </summary>
        public static double CalculateDistance(double x1, double y1, double x2, double y2)
        {
            double deltaX = x2 - x1;
            double deltaY = y2 - y1;
            return Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
        }

        /// <summary>
        /// Converts degrees to radians
        /// </summary>
        public static double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }

        /// <summary>
        /// Converts radians to degrees
        /// </summary>
        public static double RadiansToDegrees(double radians)
        {
            return radians * 180.0 / Math.PI;
        }

        /// <summary>
        /// Calculates the factorial of a number
        /// </summary>
        public static long CalculateFactorial(int n)
        {
            if (n < 0)
                throw new ArgumentException("Factorial is not defined for negative numbers");
                
            if (n == 0 || n == 1)
                return 1;
                
            long result = 1;
            for (int i = 2; i <= n; i++)
            {
                result *= i;
            }
            
            return result;
        }

        /// <summary>
        /// Checks if a number is prime
        /// </summary>
        public static bool IsPrime(int number)
        {
            if (number <= 1)
                return false;
                
            if (number <= 3)
                return true;
                
            if (number % 2 == 0 || number % 3 == 0)
                return false;
                
            for (int i = 5; i * i <= number; i += 6)
            {
                if (number % i == 0 || number % (i + 2) == 0)
                    return false;
            }
            
            return true;
        }

        /// <summary>
        /// Calculates the greatest common divisor of two numbers
        /// </summary>
        public static int CalculateGCD(int a, int b)
        {
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        /// <summary>
        /// Calculates the least common multiple of two numbers
        /// </summary>
        public static int CalculateLCM(int a, int b)
        {
            return Math.Abs(a * b) / CalculateGCD(a, b);
        }

        /// <summary>
        /// Clamps a value between a minimum and maximum
        /// </summary>
        public static T Clamp<T>(T value, T min, T max) where T : IComparable<T>
        {
            if (value.CompareTo(min) < 0)
                return min;
            if (value.CompareTo(max) > 0)
                return max;
            return value;
        }

        /// <summary>
        /// Linear interpolation between two values
        /// </summary>
        public static double Lerp(double a, double b, double t)
        {
            return a + (b - a) * Clamp(t, 0.0, 1.0);
        }
    }
}
