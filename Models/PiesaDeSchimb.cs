using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace ServiceAuto.Models;

[Table("PieseDeSchimb")]
public class PiesaDeSchimb
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [NotNull]
    public string Denumire { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Cod { get; set; } = string.Empty;

    public string Producator { get; set; } = string.Empty;

    [NotNull]
    public decimal Pret { get; set; }

    [NotNull]
    public int Stoc { get; set; }

    public int StocMinim { get; set; } = 5;

    public DateTime DataAprovizionare { get; set; } = DateTime.Now;

    [MaxLength(500)]
    public string Descriere { get; set; } = string.Empty;

    public string Categorie { get; set; } = "General"; 

    public bool EsteActiv { get; set; } = true;

    
    [Ignore]
    public string StocStatus
    {
        get
        {
            if (Stoc == 0) return "❌ Stoc epuizat";
            if (Stoc <= StocMinim) return "⚠️ Stoc redus";
            return "✓ Disponibil";
        }
    }

    [Ignore]
    public Color StocColor
    {
        get
        {
            if (Stoc == 0) return Colors.Red;
            if (Stoc <= StocMinim) return Colors.Orange;
            return Colors.Green;
        }
    }
}