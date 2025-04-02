using be_movie_booking.Domain.DTOs.Responses;
using be_movie_booking.Domain.Entities;
using be_movie_booking.Infrastructure.Interfaces.Repository;
using be_movie_booking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace be_movie_booking.Infrastructure.Respositories
{
    public class MovieRepository : Repository<Movie>, IMovieRepository
    {
        public MovieRepository(MyDbContext context) : base(context) { }

        public new async Task AddAsync(Movie movie)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {

                // Lấy tất cả các rạp từ database
                var allCinemas = await _context.Cinemas.Select(c => c.Id).ToListAsync();

                // Thêm Movie vào tất cả các rạp
                movie.MovieCinemas = allCinemas.Select(cinemaId => new MovieCinema
                {
                    CinemaId = cinemaId
                }).ToList();

                await _context.AddAsync(movie);

                // Chỉ lưu một lần duy nhất
                await _context.SaveChangesAsync();

                // Commit transaction
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public new async Task DeleteAsync(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var movie = await _dbSet.FindAsync(id);
                if (movie != null)
                {
                    // Xóa tất cả MovieCinema liên quan trước
                    _context.MovieCinemas.RemoveRange(_context.MovieCinemas.Where(mc => mc.MovieId == movie.Id));

                    // Xóa Movie
                    _dbSet.Remove(movie);

                    // Lưu thay đổi
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task<IEnumerable<MovieShowingResponse>> GetShowingMoviesAsync(int cinemaId, bool nowShowing)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            return await _context.MovieCinemas
                .Include(mc => mc.Movie)
                .Where(mc => mc.CinemaId == cinemaId)
                .Where(mc => nowShowing ? mc.Movie.ReleaseDate <= today : mc.Movie.ReleaseDate > today) // Nếu nowShow = true thì lấy ra phim đang chiếu (ngày ra mắt <= hôm nay, và ngược lại)
                .Where(mc => nowShowing
                            ? mc.Movie.ShowTimes.Any(st => st.CinemaId == cinemaId && st.ShowDate >= today)
                            : true)
                .Select(mc => new MovieShowingResponse
                {
                    Id = mc.Movie.Id,
                    Title = mc.Movie.Title,
                    Duration = mc.Movie.Duration,
                    Genre = mc.Movie.Genre,
                    Poster = mc.Movie.Poster
                })
                .ToListAsync();
        }

        public async Task<MovieShowTimeResponse?> GetMovieDetailAcsync(int movieId, int cinemaId)
        {
            return await _context.Movies
                            .Where(m => m.Id == movieId && m.MovieCinemas.Any(mc => mc.CinemaId == cinemaId))
                            .Select(m => new MovieShowTimeResponse
                            {
                                Id = m.Id,
                                Title = m.Title,
                                Description = m.Description,
                                Duration = m.Duration,
                                Genre = m.Genre,
                                Poster = m.Poster,
                                Trailer = m.Trailer,
                                ReleaseDate = m.ReleaseDate,
                                Reviews = m.Reviews.Select(r => new ReviewResponse
                                {
                                    id = r.Id,
                                    Rate = r.Rate,
                                    Comment = r.Comment,
                                    userId = r.UserId,
                                    userName = r.User.Name
                                }).OrderByDescending(r => r.id).ToList(),
                                ShowTimes = m.ShowTimes
                                    .Where(st => st.CinemaId == cinemaId)
                                    .Where(st => st.ShowDate >= DateOnly.FromDateTime(DateTime.Today))
                                    .Select(st => new ShowTimeByMovieResponse
                                    {
                                        Id = st.Id,
                                        ShowDate = st.ShowDate,
                                        StartTime = st.StartTime,
                                        EndTime = st.StartTime.AddMinutes(m.Duration),
                                    })
                                    .ToList()
                            })
                            .FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<MovieShowTimeResponse?>> GetMovieShowTimeByCinemaAcsync(int cinemaId)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            return await _context.Movies
                            .Where(m => m.MovieCinemas.Any(mc => mc.CinemaId == cinemaId) && m.ReleaseDate <= today)
                            .Where(m => m.ShowTimes.Any(st => st.CinemaId == cinemaId && st.ShowDate >= today))
                            .Select(m => new MovieShowTimeResponse
                            {
                                Id = m.Id,
                                Title = m.Title,
                                Duration = m.Duration,
                                ReleaseDate = m.ReleaseDate,
                                Genre = m.Genre,
                                Poster = m.Poster,
                                ShowTimes = m.ShowTimes
                                    .Where(st => st.CinemaId == cinemaId)
                                    .Select(st => new ShowTimeByMovieResponse
                                    {
                                        Id = st.Id,
                                        ShowDate = st.ShowDate,
                                        StartTime = st.StartTime,
                                        EndTime = st.StartTime.AddMinutes(m.Duration),
                                    })
                                    .ToList()
                            })
                            .ToListAsync();
        }
    }
}
