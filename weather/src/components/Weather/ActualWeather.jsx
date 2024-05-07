import React, { useContext, useEffect, useState } from "react";
import { AppContext } from "../Auth/AppProvider";
import { WeatherApi } from "../../configuration/API";
import { useSearchParams } from "react-router-dom";
import axios from "axios";
import { GuardSpinner } from "react-spinners-kit";
import { Loading } from "../General/Loading";
import {
  Button,
  Col,
  Container,
  FlexboxGrid,
  Grid,
  Input,
  InputGroup,
  Message,
  Panel,
  Row,
  Sidebar,
  Sidenav,
  Text,
} from "rsuite";
import { MdOutlineSearch } from "react-icons/md";
import { set } from "rsuite/esm/utils/dateUtils";
import { MyWeatherIcon } from "./MyWeatherIcon";

const ActualWeather = () => {
  const { store } = useContext(AppContext);
  const [weather, setWeather] = useState(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [searchParams, setSearchParams] = useSearchParams();
  const [city, setCity] = useState("");
  const [serachWeather, setSearchWeather] = useState(true);

  useEffect(() => {
    //console.log(cityFromParam);
    //console.log(city);
    if (serachWeather) {
      setSearchWeather(false);
      let cityFromParam = searchParams.get("cityName") ?? "Liberec";
      setCity(cityFromParam);
      //console.log(cityFromParam);
      loadWeather(cityFromParam);
    }
  }, [serachWeather]);

  const loadWeather = (cityName) => {
    axios
      .get(WeatherApi.current + "?cityName=" + cityName, {
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
        }
        searchParams.set("cityName", city);
      })
      .catch((error) => {
        //setError(error);
      });
  };
  const setSearchCity = () => {
    searchParams.set("cityName", city);
    //setSearchParams("cityName", city);
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
        <h3 style={{ textAlign: "center" }}>Počasí v {city}</h3>
        <Grid fluid>
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
