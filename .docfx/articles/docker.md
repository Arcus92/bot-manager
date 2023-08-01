# Docker

## Install the image

Download the image from the GitHub repository:

```
docker pull ghcr.io/arcus92/bot-manager:latest
```

The image contains a pre-installed bot-manager with all necessary requirements.

## Create the configuration

The BotManager need a configuration to run. Choose a good location to store your config file on your host system. 
For example:
```
mkdir ~/bot-manager
cd ~/bot-manager
```

Now create your `config.json` or copy it from an [example](https://github.com/Arcus92/bot-manager/tree/main/examples).

## Create a container

```
docker container create -v ./config.json:/config/run.json --name bot-manager ghcr.io/arcus92/bot-manager:latest
```

- `docker container create`
  
  Tells Docker to create a new container.
- `-v ./config.json:/config/run.json`
  
  Links the `config.json` from your host system to the container. The config file inside the container must be stored at
  `/config/run.json`.
- `--name bot-manager`

  Defines a name for your container for easy access later on. Without that Docker would generate a random name.
  You can define your own name as well. 
- `ghcr.io/arcus92/bot-manager:latest`

  The name of the image to creates the container. This is the image we downloaded in the previous step. It contains the
  BotManager and all its requirements pre-installed.

## Start the container

Once the container is created you can start and stop it anytime:

```
docker container start bot-manager
```

## Stop the container

```
docker container stop bot-manager
```

## Auto start the container

You can ensure that the container is started automatically with the Docker demon, unless it's manually stopped:

```
docker update --restart unless-stopped bot-manager
```

This will also restart the BotManager if the process ends unexpectedly / crashes.

You can undo this, so the container don't start with the Docker demon or on a crash:

```
docker update --restart no bot-manager
```

## Checking the logs

Peak inside the container and check the logs if the BotManager doesn't behave:

```
docker logs bot-manager
```
