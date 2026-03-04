// <copyright file="IBusFactory.cs" company="GtMotive">
//   Copyright (c) GtMotive. All rights reserved.
// </copyright>

using System;

namespace GtMotive.Estimate.Microservice.Domain.Interfaces
{
    /// <summary>
    /// Bus Factory.
    /// </summary>
    public interface IBusFactory
    {
        /// <summary>
        /// Gets a bus client for an event type.
        /// </summary>
        /// <param name="eventType">Event type.</param>
        /// <returns>Bus client.</returns>
        IBus GetClient(Type eventType);
    }
}
