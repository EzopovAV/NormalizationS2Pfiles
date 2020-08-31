namespace normalizerS2Pfiles.Interfaces
{
	public interface IS2pProviderFactory
	{
		IS2pProvider GetS2pProvider(S2pFormat format);
	}
}
