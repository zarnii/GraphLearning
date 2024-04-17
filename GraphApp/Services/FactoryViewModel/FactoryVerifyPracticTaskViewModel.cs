using GraphApp.Interfaces;
using GraphApp.Model;
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
            ViewModel.VerifyPracticViewModel> _factory;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="factory">Конкретная фабрика.</param>
        public FactoryVerifyPracticTaskViewModel(Func<VerifiedPracticTask, 
            PracticTask, 
            IList<VisualVertex>, 
            IList<VisualConnection>,
            ViewModel.VerifyPracticViewModel> factory) 
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

            return _factory.Invoke(verifiedTask, verifableTask, vertices, connection);
        }
    }
}
