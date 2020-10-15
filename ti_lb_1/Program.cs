using System;
using System.IO;
using ti_lb_1.Models;
using ti_lb_1.Controller;

namespace ti_lb_1
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Символы и их вероятности:");
			WriteGenericSymbols();
			HomeController.GenerateSymbols((int)1e6, 50, "symbols.txt", "result.txt");
			Console.WriteLine("Запись завершена.");
			//проверка
			TestGeneratedFile("symbols.txt", "result.txt");
		}

		/// <summary>
		/// Проверить на соответствие вероятности сгенерированных символов
		/// </summary>
		/// <param name="fileNameSymbols"></param>
		/// <param name="fileNameForTest"></param>
		private static void TestGeneratedFile(string fileNameSymbols, string fileNameForTest)
		{
			int countExtraSymbols = (File.ReadAllLines(fileNameForTest)).Length * 2;//для вычета символов новой строки и переноса каретки
			Symbol[] symbols = HomeController.ReadSymbolsOption(fileNameSymbols);//считывание символов и их вероятностей
			string text = File.ReadAllText(fileNameForTest);//считывание сгенерированных значений
			double[] countSymbols = new double[symbols.Length];
			double chance = 0;
			for (int i = 0; i < symbols.Length; i++)
			{
				countSymbols[i] = 0;
				for (int j = 0; j < text.Length; j++)
				{
					if (symbols[i].Value == text[j])
					{
						countSymbols[i]++;
					}
				}
				chance = countSymbols[i] / (text.Length - countExtraSymbols);
				chance = Math.Round(chance, 2);
				Console.WriteLine($"{symbols[i]}: {countSymbols[i]} / {text.Length - countExtraSymbols} = {chance} \n" +
						$"{chance} == {symbols[i].Chance}: {Math.Round(chance / symbols[i].Chance) == 1}");
			}

			Console.WriteLine("Рассчитаем безусловную энтропию.");
			double h = 0;
			for (int i = 0; i < symbols.Length; i++)
			{
				h += symbols[i].Chance * Math.Log(symbols[i].Chance, 2);
			}
			h = -h;
			Console.WriteLine("H(X) = " + h);
		}

		/// <summary>
		/// Записать генерируемые символы и их вероятности. Вывести в консоль.
		/// ФУНКЦИЯ ДЛЯ ПРИМЕРА
		/// </summary>
		private static void WriteGenericSymbols()
		{
			string lines = "A 0,02" + Environment.NewLine +
				"a 0,03" + Environment.NewLine +
				"B 0,02" + Environment.NewLine +
				"b 0,02" + Environment.NewLine +
				"C 0,25" + Environment.NewLine +
				"c 0,06" + Environment.NewLine +
				"f 0,6" + Environment.NewLine;
			HomeController.WriteSymbolsOption(lines, "symbols.txt");
			Symbol[] symbols2 = HomeController.ReadSymbolsOption("symbols.txt");
			string outStr = "";
			foreach (Symbol s in symbols2)
			{
				outStr += s.ToString() + Environment.NewLine;
			}
			Console.WriteLine(outStr); ;
		}
	}
}
