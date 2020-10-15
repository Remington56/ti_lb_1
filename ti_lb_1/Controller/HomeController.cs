using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ti_lb_1.Models;

namespace ti_lb_1.Controller
{
	public static class HomeController
	{
		/// <summary>
		/// Считывает текстовый файл с символами.
		/// </summary>
		/// <param name="fileName">Название файла.</param>
		/// <returns></returns>
		public static Symbol[] ReadSymbolsOption(string fileName)
		{
			if (!File.Exists(fileName))
				throw new Exception("Файл не обнаружен");
			string text = File.ReadAllText(fileName);
			return ReadStringSymbols(text);
		}

		/// <summary>
		/// Записать символы в текстовый файл.
		/// </summary>
		/// <param name="symbols">Массив символов</param>
		/// <param name="fileName">Название файла</param>
		public static void WriteSymbolsOption(Symbol[] symbols, string fileName)
		{
			if (!IsNormalSumChance(symbols))
				throw new Exception("Сумма вероятностей всех символов должна быть равна 1.");

			//запись в файл
			string text = "";
			for (int j = 0; j < symbols.Length; j++)
			{
				text += symbols[j].ToString() + Environment.NewLine;
			}
			File.WriteAllText(fileName, text);
		}

		/// <summary>
		/// Записать символы в текстовый файл.
		/// </summary>
		/// <param name="strSymbols"></param>
		/// <param name="fileName"></param>
		public static void WriteSymbolsOption(string strSymbols, string fileName)
		{
			Symbol[] symbols = ReadStringSymbols(strSymbols);
			WriteSymbolsOption(symbols, fileName);
		}

		/// <summary>
		/// Возвращает TRUE, если сумма вероятностей равна 1.
		/// </summary>
		/// <param name="symbols">Массив символов</param>
		/// <returns>Логическое значение</returns>
		public static bool IsNormalSumChance(Symbol[] symbols)
		{
			int chance = 0;
			for (int i = 0; i < symbols.Length; i++)
			{
				chance += (int)(symbols[i].Chance * 100);
			}
			return (chance == 100);
		}

		/// <summary>
		/// Считывание символов из строки и запись в массив символов.
		/// </summary>
		/// <param name="strSymbols">строка символов</param>
		/// <returns>массив символов</returns>
		private static Symbol[] ReadStringSymbols(string strSymbols)
		{
			string[] lines = strSymbols.Split(new string[] { Environment.NewLine, " " }, StringSplitOptions.RemoveEmptyEntries);
			Symbol[] symbols = new Symbol[lines.Length / 2];
			int k = 0;//индекс для lines, равный 2*i
			for (int i = 0; i < symbols.Length; i++, k += 2)
			{
				if (!String.IsNullOrWhiteSpace(lines[k]))
					symbols[i] = new Symbol(Convert.ToChar(lines[k]), Convert.ToDouble(lines[k + 1]));
			}
			return symbols;
		}

		/// <summary>
		/// Сгенерировать символы с заданными вероятностями
		/// и записать результат в файл
		/// </summary>
		/// <param name="countGenericSymbols">Число генерируемых символов</param>
		/// <param name="countSymbolsInLine">Число символов в строке</param>
		/// <param name="fileNameReading">Название файла, хранящего заданные символы</param>
		/// <param name="fileNameWriting">Название файла для записи результата</param>
		public static void GenerateSymbols(int countGenericSymbols, int countSymbolsInLine, 
			string fileNameReading, string fileNameWriting)
		{
			Symbol[] symbols = HomeController.ReadSymbolsOption(fileNameReading);//считывание символов и их вероятностей
			string[] genericLines = new string[countGenericSymbols / countSymbolsInLine];
			Random rand = new Random();
			double randomValue;
			int randomIndex = 0;
			int indexLine = 0;
			//массив хранит сумму вероятностей предыдущих по индексу символов.
			//Пример: [0.2], [0.3], [0.5]. indexSymbols[1] = 0.2 + 0.3;
			double[] indexSymbols = new double[symbols.Length];
			indexSymbols[0] = symbols[0].Chance;
			for (int i = 1; i < symbols.Length; i++)
			{
				indexSymbols[i] = indexSymbols[i - 1] + symbols[i].Chance;
			}
			while (countGenericSymbols > 0)
			{
				randomValue = (double)rand.Next(0, int.MaxValue)/int.MaxValue;//получение рандомного значения
				//определение символа заданной вероятностью
				int k = 0;
				while (k < indexSymbols.Length)
				{
					if (randomValue < indexSymbols[k])
					{
						randomIndex = k;//полученный индекс символа
						break;
					}
					k++;
				}
				genericLines[indexLine] += symbols[randomIndex].Value;
				if (genericLines[indexLine].Length >= countSymbolsInLine)
				{
					indexLine++;
				}
				countGenericSymbols--;
			}
			File.WriteAllLines(fileNameWriting, genericLines);
		}

	}
}
