using Microsoft.AspNetCore.Components.Forms;
using System.Text.Json;
using System.Net.Http.Json;
using Newtonsoft.Json;

public class ImageService
{
    private readonly FormularioService _formularioService;
    private readonly OpenAIService _openAIService;
    private MemoryStream _fileStream;
    private FormularioTranscription formulario;
    public string ImageBase64 { get; private set; }
    public event Action OnImageProcessed;


    public ImageService(FormularioService formularioService, OpenAIService openAIService)
    {
        _formularioService = formularioService;
        _openAIService = openAIService;
    }

    public async Task StoreFileAsync(IBrowserFile file)
    {
        _fileStream?.Dispose();
        _fileStream = new MemoryStream();
        await file.OpenReadStream().CopyToAsync(_fileStream);
        _fileStream.Position = 0;
        Console.WriteLine($"Stored {_fileStream.Length} bytes");

        ConvertImageToBase64();
    }

    public void ConvertImageToBase64()
    {
        if (_fileStream == null)
        {
            return;
        }

        _fileStream.Position = 0;
        var byteArray = _fileStream.ToArray();
        
        ImageBase64 = Convert.ToBase64String(byteArray);
        OnImageProcessed?.Invoke();
    }

    public void Dispose()
    {
        _fileStream?.Dispose();
    }

    public async Task TranscribeImage()
    {
        var response = await _openAIService.GetResponseString(ImageBase64);
        Console.WriteLine(response);
        Console.WriteLine(ProcessJson(response));
        _formularioService.FormularioData = ProcessJson(response);
    }

    public FormularioTranscription ProcessJson(string jsonEnclosedByThreeBackticks)
    {
        string json = jsonEnclosedByThreeBackticks.Replace("```", "").Replace("json", "").Trim();
        var jsonAsDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        var formulario = new FormularioTranscription();

        formulario.Nombres = jsonAsDict.ContainsKey("NOMBRES") ? jsonAsDict["NOMBRES"] : "";
        formulario.ApellidoPaterno = jsonAsDict.ContainsKey("APELLIDO PATERNO") ? jsonAsDict["APELLIDO PATERNO"] : "";
        formulario.ApellidoMaterno = jsonAsDict.ContainsKey("APELLIDO MATERNO") ? jsonAsDict["APELLIDO MATERNO"] : "";
        formulario.Rut = jsonAsDict.ContainsKey("RUT") ? jsonAsDict["RUT"] : "";
        formulario.Especialidad = jsonAsDict.ContainsKey("ESPECIALIDAD DEL FUNCIONARIO") ? jsonAsDict["ESPECIALIDAD DEL FUNCIONARIO"] : "";
        formulario.NombreUnidad = jsonAsDict.ContainsKey("NOMBRE DE LA UNIDAD") ? jsonAsDict["NOMBRE DE LA UNIDAD"] : "";
        formulario.Telefono = jsonAsDict.ContainsKey("TELÉFONO") ? jsonAsDict["TELÉFONO"] : "";
        formulario.Detalle = jsonAsDict.ContainsKey("DETALLE SITUACIÓN ECONOMICA") ? jsonAsDict["DETALLE SITUACIÓN ECONOMICA"] : "";

        return formulario; 
    }
}
