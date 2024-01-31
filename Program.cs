using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using CozyCloudBot.Services;

const string tgApiToken = "6862033522:AAE_-pOvLNvTLZqWK00vDhOV9n2rm75ZOXc";
const string weatherApiKey = "86969650cf7c416195d72537242601";
const string city = "Vladivostok";
IWeatherService weatherService = new WeatherService(weatherApiKey);

var botClient = new TelegramBotClient(tgApiToken);

using CancellationTokenSource cts = new ();

// StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
ReceiverOptions receiverOptions = new ()
{
    AllowedUpdates = Array.Empty<UpdateType>() // receive all update types except ChatMember related updates
};

botClient.StartReceiving(
    updateHandler: HandleUpdateAsync,
    pollingErrorHandler: HandlePollingErrorAsync,
    receiverOptions: receiverOptions,
    cancellationToken: cts.Token
);

while(true)
{
    await Task.Delay(1000000);
    
    string weather = await weatherService.GetWeatherAsync(city);

    await botClient.SendTextMessageAsync(
        chatId: 453437236,
        text: weather,
        cancellationToken: cts.Token);
}

async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    if (update.Message is not { } message)
        return;
    // Only process text messages
    if (message.Text is not { } messageText)
        return;

    var chatId = message.Chat.Id;

    string weather = await weatherService.GetWeatherAsync(city);

    // Echo received message text
    await botClient.SendTextMessageAsync(
        chatId: chatId,
        text: weather,
        cancellationToken: cancellationToken);
}

Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{
    var ErrorMessage = exception switch
    {
        ApiRequestException apiRequestException
            => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
        _ => exception.ToString()
    };

    Console.WriteLine(ErrorMessage);
    return Task.CompletedTask;
}