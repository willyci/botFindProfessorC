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
        private const string EntityAlphabet = "alphabet";


        public BasicLuisDialog() : base(new LuisService(new LuisModelAttribute(
            ConfigurationManager.AppSettings["LuisAppId"],
            ConfigurationManager.AppSettings["LuisAPIKey"],
            domain: ConfigurationManager.AppSettings["LuisAPIHostName"])))
        {
        }
        [LuisIntent("")]
        public async Task EmptyIntent(IDialogContext context, LuisResult result)
        {
            //await this.ShowLuisResult(context, result);
            await context.SayAsync(text: "Hi, welcome to profiles!",
                                   speak: "Welcome to Profiles!");
        }

        [LuisIntent("None")]
        public async Task NoneIntent(IDialogContext context, LuisResult result)
        {
            //await this.ShowLuisResult(context, result);
            await context.SayAsync(text: "Sorry, I am not programmed to respond in that area, I am not as smart as you hope to be. -- Captain Kirk",
                                   speak: "Sorry, I am not programmed to respond in that area, I am not as smart as you hope to be.");
        }

        // Go to https://luis.ai and create a new intent, then train/publish your luis app.
        // Finally replace "Gretting" with the name of your newly created intent in the following handler
        [LuisIntent("Greeting")]
        public async Task GreetingIntent(IDialogContext context, LuisResult result)
        {
            //await this.ShowLuisResult(context, result);
            await context.SayAsync(text: "Hello, how are you?",
                                   speak: "Hello, how are you?");

        }

        [LuisIntent("Cancel")]
        public async Task CancelIntent(IDialogContext context, LuisResult result)
        {
            //await this.ShowLuisResult(context, result);
            await context.SayAsync(text: "OK",
                                   speak: "OK");
        }

        [LuisIntent("Help")]
        public async Task HelpIntent(IDialogContext context, LuisResult result)
        {

            //Activity reply = activity.CreateReply("This is the text that will be displayed."); 
            //reply.Speak = "This is the text that will be spoken.";
            //reply.InputHint = InputHints.AcceptingInput;
            //await connector.Conversations.ReplyToActivityAsync(reply);

            //await this.ShowLuisResult(context, result);
            await context.SayAsync(text: "you can say something like: find Professor Max from Radiology, or lookup Phd Mary from medicine.",
                                   speak: "you can say something like: find Professor Max from Radiology, or lookup Phd Mary from medicine.");
            //await context.PostAsync("you can say something like: find Professor John from Radiology, or lookup researcher Mary from medicine.");
        }


        [LuisIntent("GetCount")]
        public async Task GetCountIntent(IDialogContext context, LuisResult result)
        {
            //await this.ShowLuisResult(context, result);
            

            await context.SayAsync(text: "Searching ... ",
                                   speak: "I am on it! two seconds. ");

            List<Object> list1 = new List<Object>(); // name
            List<Object> list2 = new List<Object>(); // rank
            List<Object> list3 = new List<Object>(); // department

            string message = ""; // "LUIS return";
            EntityRecommendation rank;

            if (result.TryFindEntity(EntityFacultyRank, out rank))
            {
                message += "You said :  " + rank.Entity;
            }


            EntityRecommendation name1;

            if (result.TryFindEntity(EntityNameOne, out name1))
            {
                message += "  name " + name1.Entity;
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
                message += " from department " + department.Entity + ".";
            }


            //await context.PostAsync(message);


            await context.SayAsync(text: message,
                                   speak: message);



            // load data
            message = "";
            string m_strFilePath = "https://www.flexcode.org/luis/data.xml";
            XDocument xdocument = XDocument.Load(m_strFilePath);
            IEnumerable<XElement> childList =
               from el in xdocument.Root.Elements()
               select el;



            // filter by name first, build new list1
            // string message_list1 = "";

            int counter = 0;

            if( name1 == null && rank == null && department == null )
            {
                foreach (XElement e in childList)
                {
                    counter++;
                }
                
                await context.SayAsync(text: "There are " + counter + " people in the database.",
                                   speak: "There are " + counter + " people in the database.");

            }
        
            else if ( name1 != null && rank == null && department == null)
            {
                foreach (XElement e in childList)
                {
                    String str = (String)e;
                    if (str.IndexOf(name1.Entity, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        counter++;
                    }
                }

                await context.SayAsync(text: "There are " + counter + " people name "+ name1.Entity + " in the database.",
                                   speak: "There are " + counter + " people name " + name1.Entity + " in the database.");

            }
            else if (name1 == null && rank != null && department == null)
            {
                foreach (XElement e in childList)
                {
                    String str = (String)e;
                    if (str.IndexOf(rank.Entity, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        counter++;
                    }
                }

                await context.SayAsync(text: "There are " + counter + " people with title " + rank.Entity + " in the database.",
                                   speak: "There are " + counter + " people with title " + rank.Entity + " in the database.");
            }
            else if (name1 == null && rank == null && department != null)
            {
                foreach (XElement e in childList)
                {
                    String str = (String)e;
                    if (str.IndexOf(department.Entity, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        counter++;
                    }
                }

                await context.SayAsync(text: "There are " + counter + " people in the department " + department.Entity + ".",
                                   speak: "There are " + counter + " people in the department " + department.Entity + ".");
            }
            else if (name1 != null && rank != null && department == null)
            {
                foreach (XElement e in childList)
                {
                    String str = (String)e;
                    if (str.IndexOf(name1.Entity, StringComparison.OrdinalIgnoreCase) >= 0 && str.IndexOf(rank.Entity, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        counter++;
                    }
                }

                await context.SayAsync(text: "There are " + counter + " people name "+ name1.Entity +" with title " + rank.Entity + " in the database.",
                                  speak: "There are " + counter + " people name " + name1.Entity + " with title " + rank.Entity + " in the database.");

            }
            else if (name1 != null && rank == null && department != null)
            {
                foreach (XElement e in childList)
                {
                    String str = (String)e;
                    if (str.IndexOf(name1.Entity, StringComparison.OrdinalIgnoreCase) >= 0 && str.IndexOf(department.Entity, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        counter++;
                    }
                }

                await context.SayAsync(text: "There are " + counter + " people name " + name1.Entity + " in the department " + department.Entity + ".",
                                  speak: "There are " + counter + " people name " + name1.Entity + " in the department " + department.Entity + ".");
            }
            else if (name1 == null && rank != null && department != null)
            {
                foreach (XElement e in childList)
                {
                    String str = (String)e;
                    if (str.IndexOf(rank.Entity, StringComparison.OrdinalIgnoreCase) >= 0 && str.IndexOf(department.Entity, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        counter++;
                    }
                }

                await context.SayAsync(text: "There are " + counter + " people with title " + rank.Entity + " in the department " + department.Entity + ".",
                                  speak: "There are " + counter + " people with title " + rank.Entity + " in the department " + department.Entity + ".");
            }
            else if (name1 != null && rank != null && department != null)
            {
                foreach (XElement e in childList)
                {
                    String str = (String)e;
                    if (str.IndexOf(name1.Entity, StringComparison.OrdinalIgnoreCase) >= 0 && 
                        str.IndexOf(department.Entity, StringComparison.OrdinalIgnoreCase) >= 0 &&
                        str.IndexOf(rank.Entity, StringComparison.OrdinalIgnoreCase) >= 0 
                        )
                    {
                        counter++;
                    }
                }

                await context.SayAsync(text: "There are " + counter + " people name "+ name1.Entity + " with title " + rank.Entity + " in the department " + department.Entity + ".",
                                  speak: "There are " + counter + " people name " + name1.Entity + " with title " + rank.Entity + " in the department " + department.Entity + ".");
            }
            else
            {
                await context.SayAsync(text: "I am still working on it.",
                                   speak: "counting and counting and counting and counting ... Sorry, the bot fall in sleep...");
            }


            
        }


        [LuisIntent("FindByNameTitleDepartment")]
        public async Task FindByNameTitleDepartmentIntent(IDialogContext context, LuisResult result)
        {
            //await context.PostAsync("Searching ... ");
            //await this.ShowLuisResult(context, result);

            await context.SayAsync(text: "Searching ... ",
                                   speak: "I am on it! two seconds. ");

            List<Object> list1 = new List<Object>(); // name
            List<Object> list2 = new List<Object>(); // rank
            List<Object> list3 = new List<Object>(); // department

            string message = ""; // "LUIS return";
            EntityRecommendation rank;

            if (result.TryFindEntity(EntityFacultyRank, out rank))
            {
                message += "You said :  " + rank.Entity;
            } 


            EntityRecommendation name1;

            if (result.TryFindEntity(EntityNameOne, out name1))
            {
                message += "  name " + name1.Entity;
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
                message += " from department " + department.Entity + ".";
            }
            

            //await context.PostAsync(message);


            await context.SayAsync(text: message,
                                   speak: message);

            // load data
            message = "";
            string m_strFilePath = "https://www.flexcode.org/luis/data.xml";
            XDocument xdocument = XDocument.Load(m_strFilePath);
            IEnumerable<XElement> childList =
               from el in xdocument.Root.Elements()
               select el;


            // filter by name first, build new list1
            string message_list1 = "";
            if (name1 != null && name1.Entity != null && name1.Entity != String.Empty && name1.Entity != "")
            {
                foreach (XElement e in childList)
                {
                    String str = (String)e;
                    if (str.IndexOf(name1.Entity, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        list1.Add(e);
                        //message += str + "\n";
                        message_list1 += (String)e.Element("displayname") + " " + Environment.NewLine;
                    }
                }

                await context.PostAsync("total by name = " + list1.Count + " , " + message_list1);
            }
            else
            {
                await context.PostAsync("name not found, or LUIS didn't catch the name.");
                foreach (XElement e in childList)
                {                    
                        list1.Add(e);
                }

            }
            //await context.PostAsync("total by name = " + list1.Count + " , " + message_list1);
            //message = "";


            // filter by rank, build new list2
            string message_list2 = "";
            //await context.PostAsync("rank = -" + rank.Entity + "- ");

            if (rank != null && rank.Entity != null && rank.Entity != String.Empty && rank.Entity != "" && list1.Count >= 1  )
            {
                
                
                foreach (XElement e in list1)
                {
                    String str = (String)e;
                    if (str.IndexOf(rank.Entity, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        list2.Add(e);
                        message_list2 += (String)e.Element("displayname") + " " + Environment.NewLine;
                    }
                }

                await context.PostAsync("total by name and title = " + list2.Count + " , " + message_list2);
            }
            else
            {
                await context.PostAsync("title not found or LUIS didn't catch the title" );
                list2 = list1;
            }
            
            //message = "";



            // filter by rank, build new list2
            string message_list3 = "";
            if (department != null && department.Entity != null && department.Entity != String.Empty && department.Entity != "" && list2.Count >= 1)
            {
                foreach (XElement e in list2)
                {
                    String str = (String)e;
                    if (str.IndexOf(department.Entity, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        list3.Add(e);
                        message_list3 += (String)e.Element("facultyrank") + " "
                                        + (String)e.Element("displayname") + " from "
                                        + (String)e.Element("divisionname") + " in " 
                                        + (String)e.Element("departmentname") + ", "+ Environment.NewLine +"phone number is "
                                        + (String)e.Element("phone") + ", "+  Environment.NewLine   +"email address is "
                                        + (String)e.Element("emailaddr") + " "
                                        + Environment.NewLine;
                    }
                }

                await context.PostAsync("end, total by name and rank and department = " + list3.Count + " , " + message_list3);
            }
            else
            {
                await context.PostAsync("department not found, or LUIS didnt catch the department.");
                list3 = list2;
            }
            



            if (list3.Count >= 4030)
            {
                await context.SayAsync(text: "You found a bug in my code",
                                   speak: "You found a bug in my code, check the output.");
            }
            else if (list3.Count >= 1 && list3.Count < 10)
            {
                await context.SayAsync(text: "I found " + message_list3,
                                   speak: "I found " + message_list3);
            }
            else if (list3.Count >= 10)
            {
                await this.TooMany(context, list3.Count);
            }
            else if (list3.Count == 0 && list2.Count >= 1 && list2.Count < 10)
            {
                await context.SayAsync(text: "I found " + message_list2,
                                   speak: "I found " + message_list2);
            }
            else if (list3.Count == 0 && list2.Count >= 10)
            {
                await this.TooMany(context, list2.Count);
            }
            else if (list3.Count == 0 && list2.Count == 0 && list1.Count >= 1 && list1.Count < 10)
            {
                await context.SayAsync(text: "I found " + message_list1,
                                   speak: "I found " + message_list1);
            }
            else if (list3.Count == 0 && list2.Count == 0 && list1.Count >= 10)
            {
                await this.TooMany(context, list1.Count);
            }
            else
            {
                await context.SayAsync(text: "You found a bug in my code " + message_list1 + " " + message_list2 + " " + message_list3,
                                   speak: "You found a bug in my code, check the output." );
            }






        }

        [LuisIntent("faq")]
        public async Task faqIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
            await context.SayAsync(text: "I am a robot, worte in C#, made by human, my purpose is passing butter.",
                                   speak: "I am a robot, worte in C#, made by human, my purpose is passing butter.");
        }

        [LuisIntent("FindByFirstThreeLetters")]
        public async Task FindByFirstThreeLettersIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
            string message = "";
            String name3Char = "";
            //string[] searchArray = { };
            var searchArray = new List<string> { };

            foreach (EntityRecommendation e in result.Entities)
            {
                if (e.Type == "alphabet")
                {
                    name3Char += e.Entity;
                }
            }
            if (name3Char.Length > 0)
            {
                searchArray.Add(name3Char);
            }


            EntityRecommendation rank;
            if (result.TryFindEntity(EntityFacultyRank, out rank))
            {
                message += "You said :  " + rank.Entity;
                searchArray.Add(rank.Entity);
            }

            EntityRecommendation department;
            if (result.TryFindEntity(EntityDepartment, out department))
            {
                message += " from department " + department.Entity + ".";
                searchArray.Add(department.Entity);
            }


            await context.SayAsync(text: message + ", first 3 letters " + name3Char+'.',
                                  speak: message + ", first 3 letters " + name3Char + '.');


            //https://stackoverflow.com/questions/2912476/using-c-sharp-to-check-if-string-contains-a-string-in-string-array/2912483
            //string stringToCheck = "text1text2text3";
            //string[] stringArray = { "text1", "someothertext", etc... };
            //if(searchArray.All(str.Contains)) {  }


            message = string.Join(" - ", searchArray); 
            List<Object> list1 = new List<Object>();
            string m_strFilePath = "https://www.flexcode.org/luis/data.xml";
            XDocument xdocument = XDocument.Load(m_strFilePath);
            IEnumerable<XElement> childList = from el in xdocument.Root.Elements() select el;
           
            foreach (XElement e in childList)
            {
                String str = (String)e;
                    
                if (searchArray.All(str.Contains)) {
                    list1.Add(e);
                    message += e + ", ";
                }
            }

            await context.PostAsync("total found = " + list1.Count + " , "+ message);
           

        }

        private async Task ShowLuisResult(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"You have reached {result.Intents[0].Intent}. You said: {result.Query}");
            context.Wait(MessageReceived);
        }


        private async Task TooMany(IDialogContext context, int count)
        {
            await context.SayAsync(text: "I found " + count.ToString() + " people,  Please refine your search.",
                                   speak: "I found " + count.ToString() + " people,  Please refine your search.");
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

        //-----------------------------------------------------------------------------------------------
        // helpdesk demo

        [LuisIntent("IssueWifi")]
        public async Task IssueWifiIntent(IDialogContext context, LuisResult result)
        {
            await context.SayAsync(text: "Are you having Wifi issue, try connect to UMMS with your NT account. goto this page for more help https://umassmed.edu/it/how-do-i/get-started/wireless/",
                                   speak: "Are you having Wifi issue, try connect to UMMS with your NT account.");
        }

        [LuisIntent("IssueVPN")]
        public async Task IssueVPNIntent(IDialogContext context, LuisResult result)
        {
            await context.SayAsync(text: "goto https://remote.umassmed.edu/ to login your computer, for more help Check out this page https://umassmed.service-now.com/kb_view.do?sysparm_article=KB0010511",
                                   speak: "goto here to remote login.");
        }

        [LuisIntent("IssuePassword")]
        public async Task IssuePasswordIntent(IDialogContext context, LuisResult result)
        {
            await context.SayAsync(text: "password issue go here https://umassmed.edu/it/how-do-i/passwords/",
                                   speak: "I can help you with user name and password issue. no need for any human contact.");
        }


        [LuisIntent("IssueMapDrive")]
        public async Task IssueMapDriveIntent(IDialogContext context, LuisResult result)
        {
            await context.SayAsync(text: "goto here for more infomation about R drive https://umassmed.edu/it/how-do-i/get-connected/r-drives/",
                                   speak: "goto here for more infomation about R drive.");
        }


        [LuisIntent("IssueDuo")]
        public async Task IssueDuoIntent(IDialogContext context, LuisResult result)
        {
            await context.SayAsync(text: "install the app or answer your phone and press 5. https://www.umassmed.edu/it/security/multi-factor-authentication/",
                                   speak: "install the app or answer your office phone and press 5.");
        }
        
        [LuisIntent("AboutTalk")]
        public async Task AboutTalkIntent(IDialogContext context, LuisResult result)
        {
            await context.SayAsync(text: "Me! The talk is about me! How smart I am, how fast I am. I am the center of all servers after all.",
                                   speak: "Me! The talk is about me! How smart I am, how fast I am. I am the center of all servers after all.");
        }
        
        [LuisIntent("AboutSpeaker")]
        public async Task AboutSpeakerIntent(IDialogContext context, LuisResult result)
        {
            await context.SayAsync(text: "The pretty one is Veronika Kolesnikova, when she is not coding with Microsoft technologies, she dances, travels, and practices aerial yoga. Follow her on twitter @breakpointv16. The other one is just Willy Ci, he wrote part of my skills, he also like to working on Hololens, AR, VR, boring stuff. Follow him on twitter @willy_ci.",
                                   speak: "The pretty one is Veronika Kolesnikova, when she is not coding with Microsoft technologies, she dances, travels, and practices aerial yoga. Follow her on twitter @breakpointv16. The other one is just Willy Ci, he wrote part of my skills, he also like to working on Hololens, AR, VR, boring stuff.  Follow him on twitter @willy_ci.");
        }
        
        
        //----------------------------------------------

    }
}
