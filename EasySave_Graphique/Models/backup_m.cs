﻿using System.Runtime.InteropServices.JavaScript;

namespace EasySave_Graphique.Models;

public class backup_m
{
    public bool selected { get; set; } = false;
    public string Name { get; set; }
    public string Source { get; set; }
    public string Target { get; set; }
    public string Date { get; set; }
    public string Size { get; set; }
    public string filesNB { get; set; }
    public string State { get; set; }
}