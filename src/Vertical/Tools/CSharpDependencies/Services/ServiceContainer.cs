// Copyright(c) 2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using System;
using System.IO.Abstractions;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Vertical.Tools.CSharpDependencies.Abstractions;
using Vertical.Tools.CSharpDependencies.Configuration;

namespace Vertical.Tools.CSharpDependencies.Services
{
    /// <summary>
    /// Provides access to services.
    /// </summary>
    public class ServiceContainer : IAsyncDisposable
    {
        private readonly ServiceProvider _serviceProvider;
        
        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        public ServiceContainer(RunOptions options)
        {
            var services = new ServiceCollection();
            
            services.AddSingleton(options);
            services.AddSingleton<IProjectAggregator, ProjectAggregator>();
            services.AddSingleton<IProjectSearchProvider, ProjectSearchProvider>();
            services.AddSingleton(ProjectBuilder.CreateSerializer());
            services.AddSingleton<IProjectBuilder, ProjectBuilder>();
            services.AddSingleton<IFileSystem, FileSystem>();
            services.AddSingleton<IOutputFormatter, OutputFormatter>();
            services.AddSingleton<IOptions, OptionsProvider>();
            services.AddSingleton(provider => provider.GetService<IFileSystem>().Directory);
            services.AddSingleton(CreateLogger(options));

            if (!options.Tree)
                services.AddSingleton<IOutputChannel, DefaultOutputChannel>();
            else
                services.AddSingleton<IOutputChannel, TreeOutputChannel>();
            
            _serviceProvider = services.BuildServiceProvider();
        }

        /// <summary>
        /// Gets the project aggregation component.
        /// </summary>
        public IProjectAggregator ProjectAggregator => _serviceProvider.GetRequiredService<IProjectAggregator>();

        /// <inheritdoc />
        public ValueTask DisposeAsync() => _serviceProvider.DisposeAsync();

        private static ILogger CreateLogger(RunOptions options)
        {
            var template = options.DebugOutput
                ? "[{Level:u3}] {Message:lj}{NewLine}"
                : "{Message:lj}{NewLine}";
            
            var logLevel = options.DebugOutput
                ? LogEventLevel.Debug
                : LogEventLevel.Information;

            return new LoggerConfiguration()
                .WriteTo.Console(outputTemplate: template, restrictedToMinimumLevel: logLevel)
                .MinimumLevel.Is(logLevel)
                .CreateLogger();
        }
    }
}