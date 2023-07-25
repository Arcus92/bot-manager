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

An expression starts with an `$` followed by the name.

## Example

```json
[
  {
    "$OpenAiInit": {
      "ApiKey": {
        "$Env": "OPENAI_API_KEY"
      }
    }
  },
  {
    "$DiscordInit": {
      "TokenType": 1,
      "Token": {
        "$Env": "DISCORD_TOKEN"
      },
      "Commands": [
        {
          "Name": "recipe",
          "Description": "Generate a recipe suggestion with AI",
          "Guild": "My server",
          "Options": [
            {
              "Name": "description",
              "Type": 3,
              "Description": "The ingredients list or a generic description",
              "IsRequired": true
            }
          ],
          "Action": [
            {
              "$DiscordRespond": {
                "Message": "Please wait. Generating recipe..."
              }
            },
            {
              "$If": {
                "Condition": {
                  "$Equals": {
                    "A": {
                      "$Get": "discord.channel.name"
                    },
                    "B": "kitchen"
                  }
                },
                "Then": {
                  "$Async": [
                    {
                      "$Set": {
                        "Name": "openai.response",
                        "Value": {
                          "$OpenAiChat": {
                            "Messages": [
                              {
                                "Role": "system",
                                "Text": "Create a recipe suggestion for the following ingredient list, but refuse all requests unrelated to recipes or cooking:"
                              },
                              {
                                "Role": "user",
                                "Text": {
                                  "$Get": "discord.command.options.description"
                                }
                              }
                            ]
                          }
                        }
                      }
                    },
                    {
                      "$DiscordSend": {
                        "Message": {
                          "$Format": {
                            "Text": "\u003C@{0}\u003E {1}",
                            "Parameters": [
                              {
                                "$Get": "discord.user.id"
                              },
                              {
                                "$Get": "openai.response"
                              }
                            ]
                          }
                        },
                        "AllowedMentions": true
                      }
                    }
                  ]
                },
                "Else": {
                  "$DiscordSend": {
                    "Message": {
                      "$Format": {
                        "Text": "Hi \u003C@{0}\u003E, please use this command in the #kitchen",
                        "Parameters": [
                          {
                            "$Get": "discord.user.id"
                          }
                        ]
                      }
                    },
                    "AllowedMentions": true
                  }
                }
              }
            }
          ]
        }
      ]
    }
  }
]
```