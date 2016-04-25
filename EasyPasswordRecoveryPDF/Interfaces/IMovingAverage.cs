using System;
using System.Collections.Generic;
using System.Text;

namespace EasyPasswordRecoveryPDF.Interfaces
{
	public interface IMovingAverage
	{
        double Average { get;}

		void AddSample(double val);
		void ClearSamples();
		void InitializeSamples(double val);
	}
}
