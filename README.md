# BotManager

This project is in very early development and properly not usable for your purposes.

There are many easy-to-use Discord bots out there. You can configure them over a website and you don't need to manually 
host the bot. That's excellent for non-developers. However, I always want to have full control over the software that
manages my server. I also want to have some programmable features. So I created this manager app that can be configured
and programed by a json file.

## Expressions

The idea of the json configuration is that everything can be represented as an expression. The application defines the
expressions. The available expressions can be extended by plugins. For example: there is currently an 
[OpenAI](https://openai.com/) submodule that can return GPT chat responses.

An expression starts with an `@` followed by the name.

## Example

```json
{
  "@DiscordPlugin": {
    "TokenType": 1,
    "Token": {
      "@Env": "DISCORD_TOKEN"
    },
    "Commands": [
      {
        "Name": "test",
        "Description": "A simple test command",
        "Guild": "My server",
        "Action": {
          "@DiscordRespond": {
            "Message": {
              "@Choose": [
                "You passed the test!",
                "Test successful!",
                "Good test!"
              ]
            }
          }
        }
      }
    ]
  }
}
```