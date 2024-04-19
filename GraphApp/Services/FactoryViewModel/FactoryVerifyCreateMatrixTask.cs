using GraphApp.Interfaces;
using GraphApp.Model;
using GraphApp.ViewModel.Verify;
using System;
using System.Collections.Generic;

namespace GraphApp.Services.FactoryViewModel
{
    public class FactoryVerifyCreateMatrixTask : IFactoryViewModel
    {
        private Func<AdjacencyMatrix, int[,], IList<VisualVertex>, IList<VisualConnection>, VerifyCreateMatrixTaskViewModel> _factory;

        public FactoryVerifyCreateMatrixTask(Func<AdjacencyMatrix, int[,], IList<VisualVertex>, IList<VisualConnection>, VerifyCreateMatrixTaskViewModel> factory)
        {
            _factory = factory;
        }

        public ViewModel.ViewModel Create(object[] param)
        {
            return _factory.Invoke(
                (AdjacencyMatrix)param[0], 
                (int[,])param[1],
                (IList<VisualVertex>)param[2],
                (IList<VisualConnection>)param[3]
            );
        }
    }
}
