using System.ComponentModel;

namespace ConwaysGameOfLife.Api.Serialization
{
    /// <summary>
    ///     An enumeration of serialization modes.
    /// </summary>
    public enum SerializationMode
    {
        [Description("Indicates that the default ASCII serializer should be used.")]
        Ascii,

        [Description("Indicates that the default binary serializer should be used.")]
        Binary
    }
}
