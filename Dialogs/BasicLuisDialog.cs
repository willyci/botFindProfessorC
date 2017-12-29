using System;
using System.Configuration;
using System.Threading.Tasks;

using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;

using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;


namespace Microsoft.Bot.Sample.LuisBot
{
    // For more information about this template visit http://aka.ms/azurebots-csharp-luis
    [Serializable]
    public class BasicLuisDialog : LuisDialog<object>
    {
        
        private const string EntityFacultyRank = "title";
        private const string EntityNameOne = "OnePartName";
        private const string EntityNameTwo = "TwoPartName";
        private const string EntityNameThree = "ThreePartName";
        private const string EntityDepartment = "department";
        
        public BasicLuisDialog() : base(new LuisService(new LuisModelAttribute(
            ConfigurationManager.AppSettings["LuisAppId"], 
            ConfigurationManager.AppSettings["LuisAPIKey"], 
            domain: ConfigurationManager.AppSettings["LuisAPIHostName"])))
        {
        }

        [LuisIntent("None")]
        public async Task NoneIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }

        // Go to https://luis.ai and create a new intent, then train/publish your luis app.
        // Finally replace "Gretting" with the name of your newly created intent in the following handler
        [LuisIntent("Greeting")]
        public async Task GreetingIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }

        [LuisIntent("Cancel")]
        public async Task CancelIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }

        [LuisIntent("Help")]
        public async Task HelpIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }
        
        
        [LuisIntent("GetCount")]
        public async Task GetCountIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }
        
        
        [LuisIntent("FindByNameTitleDepartment")]
        public async Task FindByNameTitleDepartmentIntent(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Searching ... ");
            await this.ShowLuisResult(context, result);
            

            
            string message = "LUIS return";
            EntityRecommendation rank;

            if (result.TryFindEntity(EntityFacultyRank, out rank))
            {
                 message += ":   Faculty rank = " + rank.Entity;
            }


            EntityRecommendation name1;

            if (result.TryFindEntity(EntityNameOne, out name1))
            {
                message += ";   name = " + name1.Entity;
            }
            
            EntityRecommendation name2;

            if (result.TryFindEntity(EntityNameTwo, out name2))
            {
                message += ";   name2 = " + name2.Entity;
            }
            
            EntityRecommendation name3;

            if (result.TryFindEntity(EntityNameThree, out name3))
            {
                message += ";   name3 = " + name3.Entity;
            }
            
            EntityRecommendation department;

            if (result.TryFindEntity(EntityDepartment, out department))
            {
                message += ";   department = " + department.Entity;
            }

            await context.PostAsync(message); 
            
        }
        
        [LuisIntent("faq")]
        public async Task faqIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }

        private async Task ShowLuisResult(IDialogContext context, LuisResult result) 
        {
            await context.PostAsync($"You have reached {result.Intents[0].Intent}. You said: {result.Query}");
            context.Wait(MessageReceived);
        }
    }
}
