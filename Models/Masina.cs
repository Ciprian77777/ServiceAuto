using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace ServiceAuto.Models;

[Table("Masini")]
public class Masina
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [NotNull]
    public int ClientId { get; set; } 

    [NotNull]
    public string Marca { get; set; } = string.Empty;

    [NotNull]
    public string Model { get; set; } = string.Empty;

    [MaxLength(20)]
    public string AnFabricatie { get; set; } = string.Empty;

    [NotNull, Unique]
    public string NrInmatriculare { get; set; } = string.Empty;

    [MaxLength(30)]
    public string SerieSasiu { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Culoare { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Combustibil { get; set; } = "Benzina"; 

    public int Kilometraj { get; set; }

    [MaxLength(500)]
    public string Observatii { get; set; } = string.Empty;

    public DateTime DataInregistrare { get; set; } = DateTime.Now;

    public bool EsteActiv { get; set; } = true;

    
    [Ignore]
    public string DisplayText => $"{Marca} {Model} - {NrInmatriculare}";
    
}
