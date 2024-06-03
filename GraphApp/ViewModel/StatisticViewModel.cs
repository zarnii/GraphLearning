using GraphApp.Command;
using GraphApp.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.Extensions;
using SkiaSharp;
using System.Collections.Generic;
using LiveChartsCore.Defaults;

namespace GraphApp.ViewModel
{
    public class StatisticViewModel : ViewModel
    {
        private INavigationService _navigationService;

        private IAccessControlService _accessControlService;

        public ICommand GoBack { get; private set; }

        public ICommand Clear { get; private set; }

        public ObservableCollection<(string, int)> Statistic { get; private set; }

        public ISeries[] Chart { get; private set; }

        public IEnumerable<ISeries> PieChart { get; private set; }

        public Axis[] XAxes { get; private set; }

        public Axis[] YAxes { get; private set; }

        public StatisticViewModel(IAccessControlService accessControlService, INavigationService navigationService)
        {
            _navigationService = navigationService;
            _accessControlService = accessControlService;
            _accessControlService.EducationMaterialMapChanged += UpdateCollection;
            StatisticInit();
            ChartInit();
            PieChartInit();

            GoBack = new RelayCommand(GoBackCommand);
            Clear = new RelayCommand(ClearCommand);

        }

        private void StatisticInit()
        {
            Statistic = new ObservableCollection<(string, int)>(
                _accessControlService.EducationMaterialMap
                    .Select(x => (x.Key.EducationMaterialTitle, x.Value.AttemptsNumber.Value))
                    .ToArray()
            );
        }

        private void ChartInit()
        {
            Chart = new ISeries[1]
            {
                new LineSeries<int>()
                {
                    Values = _accessControlService.EducationMaterialMap
                        .Select(x => x.Value.AttemptsNumber.Value)
                        .ToArray(),
                    Fill = null,
                    GeometrySize = 5,
                    LineSmoothness = 0
                }
            };

            XAxes = new Axis[1]
            {
                new Axis()
                {
                    Name = "Номер задания",
                    Labels = _accessControlService.EducationMaterialMap
                        .Select(x => x.Value.IndexNumber.ToString())
                        .ToArray(),
                    NamePaint = new SolidColorPaint(SKColors.White),
                    LabelsPaint = new SolidColorPaint(SKColors.White),
                    TextSize = 14,
                    MinStep = 1
                }
            };

            YAxes = new Axis[1]
            {
                new Axis()
                {
                    Name = "Количество попыток",
                    NamePaint = new SolidColorPaint(SKColors.White),
                    LabelsPaint = new SolidColorPaint(SKColors.White),
                    TextSize = 14,
                    MinStep = 1
                }
            };
        }

        private void PieChartInit()
        {
            var percent = CalculateCompletePercent();

            PieChart = GaugeGenerator.BuildSolidGauge(
                new GaugeItem(percent, 
                    series =>
                    {
                        series.MaxRadialColumnWidth = 50;
                        series.DataLabelsSize = 50;
                        series.DataLabelsPaint = new SolidColorPaint(SKColors.White);
                        series.Name = "Процен прохождения";
                    }));
        }

        private void GoBackCommand(object parameter)
        {
            _navigationService.NavigateTo<MainMenuViewModel>();
        }

        private void ClearCommand(object parameter)
        {
            _accessControlService.ResetMap();
            UpdateCollection(null, null);
        }

        private int CalculateCompletePercent()
        {
            var collection = _accessControlService.EducationMaterialMap.ToArray();
            var all = collection.Select(x => x.Value.AttemptsNumber.Value).Count();
            var isComplete = collection.Where(x => x.Value.IsCompleted == true).Count();

            return (int)((isComplete / (double)all) * 100);
        }

        private void UpdateCollection(object? sender, EventArgs e)
        {
            var collection = _accessControlService.EducationMaterialMap.ToArray();

            for (var i = 0; i < collection.Length; i++)
            {
                Statistic[i] = (collection[i].Key.EducationMaterialTitle, collection[i].Value.AttemptsNumber.Value);
            }

            Chart[0].Values = collection.Select(x => x.Value.AttemptsNumber.Value).ToArray();
            var pieChart = (PieSeries<ObservableValue>)PieChart.First();
            var percent = pieChart.Values.First();
            percent.Value = CalculateCompletePercent();
        }
    }
}
