using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphApp.Interfaces;
using Microsoft.Win32;

namespace GraphApp.Services
{
	public class BinaryLoaderService : IDataLoader
	{
		private readonly OpenFileDialog _openFile;
		public BinaryLoaderService()
		{
			_openFile = new OpenFileDialog();
			_openFile.Filter = "Dat files (*.dat)|*.dat";
		}

		public TLoad Load<TLoad>()
		{
			if (_openFile.ShowDialog() == false)
			{
				return default(TLoad);
			}

			return Load<TLoad>(_openFile.FileName);
		}

		public TLoad Load<TLoad>(string path)
		{
			using (var reader = new BinaryReader(File.Open(path, FileMode.OpenOrCreate)))
			{
				var data = new List<byte>();
				

				while (reader.PeekChar() > -1)
				{
					data.Add(reader.ReadByte());
				}
				var a = data.ToArray();
				
				var result = Convert.ChangeType(a[0], typeof(TLoad));

				return (TLoad)result;
			}
		}
	}
}
