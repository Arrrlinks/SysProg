namespace Program.Models;

public class save_m
{
    //Attributes
    public string _name { get; set; }
    public string _source { get; set; }
    public string _target { get; set; }
    
    //Builders
    public save_m(){}
    
    public save_m(string name, string source, string target)
    {
        _name = name;
        _source = source;
        _target = target;
    }
    
    //Methods
}