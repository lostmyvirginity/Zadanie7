using Microsoft.AspNetCore.Mvc;
using Zadanie7.Models.DTOs;

namespace Zadanie7.Services;

public interface ITripService
{
    public Task<IEnumerable<TripDTO>> GetTrips();
    public Task<Boolean> DeleteClient(int id);
    public Task<ClientTripOutputDTO> AddClientToTripAsync(int idTrip, InputClientDTO request);
}