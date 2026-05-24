using System.Numerics;
using System.Text;

namespace Novolis.Math.Geometry;

/// <summary>Text serialization helpers for geometry primitives.</summary>
public static class PrimitiveSerializers
{
    /// <summary>Serialize/deserialize <see cref="Vector3"/> values.</summary>
    public static class Vector3Serializer
    {
        /// <summary>Parses <c>(x|y|z)</c> text into a vector.</summary>
        /// <param name="vector">Serialized vector.</param>
        /// <returns>Deserialized vector.</returns>
        public static Vector3 Deserialize(string vector)
        {
            var vectorParts = vector.Split('|');
            return new Vector3(
                float.Parse(vectorParts[0].Replace("(", "")),
                float.Parse(vectorParts[1]),
                float.Parse(vectorParts[2].Replace(")", "")));
        }

        /// <summary>Formats a vector as <c>(x|y|z)</c>.</summary>
        /// <param name="vector">Source vector.</param>
        /// <param name="format">Numeric format; defaults to <c>F</c>.</param>
        /// <returns>Serialized text.</returns>
        public static string Serialize(Vector3 vector, string format = "F")
            => $"({vector.X.ToString(format)}|{vector.Y.ToString(format)}|{vector.Z.ToString(format)})";
    }

    /// <summary>Deserializes a vector string via <see cref="Vector3Serializer"/>.</summary>
    /// <param name="vector">Serialized vector.</param>
    /// <returns>Deserialized vector.</returns>
    public static Vector3 DeserializeVector3(string vector)
        => Vector3Serializer.Deserialize(vector);

    /// <summary>Serializes a vector via <see cref="Vector3Serializer"/>.</summary>
    /// <param name="vector">Source vector.</param>
    /// <param name="format">Numeric format.</param>
    /// <returns>Serialized text.</returns>
    public static string SerializeVector3(Vector3 vector, string format = "F")
        => Vector3Serializer.Serialize(vector, format);

    /// <summary>Serializes a sequence of vectors as a titled list.</summary>
    /// <param name="vectors">Vectors to serialize.</param>
    /// <param name="format">Numeric format.</param>
    /// <returns>Multi-line text block.</returns>
    public static string SerializeVector3s(IEnumerable<Vector3> vectors, string format = "F")
        => SerializeList(vectors.Select(vector => SerializeVector3(vector, format)), "Vectors");

    /// <summary>Serialize/deserialize <see cref="Edge"/> values.</summary>
    public static class EdgeSerializer
    {
        /// <summary>Parses <c>&lt;a=b&gt;</c> text into an edge.</summary>
        /// <param name="edge">Serialized edge.</param>
        /// <returns>Deserialized edge.</returns>
        public static Edge Deserialize(string edge)
        {
            var edgeParts = edge.Split('=');
            return new Edge(
                Vector3Serializer.Deserialize(edgeParts[0].Replace("<", "")),
                Vector3Serializer.Deserialize(edgeParts[1].Replace(">", "")));
        }

        /// <summary>Formats an edge as <c>&lt;a=b&gt;</c>.</summary>
        /// <param name="edge">Source edge.</param>
        /// <param name="format">Numeric format for endpoints.</param>
        /// <returns>Serialized text.</returns>
        public static string Serialize(Edge edge, string format = "F")
            => $"<{Vector3Serializer.Serialize(edge.A, format)}={Vector3Serializer.Serialize(edge.B, format)}>";
    }

    /// <summary>Deserializes an edge string via <see cref="EdgeSerializer"/>.</summary>
    /// <param name="edge">Serialized edge.</param>
    /// <returns>Deserialized edge.</returns>
    public static Edge DeserializeEdge(string edge)
        => EdgeSerializer.Deserialize(edge);

    /// <summary>Serializes an edge via <see cref="EdgeSerializer"/>.</summary>
    /// <param name="edge">Source edge.</param>
    /// <param name="format">Numeric format.</param>
    /// <returns>Serialized text.</returns>
    public static string SerializeEdge(Edge edge, string format = "F")
        => EdgeSerializer.Serialize(edge, format);

    /// <summary>Serializes a sequence of edges as a titled list.</summary>
    /// <param name="edges">Edges to serialize.</param>
    /// <param name="format">Numeric format.</param>
    /// <returns>Multi-line text block.</returns>
    public static string SerializeEdges(IEnumerable<Edge> edges, string format = "F")
        => SerializeList(edges.Select(edge => SerializeEdge(edge, format)), "Edges");

    /// <summary>Polygon serialization (currently returns empty string).</summary>
    /// <param name="polygon">Polygon to serialize.</param>
    /// <param name="format">Numeric format.</param>
    /// <returns>Serialized text.</returns>
    public static string SerializePolygon(Polygon polygon, string format = "F")
    {
        return "";
    }

    private static string SerializeList(IEnumerable<string> list, string title, string indent = "    ")
    {
        var sb = new StringBuilder();
        sb.AppendLine(title);
        foreach (var item in list) sb.AppendLine($"{indent}{item}");
        return sb.ToString();
    }
}
