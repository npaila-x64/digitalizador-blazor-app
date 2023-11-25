
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
}
