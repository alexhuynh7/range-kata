using range_kata.Models;
using Range = range_kata.Models.Range<int>;

namespace range_kata.Tests
{
    public class RangeTest
    {
        [Fact]
        public void Should_SuccesfullyCreate_Range()
        {
            Range<int> range = Range<int>.Of(5, 50);
            Assert.Equal(5, range.LowerBound());
            Assert.Equal(50, range.UpperBound());
        }

        [Fact]
        public void Should_ThrowException_When_CreateRangeWithLowerBoundBiggerThanUpperBound()
        {
            Assert.Throws<ArgumentException>(() => Range<int>.Of(10, 5));
        }

        [Fact]
        public void ClosedRange_Should_ContainsBothBoundsAndAllElementsBetween()
        {
            Range range = Range<int>.Of(5, 50);
            Assert.False(range.Contains(int.MinValue));
            Assert.False(range.Contains(4));

            Assert.True(range.Contains(5));
            Assert.True(range.Contains(40));
            Assert.True(range.Contains(50));

            Assert.False(range.Contains(51));
            Assert.False(range.Contains(int.MaxValue));
        }

        [Fact]
        public void ClosedRange_ShouldBe_State_Independent()
        {
            Range range1 = Range.Of(5, 10);
            Range range2 = Range.Of(11, 50);
            Assert.True(range1.Contains(10));
            Assert.False(range2.Contains(10));
        }

        [Fact]
        public void TestRange_Level_1()
        {
            Range validAgesForWatchingPorn = Range.Of(13, 100);
            Assert.False(validAgesForWatchingPorn.Contains(5));   // false
            Assert.True(validAgesForWatchingPorn.Contains(13));  // true
            Assert.True(validAgesForWatchingPorn.Contains(22));  // true
            Assert.True(validAgesForWatchingPorn.Contains(100)); // true
            Assert.False(validAgesForWatchingPorn.Contains(101)); // false
        }

        [Fact]
        public void TestRange_Level_2()
        {
            Range open = Range.Open(5, 7);
            Assert.False(open.Contains(5));  // false

            Range closed = Range.Closed(5, 7);
            Assert.True(closed.Contains(5));  // true

            Range openClosed = Range.OpenClosed(5, 7);
            Assert.False(openClosed.Contains(5));  // false
            Assert.True(openClosed.Contains(7));  // true

            Range closedOpen = Range.ClosedOpen(5, 7);
            Assert.True(closedOpen.Contains(5));  // true
            Assert.False(closedOpen.Contains(7));  // false
        }

        [Fact]
        public void TestRange_Level_3()
        {
            Range<string> text = Range<string>.Open("abc", "xyz");
            Assert.True(text.Contains("apple"));  // true
            Assert.False(text.Contains("xyz"));    // false

            Range<decimal> decimals = Range<decimal>.Open(1.32432m, 1.324323423423423423423m);
            Assert.False(decimals.Contains(1.32432m)); // false

            Range<DateOnly> dates = Range<DateOnly>.Closed(DateOnly.MinValue, DateOnly.MaxValue);
            Assert.True(dates.Contains(DateOnly.FromDateTime(DateTime.Now))); // true
        }

        [Fact]
        public void TestRange_Level_4()
        {
            Range<int> lessThanFive = Range.LessThan(5); // [Infinitive, 5)
            Assert.False(lessThanFive.Contains(5)); // false;
            Assert.True(lessThanFive.Contains(-9000)); // true;

            Range<int> atLeastFive = Range.AtLeast(5); // [5, Infinitive]
            Assert.True(atLeastFive.Contains(5)); // true;
            Assert.False(atLeastFive.Contains(4)); // false;

            Range<int> atMostFive = Range.AtMost(5); // [Infinitive, 5]
            Assert.True(atMostFive.Contains(5)); // true
            Assert.True(atMostFive.Contains(-234234)); // true;
            Assert.False(atMostFive.Contains(6)); // false;


            Range<DateTime> afterToday = Range<DateTime>.GreaterThan(DateTime.Today); // (today, Infinitive]
            Assert.True(afterToday.Contains(DateTime.Today.AddDays(1))); // true;
            Assert.False(afterToday.Contains(DateTime.Parse("2024-01-01"))); // false;


            Range<string> all = Range<string>.All(); // [Infinitive, Infinitive]
            Assert.True(all.Contains("anything")); // true;
            Assert.True(all.Contains("")); // true;
            Assert.True(all.Contains(null)); // true;
        }

        [Fact]
        public void TestRange_Level_5()
        {
            Range<int> lessThen100 = Range.LessThan(100);
            Assert.Equal("[Infinitive, 100)", lessThen100.ToString());

            Range<DateTime> within2024 = Range<DateTime>.Closed(DateTime.Parse("2024-01-01"), DateTime.Parse("2024-12-31"));
            // ToString result may vary due to PC date time format, so these unit tests may fail
            Assert.Equal("[1/1/2024 12:00:00 AM, 12/31/2024 12:00:00 AM]", within2024.ToString());
            // Assert.Equal("[2024-01-01, 2024-12-31]", within2024.ToString());
        }

        [Fact]
        public void TestRange_Level_6()
        {
            string rangeString = Range<int>.LessThan(100).ToString();
            Range<int> lessThan100 = Range<int>.Parse(rangeString);
            var strLessThan100 = lessThan100.ToString();

            Assert.Equal("[Infinitive, 100)", strLessThan100);
            Assert.True(lessThan100.Contains(99));

            var within2024String = "[2024-01-01, 2024-12-31]";
            var within2024 = Range<DateTime>.Parse(within2024String);

            Assert.Equal("[1/1/2024 12:00:00 AM, 12/31/2024 12:00:00 AM]", within2024.ToString());
            Assert.True(within2024.Contains(DateTime.Parse("2024-06-01")));
        }
    }
}