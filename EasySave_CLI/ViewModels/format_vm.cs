using EasySave_CLI.Models;
using EasySave_CLI.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class format_vm
{
    format_v _view = new format_v();
    private static readonly format_m _model = new format_m(); // Instance of the format model
    private static readonly language_m _language = new language_m(); // Instance of the language model
    private static string? lang = _language.RetrieveValueFromLanguageFile("LanguageChosen", "Lang");

    public void Run()
    {

        _view.Display();
        string? choosedFormat = Console.ReadLine();
        while (choosedFormat != "json" && choosedFormat != "xml")
        {
            Console.WriteLine(_language.RetrieveValueFromLanguageFile(lang, "InvalidFormat"));
            choosedFormat = Console.ReadLine();
        }
        _model.ChangeFormat(choosedFormat);
        _view.ChangedFormat(choosedFormat);
    }
}