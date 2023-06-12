using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using ZingTickets.Common;

namespace ZingTickets.MovieManagement.Extensions
{
    /// <summary>
    ///  Add All the Amazon AWS Related Dependencies here 
    /// </summary>
    public static class AmazonExtensions
    {
        public static void AddAmazonSystemsManager(this IServiceCollection services)
        {
            var ssmClient = new AmazonSimpleSystemsManagementClient();

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection() // Start with an empty configuration
                .Build();

            var parameters = RetrieveParametersFromSSM(ssmClient).GetAwaiter().GetResult();

            foreach (var parameter in parameters)
            {
                configuration[parameter.Name] = parameter.Value;
            }
            services.AddSingleton<IConfiguration>(configuration);
        }


        private static async Task<IEnumerable<Parameter>> RetrieveParametersFromSSM(AmazonSimpleSystemsManagementClient ssmClient)
        {
            try
            {
                var parameters = new List<Parameter>();

                var request = new GetParametersByPathRequest
                {
                    Path = GlobalConstants.PATH_PARAMETER_PREFIX,
                    Recursive = true,
                    WithDecryption = true // Set to true if the parameters are encrypted
                };

                do
                {
                    var response = await ssmClient.GetParametersByPathAsync(request);
                    parameters.AddRange(response.Parameters);

                    request.NextToken = response.NextToken;
                } while (!string.IsNullOrEmpty(request.NextToken));

                return parameters;
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex.Message);
                throw;
            }
        }

    }
}
