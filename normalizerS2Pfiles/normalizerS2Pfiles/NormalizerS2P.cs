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
		private string[] _source;

		private string[] _sampleStrings;

		private string _formatString;
		private string _freqUntil;
		private string _dataUntil;

		private Sample[] _samples;

		public NormalizerS2P(string[] source)
		{
			_source = source;

			GetFormatString();
			GetUntil();

			GetSamplesStrings();
			ParseSample();

			ConvertFreqToHz();
			ConvertDataTodB();
		}

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

		private void GetFormatString()
		{
			int i = 0;
			while (i != _source.Length && _source[i].First() != '#')
			{
				i++;
			}
			_formatString = _source[i];
		}

		private void GetUntil()
		{
			string[] s = _formatString.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
			_freqUntil = s[1];
			_dataUntil = s[3];
		}

		private void GetSamplesStrings()
		{
			_sampleStrings = _source.Where(t => !t.StartsWith("!") && !t.StartsWith("#")).ToArray();
		}

		private void ParseSample()
		{
			_samples = new Sample[_sampleStrings.Length];

			string[] sampleString;
			for (int i = 0; i < _samples.Length; i++)
			{
				Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");

				sampleString = _sampleStrings[i].Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
				Double.TryParse(sampleString[0], out _samples[i].Freq);
				try
				{
					_samples[i].Freq = Double.Parse(sampleString[0]);
					_samples[i].S11MagOrRe = Double.Parse(sampleString[1]);
					_samples[i].S11AngOrIm = Double.Parse(sampleString[2]);
					_samples[i].S12MagOrRe = Double.Parse(sampleString[3]);
					_samples[i].S12AngOrIm = Double.Parse(sampleString[4]);
					_samples[i].S21MagOrRe = Double.Parse(sampleString[5]);
					_samples[i].S21AngOrIm = Double.Parse(sampleString[6]);
					_samples[i].S22MagOrRe = Double.Parse(sampleString[7]);
					_samples[i].S22AngOrIm = Double.Parse(sampleString[8]);
				}
				catch (Exception)
				{

					throw;
				}
				//Console.WriteLine(_samples[i].Freq + "\t" + _samples[i].S11MagOrRe);
			}
		}

		private void ConvertFreqToHz()
		{
			switch (_freqUntil.ToUpper())
			{
				case "HZ":
					break;

				case "KHZ":
					ConvertFreq(t => t * 1E3);
					break;

				case "MHZ":
					ConvertFreq(t => t * 1E6);
					break;

				case "GHZ":
					ConvertFreq(t => t * 1E9);
					break;

				default:
					break;
			}

			void ConvertFreq(Func<double, double> func)
			{
				for (int i = 0; i < _samples.Length; i++)
				{
					_samples[i].Freq = func(_samples[i].Freq);
				}
			}
		}

		private void ConvertDataTodB()
		{
			switch (_dataUntil.ToUpper())
			{
				case "DB":
					break;

				case "MA":
					for (int i = 0; i < _samples.Length; i++)
					{
						_samples[i].S11MagOrRe = 10 * Math.Log10(_samples[i].S11MagOrRe);
						_samples[i].S12MagOrRe = 10 * Math.Log10(_samples[i].S12MagOrRe);
						_samples[i].S21MagOrRe = 10 * Math.Log10(_samples[i].S21MagOrRe);
						_samples[i].S22MagOrRe = 10 * Math.Log10(_samples[i].S22MagOrRe);
					}
					break;

				case "RI":
					Sample sample = new Sample();
					for (int i = 0; i < _samples.Length; i++)
					{
						sample = _samples[i];

						_samples[i].S11MagOrRe = 10 * Math.Log10(Math.Sqrt(Math.Pow(sample.S11MagOrRe, 2) + Math.Pow(sample.S11AngOrIm, 2)));
						_samples[i].S11AngOrIm = 180 * Math.Atan2(sample.S11AngOrIm, sample.S11MagOrRe) / Math.PI;

						_samples[i].S12MagOrRe = 10 * Math.Log10(Math.Sqrt(Math.Pow(sample.S12MagOrRe, 2) + Math.Pow(sample.S12AngOrIm, 2)));
						_samples[i].S12AngOrIm = 180 * Math.Atan2(sample.S12AngOrIm, sample.S12MagOrRe) / Math.PI;

						_samples[i].S21MagOrRe = 10 * Math.Log10(Math.Sqrt(Math.Pow(sample.S21MagOrRe, 2) + Math.Pow(sample.S21AngOrIm, 2)));
						_samples[i].S21AngOrIm = 180 * Math.Atan2(sample.S21AngOrIm, sample.S21MagOrRe) / Math.PI;

						_samples[i].S22MagOrRe = 10 * Math.Log10(Math.Sqrt(Math.Pow(sample.S22MagOrRe, 2) + Math.Pow(sample.S22AngOrIm, 2)));
						_samples[i].S22AngOrIm = 180 * Math.Atan2(sample.S22AngOrIm, sample.S22MagOrRe) / Math.PI;
					}
					break;

				default:
					break;
			}
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
