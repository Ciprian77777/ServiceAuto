using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace ServiceAuto.Models;

[Table("Mecanici")]
public class Mecanic
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Nume { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string ParolaHash { get; set; } = string.Empty;
    public string Telefon { get; set; } = string.Empty;
    public DateTime DataAngajare { get; set; }
    public bool EsteAdmin { get; set; }
    public bool EsteActiv { get; set; } = true;
}
