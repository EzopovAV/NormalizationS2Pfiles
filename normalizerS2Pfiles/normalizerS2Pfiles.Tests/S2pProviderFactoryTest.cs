using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using normalizerS2Pfiles.Enums;
using normalizerS2Pfiles.Interfaces;
using NUnit.Framework;

namespace normalizerS2Pfiles.Tests
{
	[TestFixture]
	class S2pProviderFactoryTest
	{
		private IS2pProviderFactory _s2pProviderFactory;

		[SetUp]
		public void SetUp()
		{
			_s2pProviderFactory = new S2pProviderFactory();
		}

		[Test]
		[TestCase(DataUnits.DB, FrequencyUnits.Hz, typeof(DbS2pProvider))]
		[TestCase(DataUnits.DB, FrequencyUnits.kHz, typeof(DbS2pProvider))]
		[TestCase(DataUnits.DB, FrequencyUnits.MHz, typeof(DbS2pProvider))]
		[TestCase(DataUnits.DB, FrequencyUnits.GHz, typeof(DbS2pProvider))]

		[TestCase(DataUnits.MA, FrequencyUnits.Hz, typeof(MaS2pProvider))]
		[TestCase(DataUnits.MA, FrequencyUnits.kHz, typeof(MaS2pProvider))]
		[TestCase(DataUnits.MA, FrequencyUnits.MHz, typeof(MaS2pProvider))]
		[TestCase(DataUnits.MA, FrequencyUnits.GHz, typeof(MaS2pProvider))]

		[TestCase(DataUnits.RI, FrequencyUnits.Hz, typeof(RiS2pProvider))]
		[TestCase(DataUnits.RI, FrequencyUnits.kHz, typeof(RiS2pProvider))]
		[TestCase(DataUnits.RI, FrequencyUnits.MHz, typeof(RiS2pProvider))]
		[TestCase(DataUnits.RI, FrequencyUnits.GHz, typeof(RiS2pProvider))]
		public void GetS2pProviderTest(DataUnits dataUnits, FrequencyUnits frequencyUnits, Type expectedTypeProvider)
		{
			var s2pFormat = new S2pFormat { DataUnits = dataUnits, FrequencyUnits = frequencyUnits };
			var actualProvider = _s2pProviderFactory.GetS2pProvider(s2pFormat);
			Assert.AreEqual(expectedTypeProvider, actualProvider.GetType());
		}


	}
}
