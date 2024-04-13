using GraphApp.Command;
using GraphApp.Interfaces;
using GraphApp.Model;
using GraphApp.Services;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media;

namespace GraphApp.ViewModel
{
    public class VerifyPracticViewModel: ViewModel
    {
        private INavigationService _navigationService;

        public List<VisualVertex> Vertices { get; set; }

        public List<VisualConnection> Connections { get; set; }

        public ICommand GoBack { get; private set; }

        public string ResultText { get; set; }

        public string ResultDescription { get; set; }

        public Brush ResultColor { get; set; }

        public double CanvasWidth { get; set; }

        public double CanvasHeight { get; set; }

        public VerifyPracticViewModel(
            INavigationService navigationService,
            IVerifyPracticTaskService verifyPracticTaskService,
            IAccessControlService accessControlService)
        {
            _navigationService = navigationService;
            Vertices = (List<VisualVertex>)verifyPracticTaskService.VerifiedVertices;
            Connections = (List<VisualConnection>)verifyPracticTaskService.VerifiedConnections;
            CanvasWidth = 1000;
            CanvasHeight = 900;

            Verify(verifyPracticTaskService, accessControlService);

            GoBack = new RelayCommand(GoBackCommand);
        }

        private void Verify(
            IVerifyPracticTaskService verifyPracticTaskService, 
            IAccessControlService accessControlService)
        {
            var result = verifyPracticTaskService.VerifyPracticTask();
            var isDone = true;

            if (verifyPracticTaskService.VerifiedPracticTask.NeedCheckVertexCount && !result.VertexCountIsDone)
            {
                isDone = false;
            }

            if (verifyPracticTaskService.VerifiedPracticTask.NeedCheckVertexPosition && !result.VertexPositionIsDone)
            {
                isDone = false;
            }

            if (verifyPracticTaskService.VerifiedPracticTask.NeedCheckVertexSize && !result.VertexSizeIsDone)
            {
                isDone = false;
            }

            if (verifyPracticTaskService.VerifiedPracticTask.NeedCheckVertexName && !result.VertexNameIsDone)
            {
                isDone = false;
            }

            if (verifyPracticTaskService.VerifiedPracticTask.NeedCheckConnectionCount && !result.ConnectionCountIsDone)
            {
                isDone = false;
            }

            if (verifyPracticTaskService.VerifiedPracticTask.NeedCheckConnection && !result.ConnectionIsDone)
            {
                isDone = false;
            }

            if (verifyPracticTaskService.VerifiedPracticTask.NeedCheckConnectionWeight && !result.ConnectionWeightIsDone)
            {
                isDone = false;
            }

            if (verifyPracticTaskService.VerifiedPracticTask.NeedCheckConnectionType && !result.ConnectionTypeIsDone)
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

            ResultText = "Не верно";
            ResultColor = new SolidColorBrush(Colors.Red);
        }

        private void GoBackCommand(object parameter)
        {
            _navigationService.NavigateTo<PracticViewModel>();
        }
    }
}
