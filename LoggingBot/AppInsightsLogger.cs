using Microsoft.ApplicationInsights;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoggingBot
{
    public class AppInsightsLogger : ITranscriptLogger
    {
        private TelemetryClient telemetryClient;
        
        public AppInsightsLogger(TelemetryClient telemetryClient) : base()
        {
            this.telemetryClient = telemetryClient;
        }
        
        public Task LogActivityAsync(IActivity activity)
        {
            // track only messages
            if (activity is IMessageActivity)
            {
                var data = new Dictionary<string, string>()
                {
                    { "ConversationId", activity.Conversation.Id },
                    { "FromId", activity.From.Id },
                    { "FromName", activity.From.Name },
                    { "RecipientId", activity.Recipient.Id },
                    { "RecipientName", activity.Recipient.Name },
                    { "Channel", activity.ChannelId },
                    { "Text", activity.AsMessageActivity().Text }
                };
                
                telemetryClient.TrackEvent("Message", data);
            }

            return Task.CompletedTask;
        }
    }
}
