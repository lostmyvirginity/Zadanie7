using Microsoft.AspNetCore.Mvc;
using Zadanie7.Models.DTOs;
using Zadanie7.Repository;

namespace Zadanie7.Services;

public class TripService : ITripService
{
    private readonly ITripRepository _tripRepository;

    public TripService(ITripRepository tripRepository)
    {
        _tripRepository = tripRepository ?? throw new ArgumentNullException(nameof(tripRepository));
    }

    public async Task<IEnumerable<TripDTO>> GetTrips()
    {
        var trips = await _tripRepository.GetTripsAsync();
        return trips;
    }

    public async Task<Boolean> DeleteClient(int id)
    {
        var trips = await _tripRepository.DeleteClient(id);
        return trips;
    }
    public Task<ClientTripOutputDTO> AddClientToTripAsync(int idTrip, InputClientDTO request)
    {
        return _tripRepository.AddClientToTripAsync(idTrip, request);
    }
}