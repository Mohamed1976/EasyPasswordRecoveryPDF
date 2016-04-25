using System;
using EasyPasswordRecoveryPDF.Interfaces;

namespace EasyPasswordRecoveryPDF.Common
{
	public class SimpleMovingAverage : IMovingAverage
	{
		CircularList<double> samples;
		protected double total;

		/// <summary>
		/// Get the average for the current number of samples.
		/// </summary>
		public double Average
		{
			get
			{
				if (samples.Count == 0)
				{
					throw new ApplicationException("Number of samples is 0.");
				}

				return total / samples.Count;
			}
		}

		/// <summary>
		/// Constructor, initializing the sample size to the specified number.
		/// </summary>
		public SimpleMovingAverage(int numSamples)
		{
			if (numSamples <= 0)
			{
				throw new ArgumentOutOfRangeException("numSamples can't be negative or 0.");
			}

			samples = new CircularList<double>(numSamples);
			total = 0;
		}

		/// <summary>
		/// Adds a sample to the sample collection.
		/// </summary>
		public void AddSample(double val)
		{
			if (samples.Count == samples.Length)
			{
				total -= samples.Value;
			}

			samples.Value = val;
			total += val;
			samples.Next();
		}

		/// <summary>
		/// Clears all samples to 0.
		/// </summary>
		public void ClearSamples()
		{
			total = 0;
			samples.Clear();
		}

		/// <summary>
		/// Initializes all samples to the specified value.
		/// </summary>
		public void InitializeSamples(double v)
		{
			samples.SetAll(v);
			total = v * samples.Length;
		}
	}
}
