// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace LoggingBot
{
    public class AdapterWithErrorHandler : BotFrameworkHttpAdapter
    {
        public AdapterWithErrorHandler(
            IConfiguration configuration,
            TranscriptLoggerMiddleware loggingMiddleware,
            ILogger<BotFrameworkHttpAdapter> logger
        ) : base(configuration, logger)
        {
            if (loggingMiddleware == null)
            {
                throw new NullReferenceException(nameof(loggingMiddleware));
            }

            Use(loggingMiddleware);
            
            OnTurnError = async (turnContext, exception) =>
            {
                // Log any leaked exception from the application.
                logger.LogError($"Exception caught : {exception.Message}");

                // Send a catch-all apology to the user.
                await turnContext.SendActivityAsync("Sorry, it looks like something went wrong.");
            };
        }
    }
}
