using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace normalizerS2Pfiles
{
	class NormalizerS2P
	{
		private readonly string[] _source;
		private readonly string[] _sampleStrings;

		private readonly string _formatString;
		private readonly string _freqUnit;
		private readonly string _dataUnit;

		private Sample[] _samples;

		public NormalizerS2P(string[] source)
		{
			_source = source ?? throw new ArgumentNullException(nameof(source));

			_formatString = GetFormatString(_source);

			_freqUnit = GetFreqUnit(_formatString);
			_dataUnit = GetDataUnit(_formatString);

			_sampleStrings = GetSamplesStrings(_source);
			_samples = ParseSample(_sampleStrings);

			_samples = ConvertFreqToHz(_freqUnit, _samples);
			_samples = ConvertDataTodB(_dataUnit, _samples);
		}

		/// <summary>
		/// Return all samples normalized to format # Hz S dB R 50
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Sample> GetSamples()
		{
			return _samples;
		}

		/// <summary>
		/// Return first N or less samles normalized to format # Hz S dB R 50
		/// </summary>
		/// <param name="limit"></param>
		/// <returns></returns>
		public IEnumerable<Sample> GetSamples(int limit)
		{
			for (int i = 0; i < limit; i++)
			{
				if (i == _samples.Length)
				{
					yield break;
				}

				yield return _samples[i];
			}
		}

		/// <summary>
		/// Return all data normalized to format # Hz S dB R 50
		/// </summary>
		/// <returns></returns>
		public string[] GetNormalizedS2P()
		{
			string[] commentStrings = _source.Where(t => t.StartsWith("!")).ToArray();

			string[] result = new string[1 + commentStrings.Length + 1 + _samples.Length];
			int i = 0;

			result[i] = "! This file was normalized.";
			i++;

			foreach (var item in commentStrings)
			{
				result[i] = item;
				i++;
			}

			result[i] = "# Hz S dB R 50";
			i++;

			foreach (var item in _samples)
			{
				result[i] = item.Freq.ToString() + "\t" +
					item.S11MagOrRe.ToString("E") + "\t" +
					item.S11AngOrIm.ToString("E") + "\t" +
					item.S12MagOrRe.ToString("E") + "\t" +
					item.S12AngOrIm.ToString("E") + "\t" +
					item.S21MagOrRe.ToString("E") + "\t" +
					item.S21AngOrIm.ToString("E") + "\t" +
					item.S22MagOrRe.ToString("E") + "\t" +
					item.S22AngOrIm.ToString("E");
				i++;
			}

			return result;
		}

		private string GetFormatString(string[] source)
		{
			int i = 0;
			while (source[i].First() != '#')
			{
				i++;
				if (i == source.Length)
				{
					throw new Exception("No data format string was found.");
				}
			}
			var formatString = source[i];
			return formatString;
		}

		private string GetFreqUnit(string formatString)
		{
			string[] validFrequencyUnits = new string[] { "HZ", "KHZ", "MHZ", "GHZ" };
			string[] s = formatString.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
			var freqUnit = s[1].ToUpper();
			if (!validFrequencyUnits.Contains(freqUnit))
			{
				throw new Exception("Invalid frequency units.");
			}
			return freqUnit;
		}

		private string GetDataUnit(string formatString)
		{
			string[] validDataUnits = new string[] { "DB", "MA", "RI" };
			string[] s = formatString.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
			var dataUnit = s[3].ToUpper();
			if (!validDataUnits.Contains(dataUnit))
			{
				throw new Exception("Invalid data units.");
			}
			return dataUnit;
		}

		private string[] GetSamplesStrings(string[] source)
		{
			return source.Where(t => !t.StartsWith("!") && !t.StartsWith("#")).ToArray();
		}

		private Sample[] ParseSample(string[] sampleStrings)
		{
			var samples = new Sample[sampleStrings.Length];

			string[] sampleString;
			for (int i = 0; i < samples.Length; i++)
			{
				Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");

				sampleString = sampleStrings[i].Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

				try
				{
					samples[i].Freq = Double.Parse(sampleString[0]);
					samples[i].S11MagOrRe = Double.Parse(sampleString[1]);
					samples[i].S11AngOrIm = Double.Parse(sampleString[2]);
					samples[i].S12MagOrRe = Double.Parse(sampleString[3]);
					samples[i].S12AngOrIm = Double.Parse(sampleString[4]);
					samples[i].S21MagOrRe = Double.Parse(sampleString[5]);
					samples[i].S21AngOrIm = Double.Parse(sampleString[6]);
					samples[i].S22MagOrRe = Double.Parse(sampleString[7]);
					samples[i].S22AngOrIm = Double.Parse(sampleString[8]);
				}
				catch (Exception)
				{
					throw new Exception("Invalid data.");
				}
			}

			return samples;
		}

		private Sample[] ConvertFreqToHz(string freqUnit, Sample[] samples)
		{
			Func<double, double> func = f => f;

			switch (freqUnit)
			{
				case "HZ":
					return samples;

				case "KHZ":
					func = f => f * 1E3;
					break;

				case "MHZ":
					func = f => f * 1E6;
					break;

				case "GHZ":
					func = f => f * 1E9;
					break;
			}
			
			return ConvertFreq(samples, func);
		}

		private Sample[] ConvertFreq(Sample[] samples, Func<double, double> func)
		{
			for (int i = 0; i < samples.Length; i++)
			{
				samples[i].Freq = func(samples[i].Freq);
			}
			return samples;
		}

		private Sample[] ConvertDataTodB(string dataUnit, Sample[] samples)
		{
			switch (dataUnit)
			{
				case "DB":
					break;

				case "MA":
					for (int i = 0; i < samples.Length; i++)
					{
						samples[i].S11MagOrRe = 10 * Math.Log10(_samples[i].S11MagOrRe);
						samples[i].S12MagOrRe = 10 * Math.Log10(_samples[i].S12MagOrRe);
						samples[i].S21MagOrRe = 10 * Math.Log10(_samples[i].S21MagOrRe);
						samples[i].S22MagOrRe = 10 * Math.Log10(_samples[i].S22MagOrRe);
					}
					break;

				case "RI":
					Sample sample = new Sample();
					for (int i = 0; i < samples.Length; i++)
					{
						sample = samples[i];

						samples[i].S11MagOrRe = 10 * Math.Log10(Math.Sqrt(Math.Pow(sample.S11MagOrRe, 2) + Math.Pow(sample.S11AngOrIm, 2)));
						samples[i].S11AngOrIm = 180 * Math.Atan2(sample.S11AngOrIm, sample.S11MagOrRe) / Math.PI;

						samples[i].S12MagOrRe = 10 * Math.Log10(Math.Sqrt(Math.Pow(sample.S12MagOrRe, 2) + Math.Pow(sample.S12AngOrIm, 2)));
						samples[i].S12AngOrIm = 180 * Math.Atan2(sample.S12AngOrIm, sample.S12MagOrRe) / Math.PI;

						samples[i].S21MagOrRe = 10 * Math.Log10(Math.Sqrt(Math.Pow(sample.S21MagOrRe, 2) + Math.Pow(sample.S21AngOrIm, 2)));
						samples[i].S21AngOrIm = 180 * Math.Atan2(sample.S21AngOrIm, sample.S21MagOrRe) / Math.PI;

						samples[i].S22MagOrRe = 10 * Math.Log10(Math.Sqrt(Math.Pow(sample.S22MagOrRe, 2) + Math.Pow(sample.S22AngOrIm, 2)));
						samples[i].S22AngOrIm = 180 * Math.Atan2(sample.S22AngOrIm, sample.S22MagOrRe) / Math.PI;
					}
					break;
			}

			return samples;
		}
	}

	struct Sample
	{
		public double Freq;
		public double S11MagOrRe;
		public double S11AngOrIm;
		public double S12MagOrRe;
		public double S12AngOrIm;
		public double S21MagOrRe;
		public double S21AngOrIm;
		public double S22MagOrRe;
		public double S22AngOrIm;
	}
}
