using GraphApp.Interfaces;
using GraphApp.Model;
using GraphApp.ViewModel.Verify;
using System;
using System.Collections.Generic;

namespace GraphApp.Services.FactoryViewModel
{
    public class FactoryVerifyCreateMatrixTask : IFactoryViewModel
    {
        private Func<int[,], (IList<VisualVertex>, IList<VisualConnection>), VerifyCreateMatrixTaskViewModel> _factory;

        public FactoryVerifyCreateMatrixTask(Func<int[,], (IList<VisualVertex>, IList<VisualConnection>), VerifyCreateMatrixTaskViewModel> factory)
        {
            _factory = factory;
        }

        public ViewModel.ViewModel Create(object[] param)
        {
            return _factory.Invoke(
                (int[,])param[0],
                ((IList<VisualVertex>, IList<VisualConnection>))param[1]
            );
        }
    }
}
