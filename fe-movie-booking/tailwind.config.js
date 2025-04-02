/** @type {import('tailwindcss').Config} */
export default {
  content: ["./index.html", "./src/**/*.{vue,js,ts,jsx,tsx}"],
  theme: {
    extend: {
      colors: {
        "white-custom": "#FFFFFF",
        "purple-custom": "#958BE8",
        "dark-custom": "#373737",
      },
    },
  },
  plugins: [],
};
