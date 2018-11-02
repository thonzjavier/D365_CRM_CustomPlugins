using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Activities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;

namespace ABJPluginWF
{
    public class CustomWFActivity : CodeActivity
    {
        protected override void Execute(CodeActivityContext context)
        {
            //Reusable Code
            ITracingService tracingService = context.GetExtension<ITracingService>();
            IWorkflowContext workflowContext = context.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory = context.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService service = serviceFactory.CreateOrganizationService(workflowContext.UserId);

            //Business Logic
            //Retrieve Parameters
            Guid accountid = this.inputAccountId.Get(context).Id;
            int firstParam = this.inputFirstParam.Get(context);
            int secondParam = this.inputSecondParam.Get(context);

            //Add 1st and 2nd Input Parameters
            int sumParam = firstParam + secondParam;

            //Create a task entity
            Entity task = new Entity();
            task.LogicalName = "task";
            task["subject"] = string.Format("Subject - The sum of input 1 and input 2 is: {0}", sumParam);
            task["regardingobjectid"] = new EntityReference("account", accountid);

            Guid taskid = service.Create(task);
            string message = string.Format("Message - Input 1 + Input = {0}", sumParam);
            OutputMessage.Set(context, message);
        }

        [RequiredArgument]
        [Input("InputAccountId")]
        [ReferenceTarget("account")]
        public InArgument<EntityReference> inputAccountId { get; set; }

        [Input("InputFirstParam")]
        [ReferenceTarget("account")]
        public InArgument<int> inputFirstParam { get; set; }

        [Input("InputSecondParam")]
        [ReferenceTarget("account")]
        public InArgument<int> inputSecondParam { get; set; }

        [Output("OutputResult")]
        public OutArgument<string> OutputMessage { get; set; }
    }
}
