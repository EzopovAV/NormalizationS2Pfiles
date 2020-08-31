using normalizerS2Pfiles.Enums;
using normalizerS2Pfiles.Interfaces;
using System;

namespace normalizerS2Pfiles
{
	public class S2pProviderFactory : IS2pProviderFactory
	{
		public IS2pProvider GetS2pProvider(S2pFormat format)
		{
			switch (format.DataUnits)
			{
				case DataUnits.DB:
						return new DbS2pProvider(format.FrequencyUnits);

				case DataUnits.MA:
						return new MaS2pProvider(format.FrequencyUnits);
				
				case DataUnits.RI:
						return new RiS2pProvider(format.FrequencyUnits);
				
				default:
						throw new Exception("Unsupported data format");
			}
		}
	}
}
