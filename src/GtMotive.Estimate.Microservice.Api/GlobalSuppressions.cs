// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Security Hotspot", "S6781:JWT secret keys should not be disclosed", Justification = "Local auth emulator only; key from configuration for development/Swagger.", Scope = "member", Target = "~M:GtMotive.Estimate.Microservice.Api.Controllers.LocalAuthController.ConnectToken(System.String,System.String,System.String)")]
