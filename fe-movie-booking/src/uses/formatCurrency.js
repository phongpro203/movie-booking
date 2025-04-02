export function formatCurrency(amount) {
  if (typeof amount !== "number") {
    amount = parseFloat(amount) || 0;
  }
  return amount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".");
}
