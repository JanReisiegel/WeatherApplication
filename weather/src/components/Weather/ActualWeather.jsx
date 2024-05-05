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

const ActualWeather = () => {
  const { store } = useContext(AppContext);
  const [weather, setWeather] = useState(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [searchParams, setSearchParams] = useSearchParams();
  const [city, setCity] = useState("");

  useEffect(() => {
    setCity(searchParams.get("cityName"));
    loadWeather();
  }, []);

  const loadWeather = () => {
    axios
      .get(WeatherApi.current + "?cityName=" + city, {
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
        console.error(error);
      });
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
          <InputGroup.Button onClick={loadWeather}>
            <MdOutlineSearch />
          </InputGroup.Button>
        </InputGroup>
      </Col>
      <Col xs={24} sm={24} md={14} lg={17}>
        <h3 style={{ textAlign: "center" }}>Počasí v {city}</h3>
        <FlexboxGrid justify="space-around">
          <FlexboxGrid.Item colspan={6}>
            <Panel
              bordered
              header={<Text color="yellow">Teplota</Text>}
            ></Panel>
          </FlexboxGrid.Item>
        </FlexboxGrid>
      </Col>
    </Row>
  );
};

export default ActualWeather;
