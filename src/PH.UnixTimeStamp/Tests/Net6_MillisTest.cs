using PH.UnixTimeStamp;

namespace Tests
{
	public class Net6_MillisTest
	{
		
	
		internal static DateTime UnixMinValue  = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		internal static ulong    UlongMinValue = 0;


		[Fact]
		public void Main()
		{
			var u = PH.UnixTimeStamp.MillisecsUts.MinValue;

			var now = PH.UnixTimeStamp.MillisecsUts.Now;
			DateTime utcNow = DateTime.UtcNow;


			DateTime nowLocal = DateTime.Now;

			var fromADate = PH.UnixTimeStamp.MillisecsUts.FromDateTime(nowLocal);

			MillisecsUts  implicitMillisecsUts = nowLocal;

			var fromAdd = implicitMillisecsUts.Add(TimeSpan.FromMinutes(42));
			
			Assert.True(nowLocal.Kind  == DateTimeKind.Local);
			Assert.True(fromADate.ToDateTime().Kind == DateTimeKind.Utc);
			Assert.Equal(UnixMinValue , u.ToDateTime());
			Assert.Equal(UlongMinValue, u.ToDouble());
			Assert.Equal(utcNow, now.ToDateTime(),TimeSpan.FromMilliseconds(499));
			
			Assert.NotNull(@object: implicitMillisecsUts);


			Assert.Equal(nowLocal.AddMinutes(42).ToUniversalTime(),fromAdd.ToDateTime(), TimeSpan.FromMilliseconds(499));
		}

		[Fact]
		public void DateTimeTest()
		{
			var       d = DateTime.Now;
			var       u = MillisecsUts.FromDateTime(d);
			Exception? e = null;
			try
			{
				var unspec = new DateTime(2022, 11, 1, 2, 3, 4, DateTimeKind.Unspecified);
				var uu     = MillisecsUts.FromDateTime(unspec);
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
			var u     = new MillisecsUts(1668151758);
			var minus = new MillisecsUts(42);

			var r = u > minus;

			Assert.True(r);
		}

		[Fact]
		public void Sort()
		{
			var first  = MillisecsUts.MinValue;
			var second = MillisecsUts.FromDateTime(DateTime.Now);
			var last   = MillisecsUts.FromDateTime(DateTime.Now.AddDays(42));

			var l      = new MillisecsUts[] { second, last, first, };

			var ordered = l.OrderBy(x => x).ToArray();
			

			Assert.True(ordered[0] == first);
			Assert.True(ordered[1] == second);
			Assert.True(ordered[2] == last);
		}

		[Fact]
		public void Serialize()
		{
			var now    = DateTime.UtcNow;

			var sample = new ASampleClassMillis(now);

			var newtonJson = Newtonsoft.Json.JsonConvert.SerializeObject(sample);

			var denewtonJson = Newtonsoft.Json.JsonConvert.DeserializeObject<ASampleClassMillis>(newtonJson);
			var eq           = sample.AUts == denewtonJson?.AUts;

			Assert.True(eq);
			Assert.Equal(sample.AUts, denewtonJson?.AUts);
			

		}


		
	}

	public class ASampleClassMillis
	{
		public DateTime                      ADateUtc { get; set; }
		public PH.UnixTimeStamp.MillisecsUts AUts     { get; set; }

		public ASampleClassMillis()
		{

		}

		public ASampleClassMillis(DateTime utc)
		{
			ADateUtc = utc;
			AUts     = PH.UnixTimeStamp.MillisecsUts.FromDateTime(utc);
		}

	}

}