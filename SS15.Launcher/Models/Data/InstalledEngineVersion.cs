using System.Text.Json.Serialization;

namespace Trauma.Launcher.Models.Data;

public sealed record InstalledEngineVersion(
    [property: JsonPropertyName("engine")]
    string Engine,
    [property: JsonPropertyName("version")]
    string Version,
    [property: JsonPropertyName("signature")]
    string Signature);
