using CryptoSoft;
using CryptoSoft;
namespace CryptoSoft;

public class ViewModel
{
    private View _view;
    private Model _model;
    private string _path;
    private string _key;

    public ViewModel()
    {
        _path = "";
        _key = "";
        _view = new View();
        _model = new Model();
    }
    
    public ViewModel(string path, string key)
    {
        _path = path.Replace("?"," ");
        _key = key;
        _view = new View();
        _model = new Model();
    }
    
    public void SetPath()
    {
        _path = _view.GetPath().Replace("?"," ");
    }
    
    public void SetKey()
    {
        _key = _view.GetKey();
    }
    
    public void Encrypt()
    {
        _model.encrypt(_path.Replace("?"," "), _key);
    }

    public void Run()
    {
        //if the path or the key is null, get them
        if (_path.Length == 0)
        {
            _path = _view.GetPath().Replace("?"," ");
        }
        if (_key.Length == 0)
        {
            _key = _view.GetKey();
        }
        if (File.Exists(_path.Replace("?"," ")) == false || _key.Length > 64 || _key.Length == 0)
        {
            _view.ShowError();
            return;
        }
        Encrypt();
    }
    
}