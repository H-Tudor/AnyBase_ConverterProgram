using Conversii;
using static Helpers.Helpers;

internal class Program {
	private static void Main(string[] args) {
		Conversii.Converter converter = new Conversii.Converter();

		string input = GenericInput("Enter input number: ");
		int b1 = IntInput("Enter base of the input number: "); 
		int b2 = IntInput("Enter base of the output number: ");

		Console.WriteLine(converter.Convert(b1, b2, input));
	}
}