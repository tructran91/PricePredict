using ApexCharts;
using BlazorDateRangePicker;
using Microsoft.AspNetCore.Components;
using PricePredict.Web.ApiClients;
using PricePredict.Web.Models.Application;
using PricePredict.Web.Models.Business.Indicator;

namespace PricePredict.Web.Pages.Indicator
{
    public partial class EMA
    {
        [Inject]
        private IIndicatorApi IndicatorApi { get; set; }

        private List<BreadcrumbItem> breadcrumb = new List<BreadcrumbItem>
        {
            new BreadcrumbItem { Label = "Indicator", Url = "/", IsActive = false },
            new BreadcrumbItem { Label = "EMA", Url = "/", IsActive = true }
        };

        private List<IndicatorPerformanceModel> signalsResult = new();
        private string selectedSymbol = "BTCUSDT";
        private string selectedTimeframe = "1m";
        private DateTimeOffset startDate = DateTimeOffset.Now.AddDays(-10);
        private DateTimeOffset endDate = DateTimeOffset.Now;
        private bool isLoading = true;

        // config chart
        private ApexChart<DailyPerformanceModel> _chartRef;
        private ApexChartOptions<DailyPerformanceModel> _chartOptions = new ApexChartOptions<DailyPerformanceModel>
        {
            Stroke = new Stroke
            {
                Width = 2
            }
        };

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await GetEMAIndicator();
        }

        private async Task GetEMAIndicator()
        {
            var response = await IndicatorApi.GetTradeResultsAsync(selectedSymbol, selectedTimeframe, startDate, endDate);

            if (response.IsSuccess)
            {
                signalsResult = response.Data;
                await UpdateChart();
            }
            isLoading = false;
        }

        private async Task SearchClicked()
        {
            await GetEMAIndicator();
        }

        private void HandleRangeSelect(DateRange range)
        {
            startDate = range.Start;
            endDate = range.End;
        }

        private async Task UpdateChart()
        {
            if (_chartRef != null && signalsResult.Any())
            {
                await _chartRef.UpdateSeriesAsync(true);
                await _chartRef.RenderAsync();
            }
        }
    }
}
