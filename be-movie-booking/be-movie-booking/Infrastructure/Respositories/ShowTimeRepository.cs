using be_movie_booking.Domain.DTOs.Responses;
using be_movie_booking.Domain.Entities;
using be_movie_booking.Infrastructure.Interfaces.Repository;
using be_movie_booking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace be_movie_booking.Infrastructure.Respositories
{
    public class ShowTimeRepository : Repository<ShowTime>, IShowTimeRepository
    {
        public ShowTimeRepository(MyDbContext context) : base(context) { }

        public async Task<(IEnumerable<ShowTime> Items, int TotalCount)> GetPagedAsync(int pageIndex, int pageSize)
        {
            var query = _dbSet.Select(s => new ShowTime
            {
                Id = s.Id,
                ShowDate = s.ShowDate,
                StartTime = s.StartTime,
                CinemaId = s.CinemaId,
                MovieId = s.MovieId,
                RoomId = s.RoomId,
                Movie = new Movie { Id = s.Movie.Id, Title = s.Movie.Title, Duration = s.Movie.Duration },
                Cinema = new Cinema { Id = s.Cinema.Id, Name = s.Cinema.Name },
                Room = new Room { Id = s.Room.Id, Name = s.Room.Name, CinemaId = s.Room.CinemaId },
                }).OrderByDescending(s => s.ShowDate); ;

            var totalCount = await query.CountAsync();
            var items = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            return (items, totalCount);
        }
        public async Task<IEnumerable<ShowTimeByMovieResponse>> GetShowTimeByMovieSAsync(int movieId, int cinemaId)
        {
            return await _dbSet.Where(st => st.MovieId == movieId && st.CinemaId == cinemaId)
                   .Include(st => st.Movie)
                   .Select(st => new ShowTimeByMovieResponse
                   {
                       Id = st.Id,
                       ShowDate = st.ShowDate,
                       StartTime = st.StartTime,
                       EndTime = st.StartTime.AddMinutes(st.Movie.Duration)
                   })
                   .ToListAsync();
        }
        public async Task<ShowTimeDetailResponse?> GetShowTimeDetailAsync(int showTimeId)
        {
            var result = await _dbSet
                .Where(st => st.Id == showTimeId)
                .Select(st => new ShowTimeDetailResponse
                {
                    Id = st.Id,
                    ShowDate = st.ShowDate,
                    StartTime = st.StartTime,
                    Movie = new MovieShowingResponse
                    {
                        Id = st.Movie.Id,
                        Title = st.Movie.Title,
                        Duration = st.Movie.Duration,
                        Genre = st.Movie.Genre,
                        Poster = st.Movie.Poster
                    },
                    CinemaName = st.Cinema.Name,
                    RoomName = st.Room.Name,
                    Seats = st.Room.Seats
                        .Select(seat => new SeatResponse
                        {
                            Id = seat.Id,
                            SeatNumber = seat.SeatNumber,
                            Status = seat.BookingSeats.Any(bs => bs.Booking.ShowTimeId == showTimeId &&
                                    (bs.Booking.Status == "booked" || bs.Booking.Status == "Pending"))
                                     ? (seat.BookingSeats.Any(bs => bs.Booking.ShowTimeId == showTimeId && bs.Booking.Status == "Pending") ? "hold" : "booked")
                                     : "available",

                            SeatType = seat.SeatType.Name,
                            price = seat.SeatType.Price
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<bool> ValidateRoomAndCinema(int roomId, int cinemaId)
        {
            var isRoomValid = await _context.Rooms.AnyAsync(r => r.Id == roomId && r.CinemaId == cinemaId);
            if (!isRoomValid)
            {
                return false;
            }
            return true;
        }
    }
}
