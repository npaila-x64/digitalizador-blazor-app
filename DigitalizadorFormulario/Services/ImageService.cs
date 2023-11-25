using Microsoft.AspNetCore.Components.Forms;
using System.Text.Json;
using System.Net.Http.Json;

public class ImageService
{
    private readonly FormularioService _formularioService;
    private MemoryStream _fileStream;
    private FormularioTranscription formulario;
    public string ImageBase64 { get; private set; }
    public event Action OnImageProcessed;


    public ImageService(FormularioService formularioService)
    {
        _formularioService = formularioService;
    }

    public async Task StoreFileAsync(IBrowserFile file)
    {
        _fileStream?.Dispose();
        _fileStream = new MemoryStream();
        await file.OpenReadStream().CopyToAsync(_fileStream);
        _fileStream.Position = 0;

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
        var obj = new
        {
            content = ImageBase64,
        };

        JsonContent content = JsonContent.Create(obj);

        var httpClient = new HttpClient();
        var response = await httpClient.PostAsync("http://localhost:30301/image", content);
        if (response.IsSuccessStatusCode)
        {
            formulario = await JsonSerializer.DeserializeAsync<FormularioTranscription>(await response.Content.ReadAsStreamAsync());
            _formularioService.FormularioData = formulario;
        }
        else
        {

        }
    }
}
