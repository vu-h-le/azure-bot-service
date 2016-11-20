#load "BasicForm.csx"
#load "Message.csx"

using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Azure;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

/// This dialog is the main bot dialog, which will call the Form Dialog and handle the results
[Serializable]
public class MainDialog : IDialog<BasicForm>
{
    public MainDialog()
    {
    }

    public Task StartAsync(IDialogContext context)
    {
        context.Wait(MessageReceivedAsync);
        return Task.CompletedTask;
    }

    public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
    {
        var message = await argument;
        context.Call(BasicForm.BuildFormDialog(FormOptions.PromptInStart), FormComplete);
    }

    private async Task FormComplete(IDialogContext context, IAwaitable<BasicForm> result)
    {
        try
        {
            var form = await result;
            if (form != null)
            {
                //Create a queue Message
                var queueMessage = new Message
                {
                    MicrosoftEmailAddress = form.MicrosoftEmailAddress,
                    ProjectDescription = form.ProjectDescription
                };

                // write the queue Message to the queue
                await AddMessageToQueueAsync(JsonConvert.SerializeObject(queueMessage));
                await context.PostAsync($"Message added to the queue.");

                await context.PostAsync($"Thank you {form.MicrosoftEmailAddress}! Just type anything to restart it.");
            }
            else
            {
                await context.PostAsync("Form returned empty response! Type anything to restart it.");
            }
        }
        catch (OperationCanceledException)
        {
            await context.PostAsync("You canceled the form! Type anything to restart it.");
        }

        context.Wait(MessageReceivedAsync);
    }

    public static async Task AddMessageToQueueAsync(string message)
    {
        // Retrieve storage account from connection string.
        var storageAccount = CloudStorageAccount.Parse(Utils.GetAppSetting("AzureWebJobsStorage"));

        // Create the queue client.
        var queueClient = storageAccount.CreateCloudQueueClient();

        // Retrieve a reference to a queue.
        var queue = queueClient.GetQueueReference("bot-queue");

        // Create the queue if it doesn't already exist.
        await queue.CreateIfNotExistsAsync();

        // Create a message and add it to the queue.
        var queuemessage = new CloudQueueMessage(message);
        await queue.AddMessageAsync(queuemessage);
    }
}