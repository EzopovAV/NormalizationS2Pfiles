using normalizerS2Pfiles.Interfaces;
using System;
using System.IO;

namespace normalizerS2Pfiles
{
	public class S2pFileManager
	{
		private readonly string _s2pFileName;
		private readonly IS2pReader _s2pReader;
		private readonly IS2pProviderFactory _s2pProviderFactory;

		public S2pFileManager(string s2pFileName, IS2pReader s2pReader, IS2pProviderFactory s2pProviderFactory)
		{
			if (string.IsNullOrEmpty(s2pFileName))
			{
				throw new ArgumentException("s2pFileName can't be null or empty");
			}
			_s2pFileName = s2pFileName;

			_s2pReader = s2pReader ?? throw new ArgumentNullException(nameof(s2pReader));

			_s2pProviderFactory = s2pProviderFactory ?? throw new ArgumentNullException(nameof(s2pProviderFactory));
		}

		public string NormalizeToFile(string destinationPath)
		{
			try
			{
				string[] source = File.ReadAllLines(_s2pFileName);

				var format = _s2pReader.GetFormat(source);

				var provider = _s2pProviderFactory.GetS2pProvider(format);

				string[] result = provider.GetNormalizedS2P(source);

				File.WriteAllLines(destinationPath, result);
			}
			catch (Exception e)
			{

				return "Error:/n" + e.ToString();
			}

			return "File normalized successfully.";
		}
	}
}
