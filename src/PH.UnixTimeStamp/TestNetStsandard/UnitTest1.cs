using System;
using PH.UnixTimeStamp;
using Xunit;

namespace TestNetStsandard
{
	public class UnitTest1
	{
		internal static DateTime UnixMinValue  = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		internal static double    UlongMinValue = 0;



		[Fact]
		public void Main()
		{
			var u = PH.UnixTimeStamp.Uts.MinValue;

			var      now    = PH.UnixTimeStamp.Uts.Now;
			DateTime utcNow = DateTime.UtcNow;


			DateTime nowLocal = DateTime.Now;

			var fromADate = PH.UnixTimeStamp.Uts.FromDateTime(nowLocal);

			Uts implicitUts = nowLocal;

			Assert.Equal(UnixMinValue, u.ToDateTime());
			Assert.Equal(UlongMinValue, u.ToDouble());
			Assert.Equal(utcNow, now.ToDateTime(), TimeSpan.FromMilliseconds(500));

			Assert.NotNull(@object: implicitUts);
		}
	}
}
