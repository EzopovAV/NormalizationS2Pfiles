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
		[TestCase(DataUnits.DB, typeof(DbS2pProvider))]
		[TestCase(DataUnits.MA, typeof(MaS2pProvider))]
		[TestCase(DataUnits.RI, typeof(RiS2pProvider))]

		public void GetS2pProviderTest(DataUnits dataUnits, Type expectedTypeProvider)
		{
			var s2pFormat = new S2pFormat { DataUnits = dataUnits };
			var actualProvider = _s2pProviderFactory.GetS2pProvider(s2pFormat);
			Assert.AreEqual(expectedTypeProvider, actualProvider.GetType());
		}

		[Test]
		public void GetS2pProviderUnsupportedDataFormatTest()
		{
			var s2pFormat = new S2pFormat();
			s2pFormat.DataUnits = (DataUnits)int.MaxValue;
			Assert.Throws<Exception>(() => _s2pProviderFactory.GetS2pProvider(s2pFormat));
		}

		[Test]
		public void GetS2pProviderNullTest()
		{
			Assert.Throws<NullReferenceException>(() => _s2pProviderFactory.GetS2pProvider(null));
		}
	}
}
