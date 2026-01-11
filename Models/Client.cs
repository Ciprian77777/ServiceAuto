using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace ServiceAuto.Models;

[Table("Clienti")]
public class Client
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Nume { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefon { get; set; } = string.Empty;
    public string Adresa { get; set; } = string.Empty;
    public DateTime DataInregistrare { get; set; }
    public bool EsteActiv { get; set; } = true;
}
