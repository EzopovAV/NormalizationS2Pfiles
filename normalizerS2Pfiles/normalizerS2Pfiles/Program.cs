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
			string pathSource = @"C:\petProjects\s2p files\E5071B GPPM-_Chanel-1_Rx_ATTen-0_PHase-2.s2p";
			string pathResult = @"C:\petProjects\s2p files\E5071B GPPM-_Chanel-1_Rx_ATTen-0_PHase-2_nomalized.s2p";

			string[] Source = File.ReadAllLines(pathSource);

			NormalizerS2P x = new NormalizerS2P();

			string[] Result = x.Normalize(Source);

			File.WriteAllLines(pathResult, Result);
			Console.ReadLine();
		}
	}
}
