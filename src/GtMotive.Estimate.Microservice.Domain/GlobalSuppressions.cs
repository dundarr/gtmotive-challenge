// <copyright file="GlobalSuppressions.cs" company="GtMotive">
//   Copyright (c) GtMotive. All rights reserved.
// </copyright>
// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Security Hotspot", "S4834:Controlling permissions is security-sensitive", Justification = "Needed for Microsoft.AspNetCore.Authorization.IAuthorizationService", Scope = "type", Target = "~T:GtMotive.Estimate.Microservice.Domain.Interfaces.IAuthorizationService")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S2326:Unused type parameters should be removed", Justification = "T is necessary for dependency injection.", Scope = "type", Target = "~T:GtMotive.Estimate.Microservice.Domain.Interfaces.IAppLogger`1")]
