# Range kata


Implement a class `Range` to present a range of elements having natural order.

To create a `Range` instance, simply give it a `lowerBound` and a `upperBound`. A `Range` can be used to check if it contains a value.

## Requirements

- Develop the `Range` class from level to level. Please note that you will only use the `Range` class to create different ranges, you could create many `classes` to support your implementation but you should not use them in test cases.
- You must add more tests in `RangeTest.cs` as you progress through the levels. There are several failing tests written so that you can get started quickly for Level 1.
- All tests in `RangeTest` MUST pass.
- Each level MUST be a completed by a Git commit.
- Please commit **directly** to the `master` or `main` branch.

## Class `Range` *public* API

```cs
public class Range 
{
  public static Range Of(int lowerBound, int upperBound);

  public boolean Contains(int value);

  public int LowerBound();

  public int UpperBound();
}
```

## Level 1 -  A Range for Numbers

Class `Range` can be used with `int`.

```
Range validAgesForWatchingPorn = Range.Of(13, 100);

validAgesForWatchingPorn.Contains(5); // false
validAgesForWatchingPorn.Contains(13); // true
validAgesForWatchingPorn.Contains(22); // true
validAgesForWatchingPorn.Contains(100); // true
validAgesForWatchingPorn.Contains(101); // false
```

The following constraints **MUST** be implemented:

- `Range` must be *immutable*. Once created, there is no way to change its `LowerBound` and `UpperBound`.
- `Range` must provide a _static factory method_ namely `Of(int, int)` to create a new instance.
- It is not allowed to create a `Range` with `lowerBound > upperBound`.
- The method `Contains(x)` must returns `true` only if 
`lowerBound <= x <=upperBound`.

## Level 2 - More Types of `Range`

Mathmatically, a `Range` can be `Open`, `Closed`, `OpenClosed` or `ClosedOpen`.

```
// open range excludes both bounds
// 4..5(..6..)7..8..9..10
//      ^^^^^
(5, 7)


// closed range includes both bounds
// 4..[5..6..7]..8..9..10
//    ^^^^^^^^^
[5, 7]

// open closed excludes lowerbound but includes upperbound
// 4..5(..6..7]..8..9..10
//     ^^^^^^^^
(5, 7]

// closed open includes lowerbound but excludes upperbound
// 4..[5..6..)7..8..9..10
//    ^^^^^^^
[5, 7)

```

- Extend `Range` such that it can support all of the above types. (This implies the method `Of(int,int`) will be renamed to `Closed(int, int)`)

```
Range open = Range.Open(5, 7);
open.Contains(5); //false

Range closed = Range.Closed(5, 7);
closed.Contains(5); // true

Range openClosed = Range.OpenClosed(5, 7);
openClosed.Contains(5); // false
openClosed.Contains(7); // true

Range closedOpen = Range.ClosedOpen(5, 7);
closedOpen.Contains(5); // true;
closedOpen.Contains(7); // false;

```


## Level 3 - Make it generic with all `IComparable` types

Extends the `Range` such that it can supports any types implementing `IComparable` interface.

The `IComparable` interface is implemented by several C# types (i.e `string`, )


```
Range<string> text = Range.Open("abc", "xyz");

Range<Decimal> decimals = Range.Open(Decimal.Parse("1.32432"), Decimal.Parse("1.324323423423423423423"));

Range<DateOnly> dates = Range.Closed(DateOnly.MinValue, DateOnly.MaxValue);

```

## Level 4 - Open-ended `Range`s

We want to have `Range` to support an open-ended style with `Infinitive`. For example:

```
Range<int> lessThanFive = Range.LessThan(5); // [Infinitive, 5)
lessThanFive.Contains(5); // false;
lessThanFive.Contains(-9000); // true;

Range<int> atLeastFive = Range.AtLeast(5); // [5, Infinitive]
atLeastFive.Contains(5); // true;
atLeastFive.Contains(4); // false;

Range<int> atMostFive = Range.AtMost(5); // [Infinitive, 5]
atMostFive.Contains(5); // true
atMostFive.Contains(-234234); // true;
atMostFive.Contains(6); // false;


Range<DateTime> afterToday = Range.GreaterThan(DateTime.Today); // (today, Infinitive]
afterToday.Contains(DateTime.Today.AddDays(1)); // true;
afterToday.Contains(DateTime.Parse("2024-01-01")); // false;


Range<string> all = Range.All(); // [Infinitive, Infinitive]
all.Contains("anything"); // true;
all.Contains(""); // true;
all.Contains(null); // true;
```

## Level 5 - Introduce useful `ToString()`

Implement `ToString()` method for class `Range`. The `ToString()` should represent the type and the bounds of the current `Range` instance.

```cs
Range<int> lessThen100 = Range.LessThen(100);
Assert.Equal("[Infinitive, 100)", lessThen100.ToString())

Range<DateTime> within2024 = Range.Closed(
  DateTime.Parse("2024-01-01"),
  DateTime.Parse("2024-12-31")
)

Assert.Equal("[2024-01-01, 2024-12-31]", within2024.ToString()) 
```

## Level 6 - Parse a `Range` notation.

It is possible to create a new `Range` using the result from `Range#ToString()`.

This level tests your API design skills so we won't provide much information here. Please decide the signature of `Parse` yourself.

```cs
string rangeString = Range.LessThan(100).ToString();
Range<int> lessThan100 = Range.Parse(rangeString, ...);
assert lessThan100.ToString() == "[Infinitive, 100)"
assert lessThan100.Contains(99);
```

## Level 7 - `Range` as an HTTP API

It should be possible to use `Range`'s most important feature, `Contains`, via an HTTP API at `/api/range`.

This level tests your experience in working with a web application. The requirements are quite simple:

- The HTTP API is accessible at `/api/range/*`. Additional sub paths and/or path parameters are free to use.
- The HTTP API should receive: (1) a String representation of a `Range` for a supported type and (2) a value of that type.
- The HTTP API should be able to re-construct a `Range` instance and checks if it (the range) contains the specified value. The response's body should contain such rersult.

Your are free to:

- Use any of .NET libraries which you are familiar with.
- Decide the details of the HTTP API (e.g HTTP method, sub paths, type of request, type of response etc). It is not required at all make it RESTful.

Bonus Points:

- There is a accompanying Integration Test for this feature.
- There is an Swagger OpenAPI Specification (v3) for the HTTP API.

## Meta

v2024.10.07
- Initial a range-kata coding challenge for .NET"# range-kata" 
