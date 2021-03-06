﻿using System;
using System.Collections.Generic;

using Bet.Extensions.HealthChecks.ML;

using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MachineLearningHealthChecksBuilderExtensions
    {
        /// <summary>
        ///  Add Machine Learning Health Check for ML.NET model.
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <typeparam name="TPrediction"></typeparam>
        /// <param name="builder"></param>
        /// <param name="name"></param>
        /// <param name="configure"></param>
        /// <param name="failureStatus"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        public static IHealthChecksBuilder AddMachineLearningModelCheck<TInput, TPrediction>(
            this IHealthChecksBuilder builder,
            string name,
            Action<MachineLearningHealthCheckOptions<TInput>>? configure = null,
            HealthStatus? failureStatus = default,
            IEnumerable<string>? tags = default)
                where TInput : class, new()
                where TPrediction : class, new()
        {
            if (tags == default)
            {
                tags = new[] { "machine_learning" };
            }

            builder.AddCheck<MachineLearningHealthCheck<TInput, TPrediction>>(name, failureStatus ?? HealthStatus.Unhealthy, tags);

            // Configure named options to pass the threshold into the check.
            builder.Services.Configure<MachineLearningHealthCheckOptions<TInput>>(
                name,
                options =>
                {
                    configure?.Invoke(options);
                });

            return builder;
        }
    }
}
