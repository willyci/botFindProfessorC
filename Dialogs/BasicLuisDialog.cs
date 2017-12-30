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
            
            Activity reply = activity.CreateReply("This is the text that will be displayed."); 
            reply.Speak = "This is the text that will be spoken.";
            reply.InputHint = InputHints.AcceptingInput;
            await connector.Conversations.ReplyToActivityAsync(reply);
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
            
            List<Object> list1 = new List<Object>(); // name
            List<Object> list2 = new List<Object>(); // rank
            List<Object> list3 = new List<Object>(); // department
            
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
            // load data
            message = "";            
            string m_strFilePath = "https://www.flexcode.org/luis/data.xml";
             XDocument xdocument = XDocument.Load(m_strFilePath);
             IEnumerable<XElement> childList = 
                from el in xdocument.Root.Elements() 
                select el;
            
            
            // filter by name first, build new list1
            if( name1.Entity != null && name1.Entity != String.Empty ) 
            {
                foreach (XElement e in childList){
                    String str = (String)e;
                    if(str.IndexOf(name1.Entity, StringComparison.OrdinalIgnoreCase) >= 0){
                        list1.Add(e);
                        //message += str + "\n";
                        message += (String)e.Element("displayname") + " - ";
                    }
                }
            }            
            await context.PostAsync("total by name = " + list1.Count + " , " + message );    
            message = "";
            
            
            // filter by rank, build new list2
            if( rank.Entity != null && rank.Entity != String.Empty && list1.Count >= 1 ) 
            {
                foreach (XElement e in list1){
                    String str = (String)e;
                    if(str.IndexOf(rank.Entity, StringComparison.OrdinalIgnoreCase) >= 0){
                        list2.Add(e);
                        message += str + "\n";
                    }
                }
            }            
            await context.PostAsync("total by name and rank = " + list2.Count + " , " + message );    
            message = "";
            
            
            // filter by rank, build new list2
            if( department.Entity != null && department.Entity != String.Empty && list2.Count >= 1 ) 
            {
                foreach (XElement e in list2){
                    String str = (String)e;
                    if(str.IndexOf(department.Entity, StringComparison.OrdinalIgnoreCase) >= 0){
                        list3.Add(e);
                        message += str + "\n";
                    }
                }
            }            
            await context.PostAsync("end, total by name and rank and department = " + list3.Count + " , " + message );  
            
            
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
        
        /*
        protected override async Task MessageReceived(IDialogContext context, IAwaitable<IMessageActivity> item)
        {
            // Check for empty query
            var message = await item;
            if (string.isNullOrEmpty(message.Text))
            {
                // Return the Help/Welcome
                await Help(context, null);
            }
            else
            {
                await base.MessageReceived(context, item);
            }
        }
        */
        
    }
}
