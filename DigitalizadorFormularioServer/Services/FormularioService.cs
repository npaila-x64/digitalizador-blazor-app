using System.Net.Http.Json;

public class FormularioService
{
    private readonly MongoDbService _mongoDbService;
    private FormularioTranscription _formularioData;

    public FormularioTranscription FormularioData
    {
        get => _formularioData;
        set
        {
            _formularioData = value;
            NotifyDataChanged();
        }
    } 
    
    public event Action OnDataChanged;

    private void NotifyDataChanged() => OnDataChanged?.Invoke();

    public FormularioService(MongoDbService mongoDbService)
    {
        _mongoDbService = mongoDbService;
    }

    public async Task EnviarFormulario()
    {
        Guid newUuid = Guid.NewGuid();
        var formulario = new Formulario();
        formulario.Id = newUuid.ToString();
        formulario.Nombres = _formularioData.Nombres;
        formulario.ApellidoPaterno = _formularioData.ApellidoPaterno;
        formulario.ApellidoMaterno = _formularioData.ApellidoMaterno;
        formulario.Rut = _formularioData.Rut;
        formulario.Especialidad = _formularioData.Especialidad;
        formulario.NombreUnidad = _formularioData.NombreUnidad;
        formulario.Telefono = _formularioData.Telefono;
        formulario.Detalle = _formularioData.Detalle;
        formulario.Estado = "Por evaluar";
        formulario.Peticion = "";
        _mongoDbService.AddFormularioAsync(formulario);
    }
    public async Task<List<FormularioTranscription>> ObtenerFormularios()
    {
        return await _mongoDbService.GetFormulariosAsync();
    }

}
