using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;

namespace CRMPlugIn03142022
{
    public class TaskCreation : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            // Obtain the tracing service
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            // Obtain the execution context from the service provider.  
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            // The InputParameters collection contains all the data passed in the message request.  
            if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
            {
                // Obtain the target entity from the input parameters.  
                Entity entity = (Entity)context.InputParameters["Target"];

                // Obtain the organization service reference which you will need for  
                // web service calls.  
                IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

                try
                {
                    // Plug-in business logic goes here.
                    // Create a task activity to follow up with the account customer in 7 days. 
                    #region plugin1
                    //Entity followup = new Entity("task");

                    //followup["subject"] = "This task has been created from Plugin CRMPlugin03142022.";
                    //followup["description"] =
                    //    "Follow up with the customer. Check if there are any new issues that need resolution.";
                    //followup["scheduledstart"] = DateTime.Now.AddDays(7);
                    //followup["scheduledend"] = DateTime.Now.AddDays(7);
                    //followup["category"] = context.PrimaryEntityName;

                    //// Refer to the account in the task activity.
                    //if (context.OutputParameters.Contains("id"))
                    //{
                    //    Guid regardingobjectid = new Guid(context.OutputParameters["id"].ToString());
                    //    string regardingobjectidType = "account";

                    //    followup["regardingobjectid"] =
                    //    new EntityReference(regardingobjectidType, regardingobjectid);
                    //}

                    //// Create the task in Microsoft Dynamics CRM.
                    //tracingService.Trace("FollowupPlugin: Creating the task activity.");
                    //service.Create(followup);
                    #endregion
                    #region plugin2
                    if (entity.LogicalName != "contact")
                        return;
                    QueryByAttribute query = new QueryByAttribute("contact");
                    query.ColumnSet = new ColumnSet("jobtitle");
                    query.Attributes.AddRange("jobtitle");
                    query.Values.AddRange("Purchasing Assistant");

                    EntityCollection entityCollection = service.RetrieveMultiple(query);
                    if(context.Depth == 1)
                    {
                        foreach(var enty in entityCollection.Entities)
                        {
                            enty["jobtitle"] = "Purchasing Assistant Changed";
                            service.Update(enty);
                        }
                    }
                    #endregion
                }

                catch (System.ServiceModel.FaultException<OrganizationServiceFault> ex)
                {
                    throw new InvalidPluginExecutionException("An error occurred in FollowUpPlugin.", ex);
                }

                catch (Exception ex)
                {
                    tracingService.Trace("FollowUpPlugin: {0}", ex.ToString());
                    throw;
                }
            }
        }
    }
}