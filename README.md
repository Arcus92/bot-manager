# BotManager

This project is in very early development and properly not usable for your use case!

This is a programmable Discord bot we are using for our server. As a developer I always like to have full control over 
my tools. While [MEE6](https://mee6.xyz) oder [Moobot](https://moo.bot/) are great tools for non-developers, they lack 
certain complex features that would only takes a few lines of code for developers.

The Bot-Manager should provide an easy-to-use programmable interface.

## How it works

The application takes a json configuration and runs it. The whole logic is defined in these file. The project provides 
building blocks called *expressions*. In the json file expression types starts with a dollar sign (`$`) followed by the
type name. See [Api Documentation](https://arcus92.github.io/bot-manager/api/index.html) for a list of all expressions and how they
work.

Why am I using json instead of C# or JavaScript? The idea is not to write the configuration manually. I want to write a
web interface with a [Scratch](https://scratch.mit.edu/)-like visual programming language.

### Run

Currently the `BotManager` only takes these json files als arguments. Simply drop a config file onto the binary to run 
it. If multiple json files are provided the app will run all in sequence. 

How ever this is just for the early stages and will change in future.

### Docker

The docker container runs the BotManager with the config provided in `/config/run.json`. This is just an example config
and can be overwritten via a docker volume mount.

## Examples

- [Hello world](examples/hello-world.json) - A Discord bot that says 'Hello World!' via a `/hello` command.
- [ChefGPT](examples/chef-gpt.json) - A Discord bot that returns a recipe generated by ChatGPT via a `/recipe` command.

