namespace Helpers {
	public class Helpers {
		public static int IntInput(string txt) {
			int[]? src = IntInputArray(txt);
			return src == null ? int.MinValue : src[0];
		}

		public static int[]? IntInputArray(string txt) {
			try {
				Console.Write(txt);
				char[] sep = { ' ', '.', ',', ';', '/' };
				string[] src = Console.ReadLine().Split(sep, StringSplitOptions.RemoveEmptyEntries);
				if (src.Length == 0)	return null;
				int[] output = new int[src.Length];
				int i = 0;
				foreach (string s in src)	{
					//Console.WriteLine($"DEBUG: input {i} = {s}");
					checked { output[i] = int.Parse(s); }
					i++;
				}
				return output;
			} catch (OverflowException) {
				Console.WriteLine($"ERROR: input was too high / big or too low");
				return null;
			} catch (IndexOutOfRangeException) {
				Console.WriteLine($"ERROR: input 'opt' was empty");
				return null;
			} catch (Exception e) {
				Console.WriteLine($"ERROR: {e}");
				return null;
			}
		}

		public static string[]? GenericInputArray(string txt) {
			Console.Write(txt);
			char[] sep = { ' ', '.', ',', ';', '/' };
			string[] src = Console.ReadLine().Split(sep, StringSplitOptions.RemoveEmptyEntries);
			if(src.Length == 0) return null;
			return src;
		}

		public static string? GenericInput(string txt) {
			Console.Write(txt);
			string src = Console.ReadLine();
			if(src.Length == 0) return null;
			return src;
		}
	}
}
