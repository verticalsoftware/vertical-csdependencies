// Copyright(c) 2019 Vertical Software - All rights reserved
//
// This code file has been made available under the terms of the
// MIT license. Please refer to LICENSE.txt in the root directory
// or refer to https://opensource.org/licenses/MIT

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Vertical.CommandLine;
using Vertical.Tools.CSharpDependencies.Configuration;
using Vertical.Tools.CSharpDependencies.Services;

namespace Vertical.Tools.CSharpDependencies
{
    /// <summary>
    /// Contains the entry point.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Entry point
        /// </summary>
        [ExcludeFromCodeCoverage]
        internal static async Task Main(string[] args)
        {
            try
            {
                await CommandLineApplication.RunAsync(new RunOptionsConfiguration(async options =>
                {
                    await using var services = new ServiceContainer(options);
                    await services.ProjectAggregator.AggregateProjectsAsync();
                }), args);
            }
            catch (AggregateException aggregateException)
            {
                foreach (var exception in aggregateException.InnerExceptions)
                {
                    Console.WriteLine(exception.Message);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}
