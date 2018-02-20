using RemoteCamera.Helpers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace RemoteCamera.AnomalyDetection
{
    public class AnomalyDetectionClient
    {
        #region Fields

        // Azure Time Series Anomaly Detection
        //private const string baseAddress = "https://ussouthcentral.services.azureml.net/workspaces/3c77f3e695424ed7983f8e3ba370fab6/services/ad16b76e055748658af549eee9e213a1/execute?api-version=2.0&details=true";
        //private const string apiKey = "6MKKgN0166l281uU0dnh+UoSWtXcb/RO8uWwGRd3IsTfAgPvyfJGShpSkkuIRRc1VBOTLHE45KRE2dl/lTMz5A==";

        // Z-Score thresholding
        private const string baseAddress = "https://ussouthcentral.services.azureml.net/workspaces/3c77f3e695424ed7983f8e3ba370fab6/services/6c4f6f8d577941b7aea02fd801143538/execute?api-version=2.0&details=true";
        private const string apiKey = "oL3Wh5cYbU3Z14M7YCE6SI1tDVR/k2tWhZ/oHuEDUBRfNzwUC1YZRSM5kMb4NVCM40DWjhPoWvI2XHnkp2IvOA==";

        private HttpClient httpClient;

        #endregion

        #region Constructor

        public AnomalyDetectionClient()
        {
            httpClient = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress),
            };

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        }

        #endregion

        #region Methods

        public async Task<IList<BrightnessDataPoint>> DetectAnomalyAsync(IList<BrightnessDataPoint> brightnessData)
        {
            var request = new AnomalyDetectionRequest(brightnessData);

            var response = await httpClient.PostAsJsonAsync(string.Empty, request);

            IList<BrightnessDataPoint> result; 

            if (response.IsSuccessStatusCode)
            {
                var anomalyDetectionResponse = await response.Content.ReadAsAsync<AnomalyDetectionResponse>();

                result = ConversionHelper.AnomalyDetectionResponseToBrightnessData(anomalyDetectionResponse);
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }

            return result;
        }

        #endregion
    }
}
