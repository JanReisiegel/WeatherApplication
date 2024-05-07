import { useContext, useEffect, useState } from "react";
import { MdOutlineSearch } from "react-icons/md";
import {
  Col,
  FlexboxGrid,
  Input,
  InputGroup,
  Message,
  Panel,
  Row,
  Text,
} from "rsuite";
import { AppContext } from "../Auth/AppProvider";
import { useSearchParams } from "react-router-dom";
import { Loading } from "../General/Loading";
import { WeatherApi } from "../../configuration/API";
import axios from "axios";
import { MyWeatherIcon } from "./MyWeatherIcon";

export const ForecastWeather = () => {
  const { store } = useContext(AppContext);
  const [weather, setWeather] = useState(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [searchParams, setSearchParams] = useSearchParams();
  const [city, setCity] = useState("");
  const [serachWeather, setSearchWeather] = useState(true);

  useEffect(() => {
    if (serachWeather) {
      setSearchWeather(false);
      let cityFromParam = searchParams.get("cityName") ?? "Liberec";
      setCity(cityFromParam);
      getForecastWeather(cityFromParam);
    }
  }, [serachWeather]);
  const getForecastWeather = (cityName) => {
    setLoading(true);
    axios
      .get(WeatherApi.forecast + "?cityName=" + cityName, {
        headers: {
          "Content-Type": "application/json",
          "Access-Control-Allow-Origin": "http://localhost:3000",
          userToken: store.token ? store.token : "",
        },
      })
      .then((response) => {
        console.log(response);
        if (response.status === 200) {
          setWeather(response.data);
          setLoading(false);
        }
      })
      .catch((error) => {
        console.error(error);
        setError(error.message);
        setLoading(false);
      })
      .finally(() => {
        setLoading(false);
      });
  };

  const setSearchCity = () => {
    searchParams.set("cityName", city);
    setSearchWeather(true);
  };

  if (loading) {
    return <Loading />;
  }
  if (error) {
    return <Message type="error" description={error} />;
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
