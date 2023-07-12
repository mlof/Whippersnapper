# Whippersnapper

A small transcription bot for Discord.

## Usage

1. Create a Discord bot in the developer portal.
2. Add the bot's token to `appsettings.json`.
3. Go to the OAuth2 -> Url Generator tab in the Discord Developer Portal, select the bot scope, ensure the "Send Messages" and "Embed Links" permissions are selected and copy the generated URL.
4. Paste the URL into your browser and select the server you want to add the bot to.
5. Run the bot.

It will then automatically transcribe any voice messages sent in the server.

## Configuration

The following configuration options are available:

| Option | Description | Default |
| ------ | ----------- | ------- |
| `BotToken` | The Discord bot token. | `""` |
| `ModelFile` | The path to the model file. | `"ggml-base.bin"` |
| `KeepAttachments` | Whether to keep the audio files after transcription. | `false` |
| `StatusMessage` | The message to display as the bot's status. | `"Always listening, never sleeping."` |
| `BadWords` | A list of words to censor. | `["PHP"]` |

This can be configured in `appsettings.json`. 
If you want to use a different model, you can download one from [here](https://huggingface.co/ggerganov/whisper.cpp) but i've found that the default one works well enough.
