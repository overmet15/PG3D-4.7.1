public class ResPath
{
	public static string Combine(string a, string b)
	{
		if (a == null)
		{
			a = string.Empty;
		}
		if (b == null)
		{
			b = string.Empty;
		}
		return a + "/" + b;
	}
}
