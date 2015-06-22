using System;
using System.Collections.Generic;
using System.Linq;
namespace InvestorsAssist.Algorithm
{
	public class ContinuousCalculator
	{
		private readonly List<int> _trueCounts;
		private readonly List<int> _falseCounts;
		private readonly int _lastCont;

		public int TrueMax
		{
			get { 
				return _trueCounts.Count() == 0 ? 0 : _trueCounts.Max(); 
			}
		}

		public double TrueAvg
		{
			get { 
				return _trueCounts.Count() == 0? 0.0 : _trueCounts.Average(); 
			}
		}

		public int FalseMax
		{
			get { 
				return _falseCounts.Count()== 0 ? 0 : _falseCounts.Max(); 
			}
		}

		public double FalseAvg
		{
			get { 
				return _falseCounts.Count() == 0 ? 0.0 : _falseCounts.Average(); 
			}
		}

		public int LastCont
		{
			get { return _lastCont; }
		}

		public ContinuousCalculator(double[] data, Func<double, bool> predict)
		{
			_trueCounts = new List<int>() ;
			_falseCounts = new List<int>();
			if (data == null || data.Count() == 0)
				return;
			bool last = false;
			bool cur = false;
			foreach(double d in data)
			{
				cur = predict(d);
				var list = cur ? _trueCounts : _falseCounts;
				if (last == cur)
				{
					if (list.Count() == 0)
					{
						list.Add(1);
					}
					else
					{
						list[list.Count() - 1]++;
					}
				}
				else
				{
					list.Add(1);
				}
				last = cur;
			}

			_lastCont = predict(data.Last()) ? _trueCounts.Last() : _falseCounts.Last();
		}
	}
}

