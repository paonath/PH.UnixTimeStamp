# PH.UnixTimeStamp [![NuGet Badge](https://buildstats.info/nuget/PH.UnixTimeStamp)](https://www.nuget.org/packages/PH.UnixTimeStamp/)
c# Unix Time Stamp

This package provide 2 struct: `PH.UnixTimeStamp.Uts` and `PH.UnixTimeStamp.MillisecsUts`

## Uts: Unix Time Stamp with Precision on Seconds


```csharp

//Uts represents the difference in seconds since January 1, 1970
var u = PH.UnixTimeStamp.Uts.MinValue;
Console.WriteLine("Min value: '{0}'", u); 
// Min value: '0'

DateTime dt = new DateTime(1970, 1, 2, 0, 0, 0, DateTimeKind.Utc);
var aDay = PH.UnixTimeStamp.Uts.FromDateTime(dt);
Console.WriteLine("aDay value is '{0}'", aDay); //aDay value is '86400'


long      ticks     = (long)1000000 * 1000000 * 630000;
DateTime d = new DateTime(ticks , DateTimeKind.Local);
DateTime invalidDt = new DateTime(ticks , DateTimeKind.Unspecified);



var valid = PH.UnixTimeStamp.Uts.FromDateTime(d);
Console.WriteLine("Valid as ISO DATE is '{0}'", valid.ToDateTime().ToString("O")); 
//Valid as ISO DATE is '1997-05-23T14:00:00.0000000Z'

try
{	   
	
	//this throw exception: you can use only DateTimeKind.Utc  or DateTimeKind.Local
	var invalid = PH.UnixTimeStamp.Uts.FromDateTime(invalidDt);
}
catch (Exception ex)
{
	Console.WriteLine("Exception {0} - {1}", ex.Message , ex.GetType());
	// Exception Unable to convert Unspecified kind of DateTime (Parameter 'dateTime') - System.ArgumentException
}

```



## MillisecsUts : Unix Time Stamp with Precision on Milliseconds

```csharp
//TODO: write!
```