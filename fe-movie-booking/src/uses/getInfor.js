export function getUserId() {
  const userInfoString = localStorage.getItem("userInfo");
  if (userInfoString) {
    const userInfo = JSON.parse(userInfoString);
    return userInfo?.userid || null;
  }
  return null;
}
export function getRolde() {
  const userInfoString = localStorage.getItem("userInfo");
  if (userInfoString) {
    const userInfo = JSON.parse(userInfoString);
    return userInfo?.role || null;
  }
  return null;
}
