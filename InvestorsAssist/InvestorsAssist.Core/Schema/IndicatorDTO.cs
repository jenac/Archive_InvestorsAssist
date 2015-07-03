using InvestorsAssist.Entities;
using System;

namespace InvestorsAssist.Core.Schema
{
	public abstract class IndicatorDTO
	{
		public string Symbol { get; set; }
		public DateTime Date { get; set; }

		public abstract Indicator ToIndicator();
	}
}

