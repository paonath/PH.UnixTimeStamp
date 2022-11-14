using System.Xml.Serialization;
using Microsoft.Data.Sqlite;
using PH.UnixTimeStamp;

namespace Tests
{
	public class Net6Tests
	{
		internal static DateTime UnixMinValue  = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		internal static double    DoubleMinValue = 0;


		[Fact]
		public void Main()
		{
			var u = PH.UnixTimeStamp.Uts.MinValue;

			var now = PH.UnixTimeStamp.Uts.Now;
			DateTime utcNow = DateTime.UtcNow;


			DateTime nowLocal = DateTime.Now;

			var fromADate = PH.UnixTimeStamp.Uts.FromDateTime(nowLocal);

			Uts  implicitUts = nowLocal;

			var fromAdd = implicitUts.Add(TimeSpan.FromMinutes(42));

			Assert.True(nowLocal.Kind               == DateTimeKind.Local);
			Assert.True(fromADate.ToDateTime().Kind == DateTimeKind.Utc);

			Assert.Equal(UnixMinValue , u.ToDateTime());
			Assert.Equal(DoubleMinValue, (double)u);
			Assert.Equal(utcNow, now.ToDateTime(),TimeSpan.FromMilliseconds(499));
			
			Assert.NotNull(@object: implicitUts);
			Assert.Equal(nowLocal.AddMinutes(42).ToUniversalTime(),fromAdd.ToDateTime(), TimeSpan.FromMilliseconds(499));
		}

		[Fact]
		public void DateTimeTest()
		{
			var       d = DateTime.Now;
			var       u = Uts.FromDateTime(d);
			Exception? e = null;
			try
			{
				var unspec = new DateTime(2022, 11, 1, 2, 3, 4, DateTimeKind.Unspecified);
				var uu     = Uts.FromDateTime(unspec);
			}
			catch (Exception exception)
			{
				e = exception;
			}

			Assert.NotNull(e);
			Assert.Equal(d.ToUniversalTime() , u.ToDateTime(), TimeSpan.FromSeconds(100));

		}

		[Fact]
		public void Compare()
		{
			var u     = new Uts(1668151758);
			var minus = new Uts(42);

			var r = u > minus;

			Assert.True(r);
		}

		[Fact]
		public void Sort()
		{
			var first  = Uts.MinValue;
			var second = Uts.FromDateTime(DateTime.Now);
			var last   = Uts.FromDateTime(DateTime.Now.AddDays(42));

			var l      = new Uts[] { second, last, first, };

			var ordered = l.OrderBy(x => x).ToArray();
			

			Assert.True(ordered[0] == first);
			Assert.True(ordered[1] == second);
			Assert.True(ordered[2] == last);
		}

		[Fact]
		public void Serialize()
		{
			var now    = DateTime.UtcNow;

			var sample = new ASampleClass(now);

			var newtonJson = Newtonsoft.Json.JsonConvert.SerializeObject(sample);

			var denewtonJson = Newtonsoft.Json.JsonConvert.DeserializeObject<ASampleClass>(newtonJson);
			var eq           = sample.AUts == denewtonJson?.AUts;

			var system   = System.Text.Json.JsonSerializer.Serialize(sample);
			var deSystem = System.Text.Json.JsonSerializer.Deserialize(system, typeof(ASampleClass)) as ASampleClass;

			

			Assert.True(eq);
			Assert.Equal(sample.AUts, denewtonJson?.AUts);
			Assert.Equal(sample.AUts, deSystem?.AUts);
			

		}


		
	}

	public class ASampleClass
	{
		public DateTime             ADateUtc { get; set; }
		public PH.UnixTimeStamp.Uts AUts     { get; set; }

		public ASampleClass()
		{
			
		}

		public ASampleClass(DateTime utc)
		{
			ADateUtc = utc;
			AUts     = PH.UnixTimeStamp.Uts.FromDateTime(utc);
		}

	}
}