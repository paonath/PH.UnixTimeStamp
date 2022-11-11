using System.Runtime.InteropServices;
using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace PH.UnixTimeStamp
{
	[StructLayout(LayoutKind.Auto)]
	[Serializable]
	public readonly struct MillisecsUts : IComparable, IComparable<double>, IEquatable<double>, IComparable<MillisecsUts>, IEquatable<MillisecsUts>,
	                                      ISerializable
	{
		public static Precision Precision => Precision.Milliseconds;
		public static MillisecsUts       MinValue  => new(0);
		public static MillisecsUts       Now       => GetInternalNow().Now;
		public static MillisecsUts FromDateTime(DateTime dateTime) => GetFromDateTime(dateTime);


		internal static (MillisecsUts Now, DateTime UtcNow) UtcNow => GetInternalNow();

		private static MillisecsUts GetFromDateTime(DateTime dateTime)
		{
			if (dateTime.Kind == DateTimeKind.Unspecified)
			{
				throw new ArgumentException("Unable to convert Unspecified kind of DateTime", nameof(dateTime));
			}

			if (dateTime.Kind == DateTimeKind.Local)
			{
				dateTime = dateTime.ToUniversalTime();
			}

			return new MillisecsUts(dateTime - UnixEpoch);
		}

		private static (MillisecsUts Now, DateTime UtcNow) GetInternalNow()
		{
			var d = DateTime.UtcNow;
			var n = new MillisecsUts(d - UnixEpoch);
			return (n, d);
		}

		public static readonly DateTime UnixEpoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		public readonly        double   Value;

		public MillisecsUts(double value) : this()
		{
			if (value < (double)0)
			{
				throw new ArgumentOutOfRangeException(nameof(value), "Minimum allowed value is 0");
			}

			Value = value;

		}

		



		/// <summary>
		///   Initializes a new instance of the <see cref="MillisecsUts" /> struct.
		/// </summary>
		/// <exception cref="OverflowException">The TimeSpan must be a non-negative difference</exception>
		/// <param name="differenceFromUnixStartTime">The difference from unix start time.</param>
		internal MillisecsUts(TimeSpan differenceFromUnixStartTime) :
			this(differenceFromUnixStartTime.TotalMilliseconds)
		{
		}

		public MillisecsUts Add(TimeSpan timeSpan) => new(Value + timeSpan.TotalMilliseconds);
		public MillisecsUts Add(double value) => new(Value      + value);
		


		/// <summary>
		///   Performs an implicit conversion from <see cref="System.UInt64" /> to <see cref="MillisecsUts" />.
		/// </summary>
		/// <param name="l">The double value.</param>
		/// <returns>
		///   The result of the conversion.
		/// </returns>
		public static implicit operator MillisecsUts(double l) => new(l);

		/// <summary>
		///   Performs an implicit conversion from <see cref="DateTime" /> to <see cref="MillisecsUts" />.
		/// </summary>
		/// <param name="d">The date time.</param>
		/// <exception cref="ArgumentException">Thrown if given DateTime is DateTimeKind.Unspecified</exception>
		/// <returns>
		///   The result of the conversion.
		/// </returns>
		public static implicit operator MillisecsUts(DateTime d)
		{
			switch (d.Kind)
			{
				case DateTimeKind.Unspecified:
					throw new ArgumentException("Cannot convert DateTimeKind.Unspecified", nameof(d));
				case DateTimeKind.Utc:
					return new MillisecsUts(d - UnixEpoch);
				case DateTimeKind.Local:
					return new MillisecsUts(d.ToUniversalTime() - UnixEpoch);
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

			if (obj is MillisecsUts u)
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


		public TimeSpan ToTimeSpan() => TimeSpan.FromMilliseconds(Value);
		public DateTime ToDateTime() => UnixEpoch.AddMilliseconds(Value);


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
		public int CompareTo(MillisecsUts other) => CompareTo(other.Value);

		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>
		///   <see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise,
		///   <see langword="false" />.
		/// </returns>
		public bool Equals(MillisecsUts other) => GetHashCode() == other.GetHashCode();

		

		/// <summary>Indicates whether this instance and a specified object are equal.</summary>
		/// <param name="obj">The object to compare with the current instance.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="obj" /> and this instance are the same type and represent the same value;
		///   otherwise, <see langword="false" />.
		/// </returns>
		public override bool Equals(object? obj) => obj is MillisecsUts other && Equals(other);

		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>
		///   <see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise,
		///   <see langword="false" />.
		/// </returns>
		public bool Equals(double other) => Value.Equals(other);

		private MillisecsUts(SerializationInfo info, StreamingContext context)
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
				if (enumerator.Name == "Value")
				{
					Value = Convert.ToDouble(enumerator.Value, CultureInfo.InvariantCulture);
					found = true;
				}

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

			info.AddValue("Value", Value);
		}



		public static bool operator ==(MillisecsUts left, MillisecsUts right) => left.Value == right.Value;

		public static bool operator !=(MillisecsUts left, MillisecsUts right) => !(left == right);

		public static bool operator <(MillisecsUts left, MillisecsUts right) => left.CompareTo(right) == -1;

		public static bool operator <=(MillisecsUts left, MillisecsUts right)
		{
			var t = left.CompareTo(right);
			if (t == -1 || t == 0)
			{
				return true;
			}

			return false;
		}


		public static bool operator >(MillisecsUts left, MillisecsUts right) => left.CompareTo(right) == 1;

		public static bool operator >=(MillisecsUts left, MillisecsUts right)
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
