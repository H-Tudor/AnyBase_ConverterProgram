namespace Conversii {
	public class Converter {
		private char[] pool;
		private char fractionSep;

		public Converter() {
			pool = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
			fractionSep = '.';
		}

		public Converter(char[] pool) {
			this.pool = pool;
		}

		public Converter(char[] pool, char fSep) {
			this.pool = pool;
			fractionSep = fSep;
		}

		public Converter(char[] pool, string fSep) {
			this.pool = pool;
			fractionSep = (char)fSep[0];
		}

		public Converter(string pool) {
			this.pool = new char[pool.Length];
			for(int i = 0; i < pool.Length; i++) {
				this.pool[i] = pool[i];
			}
		}

		public Converter(string pool, char fSep) {
			fractionSep = fSep;
			this.pool = new char[pool.Length];
			for(int i = 0; i < pool.Length; i++) {
				this.pool[i] = pool[i];
			}
		}

		public Converter(string pool, string fSep) {
			fractionSep = (char)fSep[0];
			this.pool = new char[pool.Length];
			for(int i = 0; i < pool.Length; i++) {
				this.pool[i] = pool[i];
			}
		}

		public string Convert(int input, int b2) {
			if(b2 == 10) return input.ToString();
			if(input == 0) return "0";
			if(b2 < 2 || b2 > pool.Length) return "";

			int b1 = 10;
			string[] numberParts = { input.ToString(), "" };

			numberParts = ConvertToTen(numberParts, b1);
			return ConvertFromTen(numberParts, b2);
		}

		public string Convert(float input, int b2) {
			if(b2 == 10) return input.ToString();
			if(input == 0) return "0";
			if(b2 < 2 || b2 > pool.Length) return "";

			int b1 = 10;

			string[] numberParts = input.ToString().Split('.');

			numberParts = ConvertToTen(numberParts, b1);
			return ConvertFromTen(numberParts, b2);
		}

		public string Convert(double input, int b2) {
			if(b2 == 10) return input.ToString();
			if(input == 0) return "0";
			if(b2 < 2 || b2 > pool.Length) return "";

			int b1 = 10;

			string[] numberParts = input.ToString().Split('.');

			numberParts = ConvertToTen(numberParts, b1);
			return ConvertFromTen(numberParts, b2);
		}

		public string Convert(int b1, int b2, string input) {
			if(b1 == b2) return input;
			if(input == String.Empty) return input;
			if(input == "0") return "0";
			if((b1 < 2 || b1 > pool.Length) || (b2 < 2 || b2 > pool.Length)) return "";

			input = input.ToUpper();
			if(BaseCheck(input, b1) == false) return "";
			string[] numberParts = input.Split(fractionSep);

			if(numberParts[0].Contains('(')) return "";

			string period = "";
			int periodStart = -1;
			if(numberParts.Length > 1) {
				if(numberParts[1].Contains('(')) {
					periodStart = numberParts[1].IndexOf('(');
					string[] temp = numberParts[1].Split('(');

					numberParts[1] = temp[0];
					period = temp[1];

					if(period.Contains(')') == false) return "";
					period.Remove(period.Length - 2, 1);
				}
			}

			numberParts = ConvertToTen(numberParts, b1, period);


			return ConvertFromTen(numberParts, b2);
		}

		public bool BaseCheck(string numberToCheck, int baseToCheck) {
			if(baseToCheck == 0) return false;
			for(int i = baseToCheck + 1; i < pool.Length; i++) {
				if(numberToCheck.Contains(pool[i])) return false;
			}
			return true;
		}

		private string[] ConvertToTen(string[] numberParts, int fromBase, string period) {
			if(fromBase == 10) {
				return numberParts;
			}

			/// Conversion Algorithm:
			/// 
			/// Each digit's position in the pool * its coresponding power (positive or negative) based on its position and distance from the fraction separator
			/// 
			/// ! Simple and Mixed Periodic Fractions not working - missing alogrithm
			/// 
			/// inputBase < 10
			/// char x in number between 0 and 9
			/// 0101(2)		= 0 * 2^3 + 1 * 2^2 + 0 * 2^1 + 1 * 2^0 = 4 + 1 = 5
			/// 0101.01(2)	= 0 * 2^3 + 1 * 2^2 + 0 * 2^1 + 1 * 2^0 + 0 ^ 2^-1 + 1 * 2^-2 = 4 + 1 + 1/4 = 5.25
			/// 
			/// inputBase > 10
			/// char x in number between 0 and Z
			/// 6B(16)		= 6 * 16^1 + 11(B) * 16^0 = 96 + 11 = 107
			/// 6B.E(16)	= 6 * 16^1 + 11(B) * 16^0 + 14(E) * 16^-1 = 96 + 11 + 14/16 = 107.875

			double numberInTen = 0;

			int maxPowerOfBase = numberParts[0].Length;
			int minPowerOfBase = -numberParts[1].Length;

			int x, y;
			double DigitValue, PowerValue;
			for(int i = maxPowerOfBase - 1; i >= minPowerOfBase; i--) {
				x = i >= 0 ? 0 : 1;
				y = x == 0 ? maxPowerOfBase - i - 1 : -i - 1;

				DigitValue = (double)Array.IndexOf(pool, numberParts[x][y]);
				PowerValue = Math.Pow((double)fromBase, (double)i);

				numberInTen += DigitValue * PowerValue;
			}

			return numberInTen.ToString().Split('.');
		}

		private string ConvertFromTen(string[] numberParts, int toBase) {
			if(toBase == 10) {
				return numberParts.Length > 1 ? $"{numberParts[0]}.{numberParts[1]}" : $"{numberParts[0]}";
			}

			/// Conversion Algorithm:
			/// 
			/// For Integer Part:
			/// 
			/// Divide the number in base 10 with the outputbase and record the remainders of each division until it becomes 0, then reorder the remainders from first to last
			/// 
			/// For Fraction Part:
			/// 
			/// Muliply the fraction with the base and add all the integer parts of the products to final number until the fraction part is 0
			/// 
			/// inputBase < 10
			/// char x in number between 0 and 9
			/// 5.25: 5 + .25
			/// 
			/// 5
			///   5 : 2 = 2 r1
			///   2 : 2 = 1 r0
			///   1 : 2 = 0 r1
			/// 
			/// .25: (25 * 10^-2) 
			///   .25 * 2 = 0 .5
			///   .5  * 2 = 1 .0

			Stack<char> IntParts = new Stack<char>();
			int IntNumber = int.Parse(numberParts[0]);
			double FractionPart = numberParts.Length > 1 && numberParts[1] != String.Empty ? int.Parse(numberParts[1]) * Math.Pow((double)10, (double)(-1 * numberParts[1].Length)) : 0;
			string outputNumber = "";

			while(IntNumber != 0) {
				char current = pool[IntNumber % toBase];
				IntParts.Push(current);
				IntNumber /= toBase;
			}

			while(IntParts.Count > 0) {
				outputNumber += IntParts.Pop();
			}

			if(FractionPart != 0) {
				outputNumber += '.';

				while(FractionPart != 0) {
					outputNumber += Math.Floor(FractionPart * toBase);
					FractionPart = (FractionPart * toBase) - Math.Floor(FractionPart * toBase);
				}
			}

			return outputNumber;
		}
	}
}
