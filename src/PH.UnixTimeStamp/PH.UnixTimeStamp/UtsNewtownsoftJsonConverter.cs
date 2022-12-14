using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PH.UnixTimeStamp
{
	public class UtsNewtownsoftJsonConverter : Newtonsoft.Json.JsonConverter<Uts> 
	{
		/// <summary>Writes the JSON representation of the object.</summary>
		/// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
		/// <param name="value">The value.</param>
		/// <param name="serializer">The calling serializer.</param>
		public override void WriteJson(JsonWriter writer, Uts value, JsonSerializer serializer)
		{
			writer.WriteValue((double)value);
		}

		/// <summary>Reads the JSON representation of the object.</summary>
		/// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
		/// <param name="objectType">Type of the object.</param>
		/// <param name="existingValue">The existing value of object being read. If there is no existing value then <c>null</c> will be used.</param>
		/// <param name="hasExistingValue">The existing value has a value.</param>
		/// <param name="serializer">The calling serializer.</param>
		/// <returns>The object value.</returns>
		public override Uts ReadJson(JsonReader reader, Type objectType, Uts existingValue, bool hasExistingValue,
		                             JsonSerializer serializer)
		{
			var value = serializer.Deserialize(reader);
			if (null == value)
			{
				return Uts.MinValue;
			}

			return new Uts(Convert.ToDouble(value));
		}
	}
}