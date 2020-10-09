using normalizerS2Pfiles.Enums;

namespace normalizerS2Pfiles
{
	public class S2pFormat
	{
		public FrequencyUnits FrequencyUnits;
		public DataUnits DataUnits;

		public bool Equals(S2pFormat other)
		{
			if (ReferenceEquals(this, other))
			{
				return true;
			}

			if (ReferenceEquals(other, null))
			{
				return false;
			}

			return FrequencyUnits == other.FrequencyUnits && DataUnits == other.DataUnits;
		}

		public override bool Equals(object obj)
		{
			S2pFormat s2PFormat = obj as S2pFormat;
			return Equals(s2PFormat);
		}

		public override int GetHashCode()
		{
			return FrequencyUnits.GetHashCode() ^ DataUnits.GetHashCode();
		}
	}
}
