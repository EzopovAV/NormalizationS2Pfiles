using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

			string[] Source = File.ReadAllLines(pathSource);

			NormalizerS2P x = new NormalizerS2P(Source);

			string[] Result = x.GetNormalizedS2P();

			File.WriteAllLines(pathResult, Result);
			Console.WriteLine(pathResult);
			Console.WriteLine(Result[0]);
			Console.ReadLine();
		}
	}
}
