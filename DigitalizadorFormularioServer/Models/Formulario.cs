using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Formulario
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId _id { get; set; }
    public string id { get; set; }
    public string nombres { get; set; }
    public string apellidoPaterno { get; set; }
    public string apellidoMaterno { get; set; }
    public string rut { get; set; }
    public string especialidad { get; set; }
    public string nombreUnidad { get; set; }
    public string telefono { get; set; }
    public string estado { get; set; }
    public string detalle { get; set; }
    public string peticion { get; set; }
}
