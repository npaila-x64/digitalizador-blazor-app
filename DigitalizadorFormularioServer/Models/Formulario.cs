using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Formulario
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId _id { get; set; }
    public string Id { get; set; }
    public string Nombres { get; set; }
    public string ApellidoPaterno { get; set; }
    public string ApellidoMaterno { get; set; }
    public string Rut { get; set; }
    public string Especialidad { get; set; }
    public string NombreUnidad { get; set; }
    public string Telefono { get; set; }
    public string Estado { get; set; }
    public string Detalle { get; set; }
    public string Peticion { get; set; }
}
