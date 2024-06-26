// <snippet_using>
using Microsoft.Azure.CognitiveServices.ContentModerator;
//using Microsoft.CognitiveServices.ContentModerator;
//using Microsoft.CognitiveServices.ContentModerator.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
// </snippet_using>

namespace TextModeration
{
    class Program
    {
        // <snippet_fields>
        // The name of the file that contains the text to evaluate.
        private static string TextFile = @"YOUR_INPUT_TEXT_FILE_COMPLETE_PATH";

        // The name of the file to contain the output from the evaluation.
        private static string OutputFile = @"YOUR_OUTPUT_TEXT_FILE_COMPLETE_PATH";
        // </snippet_fields>

        static void Main(string[] args)
        {
            // <snippet_main>
            // Load the input text.
            string text = File.ReadAllText(TextFile);
            Console.WriteLine("Screening {0}", TextFile);

            text = text.Replace(System.Environment.NewLine, " ");
            byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(text);
            MemoryStream stream = new MemoryStream(byteArray);

            // Save the moderation results to a file.
            using (StreamWriter outputWriter = new StreamWriter(OutputFile, false))
            {
                // Create a Content Moderator client and evaluate the text.
                using (var client = Clients.NewClient())
                {
                    // Screen the input text: check for profanity,
                    // autocorrect text, check for personally identifying
                    // information (PII), and classify the text into three categories
                    outputWriter.WriteLine("Autocorrect typos, check for matching terms, PII, and classify.");
                    var screenResult =
                    client.TextModeration.ScreenText("text/plain", stream, "eng", true, true, null, true);
                    outputWriter.WriteLine(
                            JsonConvert.SerializeObject(screenResult, Formatting.Indented));
                }
                outputWriter.Flush();
                outputWriter.Close();
            }
            // </snippet_main>
        }

    }

    // <snippet_client>
    // Wraps the creation and configuration of a Content Moderator client.
    public static class Clients
    {
        // The base URL fragment for Content Moderator calls.
        // Add your Azure Content Moderator endpoint to your environment variables.
        private static readonly string AzureBaseURL = "CONTENT_MODERATOR_ENDPOINT";

        // Your Content Moderator subscription key.
        // Add your Azure Content Moderator subscription key to your environment variables.
        private static readonly string CMSubscriptionKey = "CONTENT_MODERATOR_SUBSCRIPTION_KEY";

        // Returns a new Content Moderator client for your subscription.
        public static ContentModeratorClient NewClient()
        {
            // Create and initialize an instance of the Content Moderator API wrapper.
            ContentModeratorClient client = new ContentModeratorClient(new ApiKeyServiceClientCredentials(CMSubscriptionKey));

            client.Endpoint = AzureBaseURL;
            return client;
        }
    }
    // </snippet_client>
}