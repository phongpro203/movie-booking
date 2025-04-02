using be_movie_booking.Domain.Entities;
using be_movie_booking.Infrastructure.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace be_movie_booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Room>>> GetAllRooms()
        {
            return Ok(await _roomService.GetAllRoomsAsync());
        }


        [HttpPost]
        public async Task<ActionResult<Room>> CreateRoom(Room room)
        {
            var newRoom = await _roomService.CreateRoomAsync(room);
            return Ok(room);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Room>> UpdateRoom(int id, Room room)
        {
            var updatedRoom = await _roomService.UpdateRoomAsync(id, room);
            if (updatedRoom == null) return NotFound();
            return Ok(updatedRoom);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var result = await _roomService.DeleteRoomAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
