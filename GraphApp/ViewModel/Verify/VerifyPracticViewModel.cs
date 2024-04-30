using GraphApp.Command;
using GraphApp.Interfaces;
using GraphApp.Model;
using GraphApp.Services;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media;

namespace GraphApp.ViewModel.Verify
{
    public class VerifyPracticViewModel : ViewModel
    {
        private INavigationService _navigationService;

        public IList<VisualVertex> Vertices { get; set; }

        public IList<VisualConnection> Connections { get; set; }

        public ICommand GoBack { get; private set; }

        public string ResultText { get; set; }

        public string ResultDescription { get; set; }

        public Brush ResultColor { get; set; }

        public VerifyPracticViewModel(
            INavigationService navigationService,
            IAccessControlService accessControlService,
            VerifiedPracticTask verifiedPracticTask,
            PracticTask verifablePracticTask,
            IList<VisualVertex> vertices,
            IList<VisualConnection> connections)
        {
            _navigationService = navigationService;
            Vertices = vertices;
            Connections = connections;

            Verify(accessControlService, verifiedPracticTask, verifablePracticTask);

            GoBack = new RelayCommand(GoBackCommand);
        }

        private void Verify(
            IAccessControlService accessControlService,
            VerifiedPracticTask verifiedPracticTask,
            PracticTask verifablePracticTask)
        {
            var isDone = true;

            if (verifablePracticTask.NeedCheckVertexCount && !verifiedPracticTask.VertexCountIsDone)
            {
                isDone = false;
            }

            if (verifablePracticTask.NeedCheckVertexPosition && !verifiedPracticTask.VertexPositionIsDone)
            {
                isDone = false;
            }

            if (verifablePracticTask.NeedCheckVertexSize && !verifiedPracticTask.VertexSizeIsDone)
            {
                isDone = false;
            }

            if (verifablePracticTask.NeedCheckVertexName && !verifiedPracticTask.VertexNameIsDone)
            {
                isDone = false;
            }

            if (verifablePracticTask.NeedCheckConnectionCount && !verifiedPracticTask.ConnectionCountIsDone)
            {
                isDone = false;
            }

            if (verifablePracticTask.NeedCheckConnection && !verifiedPracticTask.ConnectionIsDone)
            {
                isDone = false;
            }

            if (verifablePracticTask.NeedCheckConnectionWeight && !verifiedPracticTask.ConnectionWeightIsDone)
            {
                isDone = false;
            }

            if (verifablePracticTask.NeedCheckConnectionType && !verifiedPracticTask.ConnectionTypeIsDone)
            {
                isDone = false;
            }

            if (!accessControlService.CheckEducationMaterialIsPassed(accessControlService.CurrentEducationMaterial))
            {
                accessControlService.AddAttempt(accessControlService.CurrentEducationMaterial);
            }
            CheckResult(isDone);

            if (isDone)
            {
                accessControlService.OpenNext(accessControlService.CurrentEducationMaterial);
            }
        }

        private void CheckResult(bool isDone)
        {
            if (isDone)
            {
                ResultText = "Верно";
                ResultColor = new SolidColorBrush(Colors.Green);

                return;
            }

            ResultText = "Не верно!";
            ResultColor = new SolidColorBrush(Colors.Red);
        }

        private void GoBackCommand(object parameter)
        {
            _navigationService.NavigateTo<PracticViewModel>();
        }
    }
}
