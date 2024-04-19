using GraphApp.Command;
using GraphApp.Interfaces;
using GraphApp.Model;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media;

namespace GraphApp.ViewModel.Verify
{
    public class VerifyCreateMatrixTaskViewModel : ViewModel
    {
        private INavigationService _navigationService;

        private IAccessControlService _accessControlService;

        public ICommand GoBack { get; private set; }

        public string Title { get; private set; }

        public Brush TitleColor { get; private set; }

        public AdjacencyMatrix AdjacencyMatrix { get; private set; }

        public IList<VisualVertex> Vertices { get; private set; }

        public IList<VisualConnection> Connections { get; private set; }

        public int[,] Matrix { get; private set; }

        public VerifyCreateMatrixTaskViewModel(
            INavigationService navigationService,
            IAccessControlService accessControlService,
            AdjacencyMatrix correctMatrix,
            int[,] userMatrix,
            IList<VisualVertex> vertices,
            IList<VisualConnection> connections)
        {
            _navigationService = navigationService;
            _accessControlService = accessControlService;
            AdjacencyMatrix = correctMatrix;
            Matrix = userMatrix;
            Vertices = vertices;
            Connections = connections;

            Verify(correctMatrix.Matrix, userMatrix);
            GoBack = new RelayCommand(GoBackCommand);
        }

        private void Verify(int[,] correctMatrix, int[,] userMatrix)
        {
            var flag = true;
            var rows = userMatrix.GetUpperBound(0) + 1;
            var colums = userMatrix.Length / rows;

            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < colums; j++)
                {
                    if (correctMatrix[i, j] != userMatrix[i, j])
                    {
                        flag = false;
                        break;
                    }
                }
            }

            if (!_accessControlService.CheckEducationMaterialIsPassed(_accessControlService.CurrentEducationMaterial))
            {
                _accessControlService.AddAttempt(_accessControlService.CurrentEducationMaterial);
            }

            if (flag)
            {
                _accessControlService.OpenNext(_accessControlService.CurrentEducationMaterial);
                Title = "Верно";
                TitleColor = new SolidColorBrush(Colors.Green);
            }
            else
            {
                Title = "Неверно";
                TitleColor = new SolidColorBrush(Colors.Red);
            }
        }

        private void GoBackCommand(object parameter)
        {
            _navigationService.NavigateTo<CreateMatrixTaskViewModel>();
        }
    }
}
