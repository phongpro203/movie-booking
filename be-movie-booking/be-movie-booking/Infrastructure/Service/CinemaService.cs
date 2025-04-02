using AutoMapper;
using be_movie_booking.Domain.DTOs.Responses;
using be_movie_booking.Domain.Entities;
using be_movie_booking.Infrastructure.Interfaces.Repository;
using be_movie_booking.Infrastructure.Interfaces.Service;

namespace be_movie_booking.Infrastructure.Service
{
    public class CinemaService : ICinemaService
    {
        private readonly ICinemaRepository _cinemaRepository;
        private readonly IMapper _mapper;

        public CinemaService(ICinemaRepository cinemaRepository, IMapper mapper)
        {
            _cinemaRepository = cinemaRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Cinema>> GetAllCinemaAsync()
        {
            return await _cinemaRepository.GetAllAsync();
        }

        public async Task<IEnumerable<CinemaNameResponse>> GetAllCinemasNameAsync()
        {
            return await _cinemaRepository.GetAllCinemasNameAsync();
        }
        public async Task<IEnumerable<Cinema>> GetCinemasAsync()
        {
            return await _cinemaRepository.GetAllAsync();
        }


        public async Task<Cinema> CreateCinemaAsync(Cinema cinema)
        {
            await _cinemaRepository.AddAsync(cinema);
            await _cinemaRepository.SaveChangesAsync();
            return cinema;
        }

        public async Task<Cinema?> UpdateCinemaAsync(int id, Cinema cinema)
        {
            var existingCinema = await _cinemaRepository.GetByIdAsync(id);
            if (existingCinema == null) return null;

            _mapper.Map(cinema, existingCinema);
            await _cinemaRepository.UpdateAsync(existingCinema);
            return existingCinema;
        }

        public async Task<bool> DeleteCinemaAsync(int id)
        {
            var cinema = await _cinemaRepository.GetByIdAsync(id);
            if (cinema == null) return false;

            await _cinemaRepository.DeleteAsync(id);
            await _cinemaRepository.SaveChangesAsync();
            return true;
        }
    }
}
