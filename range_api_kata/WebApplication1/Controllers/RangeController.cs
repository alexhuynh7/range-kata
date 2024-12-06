using Microsoft.AspNetCore.Mvc;
using range_kata.Models;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace range_api_kata.Controllers
{
    [ApiController]
    [Route("api/range")]
    public class RangeController : Controller
    {
        [HttpGet("contains")]
        public IActionResult Contains([FromQuery] string rangeString, [FromQuery] string value)
        {
            if (string.IsNullOrWhiteSpace(rangeString) || string.IsNullOrWhiteSpace(value))
            {
                return BadRequest("Both rangeString and value are required.");
            }

            try
            {
                // Parse range and value generically without needing to know the specific type
                var range = ParseRangeDynamic(rangeString, value);

                if (range == null)
                {
                    return BadRequest("Unable to parse the range or value.");
                }

                bool contains = range.Contains(value);
                return Ok(new { Contains = contains });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private dynamic ParseRangeDynamic(string rangeString, string value)
        {
            // Using a converter for any type by inferring the type from the input `value` string
            var converter = TypeDescriptor.GetConverter(value.GetType());
            if (!converter.CanConvertFrom(typeof(string)))
            {
                throw new InvalidOperationException($"No converter available for type: {value.GetType()}");
            }

            // Convert the value string to the inferred type
            var parsedValue = converter.ConvertFromString(null, CultureInfo.InvariantCulture, value);

            if (parsedValue == null)
            {
                throw new ArgumentException("Parsed value cannot be null.");
            }

            // Use reflection to dynamically call the generic Range.Parse method with inferred type
            Type rangeType = typeof(Range<>).MakeGenericType(parsedValue.GetType());
            MethodInfo parseMethod = rangeType.GetMethod("Parse", BindingFlags.Static | BindingFlags.Public);

            if (parseMethod == null)
            {
                throw new InvalidOperationException("Parse method not found on Range type.");
            }

            return parseMethod.Invoke(null, new object[] { rangeString });
        }
    }
}
