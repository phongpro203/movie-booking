using AutoMapper;
using be_movie_booking.Domain.DTOs.Responses;
using be_movie_booking.Domain.Entities;
using be_movie_booking.Infrastructure.Interfaces.Repository;
using be_movie_booking.Infrastructure.Interfaces.Service;
using be_movie_booking.Infrastructure.Respositories;
using Microsoft.EntityFrameworkCore;

namespace be_movie_booking.Infrastructure.Service
{
    public class ShowTimeService : IShowTimeService
    {
        private readonly IShowTimeRepository _showTimeRepository;
        private readonly IMapper _mapper;

        public ShowTimeService(IShowTimeRepository showTimeRepository, IMapper mapper)
        {
            _showTimeRepository = showTimeRepository;
            _mapper = mapper;
        }
        public async Task<PaginatedList<ShowTime>> GetShowtimeAsync(int pageIndex, int pageSize)
        {
            var (items, totalCount) = await _showTimeRepository.GetPagedAsync(pageIndex, pageSize);
            return new PaginatedList<ShowTime>(items, totalCount, pageIndex, pageSize);
        }
        public async Task<ShowTime> CreateShowTimeAsync(ShowTime showTime)
        {
            await _showTimeRepository.AddAsync(showTime);
            await _showTimeRepository.SaveChangesAsync();
            return showTime;
        }

        public async Task<ShowTime?> UpdateShowTimeAsync(int id, ShowTime showTime)
        {
            var existingShowTime = await _showTimeRepository.GetByIdAsync(id);
            if (existingShowTime == null) return null;

            // Kiểm tra RoomId có thuộc CinemaId hay không
            var isRoomValid = await _showTimeRepository.ValidateRoomAndCinema(showTime.RoomId,showTime.CinemaId);
            if (!isRoomValid)
            {
                throw new InvalidOperationException("Phòng không thuộc rạp đã chọn!");
            }
            _mapper.Map(showTime, existingShowTime);

            await _showTimeRepository.UpdateAsync(existingShowTime);
            return existingShowTime;
        }

        public async Task<bool> DeleteShowTimeAsync(int id)
        {
            var showTime = await _showTimeRepository.GetByIdAsync(id);
            if (showTime == null) return false;

            await _showTimeRepository.DeleteAsync(id);
            await _showTimeRepository.SaveChangesAsync();
            return true;
        }
        //user
        public async Task<IEnumerable<ShowTimeByMovieResponse>> GetShowTimeByMovieSAsync(int movieId, int cinemaId)
        {
            return await _showTimeRepository.GetShowTimeByMovieSAsync(movieId, cinemaId);
        }
        public async Task<ShowTimeDetailResponse?> GetShowTimeDetailAsync(int showTimeId)
        {
            return await _showTimeRepository.GetShowTimeDetailAsync(showTimeId);
        }

    }
}
