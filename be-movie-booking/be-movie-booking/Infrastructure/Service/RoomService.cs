using AutoMapper;
using be_movie_booking.Domain.Entities;
using be_movie_booking.Infrastructure.Interfaces.Repository;
using be_movie_booking.Infrastructure.Interfaces.Service;

namespace be_movie_booking.Infrastructure.Service
{
    public class RoomService : IRoomService
    {
        private readonly IRoomReposiotry _roomRepository;
        private readonly IMapper _mapper;

        public RoomService(IRoomReposiotry roomRepository, IMapper mapper)
        {
            _roomRepository = roomRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Room>> GetAllRoomsAsync()
        {
            return await _roomRepository.GetAllAsync();
        }

        public async Task<Room> CreateRoomAsync(Room room)
        {
            await _roomRepository.AddAsync(room);
            await _roomRepository.SaveChangesAsync();
            return room;
        }

        public async Task<Room?> UpdateRoomAsync(int id, Room room)
        {
            var existingRoom = await _roomRepository.GetByIdAsync(id);
            if (existingRoom == null) return null;

            _mapper.Map(room, existingRoom);
            await _roomRepository.UpdateAsync(existingRoom);
            return existingRoom;
        }

        public async Task<bool> DeleteRoomAsync(int id)
        {
            var room = await _roomRepository.GetByIdAsync(id);
            if (room == null) return false;

            await _roomRepository.DeleteAsync(id);
            await _roomRepository.SaveChangesAsync();
            return true;
        }
    }
}
