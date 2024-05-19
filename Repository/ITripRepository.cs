using Zadanie7.Models.DTOs;

namespace Zadanie7.Repository;

public interface ITripRepository
{
    public Task<IEnumerable<TripDTO>> GetTripsAsync();
    public Task<Boolean> DeleteClient(int id);
    public Task<ClientTripOutputDTO> AddClientToTripAsync(int idTrip, InputClientDTO request);
}