using System.Net.Http.Json;

public class FormularioService
{
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

    public async Task EnviarFormulario()
    {
        var obj = new
        {
            Nombres = _formularioData.Nombres,
            ApellidoPaterno = _formularioData.ApellidoPaterno,
            ApellidoMaterno = _formularioData.ApellidoMaterno,
            Rut = _formularioData.Rut,
            Especialidad = _formularioData.Especialidad,
            NombreUnidad = _formularioData.NombreUnidad,
            Telefono = _formularioData.Telefono,
            Detalle = _formularioData.Detalle,
        };

        JsonContent content = JsonContent.Create(obj);

        var httpClient = new HttpClient();
        var response = await httpClient.PostAsync("http://localhost:40401/solicitudes", content);
        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("Formulario sent!");
        }
        else
        {
            Console.WriteLine($"Error: {response.StatusCode}");
        }
    }
    public async Task<List<FormularioTranscription>> ObtenerFormularios()
    {
        var httpClient = new HttpClient();
        var response = await httpClient.GetAsync("http://localhost:40401/solicitudes");
        if (response.IsSuccessStatusCode)
        {
            // Nota: No se extraen objetos formularios, sino que se extrae un objeto que contiene los datos del mismo
            var result = await response.Content.ReadFromJsonAsync<List<FormularioTranscription>>();
            return result;
        }
        else
        {
            Console.WriteLine($"Error: {response.StatusCode}");
            return null;
        }
    }

}
