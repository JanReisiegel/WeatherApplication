import React, { useContext, useEffect, useState } from "react";
import { AppContext } from "../Auth/AppProvider";
import { useSearchParams } from "react-router-dom";
import { Loading } from "../General/Loading";
import {
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
  Toggle,
} from "rsuite";
import { MdOutlineSearch } from "react-icons/md";
import { LocationApi, WeatherApi } from "../../configuration/API";
import { countries } from "./StateOfWorld";
import Unauthorized from "../General/Unauthorized";
import axios from "axios";

const HistoryWeather = () => {
  const { store } = useContext(AppContext);
  const [weather, setWeather] = useState(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [city, setCity] = useState("");
  const [country, setCountry] = useState("");
  const [searchWeather, setSearchWeather] = useState(true);
  const [local, setLocal] = useState(false);

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

  useEffect(() => {
    let cityFromParam = params.get("cityName") ?? "Praha";
    let countryFromParam = params.get("country") ?? "Czechia";
    setCity(cityFromParam);
    setCountry(countryFromParam);
    setSearchWeather(!searchWeather);
  }, []);
  useEffect(() => {
    if (city !== "" && country !== "") {
      getHistoryWeather();
    }
  }, [searchWeather]);

  const getHistoryWeather = () => {
    setLoading(true);
    axios
      .get(
        WeatherApi.historical + "?cityName=" + city + "&country=" + country,
        {
          headers: {
            "Content-Type": "application/json",
            "Access-Control-Allow-Origin": "http://localhost:3000",
            userToken: store.token ? store.token : "",
          },
        }
      )
      .then((response) => {
        setError(null);
        setWeather(response.data);
        console.log(response.data);
        setLoading(false);
      })
      .catch((error) => {
        setError(error);
        setLoading(false);
      });
  };
  if (!store.loggedIn) {
    return <Unauthorized />;
  }
  if (loading) {
    return <Loading />;
  }
  if (error) {
    return <Message type="error">{error.message}</Message>;
  }
  return (
    <Row>
      <Col xs={24} sm={24} md={10} lg={7}>
        <h3>Město</h3>
        <Toggle
          size="lg"
          checkedChildren="Použít aktuální polohu"
          unCheckedChildren="Vybrat město"
          checked={local}
          onChange={() => {
            setLocal(!local);
            if (!local) {
              getLocation();
            }
          }}
        />
        <Divider />
        {!local ? (
          <>
            <InputGroup>
              <Input value={city} onChange={(e) => setCity(e)} />
              <InputGroup.Button onClick={setCity}>
                <MdOutlineSearch />
              </InputGroup.Button>
            </InputGroup>
            <SelectPicker
              data={countries}
              value={country}
              onChange={(e) => setCountry(e)}
              style={{ width: "242px" }}
            />
          </>
        ) : null}
      </Col>
      <Col xs={24} sm={24} md={14} lg={17}>
        <h3 style={{ textAlign: "center" }}>Počasí v {city}</h3>
        <Grid fluid>
          <Row>
            {weather?.forecast.forecastday.map((day) => {
              return (
                <Col xs={24} sm={24} md={12} lg={8} xl={6} xxl={5}>
                  <Panel
                    header={
                      <Text align="center" color="green" size={23}>
                        {day.date.split("-").reverse().join(". ")}
                      </Text>
                    }
                    bordered
                  >
                    <p>
                      <Text align="center">Průměrná teplota:</Text>
                      <Text color="yellow" align="center">
                        {day.day.avgTempC}°C
                      </Text>
                    </p>

                    <Divider />
                    <Text align="center">Maximální teplota:</Text>
                    <Text color="red" align="center">
                      {day.day.maxTempC}°C
                    </Text>
                    <Divider />
                    <Text align="center">Minimální teplota:</Text>
                    <Text color="blue" align="center">
                      {day.day.minTempC}°C
                    </Text>
                  </Panel>
                </Col>
              );
            })}
          </Row>
        </Grid>
      </Col>
    </Row>
  );
};

export default HistoryWeather;
