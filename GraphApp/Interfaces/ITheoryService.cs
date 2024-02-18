using GraphApp.Model;
using System.Collections.Generic;
using System.Windows.Controls;

namespace GraphApp.Interfaces
{
	public interface ITheoryService
	{
		Theory CurrentTheory { get; set; }

		List<Theory> TheoryControls { get; }
	}
}