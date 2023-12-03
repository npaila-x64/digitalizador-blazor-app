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
        formulario.id = newUuid.ToString();
        formulario.nombres = _formularioData.Nombres;
        formulario.apellidoPaterno = _formularioData.ApellidoPaterno;
        formulario.apellidoMaterno = _formularioData.ApellidoMaterno;
        formulario.rut = _formularioData.Rut;
        formulario.especialidad = _formularioData.Especialidad;
        formulario.nombreUnidad = _formularioData.NombreUnidad;
        formulario.telefono = _formularioData.Telefono;
        formulario.detalle = _formularioData.Detalle;
        formulario.estado = "Por evaluar";
        formulario.peticion = "";
        _mongoDbService.AddFormularioAsync(formulario);
    }
    public async Task<List<FormularioTranscription>> ObtenerFormularios()
    {
        return await _mongoDbService.GetFormulariosAsync();
    }

}
