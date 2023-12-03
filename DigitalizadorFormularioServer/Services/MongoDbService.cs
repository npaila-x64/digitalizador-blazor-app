
using MongoDB.Driver;
using System;

public class MongoDbService
{
    private readonly IMongoCollection<Formulario> _formulariosCollection;

    public MongoDbService(IConfiguration config)
    {
        var connectionString = "mongodb://root:password@localhost:40402/?authSource=admin";
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase("myDatabase");
        _formulariosCollection = database.GetCollection<Formulario>("solicitudes");
    }

    public async Task<List<FormularioTranscription>> GetFormulariosAsync()
    {
        var formularios = await _formulariosCollection.Find(_ => true).ToListAsync();
        return formularios.Select(formulario => new FormularioTranscription
        {
            Nombres = formulario.Nombres,
            ApellidoPaterno = formulario.ApellidoPaterno,
            ApellidoMaterno = formulario.ApellidoMaterno,
            Rut = formulario.Rut,
            Especialidad = formulario.Especialidad,
            NombreUnidad = formulario.NombreUnidad,
            Telefono = formulario.Telefono,
            Detalle = formulario.Detalle,
        }).ToList();
    }

    public async Task AddFormularioAsync(Formulario formulario)
    {
        await _formulariosCollection.InsertOneAsync(formulario);
    }
}
