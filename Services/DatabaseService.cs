using SQLite;
using ServiceAuto.Models;
using System.Diagnostics;

namespace ServiceAuto.Services;

public class DatabaseService
{
    private SQLiteAsyncConnection _database;

    public DatabaseService()
    {
        InitializeDatabase();
    }

    private async void InitializeDatabase()
    {
        try
        {
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "serviceauto.db3");
            _database = new SQLiteAsyncConnection(dbPath);

            await _database.CreateTableAsync<Mecanic>();
            await _database.CreateTableAsync<Client>();
            await _database.CreateTableAsync<Masina>();
            await _database.CreateTableAsync<Interventie>();
            await _database.CreateTableAsync<PiesaDeSchimb>();
            await _database.CreateTableAsync<InterventiePiesa>();

            Debug.WriteLine("✅ Baza de date creată cu succes!");

            await SeedDemoDataAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"❌ Eroare la inițializarea bazei de date: {ex.Message}");
        }
    }

    private async Task SeedDemoDataAsync()
    {
        var count = await _database.Table<Mecanic>().CountAsync();

        if (count == 0)
        {
            var mecanic = new Mecanic
            {
                Nume = "Ionescu Andrei",
                Email = "mecanic@service.ro",
                ParolaHash = "123456",
                Telefon = "0722123456",
                DataAngajare = DateTime.Now.AddYears(-2),
                EsteAdmin = true,
                EsteActiv = true
            };

            await _database.InsertAsync(mecanic);

            var piese = new List<PiesaDeSchimb>
            {
                new() {
                    Denumire = "Filtru de polen",
                    Cod = "MANN FP-001",
                    Producator = "MANN-FILTER",
                    Pret = 45.99m,
                    Stoc = 10,
                    StocMinim = 3,
                    Categorie = "Filtre",
                    Descriere = "Filtru de habitacul premium"
                },
                new() {
                    Denumire = "Ulei motor 5W30",
                    Cod = "CASTROL EDGE 5L",
                    Producator = "Castrol",
                    Pret = 189.99m,
                    Stoc = 25,
                    StocMinim = 5,
                    Categorie = "Uleiuri",
                    Descriere = "Ulei synthetic, 5 litri"
                },
                new() {
                    Denumire = "Bujie iridium",
                    Cod = "NGK 6418",
                    Producator = "NGK",
                    Pret = 89.50m,
                    Stoc = 4,
                    StocMinim = 10,
                    Categorie = "Piese motor",
                    Descriere = "Bujie iridium premium"
                },
                new() {
                    Denumire = "Disc frână față",
                    Cod = "BREMBO 09.8902.10",
                    Producator = "Brembo",
                    Pret = 320.00m,
                    Stoc = 8,
                    StocMinim = 4,
                    Categorie = "Frâne",
                    Descriere = "Disc ventilat, diametru 320mm"
                },
                new() {
                    Denumire = "Alternator",
                    Cod = "BOSCH 0124515012",
                    Producator = "Bosch",
                    Pret = 850.00m,
                    Stoc = 2,
                    StocMinim = 3,
                    Categorie = "Electric",
                    Descriere = "Alternator 120A, reconstruit"
                }
            };

            foreach (var piesa in piese)
            {
                await _database.InsertAsync(piesa);
            }

            var clienti = new List<Client>
            {
                new() {
                    Nume = "Popescu Ion",
                    Email = "ion.popescu@gmail.com",
                    Telefon = "0733111222",
                    Adresa = "Str. Libertatii nr. 15, Bucuresti"
                },
                new() {
                    Nume = "Marinescu Maria",
                    Email = "maria.m@gmail.com",
                    Telefon = "0744222333",
                    Adresa = "Bd. Unirii nr. 42, Iasi"
                },
                new() {
                    Nume = "Dumitrescu Vasile",
                    Email = "vasile.d@yahoo.com",
                    Telefon = "0755333444",
                    Adresa = "Str. Primaverii nr. 7, Cluj"
                }
            };

            foreach (var client in clienti)
            {
                await _database.InsertAsync(client);
            }

            var masini = new List<Masina>
            {
                new() {
                    ClientId = 1,
                    Marca = "Volkswagen",
                    Model = "Golf 7",
                    AnFabricatie = "2015",
                    NrInmatriculare = "B-123-ABC",
                    SerieSasiu = "WVWZZZ1KZCW123456",
                    Culoare = "Alb",
                    Combustibil = "Diesel",
                    Kilometraj = 145000
                },
                new() {
                    ClientId = 2,
                    Marca = "Audi",
                    Model = "A4",
                    AnFabricatie = "2018",
                    NrInmatriculare = "IS-456-DEF",
                    SerieSasiu = "WAUZZZ8K9JA123456",
                    Culoare = "Negru",
                    Combustibil = "Benzina",
                    Kilometraj = 85000
                }
            };

            foreach (var masina in masini)
            {
                await _database.InsertAsync(masina);
            }

            Debug.WriteLine("✅ Date demo adăugate complet!");
        }
    }

    public async Task<bool> VerificaAutentificareAsync(string email, string parola)
    {
        await Task.Delay(100); 

        var mecanic = await _database.Table<Mecanic>()
            .FirstOrDefaultAsync(m => m.Email == email && m.ParolaHash == parola);

        return mecanic != null;
    }

    public async Task<int> GetPieseCountAsync()
    {
        return await _database.Table<PiesaDeSchimb>().CountAsync();
    }

    public async Task<int> GetClientiCountAsync()
    {
        return await _database.Table<Client>().CountAsync();
    }

    public async Task<List<PiesaDeSchimb>> GetPieseDeSchimbAsync()
    {
        return await _database.Table<PiesaDeSchimb>()
            .Where(p => p.EsteActiv)
            .OrderBy(p => p.Denumire)
            .ToListAsync();
    }

    public async Task<int> SavePiesaAsync(PiesaDeSchimb piesa)
    {
        if (piesa.Id == 0)
        {
            piesa.DataAprovizionare = DateTime.Now;
            return await _database.InsertAsync(piesa);
        }
        else
        {
            return await _database.UpdateAsync(piesa);
        }
    }

    public async Task<int> DeletePiesaAsync(PiesaDeSchimb piesa)
    {
        piesa.EsteActiv = false;
        return await _database.UpdateAsync(piesa);
    }

    public async Task<PiesaDeSchimb> GetPiesaByIdAsync(int id)
    {
        return await _database.Table<PiesaDeSchimb>()
            .Where(p => p.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<List<PiesaDeSchimb>> GetPieseCuStocRedusAsync()
    {
        return await _database.Table<PiesaDeSchimb>()
            .Where(p => p.Stoc <= p.StocMinim && p.EsteActiv)
            .ToListAsync();
    }

    public async Task<List<Client>> GetClientiAsync()
    {
        return await _database.Table<Client>()
            .Where(c => c.EsteActiv)
            .OrderBy(c => c.Nume)
            .ToListAsync();
    }

    public async Task<int> SaveClientAsync(Client client)
    {
        if (client.Id == 0)
        {
            client.DataInregistrare = DateTime.Now;
            return await _database.InsertAsync(client);
        }
        else
        {
            return await _database.UpdateAsync(client);
        }
    }

    public async Task<Client> GetClientByIdAsync(int id)
    {
        return await _database.Table<Client>()
            .Where(c => c.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<int> DeleteClientAsync(Client client)
    {
        client.EsteActiv = false;
        return await _database.UpdateAsync(client);
    }
    public async Task<List<Interventie>> GetInterventiiByClientAsync(int clientId)
    {
        return await _database.Table<Interventie>()
            .Where(i => i.ClientId == clientId)
            .OrderByDescending(i => i.DataProgramare)
            .ToListAsync();
    }

    public async Task<List<Masina>> GetMasiniAsync()  
    {
        return await _database.Table<Masina>()
            .Where(m => m.EsteActiv)
            .OrderBy(m => m.Marca)
            .ThenBy(m => m.Model)
            .ToListAsync();
    }
    public async Task<int> SaveMasinaAsync(Masina masina)
    {
        if (masina.Id == 0)
        {
            masina.DataInregistrare = DateTime.Now;
            return await _database.InsertAsync(masina);
        }
        else
        {
            return await _database.UpdateAsync(masina);
        }
    }

    public async Task<List<Masina>> GetMasiniByClientAsync(int clientId)
    {
        return await _database.Table<Masina>()
            .Where(m => m.ClientId == clientId && m.EsteActiv)
            .ToListAsync();
    }

    public async Task<Masina> GetMasinaByIdAsync(int id)
    {
        return await _database.Table<Masina>()
            .Where(m => m.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<int> DeleteMasinaAsync(Masina masina)
    {
        masina.EsteActiv = false;
        return await _database.UpdateAsync(masina);
    }

    public async Task<int> SaveInterventieAsync(Interventie interventie)
    {
        if (interventie.Id == 0)
        {
            interventie.DataCreare = DateTime.Now;
            return await _database.InsertAsync(interventie);
        }
        else
        {
            return await _database.UpdateAsync(interventie);
        }
    }

    public async Task<List<Interventie>> GetInterventiiAsync()
    {
        return await _database.Table<Interventie>()
            .OrderByDescending(i => i.DataProgramare)
            .ToListAsync();
    }

    public async Task<List<Interventie>> GetInterventiiAziAsync()
    {
        var today = DateTime.Today;
        return await _database.Table<Interventie>()
            .Where(i => i.DataProgramare.Date == today)
            .ToListAsync();
    }

    public async Task<Interventie> GetInterventieByIdAsync(int id)
    {
        return await _database.Table<Interventie>()
            .Where(i => i.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<int> DeleteInterventieAsync(Interventie interventie)
    {
        interventie.Status = "Anulata";
        return await _database.UpdateAsync(interventie);
    }

    public async Task<int> SaveInterventiePiesaAsync(InterventiePiesa interventiePiesa)
    {
        if (interventiePiesa.Id == 0)
        {
            return await _database.InsertAsync(interventiePiesa);
        }
        else
        {
            return await _database.UpdateAsync(interventiePiesa);
        }
    }

    public async Task<List<InterventiePiesa>> GetPiesePentruInterventieAsync(int interventieId)
    {
        return await _database.Table<InterventiePiesa>()
            .Where(ip => ip.InterventieId == interventieId)
            .ToListAsync();
    }

    public async Task<int> DeleteInterventiePiesaAsync(InterventiePiesa interventiePiesa)
    {
        return await _database.DeleteAsync(interventiePiesa);
    }

    public async Task<List<Interventie>> GetInterventiiProgramateMaineAsync()
    {
        var tomorrow = DateTime.Today.AddDays(1);
        var dayAfterTomorrow = tomorrow.AddDays(1);

        return await _database.Table<Interventie>()
            .Where(i => i.DataProgramare >= tomorrow && i.DataProgramare < dayAfterTomorrow)
            .OrderBy(i => i.DataProgramare)
            .ToListAsync();
    }

    public async Task<int> UpdateInterventieStatusAsync(int interventieId, string newStatus)
    {
        var interventie = await GetInterventieByIdAsync(interventieId);

        if (interventie != null)
        {
            interventie.Status = newStatus;
            return await SaveInterventieAsync(interventie);
        }

        return 0;
    }

    public async Task<List<Interventie>> GetInterventiiByDateAsync(DateTime date)
    {
        var nextDay = date.AddDays(1);

        return await _database.Table<Interventie>()
            .Where(i => i.DataProgramare >= date && i.DataProgramare < nextDay)
            .OrderBy(i => i.DataProgramare)
            .ToListAsync();
    }

    

    public async Task<List<Interventie>> GetInterventiiByMasinaAsync(int masinaId)
    {
        return await _database.Table<Interventie>()
            .Where(i => i.MasinaId == masinaId)
            .OrderByDescending(i => i.DataProgramare)
            .ToListAsync();
    }

    public async Task<List<Interventie>> GetInterventiiByStatusAsync(string status)
    {
        return await _database.Table<Interventie>()
            .Where(i => i.Status == status)
            .OrderBy(i => i.DataProgramare)
            .ToListAsync();
    }

    public async Task<List<Interventie>> GetInterventiiActiveAsync()
    {
        var statuses = new List<string> { "Programata", "In Progres" };

        return await _database.Table<Interventie>()
            .Where(i => statuses.Contains(i.Status))
            .OrderBy(i => i.DataProgramare)
            .ToListAsync();
    }

    public async Task<int> UpdateInterventieMecanicAsync(int interventieId, int mecanicId)
    {
        var interventie = await GetInterventieByIdAsync(interventieId);

        if (interventie != null)
        {
            interventie.MecanicId = mecanicId;
            return await SaveInterventieAsync(interventie);
        }

        return 0;
    }

    public async Task<int> UpdateInterventieDescriereAsync(int interventieId, string descriere)
    {
        var interventie = await GetInterventieByIdAsync(interventieId);

        if (interventie != null)
        {
            interventie.Descriere = descriere;
            return await SaveInterventieAsync(interventie);
        }

        return 0;
    }
    public async Task<List<Mecanic>> GetMecaniciAsync()
    {
        return await _database.Table<Mecanic>()
            .Where(m => m.EsteActiv)
            .ToListAsync();
    }

    public async Task<List<Interventie>> GetInterventiiCuPieseAsync()
    {
        var interventiePiese = await _database.Table<InterventiePiesa>().ToListAsync();

        var interventiiCuPieseIds = interventiePiese
            .Select(ip => ip.InterventieId)
            .Distinct()
            .ToList();

        return await _database.Table<Interventie>()
            .Where(i => interventiiCuPieseIds.Contains(i.Id))
            .ToListAsync();
    }

    public async Task<Mecanic> GetMecanicByEmailAsync(string email)
    {
        return await _database.Table<Mecanic>()
            .FirstOrDefaultAsync(m => m.Email == email);
    }

    public async Task<int> SaveMecanicAsync(Mecanic mecanic)
    {
        if (mecanic.Id == 0)
        {
            mecanic.DataAngajare = DateTime.Now;
            return await _database.InsertAsync(mecanic);
        }
        else
        {
            return await _database.UpdateAsync(mecanic);
        }
    }

    public async Task<int> UpdateMecanicAsync(Mecanic mecanic)
    {
        return await _database.UpdateAsync(mecanic);
    }

    public async Task<int> DeleteMecanicAsync(Mecanic mecanic)
    {
        return await _database.DeleteAsync(mecanic);
    }

    public async Task<List<Mecanic>> GetMecaniciAsync(bool doarActiv = true)
    {
        if (doarActiv)
        {
            return await _database.Table<Mecanic>()
                .Where(m => m.EsteActiv)
                .OrderBy(m => m.Nume)
                .ToListAsync();
        }
        else
        {
            return await _database.Table<Mecanic>()
                .OrderBy(m => m.Nume)
                .ToListAsync();
        }
    }
}