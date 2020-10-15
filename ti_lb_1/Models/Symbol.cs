using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ti_lb_1.Models
{
	/// <summary>
	/// Символ с вероятностью его появления.
	/// </summary>
	public class Symbol
	{
		/// <summary>
		/// Вероятность появления символа
		/// </summary>
		private double chance;
		/// <summary>
		/// Символ.
		/// </summary>
		public char Value { get; set; }

		/// <summary>
		/// Вероятность появления символа.
		/// </summary>
		public double Chance
		{
			get { return chance; }
			set { if (value > 0 && value < 1) chance = value; }
		}

		/// <summary>
		/// Создает новый экземпляр символа.
		/// </summary>
		/// <param name="symbol">Символ</param>
		/// <param name="chance">Вероятность появления от 0 до 1</param>
		public Symbol(char symbol, double chance)
		{
			Value = symbol;
			Chance = chance;
		}

		/// <summary>
		/// Возвращает строку с символом и вероятностью его появления
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return Value + " " + Chance;
		}
	}
}
