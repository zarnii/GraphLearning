using GraphApp.Interfaces;
using GraphApp.Model;
using GraphApp.ViewModel.Verify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphApp.Services.FactoryViewModel
{
    /// <summary>
    /// Обобщенная фабрика VerifyPracticTaskVM.
    /// </summary>
    public class FactoryVerifyPracticTaskViewModel : IFactoryViewModel
    {
        /// <summary>
        /// Фабрика.
        /// </summary>
        private Func<VerifiedPracticTask, 
            PracticTask, 
            IList<VisualVertex>, 
            IList<VisualConnection>,
            int, int,
            VerifyPracticViewModel> _factory;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="factory">Конкретная фабрика.</param>
        public FactoryVerifyPracticTaskViewModel(Func<VerifiedPracticTask, 
            PracticTask, 
            IList<VisualVertex>, 
            IList<VisualConnection>,
            int, int,
            VerifyPracticViewModel> factory) 
        {
            _factory = factory;
        }

        /// <summary>
        /// Создание.
        /// </summary>
        /// <param name="param">Параметры.</param>
        /// <returns>ViewModel.</returns>
        public ViewModel.ViewModel Create(object[] param)
        {
            var verifiedTask = (VerifiedPracticTask)param[0];
            var verifableTask = (PracticTask)param[1];
            var vertices = (IList<VisualVertex>)param[2];
            var connection = (IList<VisualConnection>)param[3];
            var connectionNumberOpasity = (int)param[4];
            var connectionWeightOpasity = (int)param[5];

            return _factory.Invoke(verifiedTask, verifableTask, vertices, connection, connectionNumberOpasity, connectionWeightOpasity);
        }
    }
}
