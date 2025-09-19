using System;

namespace OpenShock.SDK.CSharp;

public class ApiClientOptions {
    /// <summary>
    /// OpenShock API server. e.g. https://api.openshock.app
    /// </summary>
    public Uri Server { get; set; } = new Uri("https://api.openshock.app");

    /// <summary>
    /// OpenShock API token.
    /// </summary>
    public required string Token { get; set; }

    /// <summary>
    /// Context of the program using the API.
    /// </summary>
    public ProgramInfo? Program { get; set; }

    public sealed class ProgramInfo {
        /// <summary>
        /// Name of the program using the API.
        /// If null, the default value is the name of the calling assembly.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Version of the program using the API.
        /// If null, the default value is the version of the calling assembly.
        /// </summary>
        public required Version Version { get; set; }
    }
}