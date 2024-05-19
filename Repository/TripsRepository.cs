using Microsoft.EntityFrameworkCore;
using Zadanie7.Models;
using Zadanie7.Models.DTOs;

namespace Zadanie7.Repository;

public class TripsRepository : ITripRepository
{
    private readonly MasterContext _context;
    public TripsRepository(MasterContext _contex)
    {
        this._context = _contex;
    }

    public async Task<IEnumerable<TripDTO>> GetTripsAsync()
    {
       
        var result = await _context
            .Trips
            .Select(e =>
                new TripDTO()
                {
                    Name = e.Name,
                    Description = e.Description,
                    DateFrom = DateOnly.FromDateTime(e.DateFrom),
                    DateTo = DateOnly.FromDateTime(e.DateTo),
                    MaxPeople = e.MaxPeople,
                    Countries = e.IdCountries.Select(c => new CountryDTO()
                    {
                        Name = c.Name
                    }),
                    Clients = e.ClientTrips.Select(ct => new ClientDTO()
                    {
                        FirstName = ct.IdClientNavigation.FirstName,
                        LastName = ct.IdClientNavigation.LastName
                    })
                }).ToListAsync();
        if (result.Count == 0) throw new Exception("No trips found");
        
        return result;
    }

    public async Task<Boolean> DeleteClient(int id)
    {
        var client = await _context.Clients.FindAsync(id);
        if (client == null)
        {
            return false;
        }

        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<ClientTripOutputDTO> AddClientToTripAsync(int idTrip, InputClientDTO request)
    {
        var trip = await _context.Trips.FindAsync(idTrip);
        if (trip == null)
        {
            return new ClientTripOutputDTO() { Message = "Trip not found", Success = false };
        }

        var client = await _context.Clients.FirstOrDefaultAsync(c => c.Pesel == request.Pesel);
        if (client == null)
        {
            client = new Client
            {
                FirstName = request.Firstname,
                LastName = request.LastName,
                Email = request.Email,
                Telephone = request.Telephone,
                Pesel = request.Pesel
            };
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
        }

        var existingClientTrip = await _context.ClientTrips
            .FirstOrDefaultAsync(ct => ct.IdClient == client.IdClient && ct.IdTrip == idTrip);

        if (existingClientTrip != null)
        {
            return new ClientTripOutputDTO() { Message = "Client is already registered for this trip", Success = false };
        }

        var clientTrip = new ClientTrip
        {
            IdClient = client.IdClient,
            IdTrip = idTrip,
            PaymentDate = request.PaymentDate,
            RegisteredAt = DateTime.Now
        };
        _context.ClientTrips.Add(clientTrip);
        await _context.SaveChangesAsync();

        return new ClientTripOutputDTO() { Message = "Client successfully registered for the trip", Success = true };
    }

}