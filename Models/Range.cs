using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System;

namespace range_kata.Models
{
    public class Range<T> where T : IComparable<T>
    {
        private readonly object? lowerBound;
        private readonly object? upperBound;
        private readonly bool includeLower;
        private readonly bool includeUpper;


        /**
        * Constructor is private BY DESIGN.
        *
        * TODO: Change the constructor to meet your requirements.
        */
        private Range(object? lowerBound, object? upperBound, bool includeLower, bool includeUpper)
        {
            this.lowerBound = lowerBound;
            this.upperBound = upperBound;
            this.includeLower = includeLower;
            this.includeUpper = includeUpper;
        }

        /**
         * Creates a new <b>closed</b> {@code Range} that includes both bounds.
         */
        public static Range<T> Of(T lowerBound, T upperBound)
        {
            if (lowerBound.CompareTo(upperBound) > 0)
            {
                throw new ArgumentException("lowerBound cannot be greater than upperBound.");
            }
            return new Range<T>(lowerBound, upperBound, true, true);
        }

        /**
         * Returns {@code true} on if the given {@code value} is contained in this
         * {@code Range}.
         */
        public bool Contains(object value)
        {
            bool lowerCheck, upperCheck;
            var newValue = (T)value;
            if (lowerBound == null) lowerCheck = true;
            else lowerCheck = includeLower ? newValue.CompareTo((T)lowerBound) >= 0 : newValue.CompareTo((T)lowerBound) > 0;

            if (upperBound == null) upperCheck = true;
            else upperCheck = includeUpper ? newValue.CompareTo((T)upperBound) <= 0 : newValue.CompareTo((T)upperBound) < 0;

            return lowerCheck && upperCheck;
        }

        /**
         * Returns the {@code LowerBound} of this {@code Range}.
         */
        public object? LowerBound()
        {
            return lowerBound;
        }

        /**
         * Returns the {@code UpperBound} of this {@code Range}.
         */
        public object? UpperBound()
        {
            return upperBound;
        }

        public static Range<T> Closed(T lowerBound, T upperBound)
        {
            if (lowerBound.CompareTo(upperBound) > 0)
            {
                throw new ArgumentException("lowerBound cannot be greater than upperBound.");
            }
            return new Range<T>(lowerBound, upperBound, true, true);
        }

        public static Range<T> Open(T lowerBound, T upperBound)
        {
            if (lowerBound.CompareTo(upperBound) >= 0)
            {
                throw new ArgumentException("lowerBound must be less than upperBound.");
            }
            return new Range<T>(lowerBound, upperBound, false, false);
        }

        public static Range<T> OpenClosed(T lowerBound, T upperBound)
        {
            if (lowerBound.CompareTo(upperBound) >= 0)
            {
                throw new ArgumentException("lowerBound must be less than upperBound.");
            }
            return new Range<T>(lowerBound, upperBound, false, true);
        }

        public static Range<T> ClosedOpen(T lowerBound, T upperBound)
        {
            if (lowerBound.CompareTo(upperBound) >= 0)
            {
                throw new ArgumentException("lowerBound must be less than upperBound.");
            }
            return new Range<T>(lowerBound, upperBound, true, false);
        }

        // Static factory method for open-ended range [Infinitive, upperBound)
        public static Range<T> LessThan(T upperBound)
        {
            return new Range<T>(null, upperBound, false, false);
        }

        // Static factory method for open-ended range [lowerBound, Infinitive]
        public static Range<T> AtLeast(T lowerBound)
        {
            return new Range<T>(lowerBound, null, true, false);
        }

        // Static factory method for open-ended range [Infinitive, upperBound]
        public static Range<T> AtMost(T upperBound)
        {
            return new Range<T>(null, upperBound, false, true);
        }

        // Static factory method for open-ended range (lowerBound, Infinitive]
        public static Range<T> GreaterThan(T lowerBound)
        {
            return new Range<T>(lowerBound, null, false, false);
        }

        // Static factory method for an unrestricted range (Infinitive, Infinitive)
        public static Range<T> All()
        {
            return new Range<T>(null, null, false, false);
        }

        public override string ToString()
        {
            string lowerBoundStr = lowerBound == null ? "Infinitive" : lowerBound.ToString();
            string upperBoundStr = upperBound == null ? "Infinitive" : upperBound.ToString();

            string lowerSymbol = includeLower || lowerBoundStr == "Infinitive" ? "[" : "(";
            string upperSymbol = includeUpper || upperBoundStr == "Infinitive" ? "]" : ")";

            return $"{lowerSymbol}{lowerBoundStr}, {upperBoundStr}{upperSymbol}";
        }

        public static Range<T> Parse(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentException("Input string cannot be null or empty", nameof(input));

            // Determine inclusiveness from symbols
            bool includeLower = input.StartsWith("[");
            bool includeUpper = input.EndsWith("]");

            // Remove brackets
            string innerString = input.Substring(1, input.Length - 2);

            // Split bounds
            string[] bounds = innerString.Split(", ");
            if (bounds.Length != 2)
                throw new FormatException("Invalid range format");


            T lowerBound, upperBound;

            // Check if bounds are missing
            if (bounds[0] == "Infinitive" && bounds[1] == "Infinitive")
                return Range<T>.All();

            if (bounds[1] == "Infinitive")
            {
                lowerBound = (T)Convert.ChangeType(bounds[0], typeof(T));

                if (includeLower)
                    return Range<T>.AtLeast(lowerBound);
                else
                    return Range<T>.GreaterThan(lowerBound);
            }

            if (bounds[0] == "Infinitive")
            {
                upperBound = (T)Convert.ChangeType(bounds[1], typeof(T));
                if (includeUpper)
                    return Range<T>.AtMost(upperBound);
                else
                    return Range<T>.LessThan(upperBound);
            }

            // Handle null values for 'Infinitive'
            lowerBound = (T)Convert.ChangeType(bounds[0], typeof(T));
            upperBound = (T)Convert.ChangeType(bounds[1], typeof(T));

            // Select appropriate factory method based on bounds inclusiveness
            return (includeLower, includeUpper) switch
            {
                (true, true) => Range<T>.Closed(lowerBound, upperBound),
                (false, false) => Range<T>.Open(lowerBound, upperBound),
                (false, true) => Range<T>.OpenClosed(lowerBound, upperBound),
                (true, false) => Range<T>.ClosedOpen(lowerBound, upperBound)
            };
        }
    }
}
