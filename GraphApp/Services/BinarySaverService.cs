using System;
using System.Configuration;
using System.IO;
using GraphApp.Interfaces;
using Microsoft.Win32;

namespace GraphApp.Services
{
	/// <summary>
	/// Сервис сохранения данных в бинарном формате.
	/// </summary>
	public class BinarySaverService : IDataSaver
	{
		private readonly SaveFileDialog _saveFile;

		public BinarySaverService()
		{

			_saveFile = new SaveFileDialog();
			_saveFile.Filter = "Dat files (*.dat)|*.dat";
		}

		public void Save<TSave>(TSave data)
		{
			if (_saveFile.ShowDialog() == false)
			{
				return;
			}

			Save<TSave>(data, _saveFile.FileName);
		}

		public void Save<TSave>(TSave data, string path)
		{
			using (var writer = new BinaryWriter(File.Open(path, FileMode.OpenOrCreate)))
			{
				try
				{
					var b = Convert.ToByte(data);
					writer.Write(Convert.ToByte(data));
				}
				catch (Exception ex)
				{

				}
			}
		}
	}
}
