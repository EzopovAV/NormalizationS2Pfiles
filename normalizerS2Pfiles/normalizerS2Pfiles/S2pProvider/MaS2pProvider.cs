using System;
using normalizerS2Pfiles.Enums;

namespace normalizerS2Pfiles
{
	public class MaS2pProvider : BaseS2pProvider
	{
		public MaS2pProvider(FrequencyUnits frequencyUnits)
			: base(frequencyUnits)
		{
		}

		protected override Sample[] ConvertDataTodB(Sample[] samples)
		{
			for (int i = 0; i < samples.Length; i++)
			{
				samples[i].S11MagOrRe = 10 * Math.Log10(samples[i].S11MagOrRe);
				samples[i].S12MagOrRe = 10 * Math.Log10(samples[i].S12MagOrRe);
				samples[i].S21MagOrRe = 10 * Math.Log10(samples[i].S21MagOrRe);
				samples[i].S22MagOrRe = 10 * Math.Log10(samples[i].S22MagOrRe);
			}

			return samples;
		}
	}
}
