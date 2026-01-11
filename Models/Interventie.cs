using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace ServiceAuto.Models;

[Table("Interventii")]
public class Interventie
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [NotNull]
    public int MasinaId { get; set; }

    [NotNull]
    public int MecanicId { get; set; }

    [NotNull]
    public int ClientId { get; set; }

    [NotNull]
    public string Descriere { get; set; } = string.Empty;

    [MaxLength(50)]
    public string TipInterventie { get; set; } = "Reparatie"; 

    public DateTime DataProgramare { get; set; } = DateTime.Now;

    public DateTime DataFinalizare { get; set; }

    [NotNull]
    public string Status { get; set; } = "Programata"; 

    public decimal CostTotal { get; set; }

    public decimal OreManopera { get; set; }

    [MaxLength(1000)]
    public string Observatii { get; set; } = string.Empty;

    public bool FacturaEmisa { get; set; } = false;

    public DateTime DataCreare { get; set; } = DateTime.Now;

    
    [Ignore]
    public string StatusDisplay
    {
        get
        {
            return Status switch
            {
                "Programata" => "📅 Programată",
                "InCurs" => "🔧 În curs",
                "Finalizata" => "✅ Finalizată",
                "Anulata" => "❌ Anulată",
                _ => Status
            };
        }
    }

    [Ignore]
    public Color StatusColor
    {
        get
        {
            return Status switch
            {
                "Programata" => Colors.Blue,
                "InCurs" => Colors.Orange,
                "Finalizata" => Colors.Green,
                "Anulata" => Colors.Red,
                _ => Colors.Gray
            };
        }
    }
    
    [Ignore]
    public string MasinaInfo { get; set; }

    [Ignore]
    public string StatusIcon
    {
        get
        {
            return Status switch
            {
                "Programată" => "📅",
                "În progres" => "🔧",
                "Finalizată" => "✅",
                "Anulată" => "❌",
                _ => "❓"
            };
        }
    }
}
