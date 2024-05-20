import { useContext, useEffect, useState } from "react";
import { MdOutlineSearch } from "react-icons/md";
import {
  Button,
  Col,
  Divider,
  Input,
  InputGroup,
  Message,
  Panel,
  Row,
  SelectPicker,
  Text,
} from "rsuite";
import { AppContext } from "../Auth/AppProvider";
import { Loading } from "../General/Loading";
import { LocationApi, WeatherApi } from "../../configuration/API";
import axios from "axios";
import { MyWeatherIcon } from "./WeatherCondition";
import { countries } from "./StateOfWorld";

export const ForecastWeather = () => {
  const { store } = useContext(AppContext);
  const [weather, setWeather] = useState(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [city, setCity] = useState("");
  const [country, setCountry] = useState("");
  const [searchWeather, setSearchWeather] = useState(true);

  let params = new URLSearchParams(window.location.search);

  const getLocation = async () => {
    setLoading(true);
    navigator.geolocation.getCurrentPosition(async (position) => {
      const { latitude, longitude } = position.coords;
      axios
        .get(
          LocationApi.getLocation +
            "?latitude=" +
            latitude +
            "&longitude=" +
            longitude,
          {
            headers: {
              "Content-Type": "application/json",
              "Access-Control-Allow-Origin": "http://localhost:3000",
            },
          }
        )
        .then((response) => {
          setError(null);
          setCity(response.data.cityName);
          setCountry(response.data.country);
          setSearchWeather(!searchWeather);
        })
        .catch((error) => {
          let cityFromParam = params.get("cityName") ?? "Praha";
          let countryFromParam = params.get("country") ?? "Czechia";
          setCity(cityFromParam);
          setCountry(countryFromParam);
        })
        .finally(() => {
          setLoading(false);
        });
    });
  };
  const getForecast = () => {
    setLoading(true);
    axios
      .get(
        WeatherApi.forecast +
          "?cityName=" +
          city.replace(" ", "") +
          "&country=" +
          country.replace(" ", ""),
        {
          headers: {
            "Content-Type": "application/json",
            userToken: store.token ? store.token : "",
          },
        }
      )
      .then((response) => {
        if (response.status === 200) {
          setWeather(response.data);
        }
      })
      .catch((error) => {
        setError(error);
      })
      .finally(() => {
        setLoading(false);
      });
  };

  useEffect(() => {
    let cityFromParam = params.get("cityName") ?? "Praha";
    let countryFromParam = params.get("country") ?? "Czech Republic";
    setCity(cityFromParam);
    setCountry(countryFromParam);
    setSearchWeather(!searchWeather);
  }, []);

  useEffect(() => {
    if (city !== "" && country !== "") {
      getForecast();
    }
  }, [searchWeather]);

  const setSearchCity = () => {
    getForecast();
  };

  if (loading) {
    return <Loading />;
  }
  if (error) {
    return (
      <Message type="error">
        {error.message ? error.message : "Něco se pokazilo"}
      </Message>
    );
  }
  return (
    <Row>
      <Col xs={24} sm={24} md={10} lg={7}>
        <h3>Město</h3>
        <InputGroup>
          <Input value={city} onChange={(e) => setCity(e)} />
          <InputGroup.Button onClick={setSearchCity}>
            <MdOutlineSearch />
          </InputGroup.Button>
        </InputGroup>
        <SelectPicker
          value={country}
          onChange={setCountry}
          data={countries}
          style={{ width: "242px" }}
        />
        <Divider>NEBO</Divider>
        <Button appearance="primary" onClick={getLocation} block>
          Použít aktuální polohu
        </Button>
      </Col>
      <Col xs={24} sm={24} md={14} lg={17}>
        <h3 style={{ textAlign: "center" }}>Předpověď počasí pro {city}</h3>
        <Row>
          {weather
            ? weather.forecastItems.map((item, index) => (
                <Col xs={24} sm={12} md={12} lg={8} xl={6} xxl={4} key={index}>
                  <Panel
                    key={index}
                    shaded
                    bordered
                    bodyFill
                    style={{ margin: "2px", padding: "2.5px" }}
                  >
                    <Text size={"xl"}>
                      Datum:{" "}
                      {item.dateTime
                        .split("T")[0]
                        .split("-")
                        .reverse()
                        .join(". ")}
                    </Text>
                    <Text>
                      Čas:{" "}
                      {item.dateTime
                        .split("T")[1]
                        .replace("Z", "")
                        .split(":")
                        .splice(0, 2)
                        .join(":")}
                    </Text>
                    <div style={{ textAlign: "center" }}>
                      <MyWeatherIcon
                        weatherCondition={item.condition}
                        size={80}
                      />
                    </div>
                    <Text>Teplota: {item.temperature}°C</Text>
                    <Text>Pocitová: {item.feelsTemperature}°C</Text>
                    <Text>Vlhkost: {item.humidity}%</Text>
                    <Text>Rychlost větru: {item.windSpeed} m/s</Text>
                    <Text>Směr větru: {item.directory}°</Text>
                    <Text>Tlak: {item.pressure} hPa</Text>
                  </Panel>
                </Col>
              ))
            : null}
        </Row>
      </Col>
    </Row>
  );
};
