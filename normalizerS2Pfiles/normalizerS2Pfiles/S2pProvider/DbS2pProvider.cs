using normalizerS2Pfiles.Enums;

namespace normalizerS2Pfiles
{
	public class DbS2pProvider : BaseS2pProvider
	{
		public DbS2pProvider(FrequencyUnits frequencyUnits)
			:base(frequencyUnits)
		{
		}

		protected override Sample[] ConvertDataTodB(Sample[] samples)
		{
			return samples;
		}
	}
}
