# BotManager

This project is in very early development and properly not usable for your purposes.

There are many easy-to-use Discord bots out there. You can configure them over a website and you don't need to manually 
host the bot. That's excellent for non-developers. However, I always want to have full control over the software that
manages my server. I also want to have some programmable features. So I created this manager app that can be configured
and programed by a json file.

## Expressions

The idea of the json configuration is that everything can be represented as an expression. The application defines the
expressions. See the example below.

An expression is defined in json by an `$` followed by the expression type name. 
See [Api Documentation](https://arcus92.github.io/bot-manager/api/index.html) for a list of all expressions and how they
work.

## Docker

The docker container runs the BotManager with the config provided in `/config/run.json`. This is just an example config
and can be overwritten via a docker volume mount.

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
      "TokenType": "Bot",
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
              "Type": "String",
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
