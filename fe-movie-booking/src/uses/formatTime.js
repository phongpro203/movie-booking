//format lại từ kiểu 12:00:00 thành 12:00 (chỉ lấy giờ và phút)
export function formatTime(time) {
  if (time) {
    return time.split(":").slice(0, 2).join(":");
  }
}
