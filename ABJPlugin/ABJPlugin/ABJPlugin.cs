using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace ABJPlugin
{
    public class AppendOnAccountCreate : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            //Extract the tracing service for use in debugging sandboxed plug-ins.
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            // Obtain the execution context from the service provider.
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            
            // Get a reference to the Organization service.
            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));

            IOrganizationServiceFactory servicefactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService client = servicefactory.CreateOrganizationService(context.UserId);

            // The InputParameters collection contains all the data passed in the message request.
            if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
            {
                // Obtain the target entity from the input parameters.
                Entity entity = (Entity)context.InputParameters["Target"];

                // Verify that the target entity represents an account.
                // If not, this plug-in was not registered correctly.

                if (entity.LogicalName == "account")
                {
                    try
                    {
                        string tempName;
                        if (entity.Attributes.Contains("name") == true)
                        {
                            tempName = entity.Attributes["name"].ToString();
                            entity.Attributes["name"] = tempName + " company";
                        }
                    }
                    catch (Exception ex)
                    {
                        tracingService.Trace("accountPlugin: {0}", ex.ToString());
                        throw;
                    }
                }
                else
                {
                    return;
                }
            }
        }
    }
}
