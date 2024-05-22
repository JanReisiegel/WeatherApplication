import React, { useContext, useEffect, useState } from "react";
import { AppContext } from "../Auth/AppProvider";
import { LocationApi, WeatherApi } from "../../configuration/API";
import axios from "axios";
import { Loading } from "../General/Loading";
import {
  Button,
  Col,
  Divider,
  Grid,
  Input,
  InputGroup,
  Message,
  Panel,
  Row,
  SelectPicker,
  Text,
} from "rsuite";
import { MdOutlineSearch } from "react-icons/md";
import { MyWeatherDescription, MyWeatherIcon } from "./WeatherCondition";
import { countries } from "./StateOfWorld";

const ActualWeather = () => {
  const { store } = useContext(AppContext);
  const [weather, setWeather] = useState(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [city, setCity] = useState(String);
  const [searchWeather, setSearchWeather] = useState(Boolean);
  const [country, setCountry] = useState(String);

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

  useEffect(() => {
    let cityFromParam = params.get("cityName") ?? "Praha";
    let countryFromParam = params.get("country") ?? "Czechia";
    setCity(cityFromParam);
    setCountry(countryFromParam);
    console.log(city, country);
    setSearchWeather(!searchWeather);
  }, []);

  useEffect(() => {
    if (city !== "" && country !== "") {
      loadWeather();
    }
  }, [searchWeather]);

  const loadWeather = () => {
    setLoading(true);
    console.log("loadWeather", city, country, city === "" && country === "");
    if (city === "" || country === "") {
      getLocation();
    }
    axios
      .get(WeatherApi.current + "?cityName=" + city + "&country=" + country, {
        headers: {
          "Content-Type": "application/json",
          userToken: store.token ? store.token : "",
        },
      })
      .then((response) => {
        setError(null);
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
  const setSearchCity = () => {
    setSearchWeather(!searchWeather);
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
        <Button
          onClick={getLocation}
          appearance="primary"
          style={{ textAlign: "center" }}
          block
        >
          Použít aktuální polohu
        </Button>
      </Col>
      <Col xs={24} sm={24} md={14} lg={17}>
        <Grid fluid>
          <Row>
            <Col xs={24} sm={24} md={24} lg={24} xl={24} xxl={24}>
              <MyWeatherDescription
                weatherCondition={weather?.condition}
                cityName={weather?.location.cityName}
                time={weather?.acquireDateTime}
              />
            </Col>
          </Row>
          <Row gutter={5}>
            <Col xs={24} sm={12} md={12} lg={8} xl={6} xxl={6}>
              <Panel bordered header={<Text color="yellow">Název města</Text>}>
                <Text style={{ fontSize: "40px", textAlign: "center" }}>
                  {weather?.location.cityName}
                </Text>
              </Panel>
            </Col>
            <Col xs={24} sm={12} md={12} lg={8} xl={6} xxl={6}>
              <Panel
                style={{ height: "142px" }}
                bordered
                header={<Text color="yellow">Zeměpisná šířka</Text>}
              >
                <Text style={{ fontSize: "25px", textAlign: "center" }}>
                  {weather?.location.latitude}°
                </Text>
              </Panel>
            </Col>
            <Col xs={24} sm={12} md={12} lg={8} xl={6} xxl={6}>
              <Panel
                bordered
                header={<Text color="yellow">Zeměpisná délka</Text>}
                style={{ height: "142px" }}
              >
                <Text style={{ fontSize: "25px", textAlign: "center" }}>
                  {weather?.location.longitude}°
                </Text>
              </Panel>
            </Col>
            <Col xs={24} sm={12} md={12} lg={8} xl={6} xxl={6}>
              <Panel bordered header={<Text color="yellow">Počasí</Text>}>
                <Text style={{ fontSize: "40px", textAlign: "center" }}>
                  <MyWeatherIcon
                    weatherCondition={weather?.condition}
                    size={43}
                  />
                </Text>
              </Panel>
            </Col>
            <Col xs={24} sm={12} md={12} lg={8} xl={8} xxl={8}>
              <Panel bordered header={<Text color="yellow">Teplota</Text>}>
                <Text style={{ fontSize: "40px", textAlign: "center" }}>
                  {weather?.temperature}°C
                </Text>
              </Panel>
            </Col>
            <Col xs={24} sm={12} md={12} lg={8} xl={8} xxl={8}>
              <Panel
                bordered
                header={<Text color="yellow">Pocitová teplota</Text>}
              >
                <Text style={{ fontSize: "40px", textAlign: "center" }}>
                  {weather?.feelsTemperature}°C
                </Text>
              </Panel>
            </Col>
            <Col xs={24} sm={12} md={12} lg={8} xl={8} xxl={8}>
              <Panel bordered header={<Text color="yellow">Oblačnost</Text>}>
                <Text style={{ fontSize: "40px", textAlign: "center" }}>
                  {weather?.cloudsNow}%
                </Text>
              </Panel>
            </Col>
            <Col xs={24} sm={12} md={12} lg={8} xl={6} xxl={6}>
              <Panel bordered header={<Text color="yellow">Tlak</Text>}>
                <Text style={{ fontSize: "40px", textAlign: "center" }}>
                  {weather?.pressure} hPa
                </Text>
              </Panel>
            </Col>
            <Col xs={24} sm={12} md={12} lg={8} xl={6} xxl={6}>
              <Panel bordered header={<Text color="yellow">Vlhkost</Text>}>
                <Text style={{ fontSize: "40px", textAlign: "center" }}>
                  {weather?.humidity}%
                </Text>
              </Panel>
            </Col>
            <Col xs={24} sm={12} md={12} lg={8} xl={6} xxl={6}>
              <Panel
                bordered
                header={<Text color="yellow">Rychlost větru</Text>}
              >
                <Text style={{ fontSize: "40px", textAlign: "center" }}>
                  {weather?.windSpeed} m/s
                </Text>
              </Panel>
            </Col>
            <Col xs={24} sm={12} md={12} lg={9} xl={6} xxl={6}>
              <Panel bordered header={<Text color="yellow">Směr větru</Text>}>
                <Text style={{ fontSize: "40px", textAlign: "center" }}>
                  {weather?.directory}°
                </Text>
              </Panel>
            </Col>
            <Col xs={24} sm={12} md={12} lg={12} xl={12} xxl={12}>
              <Panel
                bordered
                header={<Text color="yellow">Východ slunce</Text>}
              >
                <Text style={{ fontSize: "40px", textAlign: "center" }}>
                  {weather?.sunrise.split("T")[1].replace("Z", "")}
                </Text>
              </Panel>
            </Col>
            <Col xs={24} sm={12} md={12} lg={12} xl={12} xxl={12}>
              <Panel bordered header={<Text color="yellow">Západ Slunce</Text>}>
                <Text style={{ fontSize: "40px", textAlign: "center" }}>
                  {weather?.sunset.split("T")[1].replace("Z", "")}
                </Text>
              </Panel>
            </Col>
          </Row>
        </Grid>
      </Col>
    </Row>
  );
};

export default ActualWeather;
