using System;
using System.Buffers;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PH.UnixTimeStamp
{
	public class UtsSystemJsonConverter : JsonConverter<Uts>
	{
		/// <summary>Reads and converts the JSON to type <typeparamref name="T" />.</summary>
		/// <param name="reader">The reader.</param>
		/// <param name="typeToConvert">The type to convert.</param>
		/// <param name="options">An object that specifies serialization options to use.</param>
		/// <returns>The converted value.</returns>
		public override Uts Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{

			if (reader.TokenType == JsonTokenType.String)
			{
				// try to parse number directly from bytes
				ReadOnlySpan<byte> span = reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan;
				if (Utf8Parser.TryParse(span, out double number, out int bytesConsumed) && span.Length == bytesConsumed)
				{
					return new Uts(number);
				}
					

				// try to parse from a string if the above failed, this covers cases with other escaped/UTF characters
				if (double.TryParse(reader.GetString(), out number))
				{
					return new Uts(number);
				}
			}

			var r = reader.GetDouble();
			return new Uts(r);
		}

		/// <summary>Writes a specified value as JSON.</summary>
		/// <param name="writer">The writer to write to.</param>
		/// <param name="value">The value to convert to JSON.</param>
		/// <param name="options">An object that specifies serialization options to use.</param>
		public override void Write(Utf8JsonWriter writer, Uts value, JsonSerializerOptions options)
		{
			writer.WriteNumberValue(value.ToDouble());
		}
	}
}