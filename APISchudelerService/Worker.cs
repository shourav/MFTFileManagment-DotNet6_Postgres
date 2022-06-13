namespace APISchudelerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _config;
        public Worker(ILogger<Worker> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                string server = _config.GetValue<string>("MFTAPIServer");
                string port = _config.GetValue<string>("MFTAPIPort");
                string api = _config.GetValue<string>("MFTAPI");
                string makeBy = _config.GetValue<string>("SystemUserName");
                string paramter = _config.GetValue<string>("MFTParameter");
                string fullURL = server + ":" + port + api + "?" + paramter + "=" + makeBy;
                _logger.LogInformation("{time}: Calling {string}", DateTimeOffset.Now, fullURL);
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.PostAsync(fullURL, null))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        _logger.LogInformation("{time}: API Response {string}", DateTimeOffset.Now, apiResponse);
                    }
                }
                await Task.Delay(60000, stoppingToken);
            }
        }
    }
}