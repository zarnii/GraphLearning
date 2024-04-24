using GraphApp.Interfaces;
using GraphApp.Model;
using GraphApp.ViewModel.Verify;
using System;
using System.Collections.Generic;

namespace GraphApp.Services.FactoryViewModel
{
    /// <summary>
    /// Обобщенная фабрика VerifyTestViewModel.
    /// </summary>
    public class FactoryVerifyTestViewModel : IFactoryViewModel
    {
        private Func<Dictionary<Question, List<VisualAnswer>>, string, VerifyTestViewModel> _factory;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="factory">Конкретная фабрика.</param>
        public FactoryVerifyTestViewModel(Func<Dictionary<Question, List<VisualAnswer>>, string, VerifyTestViewModel> factory)
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
            var dict = (Dictionary<Question, List<VisualAnswer>>)param[0];
            var message = (string)param[1];

            return _factory.Invoke(dict, message);
        }
    }
}
