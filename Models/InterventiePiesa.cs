using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace ServiceAuto.Models;

[Table("InterventiePiese")]
public class InterventiePiesa
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [NotNull]
    public int InterventieId { get; set; }

    [NotNull]
    public int PiesaId { get; set; }

    [NotNull]
    public int Cantitate { get; set; } = 1;

    [NotNull]
    public decimal PretUnitare { get; set; }

    public decimal Discount { get; set; } = 0;

    public DateTime DataAdaugare { get; set; }
    [Ignore]
    public decimal Subtotal => (PretUnitare * Cantitate) - Discount;
}