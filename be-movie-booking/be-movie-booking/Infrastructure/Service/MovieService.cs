using AutoMapper;
using be_movie_booking.Domain.DTOs.Responses;
using be_movie_booking.Domain.Entities;
using be_movie_booking.Infrastructure.Interfaces.Repository;
using be_movie_booking.Infrastructure.Interfaces.Service;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;

namespace be_movie_booking.Infrastructure.Service
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;

        public MovieService(IMapper mapper, IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<Movie>> GetAllMovieAsync()
        {
            return await _movieRepository.GetAllAsync();
        }
        public async Task<PaginatedList<Movie>> GetMoviesAsync(int pageIndex, int pageSize)
        {
            var (items, totalCount) = await _movieRepository.GetPagedAsync(pageIndex, pageSize, orderBy: q => q.OrderByDescending(m => m.Id));
            return new PaginatedList<Movie>(items, totalCount, pageIndex, pageSize);
        }
        public async Task<Movie> CreateMovieAsync(Movie movie)
        {
            await _movieRepository.AddAsync(movie);
            await _movieRepository.SaveChangesAsync();
            return movie;
        }

        public async Task<Movie?> UpdateMovieAsync(int id, Movie movie)
        {
            var existingMovie = await _movieRepository.GetByIdAsync(id);
            if (existingMovie == null) return null;

            _mapper.Map(movie, existingMovie);

            await _movieRepository.UpdateAsync(existingMovie); 
            return existingMovie;
        }


        public async Task<bool> DeleteMovieAsync(int id)
        {
            var movie = await _movieRepository.GetByIdAsync(id);
            if (movie == null) return false;

            await _movieRepository.DeleteAsync(id);
            await _movieRepository.SaveChangesAsync();

            return true;
        }
        //User
        public async Task<IEnumerable<MovieShowingResponse>> GetShowingMoviesAsync(int cinemaId, bool nowShowing)
        {
            return await _movieRepository.GetShowingMoviesAsync(cinemaId, nowShowing);
        }
        public async Task<MovieShowTimeResponse?> GetMovieDetailAcsync(int movieId, int cinemaId)
        {
            return await _movieRepository.GetMovieDetailAcsync(movieId, cinemaId);
        }
        public async Task<IEnumerable<MovieShowTimeResponse?>> GetMovieShowTimeByCinemaAcsync(int cinemaId)
        {
            return await _movieRepository.GetMovieShowTimeByCinemaAcsync(cinemaId);
        }
    }
}
