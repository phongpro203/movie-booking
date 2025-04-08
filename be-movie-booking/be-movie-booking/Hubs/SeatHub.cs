using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

public class SeatHub : Hub
{
    // Dictionary lưu trạng thái ghế, key là {showtimeId_seatId}, value là userId
    private static ConcurrentDictionary<string, string> selectedSeats = new();

    // Người dùng chọn ghế
    public async Task SelectSeat(string showtimeId, string seatId)
    {
        var key = $"{showtimeId}_{seatId}";  // Tạo key duy nhất cho từng ghế trong mỗi suất chiếu

        // Nếu ghế chưa được chọn, thêm vào danh sách
        if (!selectedSeats.ContainsKey(key))
        {
            selectedSeats.TryAdd(key, Context.ConnectionId);  // Lưu connectionId của người chọn ghế
            await Clients.Others.SendAsync("SeatLocked", showtimeId, seatId);  // Thông báo cho những người khác rằng ghế đã bị khóa
        }
    }

    // Người dùng bỏ chọn ghế
    public async Task UnselectSeat(string showtimeId, string seatId)
    {
        var key = $"{showtimeId}_{seatId}";

        if (selectedSeats.TryGetValue(key, out string connId) && connId == Context.ConnectionId)
        {
            selectedSeats.TryRemove(key, out _);  // Xóa ghế khỏi danh sách khi người dùng bỏ chọn
            await Clients.Others.SendAsync("SeatUnlocked", showtimeId, seatId);  // Thông báo cho những người khác rằng ghế đã được mở lại
        }
    }

    // Khi người dùng mất kết nối, hệ thống sẽ xóa ghế mà người đó đang giữ
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var releasedSeats = selectedSeats
            .Where(kv => kv.Value == Context.ConnectionId)
            .Select(kv => kv.Key)
            .ToList();

        foreach (var key in releasedSeats)
        {
            selectedSeats.TryRemove(key, out _);

            var parts = key.Split('_');
            var showtimeId = parts[0];
            var seatId = parts[1];

            await Clients.Others.SendAsync("SeatUnlocked", showtimeId, seatId);
        }

        await base.OnDisconnectedAsync(exception);
    }
}
