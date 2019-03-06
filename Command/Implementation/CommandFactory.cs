﻿using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CodeBasics.Command.Implementation
{
  internal class CommandFactory : ICommandFactory
  {
    private readonly IServiceProvider services;

    public CommandFactory(IServiceProvider services)
    {
      this.services = services;
    }

    public ICommandInOutAsync<TOut> CreateAsync<TCommand, TIn, TOut>(TIn input) where TCommand : ICommandInOutAsync<TOut>
    {
      var commandOptions = services.GetRequiredService<IOptions<CommandOptions>>();
      var command = services.GetRequiredService<ICommandInOutAsync<TOut>>();

      if (command is ISetSetCommandInput<TIn> setInputCommand)
      {
        setInputCommand.SetInputParameter(input);
      }
      else
      {
        throw new ArgumentException($"Command '{typeof(TCommand).FullName}' does not implement '{typeof(ISetSetCommandInput<TIn>).Name}'. Maybe the wrong input type was implemented?");
      }

      if (command is ISetCommandOptions setOptionsCommand)
      {
        setOptionsCommand.SetCommandOptions(commandOptions.Value);
      }

      return command;
    }
  }
}
