using GeoAPI;
using GeoAPI.Geometries;
using GoogleMaps.LocationServices;

namespace WeatherApi.Services
{
    public class LocationFormatTransform
    {
        private IGeometryServices _locationService;

        public LocationFormatTransform()
        {
            _locationService = new NetTopologySuite.NtsGeometryServices();
        }

        public MapPoint TransformAdressToPoint(string address)
        {
            var point = _locationService.GetLatLongFromAddress(address);
            return point;
        }

        public string TransformPointToCityName(MapPoint point)
        {
            var address = _locationService.GetAddressFromLatLang(point.Latitude, point.Longitude);
            return address.City;
        }
    }
}
