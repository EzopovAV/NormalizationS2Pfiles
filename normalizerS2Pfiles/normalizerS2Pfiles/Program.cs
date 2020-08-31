using System;

namespace normalizerS2Pfiles
{
	class Program
	{
		static void Main(string[] args)
		{
			string pathSource = @"..\..\..\s2p files\E5071B GPPM-_Chanel-1_Rx_ATTen-0_PHase-2.s2p";
			string pathResult = @"..\..\..\s2p files\E5071B GPPM-_Chanel-1_Rx_ATTen-0_PHase-2_nomalized.s2p";
			pathSource = @"..\..\..\s2p files\ADS_Re.s2p";
			pathResult = @"..\..\..\s2p files\ADS_Re.s2p_nomalized.s2p";

			var s2pFileManager = new S2pFileManager(pathSource, new S2pReader(), new S2pProviderFactory());
			var result = s2pFileManager.NormalizeToFile(pathResult);
			Console.WriteLine(result);

			Console.ReadLine();
		}
	}
}
