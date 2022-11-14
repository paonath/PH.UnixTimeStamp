#region

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

#endregion

namespace PH.UnixTimeStamp
{
	/// <summary>
	/// Unix Time Stamp with Precision on Seconds
	/// </summary>
	/// <seealso cref="System.IComparable" />
	/// <seealso cref="System.IComparable&lt;System.Double&gt;" />
	/// <seealso cref="System.IEquatable&lt;System.Double&gt;" />
	/// <seealso cref="System.IComparable&lt;PH.UnixTimeStamp.Uts&gt;" />
	/// <seealso cref="System.IEquatable&lt;PH.UnixTimeStamp.Uts&gt;" />
	/// <seealso cref="System.Runtime.Serialization.ISerializable" />
	[StructLayout(LayoutKind.Auto)]
	[Serializable]
	[JsonConverter(typeof(UtsNewtownsoftJsonConverter))]
	[System.Text.Json.Serialization.JsonConverter(typeof(UtsSystemJsonConverter))]
	public readonly struct Uts : IComparable, IComparable<double>, IEquatable<double>, IComparable<Uts>, IEquatable<Uts> , ISerializable
	{
		public static Precision Precision => Precision.Seconds;
		public static Uts       MinValue  => new(0);
		public static Uts       Now       => new(DateTime.UtcNow - UnixEpoch);
		public static Uts FromDateTime(DateTime dateTime) => GetFromDateTime(dateTime);


		
		private static Uts GetFromDateTime(DateTime dateTime)
		{
			if (dateTime.Kind == DateTimeKind.Unspecified)
			{
				throw new ArgumentException("Unable to convert Unspecified kind of DateTime", nameof(dateTime));
			}

			if (dateTime.Kind == DateTimeKind.Local)
			{
				dateTime = dateTime.ToUniversalTime();
			}

			return new Uts(dateTime - UnixEpoch);
		}

		
		public static readonly DateTime UnixEpoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		private readonly        double   Value;

		public Uts(double value):this()
		{
			if (value < (double)0)
			{
				throw new ArgumentOutOfRangeException(nameof(value), "Minimum allowed value is 0");
			}

			Value = value;

		}

		



		/// <summary>
		///   Initializes a new instance of the <see cref="Uts" /> struct.
		/// </summary>
		/// <exception cref="OverflowException">The TimeSpan must be a non-negative difference</exception>
		/// <param name="differenceFromUnixStartTime">The difference from unix start time.</param>
		internal Uts(TimeSpan differenceFromUnixStartTime) :
			this(differenceFromUnixStartTime.TotalSeconds)
		{
		}

		public Uts Add(TimeSpan timeSpan) => new(Value + timeSpan.TotalSeconds);
		public Uts Add(double value) => new(Value      + value);
		




		/// <summary>
		///   Performs an implicit conversion from <see cref="System.Double" /> to <see cref="Uts" />.
		/// </summary>
		/// <param name="l">The double value.</param>
		/// <returns>
		///   The result of the conversion.
		/// </returns>
		public static implicit operator Uts(double l) => new(l);

		/// <summary>
		/// Performs an implicit conversion from <see cref="Uts"/> to <see cref="System.Double"/>.
		/// </summary>
		/// <param name="unixTimeStamp">The u.</param>
		/// <returns>
		/// The result of the conversion.
		/// </returns>
		public static implicit operator double(Uts unixTimeStamp) => unixTimeStamp.ToDouble();


		

		/// <summary>
		///   Performs an implicit conversion from <see cref="DateTime" /> to <see cref="Uts" />.
		/// </summary>
		/// <param name="d">The date time.</param>
		/// <exception cref="ArgumentException">Thrown if given DateTime is DateTimeKind.Unspecified</exception>
		/// <returns>
		///   The result of the conversion.
		/// </returns>
		public static implicit operator Uts(DateTime d)
		{
			switch (d.Kind)
			{
				case DateTimeKind.Unspecified:
					throw new ArgumentException("Cannot convert DateTimeKind.Unspecified", nameof(d));
				case DateTimeKind.Utc:
					return new Uts(d - UnixEpoch);
				case DateTimeKind.Local:
					return new Uts(d.ToUniversalTime() - UnixEpoch);
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		/// <summary>Returns the hash code for this instance.</summary>
		/// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
		public override int GetHashCode() => Value.GetHashCode();

		/// <summary>Returns the fully qualified type name of this instance.</summary>
		/// <returns>The fully qualified type name.</returns>
		public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);


		/// <summary>
		///   Compares the current instance with another object of the same type and returns an integer that indicates
		///   whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
		/// </summary>
		/// <param name="obj">An object to compare with this instance.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="obj" /> is not the same type as this instance.
		/// </exception>
		/// <returns>
		///   A value that indicates the relative order of the objects being compared. The return value has these meanings:
		///   <list type="table">
		///     <listheader>
		///       <term> Value</term><description> Meaning</description>
		///     </listheader>
		///     <item>
		///       <term> Less than zero</term>
		///       <description> This instance precedes <paramref name="obj" /> in the sort order.</description>
		///     </item>
		///     <item>
		///       <term> Zero</term>
		///       <description> This instance occurs in the same position in the sort order as <paramref name="obj" />.</description>
		///     </item>
		///     <item>
		///       <term> Greater than zero</term>
		///       <description> This instance follows <paramref name="obj" /> in the sort order.</description>
		///     </item>
		///   </list>
		/// </returns>
		public int CompareTo(object? obj)
		{
			if (null == obj)
			{
				return 1;
			}

			if (obj is Uts u)
			{
				return CompareTo(u);
			}

			var t = (double)obj;
			return CompareTo(t);
		}

		
		/// <summary>
		///   Converts the value of this instance to an equivalent <see cref="T:System.String" /> using the specified
		///   culture-specific formatting information.
		/// </summary>
		/// <param name="provider">
		///   An <see cref="T:System.IFormatProvider" /> interface implementation that supplies
		///   culture-specific formatting information.
		/// </param>
		/// <returns>A <see cref="T:System.String" /> instance equivalent to the value of this instance.</returns>
		public string ToString(IFormatProvider? provider) => Convert.ToString(Value, provider);
		
		


		/// <summary>
		///   Compares the current instance with another object of the same type and returns an integer that indicates
		///   whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
		/// </summary>
		/// <param name="other">An object to compare with this instance.</param>
		/// <returns>
		///   A value that indicates the relative order of the objects being compared. The return value has these meanings:
		///   <list type="table">
		///     <listheader>
		///       <term> Value</term><description> Meaning</description>
		///     </listheader>
		///     <item>
		///       <term> Less than zero</term>
		///       <description> This instance precedes <paramref name="other" /> in the sort order.</description>
		///     </item>
		///     <item>
		///       <term> Zero</term>
		///       <description> This instance occurs in the same position in the sort order as <paramref name="other" />.</description>
		///     </item>
		///     <item>
		///       <term> Greater than zero</term>
		///       <description> This instance follows <paramref name="other" /> in the sort order.</description>
		///     </item>
		///   </list>
		/// </returns>
		public int CompareTo(double other) => Value.CompareTo(other);

		/// <summary>Returns the <see cref="T:System.TypeCode" /> for this instance.</summary>
		/// <returns>
		///   The enumerated constant that is the <see cref="T:System.TypeCode" /> of the class or value type that
		///   implements this interface.
		/// </returns>
		public TypeCode GetTypeCode() => Value.GetTypeCode();


		public double ToDouble() => Value;

		public TimeSpan ToTimeSpan() => TimeSpan.FromSeconds(Value);
		public DateTime ToDateTime() => UnixEpoch.AddSeconds(Value);


		/// <summary>
		///   Compares the current instance with another object of the same type and returns an integer that indicates
		///   whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
		/// </summary>
		/// <param name="other">An object to compare with this instance.</param>
		/// <returns>
		///   A value that indicates the relative order of the objects being compared. The return value has these meanings:
		///   <list type="table">
		///     <listheader>
		///       <term> Value</term><description> Meaning</description>
		///     </listheader>
		///     <item>
		///       <term> Less than zero</term>
		///       <description> This instance precedes <paramref name="other" /> in the sort order.</description>
		///     </item>
		///     <item>
		///       <term> Zero</term>
		///       <description> This instance occurs in the same position in the sort order as <paramref name="other" />.</description>
		///     </item>
		///     <item>
		///       <term> Greater than zero</term>
		///       <description> This instance follows <paramref name="other" /> in the sort order.</description>
		///     </item>
		///   </list>
		/// </returns>
		public int CompareTo(Uts other) => CompareTo(other.Value);

		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>
		///   <see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise,
		///   <see langword="false" />.
		/// </returns>
		public bool Equals(Uts other) => GetHashCode() == other.GetHashCode();

		

		/// <summary>Indicates whether this instance and a specified object are equal.</summary>
		/// <param name="obj">The object to compare with the current instance.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="obj" /> and this instance are the same type and represent the same value;
		///   otherwise, <see langword="false" />.
		/// </returns>
		public override bool Equals(object? obj) => obj is Uts other && Equals(other);

		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>
		///   <see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise,
		///   <see langword="false" />.
		/// </returns>
		public bool Equals(double other) => Value.Equals(other);

		private Uts(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException(nameof(info));
			}

			Value = 0;
			bool found = false;

			// Get the data
			SerializationInfoEnumerator enumerator = info.GetEnumerator();
			while (enumerator.MoveNext())
			{

				//if (enumerator.Name == "Value")
				//{
				//	Value = Convert.ToDouble(enumerator.Value, CultureInfo.InvariantCulture);
				//	found = true;
				//}
				Value = Convert.ToDouble(enumerator.Value, CultureInfo.InvariantCulture);
				found = true;
			}

			if (!found)
			{
				throw new SerializationException("Missing double Value");
			}
		}

		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException(nameof(info));
			}

			info.AddValue(string.Empty, Value, typeof(double));
		}



		public static bool operator ==(Uts left, Uts right) => left.Value == right.Value;

		public static bool operator !=(Uts left, Uts right) => !(left == right);

		public static bool operator <(Uts left, Uts right) => left.CompareTo(right) == -1;

		public static bool operator <=(Uts left, Uts right)
		{
			var t = left.CompareTo(right);
			if (t == -1 || t == 0)
			{
				return true;
			}

			return false;
		}


		public static bool operator >(Uts left, Uts right) => left.CompareTo(right) == 1;

		public static bool operator >=(Uts left, Uts right)
		{
			var t = left.CompareTo(right);
			if (t == 1 || t == 0)
			{
				return true;
			}

			return false;
		}
	}
}