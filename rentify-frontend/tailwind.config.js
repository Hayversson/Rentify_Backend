/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ['./src/**/*.{html,ts}'],
  theme: {
    extend: {
      fontFamily: {
        sans: ['Inter', 'system-ui', 'sans-serif']
      },
      colors: {
        brand: {
          50: '#f2f7ff',
          100: '#e6efff',
          200: '#cfe0ff',
          300: '#a7c6ff',
          400: '#6fa1ff',
          500: '#3b82f6',
          600: '#2563eb',
          700: '#1d4ed8',
          800: '#1e40af',
          900: '#1e3a8a'
        }
      },
      boxShadow: {
        glow: '0 0 0 1px rgba(59,130,246,0.35), 0 12px 30px -12px rgba(59,130,246,0.6)'
      }
    }
  },
  plugins: []
};
