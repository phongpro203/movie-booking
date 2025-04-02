// Khởi tạo ra 4 ngày liên tiếp từ hôm nay
export function getUpcomingDates() {
  const weekdays = ["CN", "T2", "T3", "T4", "T5", "T6", "T7"];

  // Hàm lấy danh sách 4 ngày tính từ hôm nay
  let datesArray = [];
  let today = new Date();

  for (let i = 0; i < 4; i++) {
    let newDate = new Date();
    newDate.setDate(today.getDate() + i); // Cộng i ngày vào hôm nay

    datesArray.push({
      day: newDate.getDate().toString().padStart(2, "0"), // Lấy ngày (dd)
      month: (newDate.getMonth() + 1).toString(), // Lấy tháng (mm)
      weekday: weekdays[newDate.getDay()], // Lấy thứ trong tuần
      fullDate: `${newDate.getFullYear()}-${(newDate.getMonth() + 1)
        .toString()
        .padStart(2, "0")}-${newDate.getDate().toString().padStart(2, "0")}`,
    });
  }
  return datesArray;
}
