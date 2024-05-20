export const UserApi = {
  basic:
    (process.env.NODE_ENV === "development"
      ? "https://localhost:7248"
      : "https://stinweatherapi.azurewebsites.net") + "/api/Users",
  login:
    (process.env.NODE_ENV === "development"
      ? "https://localhost:7248"
      : "https://stinweatherapi.azurewebsites.net") + "/api/Users/login",
  register:
    (process.env.NODE_ENV === "development"
      ? "https://localhost:7248"
      : "https://stinweatherapi.azurewebsites.net") + "/api/Users/register",
  logout:
    (process.env.NODE_ENV === "development"
      ? "https://localhost:7248"
      : "https://stinweatherapi.azurewebsites.net") + "/api/Users/logout",
  getOne:
    (process.env.NODE_ENV === "development"
      ? "https://localhost:7248"
      : "https://stinweatherapi.azurewebsites.net") + "/api/Users/one",
  getAll:
    (process.env.NODE_ENV === "development"
      ? "https://localhost:7248"
      : "https://stinweatherapi.azurewebsites.net") + "/api/Users/all",
};
export const WeatherApi = {
  current:
    (process.env.NODE_ENV === "development"
      ? "https://localhost:7248"
      : "https://stinweatherapi.azurewebsites.net") + "/api/Weather/actual",
  forecast:
    (process.env.NODE_ENV === "development"
      ? "https://localhost:7248"
      : "https://stinweatherapi.azurewebsites.net") + "/api/Weather/forecast",
  historical:
    (process.env.NODE_ENV === "development"
      ? "https://localhost:7248"
      : "https://stinweatherapi.azurewebsites.net") + "/api/Weather/history",
};
export const LocationApi = {
  basic:
    (process.env.NODE_ENV === "development"
      ? "https://localhost:7248"
      : "https://stinweatherapi.azurewebsites.net") + "/api/Locations",
  getAll:
    (process.env.NODE_ENV === "development"
      ? "https://localhost:7248"
      : "https://stinweatherapi.azurewebsites.net") + "/api/Locations/all",
  getLocation:
    (process.env.NODE_ENV === "development"
      ? "https://localhost:7248"
      : "https://stinweatherapi.azurewebsites.net") + "/api/Locations/coords",
};
